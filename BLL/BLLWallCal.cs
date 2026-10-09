/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.BLL
 * 文件名：  BLLExp
 * 版本号：  V1.0.0.0
 * 唯一标识：2951152b-32bb-4f70-861a-78ea2f2d2012
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/23 18:51:29
 * 描述：
  * 。BLL，系统辨识部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/18 16:22:39		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Threading;
using BRX.DAL.CalDAL.CalDALModel;
using BRX.Model;
using BRX.Model.Exp;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using static BRX.Model.Enums.Enums;

namespace BRX.BLL
{
    public partial class Bll : ObservableObject
    {
        /// <summary>
        /// 炉壁校准检测事务
        /// </summary>
        private void WallCalBLLAsync()
        {
            if (Dev.RunMode != RunMode.WallCal_Mode)
                return;

            if (ExistWallCalCMD)
            {
                if (WallCalDQ.HeightPointDQ.TimePoint.IsStarted&& !WallCalDQ.HeightPointDQ.IsReced)
                {
                    TimeSpan spanFroSpStart = DateTime.Now - WallCalDQ.HeightPointDQ.TimePoint.StartTime;
                    WallCalDQ.HeightPointDQ.TimePoint.TimeFromStart = (int)spanFroSpStart.TotalSeconds;

                    //判断平衡数据插入
                    Dev.T1StbBalanceEst.ADD(Dev.AIList[0].ValueFinal);
                    Dev.T2StbBalanceEst.ADD(Dev.AIList[1].ValueFinal);
                    //分析控温数据是否满足条件
                    Dev.T1StbBalanceEst.Evaluate();
                    Dev.T2StbBalanceEst.Evaluate();

                    //记录原始数据
                    RecWallCalPoint();
                    WallCalDQ.HeightPointDQ.RecPointNow++;
                }
                
                //控温计算
                double tempValueNow;        //当前值
                double tempPressAim;        //控制目标
                double tempPressSet;        //设定值
                double tempErr;             //PID计算用误差
                if (Dev.IsStd2023)
                    tempValueNow = Math.Abs((Dev.AIList[0].ValueFinal + Dev.AIList[1].ValueFinal) / 2);
                else
                    tempValueNow = Math.Abs(Dev.AIList[0].ValueFinal);
                tempPressSet = Dev.TCtlAimTest;
                tempPressAim = tempPressSet;
                if (tempPressAim > tempPressSet)
                    tempPressAim = tempPressSet;
                tempErr = tempPressAim - tempValueNow;
                PID_T_BLL.PID_Param = GePIDParam(tempErr);
                PID_T_BLL.PID_Param.ControllerEnable = true;
                PID_T_BLL.CalculatePID(tempErr);
                //AO输出
                //软启动
                double aoOutMax = SoftBoot();
                double aoOutShould = PID_T_BLL.UK;
                if (aoOutShould >= aoOutMax)
                    aoOutShould = aoOutMax;
                Dev.AOList[0].ValueFinal = aoOutShould;
                Dev.AOList[0].CalcAODatas();

                //炉内温度自动均值调零
                AutoAverage();
                //炉壁温度自动均值调零
                AutoZeroWallCal();
            }
            else
            {
                Dev.RunMode = RunMode.Wait_Mode;
                Dev.IsBusy = false;

                BllRst();
            }
        }


        #region 辅助方法

        /// <summary>
        /// 记录当前炉壁校准点数值
        /// </summary>
        private void RecWallCalPoint()
        {
            WallCalInitRec newRec = new WallCalInitRec();

            newRec.ExpNO = WallCalDQ.ExpNO;
            newRec.HeightNO = WallCalDQ.HeightPointDQ.LevelNO;
            newRec.Height = WallCalDQ.HeightPointDQ.H;
            newRec.RecTime = DateTime.Now;
            newRec.RecNum = WallCalDQ.HeightPointDQ.RecPointNow;
            newRec.T1 = Dev.AIList[5].ValueFinal;
            newRec.T2 = Dev.AIList[6].ValueFinal;
            newRec.T3 = Dev.AIList[7].ValueFinal;
            newRec.Tf1 = Dev.AIList[0].ValueFinal;
            newRec.Tf2 = Dev.AIList[1].ValueFinal;
            newRec.Vo = Dev.AOList[0].ValueFinal;
            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
               WallCalDQ.HeightPointDQ.RecList_WallCal.Add(newRec);
            }
            ));
            Thread.Sleep(30);

            //保存试验数据记录
            Messenger.Default.Send<WallCalInitRec>(newRec, "SaveWallCalInitRecMessage");
        }

        #endregion


        #region 自动调零

        /// <summary>
        /// 炉壁校准已自动调零标志
        /// </summary>
        private bool _isZeroedWallCal = false;
        /// <summary>
        /// 炉壁校准已自动调零标志
        /// </summary>
        private bool IsZeroedWallCal
        {
            get { return _isZeroedWallCal; }
            set
            {
                _isZeroedWallCal = value;
                RaisePropertyChanged(() => IsZeroedWallCal);
            }
        }

        /// <summary>
        /// 平均范围计时
        /// </summary>
        private Time _zeroWallCalTime = new Time();
        /// <summary>
        /// 平均范围计时开始标志
        /// </summary>
        public Time ZeroWallCalTime
        {
            get { return _zeroWallCalTime; }
            set
            {
                _zeroWallCalTime = value;
                RaisePropertyChanged(() => AvergeTime);
            }
        }

        /// <summary>
        /// 炉壁标定传感器自动平均（炉内温度>740且输出电压大于40%）
        /// </summary>
        private void AutoZeroWallCal()
        {
            if (IsZeroedWallCal)
                return;

            if ((Dev.AIList[0].ValueFinal >= 740) && (Dev.AIList[1].ValueFinal >= 740) && (Dev.AOList[0].ValueFinal >= 40))
            {
                if (!ZeroWallCalTime.IsStarted)
                {
                    ZeroWallCalTime.IsStarted = true;
                    ZeroWallCalTime.StartTime = DateTime.Now;
                }

                TimeSpan tempSpan = DateTime.Now - ZeroWallCalTime.StartTime;
                if (tempSpan.TotalSeconds >= 60)  //均值调零
                {
                    Dev.AIList[5].ZeroCalValue = ((Dev.AIList[5].ValueCaledNonZero + Dev.AIList[6].ValueCaledNonZero + Dev.AIList[7].ValueCaledNonZero) / 3 - Dev.AIList[5].ValueCaledNonZero) * Dev.AIList[5].KCalValue;
                    Dev.AIList[6].ZeroCalValue = ((Dev.AIList[5].ValueCaledNonZero + Dev.AIList[6].ValueCaledNonZero + Dev.AIList[7].ValueCaledNonZero) / 3 - Dev.AIList[6].ValueCaledNonZero) * Dev.AIList[6].KCalValue;
                    Dev.AIList[7].ZeroCalValue = ((Dev.AIList[5].ValueCaledNonZero + Dev.AIList[6].ValueCaledNonZero + Dev.AIList[7].ValueCaledNonZero) / 3 - Dev.AIList[7].ValueCaledNonZero) * Dev.AIList[7].KCalValue;
                    
                    IsZeroedWallCal = true;
                }
            }
            else
            {
                ZeroWallCalTime.Reset();
            }
        }

        #endregion


        #region 消息处理

        /// <summary>
        /// 测试试验消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void StartWallCalMessage(int msg)
        {
            //开始试验
            if (msg == 1)
            {
                WallCalDQ.HeightPointDQ.IsReced = false;

                WallCalDQ.WallCalDataReset(1);
                WallCalDQ.WallCalDataReset(2);
                WallCalDQ.WallCalDataReset(3);
                WallCalDQ.IsCompleted = false;

                //计算数据
                Messenger.Default.Send<string>("WallCalData", "CalcCalDataMessage");
                Thread.Sleep(50);
                //保存试验数据
                Messenger.Default.Send<string>("SaveWallCal", "SaveWallCalMessage");
                Thread.Sleep(100);

                //控温稳定数据重置、设定
                Dev.T1StbBalanceEst.DataReset();
                Dev.T1StbBalanceEst.ValueAim = Dev.TCtlAimTest;
                Dev.T1StbBalanceEst.EPermit = Dev.TCtlErrPermit;
                Dev.T1StbBalanceEst.DeviationPermit = Dev.TCtlDeviationPermit;
                Dev.T1StbBalanceEst.CountLimit = Dev.Time_Stabilize * 1000 / Dev.Period_BLL;
                Dev.T1StbBalanceEst.DriftPermit = Dev.TCtlDriftPermit;
                Dev.T1StbBalanceEst.TimeEqu = Dev.Time_Stabilize;
                Dev.T2StbBalanceEst.DataReset();
                Dev.T2StbBalanceEst.CountLimit = Dev.Time_Stabilize * 1000 / Dev.Period_BLL;
                Dev.T2StbBalanceEst.ValueAim = Dev.TCtlAimTest;
                Dev.T2StbBalanceEst.EPermit = Dev.TCtlErrPermit;
                Dev.T2StbBalanceEst.DeviationPermit = Dev.TCtlDeviationPermit;
                Dev.T2StbBalanceEst.DriftPermit = Dev.TCtlDriftPermit;
                Dev.T2StbBalanceEst.TimeEqu = Dev.Time_Stabilize;

                PID_T_BLL.Reset();
                ExistWallCalCMD = true;
                StopCMD = false;
                Dev.RunMode = RunMode.WallCal_Mode;
                Dev.IsBusy = true;
            }

            //当前点准备就绪
            if (msg == 2)
            {
                WallCalDQ.WallCalDataReset(WallCalDQ.HeightNoDQ);
                WallCalDQ.IsCompleted = false;
                
                //计算数据
                Messenger.Default.Send<string>("WallCalData", "CalcCalDataMessage");
                Thread.Sleep(50);

                //保存试验数据
                Messenger.Default.Send<string>("SaveWallCal", "SaveWallCalMessage");
                Thread.Sleep(100);

                //平衡数据重置、设定
                Dev.T1StbBalanceEst.DataReset();
                Dev.T1StbBalanceEst.ValueAim = Dev.TCtlAimTest;
                Dev.T1StbBalanceEst.EPermit = Dev.TCtlErrPermit;
                Dev.T1StbBalanceEst.DeviationPermit = Dev.TCtlDeviationPermit;
                Dev.T1StbBalanceEst.CountLimit = Dev.Time_Stabilize * 1000 / Dev.Period_BLL;
                Dev.T1StbBalanceEst.DriftPermit = Dev.TCtlDriftPermit;
                Dev.T1StbBalanceEst.TimeEqu = Dev.Time_Stabilize;
                Dev.T2StbBalanceEst.DataReset();
                Dev.T2StbBalanceEst.CountLimit = Dev.Time_Stabilize * 1000 / Dev.Period_BLL;
                Dev.T2StbBalanceEst.ValueAim = Dev.TCtlAimTest;
                Dev.T2StbBalanceEst.EPermit = Dev.TCtlErrPermit;
                Dev.T2StbBalanceEst.DeviationPermit = Dev.TCtlDeviationPermit;
                Dev.T2StbBalanceEst.DriftPermit = Dev.TCtlDriftPermit;
                Dev.T2StbBalanceEst.TimeEqu = Dev.Time_Stabilize;
                Thread.Sleep(30);

                WallCalDQ.HeightPointDQ.TimePoint.IsStarted = true;
                WallCalDQ.HeightPointDQ.TimePoint.StartTime = DateTime.Now;

                WallCalDQ.IsTesting = true;

                //清空原始数据记录
                Messenger.Default.Send<int>(WallCalDQ.HeightPointDQ.LevelNO, "WallCalInitRecClearMessage");
            }

            //当前点准备停止
            if (msg == 4)
            {
                WallCalDQ.WallCalDataReset(WallCalDQ.HeightNoDQ);
                WallCalDQ.IsCompleted = false;
                WallCalDQ.HeightPointDQ.TimePoint.IsStarted = false;
                WallCalDQ.HeightPointDQ.TimePoint.StartTime = DateTime.MinValue;
                WallCalDQ.HeightPointDQ.TimePoint.IsEnded = false;
                WallCalDQ.HeightPointDQ.TimePoint.EndTime = DateTime.MinValue;
                WallCalDQ.IsTesting = false;

                //计算数据
                Messenger.Default.Send<string>("WallCalData", "CalcCalDataMessage");
                Thread.Sleep(50);

                //保存试验数据
                Messenger.Default.Send<string>("SaveWallCal", "SaveWallCalMessage");
                Thread.Sleep(100);

                //平衡数据重置、设定
                Dev.T1StbBalanceEst.DataReset();
                Dev.T1StbBalanceEst.ValueAim = Dev.TCtlAimTest;
                Dev.T1StbBalanceEst.EPermit = Dev.TCtlErrPermit;
                Dev.T1StbBalanceEst.DeviationPermit = Dev.TCtlDeviationPermit;
                Dev.T1StbBalanceEst.CountLimit = Dev.Time_Stabilize * 1000 / Dev.Period_BLL;
                Dev.T1StbBalanceEst.DriftPermit = Dev.TCtlDriftPermit;
                Dev.T1StbBalanceEst.TimeEqu = Dev.Time_Stabilize;
                Dev.T2StbBalanceEst.DataReset();
                Dev.T2StbBalanceEst.CountLimit = Dev.Time_Stabilize * 1000 / Dev.Period_BLL;
                Dev.T2StbBalanceEst.ValueAim = Dev.TCtlAimTest;
                Dev.T2StbBalanceEst.EPermit = Dev.TCtlErrPermit;
                Dev.T2StbBalanceEst.DeviationPermit = Dev.TCtlDeviationPermit;
                Dev.T2StbBalanceEst.DriftPermit = Dev.TCtlDriftPermit;
                Dev.T2StbBalanceEst.TimeEqu = Dev.Time_Stabilize;
                Thread.Sleep(30);

                //清空原始数据记录
                Messenger.Default.Send<int>(WallCalDQ.HeightPointDQ.LevelNO, "WallCalInitRecClearMessage");
            }

            //记录当前点数据
            if (msg == 3)
            {
                if (Dev.IsBusy && WallCalDQ.HeightPointDQ.TimePoint.IsStarted && Dev.T1StbBalanceEst.IsFitAll)
                {
                    if ((Dev.IsStd2023 && Dev.T2StbBalanceEst.IsFitAll) || Dev.IsStd2010)
                    {
                        //记录数据，修改完成标志量
                        WallCalDQ.HeightPointDQ.IsReced = true;
                        WallCalDQ.HeightPointDQ.RecTime = DateTime.Now;
                        WallCalDQ.HeightPointDQ.RecTime = DateTime.Now;
                        WallCalDQ.HeightPointDQ.T1 = Dev.AIList[5].ValueFinal;
                        WallCalDQ.HeightPointDQ.T2 = Dev.AIList[6].ValueFinal;
                        WallCalDQ.HeightPointDQ.T3 = Dev.AIList[7].ValueFinal;
                        WallCalDQ.HeightPointDQ.Tf1 = Dev.AIList[0].ValueFinal;
                        WallCalDQ.HeightPointDQ.Tf2 = Dev.AIList[1].ValueFinal;

                        WallCalDQ.HeightPointDQ.TimePoint.IsEnded = true;
                        WallCalDQ.HeightPointDQ.TimePoint.EndTime = DateTime.Now;
                        WallCalDQ.HeightPointDQ.TimePoint.IsStarted = false;
                        WallCalDQ.IsTesting = false;

                        //更新全部完成标志
                        bool allCompleted = true;
                        for (int i = 0; i < WallCalDQ.WallCalPointsList.Count; i++)
                        {
                            if (!WallCalDQ.WallCalPointsList[i].IsReced)
                                allCompleted = false;
                        }
                        WallCalDQ.IsCompleted = allCompleted;
                        //所有点是否均完成
                        if (WallCalDQ.IsCompleted)
                            ExistWallCalCMD = false;

                        //计算数据
                        Messenger.Default.Send<string>("WallCalData", "CalcCalDataMessage");
                        Thread.Sleep(50);

                        RaisePropertyChanged(() => WallCalDQ.QtyComplete);
                        RaisePropertyChanged(() => WallCalDQ.QtyLeft);

                        //保存试验数据
                        Messenger.Default.Send<string>("SaveWallCal", "SaveWallCalMessage");
                        Thread.Sleep(100);

                        //平衡数据重置、设定
                        Dev.T1StbBalanceEst.DataReset();
                        Dev.T1StbBalanceEst.ValueAim = Dev.TCtlAimTest;
                        Dev.T1StbBalanceEst.EPermit = Dev.TCtlErrPermit;
                        Dev.T1StbBalanceEst.DeviationPermit = Dev.TCtlDeviationPermit;
                        Dev.T1StbBalanceEst.CountLimit = Dev.Time_Stabilize * 1000 / Dev.Period_BLL;
                        Dev.T1StbBalanceEst.DriftPermit = Dev.TCtlDriftPermit;
                        Dev.T1StbBalanceEst.TimeEqu = Dev.Time_Stabilize;
                        Dev.T2StbBalanceEst.DataReset();
                        Dev.T2StbBalanceEst.CountLimit = Dev.Time_Stabilize * 1000 / Dev.Period_BLL;
                        Dev.T2StbBalanceEst.ValueAim = Dev.TCtlAimTest;
                        Dev.T2StbBalanceEst.EPermit = Dev.TCtlErrPermit;
                        Dev.T2StbBalanceEst.DeviationPermit = Dev.TCtlDeviationPermit;
                        Dev.T2StbBalanceEst.DriftPermit = Dev.TCtlDriftPermit;
                        Dev.T2StbBalanceEst.TimeEqu = Dev.Time_Stabilize;
                        Thread.Sleep(30);
                    }
                }
            }
        }

        #endregion
    }
}
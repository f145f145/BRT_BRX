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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using static BRX.Model.Enums.Enums;

namespace BRX.BLL
{
    public partial class Bll : ObservableObject
    {
        /// <summary>
        /// 炉内校准检测事务
        /// </summary>
        private void CenterCalBLLAsync()
        {
            if (Dev.RunMode != RunMode.CenterCal_Mode)
                return;

            if (ExistCenterCalCMD)
            {
                if (CenterCalDQ.HeightPointDQ.TimePoint.IsStarted&& !CenterCalDQ.HeightPointDQ.IsReced)
                {
                    TimeSpan spanFroSpStart = DateTime.Now - CenterCalDQ.HeightPointDQ.TimePoint.StartTime;
                    CenterCalDQ.HeightPointDQ.TimePoint.TimeFromStart = (int)spanFroSpStart.TotalSeconds;

                    //判断平衡数据插入
                    Dev.T1StbBalanceEst.ADD(Dev.AIList[0].ValueFinal);
                    Dev.T2StbBalanceEst.ADD(Dev.AIList[1].ValueFinal);
                    //分析控温数据是否满足条件
                    Dev.T1StbBalanceEst.Evaluate();
                    Dev.T2StbBalanceEst.Evaluate();

                    //记录原始数据
                    RecCenterCalPoint();
                    CenterCalDQ.HeightPointDQ.RecPointNow++;
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
        /// 记录当前炉内校准点数值
        /// </summary>
        private void RecCenterCalPoint()
        {
            CenterCalInitRec newRec = new CenterCalInitRec();

            newRec.ExpNO = CenterCalDQ.ExpNO;
            newRec.HeightNO = CenterCalDQ.HeightPointDQ.PointNO;
            newRec.Height = CenterCalDQ.HeightPointDQ.PointH;
            newRec.RecTime = DateTime.Now;
            newRec.RecNum = CenterCalDQ.HeightPointDQ.RecPointNow;
            newRec.Tc = Dev.AIList[4].ValueFinal;
            newRec.Tf1 = Dev.AIList[0].ValueFinal;
            newRec.Tf2 = Dev.AIList[1].ValueFinal;
            newRec.Vo = Dev.AOList[0].ValueFinal;
            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
               CenterCalDQ.HeightPointDQ.RecList_CenterCal.Add(newRec);
            }
            ));
            Thread.Sleep(30);

            //保存试验数据记录
            Messenger.Default.Send<CenterCalInitRec>(newRec, "SaveCenterCalInitRecMessage");
        }

        #endregion
        

        #region 消息处理

        /// <summary>
        /// 炉内校准试验消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void StartCenterCalMessage(int msg)
        {
            //开始试验
            if (msg == 1)
            {
                CenterCalDQ.HeightPointDQ.IsReced = false;

                for (int i = 0; i < CenterCalDQ.CenterCalPointsList.Count; i++)
                {
                    CenterCalDQ.CenterCalPointsList[i].DataReset();
                }
                CenterCalDQ.IsCompleted = false;

                //计算数据
                Messenger.Default.Send<string>("CenterCalData", "CalcCalDataMessage");
                Thread.Sleep(50);
                //保存试验数据
                Messenger.Default.Send<string>("SaveCenterCal", "SaveCenterCalMessage");
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
                ExistCenterCalCMD = true;
                StopCMD = false;
                Dev.RunMode = RunMode.CenterCal_Mode;
                Dev.IsBusy = true;
            }

            //当前点准备就绪
            if (msg == 2)
            {
                CenterCalDQ.CenterCalDataReset(CenterCalDQ.HeightNoDQ);
                CenterCalDQ.IsCompleted = false;
                
                //计算数据
                Messenger.Default.Send<string>("CenterCalData", "CalcCalDataMessage");
                Thread.Sleep(50);

                //保存试验数据
                Messenger.Default.Send<string>("SaveCenterCal", "SaveCenterCalMessage");
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

                CenterCalDQ.HeightPointDQ.TimePoint.IsStarted = true;
                CenterCalDQ.HeightPointDQ.TimePoint.StartTime = DateTime.Now;

                CenterCalDQ.IsTesting = true;

                //清空原始数据记录
                Messenger.Default.Send<int>(CenterCalDQ.HeightPointDQ.PointNO, "CenterCalInitRecClearMessage");
            }

            //当前点准备停止
            if (msg == 4)
            {
                CenterCalDQ.CenterCalDataReset(CenterCalDQ.HeightNoDQ);
                CenterCalDQ.IsCompleted = false;
                CenterCalDQ.HeightPointDQ.TimePoint.IsStarted = false;
                CenterCalDQ.HeightPointDQ.TimePoint.StartTime = DateTime.MinValue;
                CenterCalDQ.HeightPointDQ.TimePoint.IsEnded = false;
                CenterCalDQ.HeightPointDQ.TimePoint.EndTime = DateTime.MinValue;
                CenterCalDQ.IsTesting = false;

                //计算数据
                Messenger.Default.Send<string>("CenterCalData", "CalcCalDataMessage");
                Thread.Sleep(50);

                //保存试验数据
                Messenger.Default.Send<string>("SaveCenterCal", "SaveCenterCalMessage");
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
                Messenger.Default.Send<int>(CenterCalDQ.HeightPointDQ.PointNO, "CenterCalInitRecClearMessage");
            }

            //记录当前点数据
            if (msg == 3)
            {
                if (Dev.IsBusy && CenterCalDQ.HeightPointDQ.TimePoint.IsStarted && Dev.T1StbBalanceEst.IsFitAll)
                {
                    if ((Dev.IsStd2023 && Dev.T2StbBalanceEst.IsFitAll) || Dev.IsStd2010)
                    {
                        //记录数据，修改完成标志量
                        CenterCalDQ.HeightPointDQ.IsReced = true;
                        CenterCalDQ.HeightPointDQ.RecTime = DateTime.Now;
                        CenterCalDQ.HeightPointDQ.RecTime = DateTime.Now;
                        CenterCalDQ.HeightPointDQ.Tfc = Dev.AIList[4].ValueFinal;
                        CenterCalDQ.HeightPointDQ.Tf1 = Dev.AIList[0].ValueFinal;
                        CenterCalDQ.HeightPointDQ.Tf2 = Dev.AIList[1].ValueFinal;

                        CenterCalDQ.HeightPointDQ.TimePoint.IsEnded = true;
                        CenterCalDQ.HeightPointDQ.TimePoint.EndTime = DateTime.Now;
                        CenterCalDQ.HeightPointDQ.TimePoint.IsStarted = false;
                        CenterCalDQ.IsTesting = false;

                        //更新全部完成标志
                        bool allCompleted = true;
                        for (int i = 0; i < CenterCalDQ.CenterCalPointsList.Count; i++)
                        {
                            if (!CenterCalDQ.CenterCalPointsList[i].IsReced)
                                allCompleted = false;
                        }
                        CenterCalDQ.IsCompleted = allCompleted;
                        //所有点是否均完成
                        if (CenterCalDQ.IsCompleted)
                            ExistCenterCalCMD = false;

                        //计算数据
                        Messenger.Default.Send<string>("CenterCalData", "CalcCalDataMessage");
                        Thread.Sleep(50);

                        //保存试验数据
                        Messenger.Default.Send<string>("SaveCenterCal", "SaveCenterCalMessage");
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
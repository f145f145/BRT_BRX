/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
  * 。BLL，试验检测
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/18 16:22:39		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using BRX.Model.Exp;
using static BRX.Model.Enums.Enums;
using System.Windows;

namespace BRX.BLL
{
    public partial class Bll : ObservableObject
    {
        /// <summary>
        /// 测试试验事务
        /// </summary>
        private void ExpBLLAsync()
        {
            if (Dev.RunMode != RunMode.Exp_BRXMode)
                return;

            if (ExistTestCMD)
            {
                try
                {
                    if (!ExpBRXDQ.SpDQ.IsCompleted)
                    {
                        try
                        {
                            if (!ExpBRXDQ.SpDQ.TimeSp.IsStarted)
                            {
                                ExpStartDataSet();
                                Thread.Sleep(500);

                                ExpBRXDQ.SpDQ.TimeSp.IsStarted = true;
                                ExpBRXDQ.SpDQ.TimeSp.StartTime = DateTime.Now;
                            }

                            TimeSpan spanFroSpStart = DateTime.Now - ExpBRXDQ.SpDQ.TimeSp.StartTime;
                            ExpBRXDQ.SpDQ.TimeSp.TimeFromStart = spanFroSpStart.TotalSeconds;

                            //判断平衡数据插入
                            Dev.T1StbBalanceEst.ADDXY(ExpBRXDQ.SpDQ.RecPointNow, Dev.AIList[0].ValueFinal);
                            Dev.T2StbBalanceEst.ADDXY(ExpBRXDQ.SpDQ.RecPointNow, Dev.AIList[1].ValueFinal);
                            Thread.Sleep(50);

                            //加热控温阶段
                            if (!ExpBRXDQ.SpDQ.TimeTest30.IsStarted)
                            {
                                try
                                {
                                    ExpBRXDQ.SpDQ.StageDQ = TestStage.TCtl_Stage;
                                    //分析是否达到稳定
                                    if ((DateTime.Now - ExpBRXDQ.SpDQ.TimeBefore_StabilizeEval).TotalSeconds >= Dev.Period_Stabilize)
                                    {
                                        ExpBRXDQ.SpDQ.TimeBefore_StabilizeEval = DateTime.Now;
                                        //分析控温数据是否满足条件
                                        Dev.T1StbBalanceEst.Evaluate();
                                        Dev.T2StbBalanceEst.Evaluate();
                                    }

                                    //控温计算
                                    double tempTNow;        //当前值
                                    double tempTSet;        //设定值
                                    double tempTErr;        //PID计算用误差
                                    double tempPIDTout;     //控温PID输出
                                    double aoOutShould;

                                    if (Dev.IsStd2023)
                                        tempTNow = Math.Abs((Dev.AIList[0].ValueFinal + Dev.AIList[1].ValueFinal) / 2);
                                    else
                                        tempTNow = Math.Abs(Dev.AIList[0].ValueFinal);

                                    //第一次达到设定值前全功率加热，达到后延时启动PID控温，连续试验时的后续试验始终PID控温
                                    if (!Dev.TDelayTime.IsEnded)
                                    {
                                        if (!Dev.TDelayTime.IsStarted)
                                        {
                                            if (tempTNow >= Dev.TStartPID)
                                            {
                                                Dev.TDelayTime.IsStarted = true;
                                                Dev.TDelayTime.StartTime = DateTime.Now;

                                                if (Dev.WithPowerSenser)
                                                {
                                                    PID_Power_BLL.Sum_UKi = Dev.TDelayU;
                                                    PID_T_BLL.Sum_UKi = Dev.TDelayP;
                                                }
                                                else
                                                    PID_T_BLL.Sum_UKi = Dev.TDelayU;
                                            }
                                            aoOutShould = Dev.RatioAoOutMax;
                                        }
                                        else
                                        {
                                            TimeSpan tDalaySpan = DateTime.Now - Dev.TDelayTime.StartTime;
                                            if (tDalaySpan.TotalSeconds >= Dev.DelayTime)
                                            {
                                                Dev.TDelayTime.IsEnded = true;

                                                if (Dev.WithPowerSenser)
                                                {
                                                    PID_Power_BLL.Sum_UKi = Dev.InitSumUI_U;
                                                    PID_T_BLL.Sum_UKi = Dev.InitSumUI_P;
                                                }
                                                else
                                                    PID_T_BLL.Sum_UKi = Dev.InitSumUI_U;
                                            }
                                            // 功率控制计算
                                            if (Dev.WithPowerSenser)
                                            {
                                                double tempPowerNow;        //当前值
                                                double tempPowerSet;        //设定值
                                                double tempPowerErr;        //PID计算用误差
                                                tempPowerNow = Math.Abs(Dev.AIList[10].ValueFinal);
                                                tempPowerSet = Dev.TDelayP;
                                                tempPowerErr = tempPowerSet - tempPowerNow;
                                                PID_Power_BLL.PID_Param = Dev.PID_Power.PID_Param;
                                                PID_Power_BLL.PID_Param.ControllerEnable = true;
                                                PID_Power_BLL.CalculatePID(tempPowerErr);
                                                // System.Diagnostics.Trace.Write("\r\n Err:" + PID_Power_BLL.ErrK + "  Usum:" + PID_Power_BLL.UK + "\r\n UkP:" + PID_Power_BLL.UK_P + "  UkI:" + PID_Power_BLL.UK_I + "  UkD:" + PID_Power_BLL.UK_D);

                                                aoOutShould = PID_Power_BLL.UK;
                                            }
                                            else
                                                aoOutShould = Dev.TDelayU;
                                        }
                                        //650°以上时降低加热功率
                                        if ((tempTNow >= 650) && (aoOutShould >= 80))
                                            aoOutShould = 65;
                                    }
                                    else
                                    {
                                        tempTSet = Dev.TCtlAimTest;
                                        tempTErr = tempTSet - tempTNow;
                                        PID_T_BLL.PID_Param = GePIDParam(tempTErr);
                                        PID_T_BLL.PID_Param.ControllerEnable = true;
                                        PID_T_BLL.CalculatePID(tempTErr);
                                        tempPIDTout = PID_T_BLL.UK;
                                        //System.Diagnostics.Trace.Write("\r\n Err:" + PID_T_BLL.ErrK + "  Usum:" + PID_T_BLL.UK + "\r\n UkP:" + PID_T_BLL.UK_P + "  UkI:" + PID_T_BLL.UK_I + "  UkD:" + PID_T_BLL.UK_D);

                                        // 功率控制计算
                                        if (Dev.WithPowerSenser)
                                        {
                                            double tempPowerNow;        //当前值
                                            double tempPowerSet;        //设定值
                                            double tempPowerErr;        //PID计算用误差
                                            tempPowerNow = Math.Abs(Dev.AIList[10].ValueFinal);
                                            tempPowerSet = tempPIDTout;
                                            tempPowerErr = tempPowerSet - tempPowerNow;
                                            PID_Power_BLL.PID_Param = Dev.PID_Power.PID_Param;
                                            PID_Power_BLL.PID_Param.ControllerEnable = true;
                                            PID_Power_BLL.CalculatePID(tempPowerErr);
                                            // System.Diagnostics.Trace.Write("\r\n Err:" + PID_Power_BLL.ErrK + "  Usum:" + PID_Power_BLL.UK + "\r\n UkP:" + PID_Power_BLL.UK_P + "  UkI:" + PID_Power_BLL.UK_I + "  UkD:" + PID_Power_BLL.UK_D);

                                            aoOutShould = PID_Power_BLL.UK;
                                        }
                                        else
                                            aoOutShould = tempPIDTout;
                                    }

                                    double aoOutMax = SoftBoot();       //软启动限值

                                    if (aoOutShould >= aoOutMax)
                                        aoOutShould = aoOutMax;
                                    Dev.AOList[0].ValueFinal = aoOutShould;
                                    Dev.AOList[0].CalcAODatas();

                                    //更新最后几分钟输出功率/电压列表
                                    VoValueList.Add(PID_T_BLL.UK);
                                    while (VoValueList.Count > Dev.TimeCalcPower * 1000 / Dev.Period_BLL)
                                        VoValueList.RemoveAt(0);

                                    //自动均值调零
                                    AutoAverage();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                            }
                            //正式测试阶段
                            else
                            {
                                try
                                {
                                    //附加热电偶判断平衡数据插入
                                    Dev.TssStbBalanceEst.ADDXY(ExpBRXDQ.SpDQ.RecPointNow, Dev.AIList[2].ValueFinal);
                                    Dev.TscStbBalanceEst.ADDXY(ExpBRXDQ.SpDQ.RecPointNow, Dev.AIList[3].ValueFinal);

                                    if (Test30BeginJust)
                                    {
                                        //平衡数据重置、设定
                                        Dev.T1StbBalanceEst.DataReset();
                                        Dev.T1StbBalanceEst.CountLimit = Dev.Time_FinalEqu * 1000 / Dev.Period_BLL;
                                        Dev.T1StbBalanceEst.DriftPermit = Dev.TFinalEquDriftPermit;
                                        Dev.T1StbBalanceEst.TimeEqu = Dev.Time_FinalEqu;
                                        Dev.T2StbBalanceEst.DataReset();
                                        Dev.T2StbBalanceEst.CountLimit = Dev.Time_FinalEqu * 1000 / Dev.Period_BLL;
                                        Dev.T2StbBalanceEst.DriftPermit = Dev.TFinalEquDriftPermit;
                                        Dev.T2StbBalanceEst.TimeEqu = Dev.Time_FinalEqu;
                                        Dev.T1StbBalanceEst.Evaluate();
                                        Dev.T2StbBalanceEst.Evaluate();
                                        Thread.Sleep(50);
                                        Test30BeginJust = false;

                                        var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            Messenger.Default.Send<string>("已进入30min测试阶段，请尽快将试样放入炉内!!!", "PopMessage");
                                        }));
                                        Thread.Sleep(5000); //延时5秒
                                    }

                                    ExpBRXDQ.SpDQ.StageDQ = TestStage.Test_Stage;
                                    //计算正式测试开始以后的时间
                                    TimeSpan spanFroTest30Start = DateTime.Now - ExpBRXDQ.SpDQ.TimeTest30.StartTime;
                                    ExpBRXDQ.SpDQ.TimeTest30.TimeFromStart = spanFroTest30Start.TotalSeconds;

                                    //到达最长测试时长（60min）
                                    if (ExpBRXDQ.SpDQ.TimeTest30.TimeFromStart >= Dev.Time_FinalEquEvalMax)
                                    {
                                        ExpBRXDQ.SpDQ.IsCompleted = true;
                                        ExpBRXDQ.SpDQ.StageDQ = TestStage.TestEnd_Stage;
                                    }
                                    //测试30min以后
                                    else if (ExpBRXDQ.SpDQ.TimeTest30.TimeFromStart >= Dev.Time_FinalEquEvalFist)
                                    {
                                        if ((DateTime.Now - ExpBRXDQ.SpDQ.TimeBefore_FinalEquEval).TotalSeconds >= Dev.Period_FinalEqu)
                                        {
                                            ExpBRXDQ.SpDQ.TimeBefore_FinalEquEval = DateTime.Now;
                                            //分析控温数据是否满足条件
                                            Dev.T1StbBalanceEst.Evaluate();
                                            Dev.T2StbBalanceEst.Evaluate();
                                            Dev.TssStbBalanceEst.Evaluate();
                                            Dev.TscStbBalanceEst.Evaluate();
                                            if (Dev.T1StbBalanceEst.IsDriftMeetsReqs)
                                            {
                                                if (((Dev.IsStd2023 && Dev.T2StbBalanceEst.IsDriftMeetsReqs) || Dev.IsStd2010)&&((!ExpBRXDQ.UseAddT)||(ExpBRXDQ.UseAddT && Dev.TssStbBalanceEst.IsDriftMeetsReqs && Dev.TscStbBalanceEst.IsDriftMeetsReqs)))
                                                {
                                                    ExpBRXDQ.SpDQ.IsCompleted = true;
                                                    ExpBRXDQ.SpDQ.StageDQ = TestStage.TestEnd_Stage;
                                                }
                                            }
                                        }
                                    }


                                    double aoOutShould;
                                    // 功率恒定
                                    if (Dev.WithPowerSenser)
                                    {
                                        double tempPowerNow;        //当前值
                                        double tempPowerSet;        //设定值
                                        double tempPowerErr;        //PID计算用误差
                                        tempPowerNow = Math.Abs(Dev.AIList[10].ValueFinal);
                                        tempPowerSet = ExpBRXDQ.SpDQ.VoFinal - Dev.OutReduceP;
                                        tempPowerErr = tempPowerSet - tempPowerNow;
                                        PID_Power_BLL.PID_Param = Dev.PID_Power.PID_Param;
                                        PID_Power_BLL.PID_Param.ControllerEnable = true;
                                        PID_Power_BLL.CalculatePID(tempPowerErr);
                                        //   System.Diagnostics.Trace.Write("\r\n Err:" + PID_Power_BLL.ErrK + "  Usum:" + PID_Power_BLL.UK + "\r\n UkP:" + PID_Power_BLL.UK_P + "  UkI:" + PID_Power_BLL.UK_I + "  UkD:" + PID_Power_BLL.UK_D);

                                        aoOutShould = PID_Power_BLL.UK;
                                    }
                                    else
                                        aoOutShould = ExpBRXDQ.SpDQ.VoFinal - Dev.OutReduceU;

                                    //输出控温期间的最终值
                                    double aoOutMax = SoftBoot();       //软启动限值
                                    if (aoOutShould >= aoOutMax)
                                        aoOutShould = aoOutMax;
                                    Dev.AOList[0].ValueFinal = aoOutShould;
                                    Dev.AOList[0].CalcAODatas();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                            }
                            //记录原始数据
                            RecPoint();
                            ExpBRXDQ.SpDQ.RecPointNow++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            ExpBRXDQ.SpDQ.CopyToRecListStb();
                            ExpBRXDQ.SpDQ.CopyToRecListTest();

                            //计算数据
                            Messenger.Default.Send<string>("ExpBRXData", "CalcExpDataMessage");
                            Thread.Sleep(200);
                            //保存试验数据
                            Messenger.Default.Send<string>("SaveDataExpBRX", "SaveExpBRXMessage");
                            Thread.Sleep(200);

                            //更新试验列表
                            Messenger.Default.Send<string>("UpdateExpListMessage", "UpdateExpListMessage");

                            int next = ExpEndJudge();     //判断是直接结束还是进行下一个试样检测
                            //无需自动进行下一个试样，或所有试样均已完成时，结束实验
                            if ((next < 1) || (next > ExpBRXDQ.SpList.Count))
                            {
                                ExistTestCMD = false;
                                Dev.RunMode = RunMode.Wait_Mode;
                                Dev.IsBusy = false;

                                BllRst();
                            }
                            //若参数设定为自动进行下一个试样检测，且有试样未完成
                            else
                            {
                                Messenger.Default.Send<int>(next, "NextSpMessage");     //切换至下一个试样
                                Thread.Sleep(500);

                                ExpBRXDQ.SpDQ.IsCompleted = false;
                                ExpBRXDQ.SpDQ.TimeSp.IsStarted = false;

                                TestStarted = false;
                                ExistTestCMD = true;
                                StopCMD = false;
                                Dev.RunMode = RunMode.Exp_BRXMode;
                                Dev.IsBusy = true;
                            }

                            //弹窗提示，播放语音提示
                            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                Messenger.Default.Send<int>(next, "ExpEndedMessage");
                            }));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        #region 辅助方法

        /// <summary>
        /// 记录当前数值
        /// </summary>
        private void RecPoint()
        {
            double power;
            if (Dev.WithPowerSenser)
                power = Dev.AIList[10].ValueFinal;
            else
                power = Dev.AOList[0].ValueFinal;

            TestRec newRec = new TestRec
            {
                IsRecorded = true,
                No = ExpBRXDQ.SpDQ.RecPointNow,
                RecTime = DateTime.Now,
                Stage = ExpBRXDQ.SpDQ.StageDQ,
                T1 = Dev.AIList[0].ValueFinal,
                T2 = Dev.AIList[1].ValueFinal,
                Tsc = Dev.AIList[2].ValueFinal,
                Tss = Dev.AIList[3].ValueFinal,
                Vo = power,
                IsFired = Dev.IsFired
            };

            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ExpBRXDQ.SpDQ.RecList_All.Add(newRec);
            }
            ));
            Thread.Sleep(50);

            //保存试验数据记录
            Messenger.Default.Send<TestRec>(newRec, "SaveExpInitRecMessage");
        }


        /// <summary>
        /// 试验开始时数据设定、重置
        /// </summary>
        private void ExpStartDataSet()
        {
            VoValueList = new List<double>();
            var task = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ExpBRXDQ.SpDQ.DataReset();
            }
            ));
            task.Completed += new EventHandler(Task_Completed);
            //清空原始数据记录，保存复位后的数据
            Messenger.Default.Send<int>(ExpBRXDQ.SpDQ.SpNO, "SpInitRecClearMessage");
            Messenger.Default.Send<string>("SaveDataExpBRX", "SaveExpBRXMessage");

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
            Dev.T1StbBalanceEst.Evaluate();
            Dev.T2StbBalanceEst.Evaluate();
            //试样温度平衡判断
            Dev.TssStbBalanceEst.DataReset();
            Dev.TssStbBalanceEst.ValueAim = Dev.TCtlAimTest;
            Dev.TssStbBalanceEst.EPermit = Dev.TCtlErrPermit;
            Dev.TssStbBalanceEst.DeviationPermit = Dev.TCtlDeviationPermit;
            Dev.TssStbBalanceEst.CountLimit = Dev.Time_Stabilize * 1000 / Dev.Period_BLL;
            Dev.TssStbBalanceEst.DriftPermit = Dev.TCtlDriftPermit;
            Dev.TssStbBalanceEst.TimeEqu = Dev.Time_Stabilize;
            Dev.TscStbBalanceEst.DataReset();
            Dev.TscStbBalanceEst.ValueAim = Dev.TCtlAimTest;
            Dev.TscStbBalanceEst.EPermit = Dev.TCtlErrPermit;
            Dev.TscStbBalanceEst.DeviationPermit = Dev.TCtlDeviationPermit;
            Dev.TscStbBalanceEst.CountLimit = Dev.Time_Stabilize * 1000 / Dev.Period_BLL;
            Dev.TscStbBalanceEst.DriftPermit = Dev.TCtlDriftPermit;
            Dev.TscStbBalanceEst.TimeEqu = Dev.Time_Stabilize;
            Dev.TssStbBalanceEst.Evaluate();
            Dev.TscStbBalanceEst.Evaluate();
            //第一次达到740以上
            Dev.Reach735First = false;
        }


        /// <summary>
        /// 判定试验是否结束
        /// </summary>
        private int ExpEndJudge()
        {
            if (Dev.IsAutoContinue)
            {
                for(int i=0;i<ExpBRXDQ.SpList.Count;i++)
                {
                    if (!ExpBRXDQ.SpList[i].IsCompleted)
                        return i + 1;
                }
                return -1;  //所有试样均已完成
            }
            else
                return -1;  //无需自动进行
        }

        #endregion


        #region 消息处理

        /// <summary>
        /// 测试试验消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void StartTestMessage(int msg)
        {
            //开始试验
            if (msg == 1)
            {
                ExpBRXDQ.IsCompleted = false;
                RaisePropertyChanged(() => ExpBRXDQ.QtyComplete);



                ExpBRXDQ.SpDQ.IsCompleted = false;
                ExpBRXDQ.SpDQ.TimeSp.IsStarted = false;

                //保存试验数据
                Messenger.Default.Send<string>("SaveDataExpBRX", "SaveExpBRXMessage");
                Thread.Sleep(200);
                Messenger.Default.Send<string>("UpdateExpListMessage", "UpdateExpListMessage");

                PID_T_BLL.Reset();

                TestStarted = false;
                ExistTestCMD = true;
                StopCMD = false;
                Dev.RunMode = RunMode.Exp_BRXMode;
                Dev.IsBusy = true;
            }
            //放入试样
            if (msg == 2)
            {
                if (Dev.IsBusy && ExpBRXDQ.SpDQ.TimeSp.IsStarted && (!ExpBRXDQ.SpDQ.TimeTest30.IsStarted) && (!ExpBRXDQ.SpDQ.IsCompleted) && Dev.T1StbBalanceEst.IsFitAll)
                {
                    if ((Dev.IsStd2023 && Dev.T2StbBalanceEst.IsFitAll) || Dev.IsStd2010)
                    {
                        Test30BeginJust = true;
                        ExpBRXDQ.SpDQ.TimeTest30.IsStarted = true;
                        ExpBRXDQ.SpDQ.TimeTest30.StartTime = DateTime.Now;

                        if (Dev.AutoPower)  //自动计算恒功率值时，输出电压/功率等于最后几分钟的平均值
                            ExpBRXDQ.SpDQ.VoFinal = VoValueList.Average();
                        else
                        {
                            if (Dev.WithPowerSenser)
                                ExpBRXDQ.SpDQ.VoFinal = Dev.FixedPower_P;
                            else
                                ExpBRXDQ.SpDQ.VoFinal = Dev.FixedPower_U;
                        }

                        //恒功率值
                        ExpBRXDQ.SpDQ.TestPower = ExpBRXDQ.SpDQ.VoFinal;
                        //初始温度
                        ExpBRXDQ.SpDQ.TStart1 = Dev.T1StbBalanceEst.Y_Avg;
                        ExpBRXDQ.SpDQ.TStart2 = Dev.T2StbBalanceEst.Y_Avg;
                    }
                }
            }
        }


        /// <summary>
        /// 有火焰消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void FiredMessage(int msg)
        {
            if (msg == 1)
            {
                Dev.IsFired = true;
            }
            else if (msg == 0)
            {
                Dev.IsFired = false;
            }
        }

        #endregion
    }
}
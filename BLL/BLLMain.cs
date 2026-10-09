/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 。BLL，主体部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/18 16:22:39		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using BRX.Model.Dev;
using BRX.Model.Enums;
using BRX.Model.Exp;
using BRX.View;
using static BRX.Model.Enums.Enums;
using CtrlMethod;
using BRX.Model;

namespace BRX.BLL
{
    public partial class Bll : ObservableObject
    {
        public Bll(DevModel dev, ExpModel_Test exp, ExpModel_CalWall wallCal, ExpModel_CalCenter centerCall)
        {
            Dev = dev;
            ExpBRXDQ = exp;
            WallCalDQ = wallCal;
            CenterCalDQ = centerCall;

            //有窗口新增或关闭
            Messenger.Default.Register<string>(this, "WindowClosed", WindowClosedMessage);
            Messenger.Default.Register<Window>(this, "NewWindowCreated", WindowCreatedMessage);

            //紧急停机指令消息
            Messenger.Default.Register<int>(this, "JJTJMessage", JJTJMessage);
            //停机指令消息
            Messenger.Default.Register<int>(this, "StopMessage", StopMessage);

            //模式切换指令消息
            Messenger.Default.Register<RunMode>(this, "DevModeChangeMessage", DevModeChangeMessage);

            //测试试验指令消息
            Messenger.Default.Register<int>(this, "StartTestMessage", StartTestMessage);
            Messenger.Default.Register<int>(this, "FiredMessage", FiredMessage);

            //校准检测指令消息
            Messenger.Default.Register<int>(this, "StartWallCalMessage", StartWallCalMessage);
            Messenger.Default.Register<int>(this, "StartCenterCalMessage", StartCenterCalMessage);

            //调试指令消息
            Messenger.Default.Register<double>(this, "SDDbgMessage", SDDbgMessage);
            Messenger.Default.Register<double[]>(this, "PIDDbgMessage", PIDDbgMessage);
            Messenger.Default.Register<double[]>(this, "PowerPIDDbgMessage", PowerPIDDbgMessage);

            //系统辨识指令消息
            Messenger.Default.Register<int>(this, "StartIDMessage", StartIDMessage);

            BllInit();

            //主控流程定时器初始化
            BLLTimerInit();
        }


        #region 主流程定时器

        /// <summary>
        /// BLL主定时器
        /// </summary>
        private System.Threading.Timer BLLTimer;

        /// <summary>
        /// PLC读写定时器初始化函数
        /// </summary>
        private void BLLTimerInit()
        {
            BLLTimer = new System.Threading.Timer(new System.Threading.TimerCallback(BLLTimerTick), this, 0, Dev.Period_BLL);
        }

        /// <summary>
        /// BLL主定时器回调函数
        /// </summary>
        /// <param name="state"></param>
        private void BLLTimerTick(object state)
        {
            //BLL锁定，避免多次进入冲突
            lock (this)
            {
                if (IsBllBusy)
                    return;
                IsBllBusy = true;
            }

            //更新当前时间
            Dev.TimeNow = DateTime.Now;

            try
            {
                //紧急停机模式处理
                if (Dev.RunMode == RunMode.JJTJ_Mode)
                {
                    JJTJBLL();
                }

                //模式切换
                else if (LastMode != Dev.RunMode)
                {
                    DOReset();
                    AOReset();
                }
                //模式未变化
                else
                {
                    switch (Dev.RunMode)
                    {
                        //紧急停机模式
                        case RunMode.JJTJ_Mode:
                            JJTJBLL();
                            break;

                        //待机模式
                        case RunMode.Wait_Mode:
                            WaitBLL();
                            break;

                        //手动调试模式
                        case RunMode.SDDbg_Mode:
                            SDDbgBLLAsync();
                            break;

                        //温控PID调试模式
                        case RunMode.TPID_Mode:
                            PIDDbgBLLAsync();
                            break;

                        //功率PID调试模式
                        case RunMode.PowerPID_Mode:
                            PowerPIDDbgBLLAsync();
                            break;

                        //测试试验模式
                        case RunMode.Exp_BRXMode:
                            ExpBLLAsync();
                            break;

                        //炉壁校准试验模式
                        case RunMode.WallCal_Mode:
                            WallCalBLLAsync();
                            break;

                        //炉内校准试验模式
                        case RunMode.CenterCal_Mode:
                            CenterCalBLLAsync();
                            break;

                        //系统辨识模式
                        case RunMode.ID_Mode:
                            IDBLLAsync();
                            break;
                    }
                    //停机状态
                    if (StopCMD)
                    {
                        AOReset();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //更新上循环模式
            LastMode = Dev.RunMode;

            //BLL解锁
            IsBllBusy = false;
        }

        #endregion


        #region 装置、试验等基本属性

        /// <summary>
        /// 装置
        /// </summary>
        private DevModel _dev;
        /// <summary>
        /// 装置
        /// </summary>
        public DevModel Dev
        {
            get { return _dev; }
            set
            {
                _dev = value;
                RaisePropertyChanged(() => Dev);
            }
        }

        /// <summary>
        /// 当前试验
        /// </summary>
        private ExpModel_Test _expBRXDQ;
        /// <summary>
        /// 当前试验
        /// </summary>
        public ExpModel_Test ExpBRXDQ
        {
            get { return _expBRXDQ; }
            set
            {
                _expBRXDQ = value;
                RaisePropertyChanged(() => ExpBRXDQ);
            }
        }

        /// <summary>
        /// 当前炉壁校准试验
        /// </summary>
        private ExpModel_CalWall _wallCalDQ;
        /// <summary>
        /// 当前炉壁校准试验
        /// </summary>
        public ExpModel_CalWall WallCalDQ
        {
            get { return _wallCalDQ; }
            set
            {
                _wallCalDQ = value;
                RaisePropertyChanged(() => WallCalDQ);
            }
        }

        /// <summary>
        /// 当前炉内校准试验
        /// </summary>
        private ExpModel_CalCenter _centerCalDQ;
        /// <summary>
        /// 当前炉内校准试验
        /// </summary>
        public ExpModel_CalCenter CenterCalDQ
        {
            get { return _centerCalDQ; }
            set
            {
                _centerCalDQ = value;
                RaisePropertyChanged(() => CenterCalDQ);
            }
        }

        #endregion


        #region PID控制器

        /// <summary>
        /// 温度PID控制器
        /// </summary>
        private PID_CtrlModel _pid_T_BLL = new PID_CtrlModel() { PID_Param = new PID_ParamModel() { ControllerName = "温度控制PID", ControllerNo = "PID_T_Err15_50" } };
        /// <summary>
        /// 温度PID控制器
        /// </summary>
        public PID_CtrlModel PID_T_BLL
        {
            get { return _pid_T_BLL; }
            set
            {
                _pid_T_BLL = value;
                RaisePropertyChanged(() => PID_T_BLL);
            }
        }

        /// <summary>
        /// 功率PID控制器
        /// </summary>
        private PID_CtrlModel _pid_Power_BLL = new PID_CtrlModel() { PID_Param = new PID_ParamModel() { ControllerName = "功率控制PID", ControllerNo = "PID_Power" } };
        /// <summary>
        /// 功率PID控制器
        /// </summary>
        public PID_CtrlModel PID_Power_BLL
        {
            get { return _pid_Power_BLL; }
            set
            {
                _pid_Power_BLL = value;
                RaisePropertyChanged(() => PID_Power_BLL);
            }
        }

        #endregion


        #region 运行辅助属性

        /// <summary>
        /// 上循环模式
        /// </summary>
        private RunMode _lastMode = RunMode.Wait_Mode;
        /// <summary>
        /// 上循环模式
        /// </summary>
        private RunMode LastMode
        {
            get { return _lastMode; }
            set
            {
                _lastMode = value;
                RaisePropertyChanged(() => LastMode);
            }
        }

        /// <summary>
        /// BLL占用状态
        /// </summary>
        private bool _isBllBusy = false;
        /// <summary>
        /// BLL占用状态
        /// </summary>
        private bool IsBllBusy
        {
            get { return _isBllBusy; }
            set
            {
                _isBllBusy = value;
                RaisePropertyChanged(() => IsBllBusy);
            }
        }

        /// <summary>
        /// 停机指令
        /// </summary>
        private bool _stopCMD = false;
        /// <summary>
        /// 停机指令
        /// </summary>
        private bool StopCMD
        {
            get { return _stopCMD; }
            set
            {
                _stopCMD = value;
                RaisePropertyChanged(() => StopCMD);
            }
        }

        /// <summary>
        /// 测试指令
        /// </summary>
        private bool _existTestCMD = false;
        /// <summary>
        /// 测试指令
        /// </summary>
        private bool ExistTestCMD
        {
            get { return _existTestCMD; }
            set
            {
                _existTestCMD = value;
                RaisePropertyChanged(() => ExistTestCMD);
            }
        }

        /// <summary>
        /// 炉壁校准指令
        /// </summary>
        private bool _existWallCalCMD = false;
        /// <summary>
        /// 炉壁校准指令
        /// </summary>
        private bool ExistWallCalCMD
        {
            get { return _existWallCalCMD; }
            set
            {
                _existWallCalCMD = value;
                RaisePropertyChanged(() => ExistWallCalCMD);
            }
        }

        /// <summary>
        /// 炉心校准指令
        /// </summary>
        private bool _existCenterCalCMD = false;
        /// <summary>
        /// 炉心校准指令
        /// </summary>
        private bool ExistCenterCalCMD
        {
            get { return _existCenterCalCMD; }
            set
            {
                _existCenterCalCMD = value;
                RaisePropertyChanged(() => ExistCenterCalCMD);
            }
        }

        /// <summary>
        /// 最后10分钟输出值
        /// </summary>
        private List<double> _voValueList = new List<double>();
        /// <summary>
        /// 上循环模式
        /// </summary>
        private List<double> VoValueList
        {
            get { return _voValueList; }
            set
            {
                _voValueList = value;
                RaisePropertyChanged(() => VoValueList);
            }
        }

        /// <summary>
        /// 试样刚放入
        /// </summary>
        private bool _test30BeginJust = false;
        /// <summary>
        /// 试样刚放入
        /// </summary>
        private bool Test30BeginJust
        {
            get { return _test30BeginJust; }
            set
            {
                _test30BeginJust = value;
                RaisePropertyChanged(() => Test30BeginJust);
            }
        }

        /// <summary>
        /// 试样检测已开始
        /// </summary>
        private bool _testStarted = false;
        /// <summary>
        /// 试样检测已开始
        /// </summary>
        private bool TestStarted
        {
            get { return _testStarted; }
            set
            {
                _testStarted = value;
                RaisePropertyChanged(() => TestStarted);
            }
        }

        #endregion


        #region 750±10℃自动平均温度

        /// <summary>
        /// 已自动平均标志
        /// </summary>
        private bool _isAverged = false;
        /// <summary>
        /// 已自动平均标志
        /// </summary>
        private bool IsAverged
        {
            get { return _isAverged; }
            set
            {
                _isAverged = value;
                RaisePropertyChanged(() => IsAverged);
            }
        }

        /// <summary>
        /// 平均范围计时
        /// </summary>
        private Time _avergeTime = new Time();
        /// <summary>
        /// 平均范围计时开始标志
        /// </summary>
        public Time AvergeTime
        {
            get { return _avergeTime; }
            set
            {
                _avergeTime = value;
                RaisePropertyChanged(() => AvergeTime);
            }
        }

        /// <summary>
        /// 差值累积(t2-t1)
        /// </summary>
        private double _difSum = 0;
        /// <summary>
        /// 差值累积(t2-t1)
        /// </summary>
        public double DifSum
        {
            get { return _difSum; }
            set
            {
                _difSum = value;
                RaisePropertyChanged(() => DifSum);
            }
        }

        /// <summary>
        /// 750±10℃范围10分钟后自动平均T1和T2温度
        /// </summary>
        private void AutoAverage()
        {
            try
            {
                if (IsAverged)
                    return;
                if (Dev.IsStd2010)
                    return;

                if ((Dev.AIList[0].ValueFinal >= 742) && (Dev.AIList[1].ValueFinal >= 742) &&
                    (Dev.AIList[0].ValueFinal <= 758) && (Dev.AIList[1].ValueFinal <= 758))
                {
                    if (!AvergeTime.IsStarted)
                    {
                        AvergeTime.IsStarted = true;
                        AvergeTime.StartTime = DateTime.Now;
                    }

                    TimeSpan tempSpan = DateTime.Now - AvergeTime.StartTime;
                    if (tempSpan.TotalSeconds >= 600)  //均值调零
                    {
                        double difAvg = DifSum * Dev.Period_BLL / 600000;    //t2-t1的平均值
                        Dev.AIList[0].ZeroCalValue = difAvg / 2 * Dev.AIList[0].KCalValue;
                        Dev.AIList[1].ZeroCalValue = -difAvg / 2 * Dev.AIList[1].KCalValue;
                        IsAverged = true;
                    }

                    DifSum = DifSum + Dev.AIList[1].ValueCaledNonZero - Dev.AIList[0].ValueCaledNonZero;        //累积差值
                }
                else
                {
                    DifSum = 0;
                    AvergeTime.Reset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 软启动及最大输出保护

        /// <summary>
        /// 软启动时间
        /// </summary>
        private Time _timeSoftBoot = new Time();
        /// <summary>
        /// 软启动时间
        /// </summary>
        private Time TimeSoftBoot
        {
            get { return _timeSoftBoot; }
            set
            {
                _timeSoftBoot = value;
                RaisePropertyChanged(() => TimeSoftBoot);
            }
        }

        /// <summary>
        /// 软启动及最高输出电压保护。电压升高速率不大于“最高电压/软启动时间”
        /// </summary>
        /// <returns>当前时刻输出电压最高限值</returns>
        private double SoftBoot()
        {
            double voMaxNow = 0;//当前最大输出电压限值
            double tnTemp;             //当前炉内温度

            try
            {
                if (!TimeSoftBoot.IsStarted)
                {
                    TimeSoftBoot.IsStarted = true;
                    TimeSoftBoot.StartTime = DateTime.Now;
                }

                if (Dev.IsStd2023)
                    tnTemp = Math.Abs((Dev.AIList[0].ValueFinal + Dev.AIList[1].ValueFinal) / 2);
                else
                    tnTemp = Math.Abs(Dev.AIList[0].ValueFinal);
                //炉内温度在300℃以上时软启动1分钟，否则用设定时间
                TimeSpan spanTime = DateTime.Now - TimeSoftBoot.StartTime;  //计算已开始时间
                if (tnTemp >= 300)
                    voMaxNow = Dev.RatioAoOutMax * spanTime.TotalMinutes/1;  //计算当前电压限值
                else
                    voMaxNow = Dev.RatioAoOutMax * spanTime.TotalMinutes / Dev.SoftBootTime;  //计算当前电压限值
                if (voMaxNow >= Dev.RatioAoOutMax)
                    voMaxNow = Dev.RatioAoOutMax;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return voMaxNow;
        }

        #endregion


        #region 辅助方法

        /// <summary>
        /// 根据当前温度获取PID参数
        /// </summary>
        /// <param name="tNow">当前温度</param>
        /// <returns></returns>
        private PID_ParamModel GePIDParam(double err)
        {
            PID_ParamModel tempPIDparam = new PID_ParamModel();

            try
            {
                if (Math.Abs(err) >= 50)
                    tempPIDparam = Dev.PID_T_Err50Up.PID_Param;
                else if ((Math.Abs(err) < 50) && (Math.Abs(err) >= 15))
                    tempPIDparam = Dev.PID_T_Err15_50.PID_Param;
                else
                    tempPIDparam = Dev.PID_T_Err0_15.PID_Param;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return tempPIDparam;
        }


        #endregion


        #region 初始化、复位方法

        /// <summary>
        /// BLL初始化
        /// </summary>
        public void BllInit()
        {
            Dev.IsBusy = false;
            LastMode = RunMode.Wait_Mode;
        }


        /// <summary>
        /// BLL复位
        /// </summary>
        public void BllRst()
        {
            //状态复位
            AOReset();
            DOReset();

            //指令复位
            StopCMD = false;
            ExistTestCMD = false;
            ExistSDCMD = false;
            ExistWallCalCMD = false;
            ExistCenterCalCMD = false;
            SDCMD = 0;
            PIDCMD = new Double[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            PowerPIDCMD = new Double[] { 0, 0, 0, 0, 0, 0, 0, 0 };

            //PID禁止
            PID_T_BLL.PID_Param.ControllerEnable = false;
            PID_T_BLL.CalculatePID(0);
            PID_T_BLL.PID_Param = new PID_ParamModel();
            PID_Power_BLL.PID_Param.ControllerEnable = false;
            PID_Power_BLL.CalculatePID(0);
            PID_Power_BLL.PID_Param = new PID_ParamModel();

            VoValueList = new List<double>();

            WallCalDQ.IsTesting = false;
            //自动调零参数复位
            IsAverged = false;
            AvergeTime.Reset();
            DifSum = 0;
            IsZeroedWallCal = false;
            ZeroWallCalTime.Reset();
            //软启动复位
            TimeSoftBoot.Reset();

            //第一次达到740以上
            Dev.Reach735First = false;
            Dev.TDelayTime = new Time();
            //平衡状态解锁
            Dev.T1StbBalanceEst.IsAdding = false;
            Dev.T2StbBalanceEst.IsAdding = false;
            //放入试样状态复位
            Test30BeginJust = false;
        }

        /// <summary>
        /// DO输出复位
        /// </summary>
        public void DOReset()
        {

        }

        /// <summary>
        /// AO输出复位
        /// </summary>
        public void AOReset()
        {
            for (int i = 0; i < Dev.AOList.Count; i++)
            {
                Dev.AOList[i].ValueFinal = 0;
            }
        }

        #endregion


        #region 模式切换、停机消息

        /// <summary>
        /// 模式切换消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void DevModeChangeMessage(Enums.RunMode msg)
        {
            Enums.RunMode cmd = msg;
            //紧急停机模式只能复位，不可直接切换为其它模式
            if (Dev.RunMode == RunMode.JJTJ_Mode)
            {
                MessageBox.Show("装置正处于紧急停机模式，请复位！");
                return;
            }
            //正在运行的非待机模式应先停机
            if ((cmd != RunMode.Wait_Mode) && (Dev.IsBusy))
            {
                MessageBox.Show("装置忙，请先停止其它试验或调试！");
                return;
            }
            //模式切换，装置忙状态切换
            if (cmd == RunMode.Wait_Mode)
            {
                Dev.RunMode = cmd;
                Dev.IsBusy = false;
            }
            else
            {
                Dev.RunMode = cmd;
                Dev.IsBusy = true;
            }
        }

        /// <summary>
        /// 停机消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void StopMessage(int msg)
        {
            int cmd = (int)msg;
            if (Dev.RunMode != RunMode.JJTJ_Mode)
            {
                if ((cmd == 1) && Dev.IsBusy)
                {
                    BllRst();

                    StopCMD = true;
                    Dev.RunMode = RunMode.Wait_Mode;
                    Dev.IsBusy = false;
                    ExpBRXDQ.SpDQ.StageDQ = TestStage.Wait_Stage;
                    ExpBRXDQ.SpDQ.TimeSp.Reset();
                    ExpBRXDQ.SpDQ.TimeTest30.Reset();
                }
            }
        }

        #endregion


        #region 窗口打开关闭消息

        /// <summary>
        /// 退出时需复位模式的窗口列表
        /// </summary>
        private static List<string> WindowListNeedRst = new List<string>()
        {
            WinNames.BRXWinName,WinNames.DbgWinName,WinNames.WallCalWinName,WinNames.CenterCalWinName
        };

        /// <summary>
        /// 窗口已关闭消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void WindowClosedMessage(string msg)
        {
            bool need = false;
            for (int i = 0; i < WindowListNeedRst.Count; i++)
            {
                if (msg == WindowListNeedRst[i])
                {
                    need = true;
                    break;
                }
            }

            if ((need) && (Dev.RunMode != RunMode.JJTJ_Mode))
            {
                BllRst();
                Dev.RunMode = RunMode.Wait_Mode;
                Dev.IsBusy = false;
            }
        }

        /// <summary>
        /// 窗口已新增消息处理
        /// </summary>
        /// <param name="msgWindow"></param>
        private void WindowCreatedMessage(Window msgWindow)
        {

        }
        #endregion


        public void Task_Completed(object sender, EventArgs e)
        {
        }
    }
}

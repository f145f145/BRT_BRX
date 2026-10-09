/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 装置Model，运行参数部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/3 22:45:36		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System;
using System.Windows.Threading;
using static BRX.Model.Enums.Enums;

namespace BRX.Model.Dev
{
    public partial class DevModel : ObservableObject
    {

        /// <summary>
        /// 输出电压（110V基准）
        /// </summary>
        public double VO_110VBase
        {
            get { return AOList[0].ValueFinal * Vin/100; }
        }

        /// <summary>
        /// 输出功率（110V基准）
        /// </summary>
        public double Power_110VBase
        {
            get { return VO_110VBase* VO_110VBase/R; }
        }

        /// <summary>
        /// 温度T1控制及平衡判断
        /// </summary>
        private StabilizationModel _t1StbBalanceEst = new StabilizationModel();
        /// <summary>
        /// 温度T1控制及平衡判断
        /// </summary>
        public StabilizationModel T1StbBalanceEst
        {
            get { return _t1StbBalanceEst; }
            set
            {
                _t1StbBalanceEst = value;
                RaisePropertyChanged(() => T1StbBalanceEst);
            }
        }

        /// <summary>
        /// 温度T2控制及平衡判断
        /// </summary>
        private StabilizationModel _t2StbBalanceEst = new StabilizationModel();
        /// <summary>
        /// 温度T2控制及平衡判断
        /// </summary>
        public StabilizationModel T2StbBalanceEst
        {
            get { return _t2StbBalanceEst; }
            set
            {
                _t2StbBalanceEst = value;
                RaisePropertyChanged(() => T2StbBalanceEst);
            }
        }

        /// <summary>
        /// 试样表面温度平衡判断
        /// </summary>
        private StabilizationModel _tssStbBalanceEst = new StabilizationModel();
        /// <summary>
        /// 试样表面温度平衡判断
        /// </summary>
        public StabilizationModel TssStbBalanceEst
        {
            get { return _tssStbBalanceEst; }
            set
            {
                _tssStbBalanceEst = value;
                RaisePropertyChanged(() => TssStbBalanceEst);
            }
        }

        /// <summary>
        /// 试样中心温度平衡判断
        /// </summary>
        private StabilizationModel _tscStbBalanceEst = new StabilizationModel();
        /// <summary>
        /// 试样中心温度平衡判断
        /// </summary>
        public StabilizationModel TscStbBalanceEst
        {
            get { return _tscStbBalanceEst; }
            set
            {
                _tscStbBalanceEst = value;
                RaisePropertyChanged(() => TscStbBalanceEst);
            }
        }

        /// <summary>
        /// 装置占用状态
        /// </summary>
        private bool _isBusy = false;
        /// <summary>
        /// 装置占用状态
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
                RaisePropertyChanged(() => IsNotBusy);
            }
        }

        /// <summary>
        /// 装置空闲状态
        /// </summary>
        public bool IsNotBusy
        {
            get { return !_isBusy; }
        }

        /// <summary>
        /// 装置运行模式
        /// </summary>
        private RunMode _runMode = RunMode.Wait_Mode;
        /// <summary>
        /// 装置运行模式
        /// </summary>
        public RunMode RunMode
        {
            get { return _runMode; }
            set
            {
                _runMode = value;
                RaisePropertyChanged(() => RunMode);
            }
        }
        
        /// <summary>
        /// 当前时间
        /// </summary>
        private DateTime _timeNow = DateTime.Now;
        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime TimeNow
        {
            get { return _timeNow; }
            set
            {
                _timeNow = value;
                RaisePropertyChanged(() => TimeNow);
                RaisePropertyChanged(() => TimeSpanPowerOn);
            }
        }

        /// <summary>
        /// 开机时间点
        /// </summary>
        private readonly DateTime _timePowerOn = DateTime.Now;
        /// <summary>
        /// 开机时间点
        /// </summary>
        public DateTime TimePowerOn
        {
            get { return _timePowerOn; }
        }

        /// <summary>
        /// 开机时长
        /// </summary>
        public Int32 TimeSpanPowerOn
        {
            get
            {
                TimeSpan span = DateTime.Now - _timePowerOn;
                return Convert.ToInt32(span.TotalSeconds);
            }
        }

        /// <summary>
        /// 温度读取连续错误次数
        /// </summary>
        private int _comTErrTimes = 0;
        /// <summary>
        /// 温度读取连续错误次数
        /// </summary>
        public int ComTErrTimes
        {
            get { return _comTErrTimes; }
            set
            {
                _comTErrTimes = value;
                RaisePropertyChanged(() => ComTErrTimes);
            }
        }

        /// <summary>
        /// AO读写连续错误次数
        /// </summary>
        private int _comAOErrTimes = 0;
        /// <summary>
        /// AO读写连续错误次数
        /// </summary>
        public int ComAOErrTimes
        {
            get { return _comAOErrTimes;}
            set
            {
                _comAOErrTimes = value;
                RaisePropertyChanged(() => ComAOErrTimes);
            }
        }

        /// <summary>
        /// 功率模块读取连续错误次数
        /// </summary>
        private int _comPowerErrTimes = 0;
        /// <summary>
        /// 功率模块读取连续错误次数
        /// </summary>
        public int ComPowerErrTimes
        {
            get { return _comPowerErrTimes; }
            set
            {
                _comPowerErrTimes = value;
                RaisePropertyChanged(() => ComPowerErrTimes);
            }
        }

        /// <summary>
        /// 错误提示信息
        /// </summary>
        private string _errorInfo = "";
        /// <summary>
        /// 错误提示信息
        /// </summary>
        public string ErrorInfo
        {
            get { return _errorInfo; }
            set
            {
                _errorInfo = value;
                RaisePropertyChanged(() => ErrorInfo);
            }
        }

        /// <summary>
        /// 故障信息传递消息
        /// </summary>
        /// <param name="msg"></param>
        private void ErrorMessage(string msg)
        {
            ErrorInfo = msg;
        }

        /// <summary>
        /// 错误信息复位定时器
        /// </summary>
        readonly DispatcherTimer ErrInfoRstTimer = new DispatcherTimer();

        /// <summary>
        /// 错误信息复位定时器回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrInfoRstTimer_Tick(object sender, EventArgs e)
        {
            if ((!ComTStop) && (!ComAOStop)&& (!ComPowerStop))
                ErrorInfo = "";
        }

        /// <summary>
        /// 停止热电偶模块通讯指令
        /// </summary>
        private bool _comTStop = false;
        /// <summary>
        /// 停止热电偶模块通讯指令
        /// </summary>
        public bool ComTStop
        {
            get { return _comTStop; }
            set
            {
                _comTStop = value;
                RaisePropertyChanged(() => ComTStop);
            }
        }

        /// <summary>
        /// 停止功率模块通讯指令
        /// </summary>
        private bool _comPowerStop = false;
        /// <summary>
        /// 停止功率模块通讯指令
        /// </summary>
        public bool ComPowerStop
        {
            get { return _comPowerStop; }
            set
            {
                _comPowerStop = value;
                RaisePropertyChanged(() => ComPowerStop);
            }
        }


        /// <summary>
        /// 停止AO模块通讯指令
        /// </summary>
        private bool _comAOStop = false;
        /// <summary>
        /// 停止AO模块通讯指令
        /// </summary>
        public bool ComAOStop
        {
            get { return _comAOStop; }
            set
            {
                _comAOStop = value;
                RaisePropertyChanged(() => ComAOStop);
            }
        }

        /// <summary>
        /// 出现火焰状态
        /// </summary>
        private bool _isFired = false;
        /// <summary>
        /// 出现火焰状态
        /// </summary>
        public bool IsFired
        {
            get { return _isFired; }
            set
            {
                _isFired = value;
                RaisePropertyChanged(() => IsFired);
                RaisePropertyChanged(() => IsNotFired);
            }
        }
        /// <summary>
        /// 未出现火焰状态
        /// </summary>
        public bool IsNotFired
        {
            get { return !_isFired; }
        }


        /// <summary>
        /// 第一次到达735℃
        /// </summary>
        private bool _reach735First = false;
        /// <summary>
        /// 第一次到达735℃
        /// </summary>
        public bool Reach735First
        {
            get { return _reach735First; }
            set
            {
                _reach735First = value;
                RaisePropertyChanged(() => Reach735First);
            }
        }



        /// <summary>
        /// 延时温控时间
        /// </summary>
        private Time _tDelayTime = new Time();
        /// <summary>
        /// 延时温控时间
        /// </summary>
        public Time TDelayTime
        {
            get { return _tDelayTime; }
            set
            {
                _tDelayTime = value;
                RaisePropertyChanged(() => TDelayTime);
            }
        }

        
        /// <summary>
        /// 管理员登录状态
        /// </summary>
        private bool _isAdmin = false;
        /// <summary>
        /// 管理员登录状态
        /// </summary>
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                RaisePropertyChanged(() => IsAdmin);
            }
        }

        /// <summary>
        /// 输入的管理员登录密码，正确密码为brt12345678
        /// </summary> 
        private string _passWord = "";
        /// <summary>
        /// 输入的管理员登录密码，正确密码为brt12345678
        /// </summary>
        public string PassWord
        {
            get { return _passWord; }
            set
            {
                _passWord = value;
                RaisePropertyChanged(() => PassWord);
            }
        }

    }
}

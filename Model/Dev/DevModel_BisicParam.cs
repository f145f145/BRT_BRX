/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/4 5:34:24
 * 描述：
 * 装置Model，装置基本参数部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/3 22:45:36		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;

namespace BRX.Model.Dev
{
    public partial class DevModel : ObservableObject
    {
        #region 软启动
        
        /// <summary>
        /// 最大输出百分比(%)
        /// </summary>
        private double _ratioAoOutMax = 80;
        /// <summary>
        /// 最大输出百分比(%)
        /// </summary>
        public double RatioAoOutMax
        {
            get { return _ratioAoOutMax; }
            set
            {
                if (value < 20)
                    _ratioAoOutMax = 20;
                else if (_ratioAoOutMax > 100)
                    _ratioAoOutMax = 100;
                else
                    _ratioAoOutMax = value;
                RaisePropertyChanged(() => RatioAoOutMax);
            }
        }

        /// <summary>
        /// 软启动总时长(分钟)
        /// </summary>
        private double _softBootTime = 10;
        /// <summary>
        /// 软启动总时长(分钟)
        /// </summary>
        public double SoftBootTime
        {
            get { return _softBootTime; }
            set
            {
                if (value < 1)
                    _softBootTime = 1;
                else if (_softBootTime > 20)
                    _softBootTime = 20;
                else
                    _softBootTime = value;
                RaisePropertyChanged(() => SoftBootTime);
            }
        }

        #endregion


        #region 绘图参数

        /// <summary>
        /// 绘图更新周期(ms)
        /// </summary>
        private int _plotPeriod = 1000;
        /// <summary>
        /// 绘图更新周期(ms)
        /// </summary>
        public int PlotPeriod
        {
            get { return _plotPeriod; }
            set
            {
                _plotPeriod = Math.Abs(value);
                RaisePropertyChanged(() => PlotPeriod);
            }
        }

        /// <summary>
        /// 单曲线绘图总点数
        /// </summary>
        private int _pointsPerLine = 5000;
        /// <summary>
        /// 单曲线绘图总点数
        /// </summary>
        public int PointsPerLine
        {
            get { return _pointsPerLine; }
            set
            {
                _pointsPerLine = Math.Abs(value);
                RaisePropertyChanged(() => PointsPerLine);
            }
        }

        #endregion


        #region 装置时间相关

        /// <summary>
        /// 计算间隔时间(ms)
        /// </summary>
        private int _period_BLL = 1000;
        /// <summary>
        /// 计算间隔时间(ms)
        /// </summary>
        public int Period_BLL
        {
            get { return _period_BLL; }
            set
            {
                _period_BLL = Math.Abs(value);
                RaisePropertyChanged(() => Period_BLL);
                Messenger.Default.Send<int>(_period_BLL, "PllPeriodChanged");
            }
        }

        /// <summary>
        /// 通讯周期(ms)
        /// </summary>
        private int _period_Comm = 1000;
        /// <summary>
        /// 通讯周期(ms)
        /// </summary>
        public int Period_Comm
        {
            get { return _period_Comm; }
            set
            {
                _period_Comm = Math.Abs(value);
                RaisePropertyChanged(() => Period_Comm);
            }
        }

        #endregion


        #region 恒功率控制相关

        /// <summary>
        /// 自动计算恒功率值（true表示自动计算，false用固定数值）
        /// </summary>
        private bool _autoPower = true;
        /// <summary>
        /// 自动计算恒功率值（true表示自动计算，false用固定数值）
        /// </summary>
        public bool AutoPower
        {
            get { return _autoPower; }
            set
            {
                _autoPower = value;
                RaisePropertyChanged(() => AutoPower);
                RaisePropertyChanged(() => IsFixedPower);
            }
        }
        /// <summary>
        /// 不自动计算恒功率值
        /// </summary>
        public bool IsFixedPower
        {
            get { return !_autoPower; }
        }


        /// <summary>
        /// 自动计算恒功率时间（s）
        /// </summary>
        private int _timeCalcPower = 600;
        /// <summary>
        /// 自动计算恒功率时间（s）
        /// </summary>
        public int TimeCalcPower
        {
            get { return _timeCalcPower; }
            set
            {
                _timeCalcPower = Math.Abs(value);
                RaisePropertyChanged(() => TimeCalcPower);
            }
        }


        /// <summary>
        /// 固定功率值（功率）
        /// </summary>
        private double _fixedPower_P = 850;
        /// <summary>
        /// 固定功率值（功率）
        /// </summary>
        public double FixedPower_P
        {
            get { return _fixedPower_P; }
            set
            {
                _fixedPower_P = Math.Abs(value);
                RaisePropertyChanged(() => FixedPower_P);
            }
        }


        /// <summary>
        /// 固定功率值（电压百分比）
        /// </summary>
        private double _fixedPower_U = 50;
        /// <summary>
        /// 固定功率值（电压百分比）
        /// </summary>
        public double FixedPower_U
        {
            get { return _fixedPower_U; }
            set
            {
                _fixedPower_U = Math.Abs(value);
                RaisePropertyChanged(() => FixedPower_U);
            }
        }


        /// <summary>
        /// 放入试样后输出减小值（电压U）
        /// </summary>
        private double _outReduceU = 1;
        /// <summary>
        /// 放入试样后输出减小值（电压U）
        /// </summary>
        public double OutReduceU
        {
            get { return _outReduceU; }
            set
            {
                _outReduceU = Math.Abs(value);
                RaisePropertyChanged(() => OutReduceU);
            }
        }


        /// <summary>
        /// 放入试样后输出减小值（功率P）
        /// </summary>
        private double _outReduceP = 1;
        /// <summary>
        /// 放入试样后输出减小值（功率P）
        /// </summary>
        public double OutReduceP
        {
            get { return _outReduceP; }
            set
            {
                _outReduceP = Math.Abs(value);
                RaisePropertyChanged(() => OutReduceP);
            }
        }

        /// <summary>
        /// PID初始积分值U
        /// </summary>
        private double _initSumUI_U = 0;
        /// <summary>
        /// PID初始积分值U
        /// </summary>
        public double InitSumUI_U
        {
            get { return _initSumUI_U; }
            set
            {
                _initSumUI_U = Math.Abs(value);
                RaisePropertyChanged(() => InitSumUI_U);
            }
        }

        /// <summary>
        /// PID初始积分值P
        /// </summary>
        private double _initSumUI_P = 0;
        /// <summary>
        /// PID初始积分值P
        /// </summary>
        public double InitSumUI_P
        {
            get { return _initSumUI_P; }
            set
            {
                _initSumUI_P = Math.Abs(value);
                RaisePropertyChanged(() => InitSumUI_P);
            }
        }

        #endregion


        #region 控温及平衡相关

        /// <summary>
        /// PID控温起始温度（℃）
        /// </summary>
        private double _tStartPID = 720;
        /// <summary>
        /// PID控温起始温度（℃）
        /// </summary>
        public double TStartPID
        {
            get { return _tStartPID; }
            set
            {
                _tStartPID = Math.Abs(value);
                RaisePropertyChanged(() => TStartPID);
            }
        }

        /// <summary>
        /// 延时温控时长（s）
        /// </summary>
        private int _delayTime = 600;
        /// <summary>
        /// 延时温控时长（s）
        /// </summary>
        public int DelayTime
        {
            get { return _delayTime; }
            set
            {
                _delayTime = Math.Abs(value);
                RaisePropertyChanged(() => DelayTime);
            }
        }


        /// <summary>
        /// 延时温控输出电压
        /// </summary>
        private double _tDelayU = 60;
        /// <summary>
        /// 延时温控输出电压
        /// </summary>
        public double TDelayU
        {
            get { return _tDelayU; }
            set
            {
                _tDelayU = Math.Abs(value);
                RaisePropertyChanged(() => TDelayU);
            }
        }


        /// <summary>
        /// 延时温控输出功率
        /// </summary>
        private double _tDelayP = 60;
        /// <summary>
        /// 延时温控输出功率
        /// </summary>
        public double TDelayP
        {
            get { return _tDelayP; }
            set
            {
                _tDelayP = Math.Abs(value);
                RaisePropertyChanged(() => TDelayP);
            }
        }


        /// <summary>
        /// 稳定判定时长（s）
        /// </summary>
        private int _time_Stabilize = 600;
        /// <summary>
        /// 稳定判定时长（s）
        /// </summary>
        public int Time_Stabilize
        {
            get { return _time_Stabilize; }
            set
            {
                _time_Stabilize = Math.Abs(value);
                RaisePropertyChanged(() => Time_Stabilize);
            }
        }

        /// <summary>
        /// 控温稳定判定间隔（s）
        /// </summary>
        private int _period_Stabilize = 1;
        /// <summary>
        /// 控温稳定判定间隔（s）
        /// </summary>
        public int Period_Stabilize
        {
            get { return _period_Stabilize; }
            set
            {
                _period_Stabilize = Math.Abs(value);
                RaisePropertyChanged(() => Period_Stabilize);
            }
        }

        /// <summary>
        /// 最终平衡判定时长（s）
        /// </summary>
        private int _time_FinalEqu = 600;
        /// <summary>
        /// 最终平衡判定时长（s）
        /// </summary>
        public int Time_FinalEqu
        {
            get { return _time_FinalEqu; }
            set
            {
                _time_FinalEqu = Math.Abs(value);
                RaisePropertyChanged(() => Time_FinalEqu);
            }
        }

        /// <summary>
        /// 最终平衡判定间隔（s）
        /// </summary>
        private int _period_FinalEqu = 1;
        /// <summary>
        /// 最终平衡判定间隔（s）
        /// </summary>
        public int Period_FinalEqu
        {
            get { return _period_FinalEqu; }
            set
            {
                _period_FinalEqu = Math.Abs(value);
                RaisePropertyChanged(() => Period_FinalEqu);
            }
        }

        /// <summary>
        /// 最终平衡判定时间第一次（s）
        /// </summary>
        private int _time_FinalEquEvalFist = 1800;
        /// <summary>
        /// 最终平衡判定时间第一次（s）
        /// </summary>
        public int Time_FinalEquEvalFist
        {
            get { return _time_FinalEquEvalFist; }
            set
            {
                _time_FinalEquEvalFist = Math.Abs(value);
                RaisePropertyChanged(() => Period_FinalEqu);
            }
        }

        /// <summary>
        /// 最长测试时间（s）
        /// </summary>
        private int _time_FinalEquEvalMax = 3600;
        /// <summary>
        /// 最长测试时间（s）
        /// </summary>
        public int Time_FinalEquEvalMax
        {
            get { return _time_FinalEquEvalMax; }
            set
            {
                _time_FinalEquEvalMax = Math.Abs(value);
                RaisePropertyChanged(() => Time_FinalEquEvalMax);
            }
        }

        /// <summary>
        /// 测试控温目标（℃）
        /// </summary>
        private double _tCtlAimTest = 750;
        /// <summary>
        /// 测试控温目标（℃）
        /// </summary>
        public double TCtlAimTest
        {
            get { return _tCtlAimTest; }
            set
            {
                _tCtlAimTest = value;
                RaisePropertyChanged(() => TCtlAimTest);
            }
        }

        /// <summary>
        /// 测试控温允许误差（℃）
        /// </summary>
        private double _tCtlErrPermit = 5;
        /// <summary>
        /// 测试控温允许误差（℃）
        /// </summary>
        public double TCtlErrPermit
        {
            get { return _tCtlErrPermit; }
            set
            {
                _tCtlErrPermit = Math.Abs(value);
                RaisePropertyChanged(() => TCtlErrPermit);
            }
        }

        /// <summary>
        /// 测试控温允许漂移（℃）
        /// </summary>
        private double _tCtlDriftPermit = 2;
        /// <summary>
        /// 测试控温允许漂移（℃）
        /// </summary>
        public double TCtlDriftPermit
        {
            get { return _tCtlDriftPermit; }
            set
            {
                _tCtlDriftPermit = Math.Abs(value);
                RaisePropertyChanged(() => TCtlDriftPermit);
            }
        }

        /// <summary>
        /// 测试控温允许偏差（℃）
        /// </summary>
        private double _tCtlDeviationPermit = 10;
        /// <summary>
        /// 测试控温允许偏差（℃）
        /// </summary>
        public double TCtlDeviationPermit
        {
            get { return _tCtlDeviationPermit; }
            set
            {
                _tCtlDeviationPermit = Math.Abs(value);
                RaisePropertyChanged(() => TCtlDeviationPermit);
            }
        }

        /// <summary>
        /// 最终平衡允许漂移（℃）
        /// </summary>
        private double _tFinalEquDriftPermit = 2;
        /// <summary>
        /// 最终平衡允许漂移（℃）
        /// </summary>
        public double TFinalEquDriftPermit
        {
            get { return _tFinalEquDriftPermit; }
            set
            {
                _tFinalEquDriftPermit = Math.Abs(value);
                RaisePropertyChanged(() => TFinalEquDriftPermit);
            }
        }

        /// <summary>
        /// 升温速度（℃/min）
        /// </summary>
        private double _tUpSpeed = 10;
        /// <summary>
        /// 升温速度（℃/min）
        /// </summary>
        public double TUpSpeed
        {
            get { return _tUpSpeed; }
            set
            {
                _tUpSpeed = value;
                RaisePropertyChanged(() => TUpSpeed);
            }
        }


        #endregion


        #region 最后的实验

        /// <summary>
        /// 开机载入是否最后试验（false时开机载入默认试验，true时开机载入最后试验）
        /// </summary>
        private bool _isLoadLastExpPowerOn = false;
        /// <summary>
        /// 开机载入是否最后试验（false时开机载入默认试验，true时开机载入最后试验）
        /// </summary>
        public bool IsLoadLastExpPowerOn
        {
            get { return _isLoadLastExpPowerOn; }
            set
            {
                _isLoadLastExpPowerOn = value;
                RaisePropertyChanged(() => IsLoadLastExpPowerOn);
            }
        }

        /// <summary>
        /// 最后打开的试验编号
        /// </summary>
        private string _expNOLast = "DefaultExp";
        /// <summary>
        /// 最后打开的试验编号
        /// </summary>
        public string ExpNOLast
        {
            get { return _expNOLast; }
            set
            {
                _expNOLast = value;
                RaisePropertyChanged(() => ExpNOLast);
            }
        }

        /// <summary>
        /// 最后的炉壁标定试验编号
        /// </summary>
        private string _wallCalNOLast = "DefaultWallCal";
        /// <summary>
        /// 最后的炉壁标定试验编号
        /// </summary>
        public string WallCalNOLast
        {
            get { return _wallCalNOLast; }
            set
            {
                _wallCalNOLast = value;
                RaisePropertyChanged(() => WallCalNOLast);
            }
        }

        /// <summary>
        /// 最后的炉内标定试验编号
        /// </summary>
        private string _centerCalNOLast = "DefaultCenterCal";
        /// <summary>
        /// 最后的炉内标定试验编号
        /// </summary>
        public string CenterCalNOLast
        {
            get { return _centerCalNOLast; }
            set
            {
                _centerCalNOLast = value;
                RaisePropertyChanged(() => CenterCalNOLast);
            }
        }

        #endregion


        /// <summary>
        /// 是否加密试验报告
        /// </summary>
        private bool _encryptRPT = true;
        /// <summary>
        /// 是否加密试验报告
        /// </summary>
        public bool EncryptRPT
        {
            get { return _encryptRPT; }
            set
            {
                _encryptRPT = value;
                RaisePropertyChanged(() => EncryptRPT);
            }
        }


        /// <summary>
        /// 隐藏式样2-5
        /// </summary>
        private bool _hideSY2345 = true;
        /// <summary>
        /// 隐藏式样2-5
        /// </summary>
        public bool HideSY2345
        {
            get { return _hideSY2345; }
            set
            {
                _hideSY2345 = value;

                if (_hideSY2345)
                    IsAutoContinue = false; //隐藏式样2-5时，不能自动转入下一个试样
                RaisePropertyChanged(() => HideSY2345);
            }
        }


        /// <summary>
        /// 是否连续进行试验（当前试样完成后自动进行下一个未完成的试样，从0号开始查找）
        /// </summary>
        private bool _isAutoContinue = false;
        /// <summary>
        /// 是否连续进行试验（当前试样完成后自动进行下一个未完成的试样，从0号开始查找）
        /// </summary>
        public bool IsAutoContinue
        {
            get { return _isAutoContinue; }
            set
            {
                _isAutoContinue = value;
                RaisePropertyChanged(() => IsAutoContinue);
            }
        }


        /// <summary>
        /// 有功率检测模块
        /// </summary>
        private bool _withPowerSenser = false;
        /// <summary>
        /// 有功率检测模块
        /// </summary>
        public bool WithPowerSenser
        {
            get { return _withPowerSenser; }
            set
            {
                _withPowerSenser = value;
                RaisePropertyChanged(() => WithPowerSenser);
                RaisePropertyChanged(() => WithoutPowerSenser);
            }
        }
        /// <summary>
        /// 没有功率检测模块
        /// </summary>
        public bool WithoutPowerSenser
        {
            get { return !(_withPowerSenser); }
        }

        /// <summary>
        /// 加热炉内阻
        /// </summary>
        private double _r = 8;
        /// <summary>
        /// 加热炉内阻
        /// </summary>
        public double R
        {
            get { return _r; }
            set
            {
                _r = value;
                RaisePropertyChanged(() => R);
            }
        }

        /// <summary>
        /// 温度采集模块类型,0阿尔泰DAM-3138H
        /// </summary>
        private string _tModelType = "DAM3138";
        /// <summary>
        /// 温度采集模块类型,0阿尔泰DAM-3138H
        /// </summary>
        public string TModelType
        {
            get { return _tModelType; }
            set
            {
                _tModelType = value;
                RaisePropertyChanged(() => TModelType);
            }
        }

        /// <summary>
        /// 模拟量输出模块类型，0阿尔泰DAM3060C，1宇泰UT-5564A
        /// </summary>
        private string _aoModelType = "DAM3060C";
        /// <summary>
        /// 模拟量输出模块类型，0阿尔泰DAM3060C，1宇泰UT-5564A
        /// </summary>
        public string AOModelType
        {
            get { return _aoModelType; }
            set
            {
                _aoModelType = value;
                RaisePropertyChanged(() => AOModelType);
            }
        }


        /// <summary>
        /// 是否用线性回归计算稳定电压
        /// </summary>
        private bool _useLRCalcU = false;
        /// <summary>
        /// 是否用线性回归计算稳定电压
        /// </summary>
        public bool UseLRCalcU
        {
            get { return _useLRCalcU; }
            set
            {
                _useLRCalcU = value;
                RaisePropertyChanged(() => UseLRCalcU);
            }
        }


        /// <summary>
        /// 最大电压
        /// </summary>
        private double _vin = 110;
        /// <summary>
        /// 最大电压
        /// </summary>
        public double Vin
        {
            get { return _vin; }
            set
            {
                _vin = value;
                RaisePropertyChanged(() => Vin);
            }
        }

        /// <summary>
        /// 系统辨识输出电压比例
        /// </summary>
        private double _vOut_ID = 55;
        /// <summary>
        /// 系统辨识输出电压比例
        /// </summary>
        public double VOut_ID
        {
            get { return _vOut_ID; }
            set
            {
                _vOut_ID = value;
                RaisePropertyChanged(() => VOut_ID);
            }
        }

        /// <summary>
        /// 标准选择（第二个炉内温度）
        /// </summary>
        private string _stdSelected = "GB/T 5464-2010";
        /// <summary>
        /// 标准选择（第二个炉内温度）
        /// </summary>
        public string StdSelected
        {
            get { return _stdSelected; }
            set
            {
                _stdSelected = value;
                RaisePropertyChanged(() => StdSelected);
                RaisePropertyChanged(() => IsStd2023);
                RaisePropertyChanged(() => IsStd2010);
            }
        }

        /// <summary>
        /// 是新标准
        /// </summary>
        public bool IsStd2023
        {
            get { return StdSelected== "GB/T 5464-2023"?true:false; }
        }

        /// <summary>
        /// 是旧标准
        /// </summary>
        public bool IsStd2010
        {
            get { return StdSelected == "GB/T 5464-2010" ? true : false; }
        }


        /// <summary>
        /// 标准列表
        /// </summary>
        public ObservableCollection<string> _stdList = new ObservableCollection<string>() { "GB/T 5464-2010", "GB/T 5464-2023" };
        /// <summary>
        /// 标准列表
        /// </summary>
        public ObservableCollection<string> StdList
        {
            get { return _stdList; }
        }

        /// <summary>
        /// 热电偶采集器类型列表
        /// </summary>
        public ObservableCollection<string> _tModelTypeList = new ObservableCollection<string>() { "DAM3138", "DAM3134" };
        /// <summary>
        /// 热电偶采集器类型列表
        /// </summary>
        public ObservableCollection<string> TModelTypeList
        {
            get { return _tModelTypeList; }
        }

        /// <summary>
        /// 模拟量输出模块类型列表
        /// </summary>
        public ObservableCollection<string> _aoModelTypeList = new ObservableCollection<string>() { "DAM3060C", "UT5564A", "Modbus-4AI4AO" };
        /// <summary>
        /// 模拟量输出模块类型列表
        /// </summary>
        public ObservableCollection<string> AOModelTypeList
        {
            get { return _aoModelTypeList; }
        }
    }
}
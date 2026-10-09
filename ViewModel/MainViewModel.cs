/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/2/27 8:00:00
 * 描述：
 * 主窗口、数据窗口、设定窗口、管理窗口等综合ViewModel
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/2/27 8:00:00		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using BRX.Model.Dev;
using System;
using System.Windows;
using BRX.View;
using GalaSoft.MvvmLight.Command;
using BRX.DAL.DevDAL;
using BRX.Communication;
using BRX.Model.Exp;
using BRX.BLL;
using BRX.DAL.ExpDAL;
using BRX.DAL.RepDAL;
using BRX.DBDataSetTableAdapters;
using BRX.Model.Enums;
using static BRX.Model.Enums.Enums;
using Color = System.Drawing.Color;
using ChartModel;
using System.Windows.Threading;
using System.Collections.Generic;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Media;
using BRX.DAL;

namespace BRX.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            //有窗口新增或关闭
            Messenger.Default.Register<string>(this, "WindowClosed", WindowClosedMessage);
            Messenger.Default.Register<Window>(this, "NewWindowCreated", WindowCreatedMessage);

            //切换试样消息
            Messenger.Default.Register<int>(this, "NextSpMessage", NextSpMessage);

            //更新试样温升曲线
            Messenger.Default.Register<string>(this, "UpdateSpWSPlotMessage", UpdateSpWSPlotMessage);

            //播放声音消息
            Messenger.Default.Register<int>(this, "PlaySoundMessage", PlaySoundMessage);

            //装置初始化
            Dev = new DevModel();

            //试验初始化
            ExpBRXDQ = new ExpModel_Test();
            Messenger.Default.Send<int>(Dev.Period_BLL, "PllPeriodChanged");
            WallCalDQ = new ExpModel_CalWall();
            CenterCalDQ = new ExpModel_CalCenter();

            //装置数据、试验读写初始化
            DevDAL = new DevDAL(Dev);
            ExpDAL = new ExpDAL(Dev, ExpBRXDQ);
            Messenger.Default.Send<string>("LoadDevSettings", "DevDataRWMessage");

            if (Dev.IsLoadLastExpPowerOn)
                Messenger.Default.Send<string>(Dev.ExpNOLast, "LoadExpBRXMessage");
            else
               Messenger.Default.Send<string>("DefaultExp", "LoadExpBRXMessage");

            //载入PID调试参数
            LoadPIDParam();

            //报告导出初始化
            RepDAL = new RepDAL(Dev, ExpBRXDQ);

            //主控流程初始化
            BLL = new Bll(Dev, ExpBRXDQ,WallCalDQ,CenterCalDQ);

            //通讯初始化
            Communication = new Comm(Dev);

            A10TableAdapter.Fill(ExpBRXTable);
            //试验列表已更新
            Messenger.Default.Register<DBDataSet.A10检测试验参数DataTable>(this, "ExpBRXTableChanged", ExpBRXTableChanged);


            //绘图数据初始化，绘图定时器初始化
            PlotInit();
            PlotTimer.Tick += new EventHandler(PlotTimer_Tick);
            PlotTimer.Interval = TimeSpan.FromMilliseconds(Dev.PlotPeriod);
            PlotTimer.Start();

            //注册管理员登录消息
            Messenger.Default.Register<string>(this, "AdminLogginMessage", AdminLogginMessage);
        }

        #region 装置、试验、通讯、数据读写属性

        /// <summary>
        /// 装置参数
        /// </summary>
        private DevModel _dev;
        /// <summary>
        /// 装置参数
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
        /// 当前测试试验
        /// </summary>
        private ExpModel_Test _expBRXDQ;
        /// <summary>
        /// 当前测试试验
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

        /// <summary>
        /// 通讯
        /// </summary>
        private Comm _communication;
        /// <summary>
        /// 通讯
        /// </summary>
        private Comm Communication
        {
            get { return _communication; }
            set
            {
                _communication = value;
                RaisePropertyChanged(() => Communication);
            }
        }

        /// <summary>
        /// 流程控制
        /// </summary>
        private Bll _bll;
        /// <summary>
        /// 流程控制
        /// </summary>
        public Bll BLL
        {
            get { return _bll; }
            set
            {
                _bll = value;
                RaisePropertyChanged(() => BLL);
            }
        }

        /// <summary>
        /// 装置参数读写
        /// </summary>
        private DevDAL _devDAL;
        /// <summary>
        /// 装置参数读写
        /// </summary>
        private DevDAL DevDAL
        {
            get { return _devDAL; }
            set
            {
                _devDAL = value;
                RaisePropertyChanged(() => DevDAL);
            }
        }

        /// <summary>
        /// 试验数据读写
        /// </summary>
        private ExpDAL _expDAL;
        /// <summary>
        /// 试验数据读写
        /// </summary>
        private ExpDAL ExpDAL
        {
            get { return _expDAL; }
            set
            {
                _expDAL = value;
                RaisePropertyChanged(() => ExpDAL);
            }
        }

        /// <summary>
        /// 试验报告读写
        /// </summary>
        private RepDAL _repDAL;
        /// <summary>
        /// 试验报告读写
        /// </summary>
        public RepDAL RepDAL
        {
            get { return _repDAL; }
            set
            {
                _repDAL = value;
                RaisePropertyChanged(() => RepDAL);
            }
        }

        #endregion


        #region 试验数据学习

        /// <summary>
        /// 数据浏览用试验
        /// </summary>
        private ExpModel_Test _expForView = new ExpModel_Test();
        /// <summary>
        /// 数据浏览用试验
        /// </summary>
        public ExpModel_Test ExpForView
        {
            get { return _expForView; }
            set
            {
                _expForView = value;
                RaisePropertyChanged(() => ExpForView);
            }
        }

        /// <summary>
        /// 复制的试样
        /// </summary>
        private ExpModel_Sp _spForCopy = new ExpModel_Sp();
        /// <summary>
        /// 复制的试样
        /// </summary>
        public ExpModel_Sp SpForCopy
        {
            get { return _spForCopy; }
            set
            {
                _spForCopy = value;
                RaisePropertyChanged(() => SpForCopy);
            }
        }


        #endregion


        #region 检测试验管理相关属性

        /// <summary>
        /// 试验列表Table
        /// </summary>
        private BRX.DBDataSet.A10检测试验参数DataTable _expBRXTable = new BRX.DBDataSet.A10检测试验参数DataTable();
        /// <summary>
        /// 试验列表Table
        /// </summary>
        public BRX.DBDataSet.A10检测试验参数DataTable ExpBRXTable
        {
            get { return _expBRXTable; }
            set
            {
                _expBRXTable = value;
                RaisePropertyChanged(() => ExpBRXTable);
            }
        }

        /// <summary>
        /// A10检测试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A10检测试验参数TableAdapter _a01TableAdapter = new A10检测试验参数TableAdapter();

        /// <summary>
        /// A10检测试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A10检测试验参数TableAdapter A10TableAdapter
        {
            get { return _a01TableAdapter; }
            set
            {
                _a01TableAdapter = value;
                RaisePropertyChanged(() => A10TableAdapter);
            }
        }

        /// <summary>
        /// 列表中选中的测试试验编号
        /// </summary>
        private string _selectedExpNOStr_BRX = "";
        /// <summary>
        /// 列表中选中的测试试验编号
        /// </summary>
        public string SelectedExpNOStr_BRX
        {
            get { return _selectedExpNOStr_BRX; }
            set
            {
                _selectedExpNOStr_BRX = value;
                RaisePropertyChanged(() => SelectedExpNOStr_BRX);
            }
        }

        /// <summary>
        /// 将要复制的测试新试验编号
        /// </summary>
        private string _expNOCopyNew_BRX = "";
        /// <summary>
        /// 将要复制的测试新试验编号
        /// </summary>
        public string ExpNOCopyNew_BRX
        {
            get { return _expNOCopyNew_BRX; }
            set
            {
                _expNOCopyNew_BRX = value;
                RaisePropertyChanged(() => ExpNOCopyNew_BRX);
            }
        }

        /// <summary>
        /// 将要新增的测试试验编号
        /// </summary>
        private string _expNONew_BRX = "";
        /// <summary>
        /// 将要新增的测试试验编号
        /// </summary>
        public string ExpNONew_BRX
        {
            get { return _expNONew_BRX; }
            set
            {
                _expNONew_BRX = value;
                RaisePropertyChanged(() => ExpNONew_BRX);
            }
        }

        #endregion


        /// <summary>
        ///更新试验列表消息消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void ExpBRXTableChanged(BRX.DBDataSet.A10检测试验参数DataTable msg)
        {
            ExpBRXTable = (DBDataSet.A10检测试验参数DataTable)msg.Copy();
        }


        #region 主窗口、装置设定菜单、按钮操作指令消息

        /// <summary>
        /// 传递主窗口指令
        /// </summary>
        private RelayCommand<String> _mainWinCommand;
        /// <summary>
        /// 传递主窗口指令
        /// </summary>
        public RelayCommand<String> MainWinCommand
        {
            get
            {
                if (_mainWinCommand == null)
                    _mainWinCommand = new RelayCommand<String>((p) => ExecuteMainWinCMD(p));
                return _mainWinCommand;

            }
            set { _mainWinCommand = value; }
        }

        /// <summary>
        /// 主窗口指令回调。根据操作传递消息
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteMainWinCMD(String num)
        {
            int i = Convert.ToInt16(num);

            try
            {
                //紧急停机
                if (i == 911)
                {
                    Messenger.Default.Send<int>(911, "JJTJMessage");
                }
                //紧急停机复位
                if (i == 918)
                {
                    Messenger.Default.Send<int>(918, "JJTJMessage");
                }

                else if (i == 8699)     //备份数据
                {
                    Messenger.Default.Send<string>("All", "DataBackUpMessage");
                }


                #region 打开、关闭窗口操作

                //调试


                //装置设定窗口
                else if (i == 5141) //调零标定
                {
                    Messenger.Default.Send<string>(WinNames.AIOCalWinName, "OpenGivenNameWin");
                }
                //校准
                else if (i == 5111) //炉壁校准
                {
                    if (Dev.IsBusy && (Dev.RunMode != RunMode.WallCal_Mode))
                    {
                        MessageBox.Show("装置忙，请先其它试验再打开炉壁校准窗口！");
                        return;
                    }
                    Messenger.Default.Send<string>(WinNames.WallCalWinName, "OpenGivenNameWin");
                }
                else if (i == 5112) //炉内校准
                {
                    if (Dev.IsBusy && (Dev.RunMode != RunMode.CenterCal_Mode))
                    {
                        MessageBox.Show("装置忙，请先其它试验再打开炉内校准窗口！");
                        return;
                    }
                    Messenger.Default.Send<string>(WinNames.CenterCalWinName, "OpenGivenNameWin");
                }
                //装置参数
                else if (i == 5151)     //装置基本信息
                {
                    Messenger.Default.Send<string>(WinNames.BasicInfoWinName, "OpenGivenNameWin");
                }
                else if (i == 5152)     //装置基本参数
                {
                    Messenger.Default.Send<string>(WinNames.BasicParamWinName, "OpenGivenNameWin");
                }
                else if (i == 5153)     //公司信息
                {
                    Messenger.Default.Send<string>(WinNames.CoInfoWinName, "OpenGivenNameWin");
                }
                else if (i == 5154)     //通讯参数
                {
                    Messenger.Default.Send<string>(WinNames.ComSetWinName, "OpenGivenNameWin");
                }
                else if (i == 5155)     //PID参数
                {
                    Messenger.Default.Send<string>(WinNames.PidSetWinName, "OpenGivenNameWin");
                }

                else if (i == 5157)     //模拟量参数
                {
                    Messenger.Default.Send<string>(WinNames.AIOParamWinName, "OpenGivenNameWin");
                }

                else if (i == 5158)     //模拟量通道参数
                {
                    Messenger.Default.Send<string>(WinNames.AIOChannelWinName, "OpenGivenNameWin");
                }

                //所有子窗口
                else if (i == 5299)     //所有子窗口
                {
                    Messenger.Default.Send<string>("All", "CloseGivenNameWin");
                }

                #endregion


                #region 检测试验管理

                //新建测试试验
                if (i == 2101)
                {
                    if (ExpNONew_BRX != "")
                        Messenger.Default.Send<string>(ExpNONew_BRX, "NewExpBRXMessage");
                }

                //删除选中的测试试验
                else if (i == 2201)
                {
                    Messenger.Default.Send<string>(SelectedExpNOStr_BRX, "DelExpBRXMessage");
                }

                //载入选中的测试试验
                else if (i == 2301)
                {
                    //装置忙，无法载入
                    if (Dev.IsBusy)
                    {
                        MessageBox.Show("装置忙，请先停止正在进行的检测或退出软件后重新打开！", "错误提示");
                    }
                    else
                    {
                        Messenger.Default.Send<string>(SelectedExpNOStr_BRX, "LoadExpBRXMessage");
                    }
                }

                //关闭当前测试试验
                else if (i == 2401)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("确认关闭当前试验并载入默认试验？", "提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.Yes)
                    {
                        //装置忙
                        if (Dev.IsBusy)
                        {
                            MessageBox.Show("装置忙，请先停止正在进行的检测，或退出软件后重新打开！", "错误提示");
                        }
                        else
                        {
                            Messenger.Default.Send<string>("DefaultExp", "LoadExpBRXMessage");
                        }
                    }
                }

                //保存当前试验
                else if (i == 2501)
                {
                    if (Dev.IsBusy)
                    {
                        MessageBox.Show("装置忙，请先停止检测再修改试验参数！", "提示", MessageBoxButton.OK);
                        return;
                    }
                    //ExpBRXDQ.ExpSet();
                    Messenger.Default.Send<string>("SaveExpBRX", "SaveExpBRXMessage");
                    Messenger.Default.Send<string>(ExpBRXDQ.ExpNO, "LoadExpBRXMessage");

                    MessageBox.Show("修改关键参数，需要退出后重新打开软件才能生效！", "提示", MessageBoxButton.OK);
                }

                //取消修改
                else if (i == 2601)
                {
                    //装置忙，无法载入
                    if (Dev.IsBusy)
                    {
                        MessageBox.Show("装置忙，请先停止正在进行的检测或退出软件后重新打开！", "错误提示");
                    }
                    else
                    {
                        Messenger.Default.Send<string>(ExpBRXDQ.ExpNO, "LoadExpBRXMessage");
                    }
                }

                //复制选中的测试试验
                else if (i == 2701)
                {
                    if (ExpNOCopyNew_BRX != "")
                    {
                        string[] msg = new String[2] { SelectedExpNOStr_BRX, ExpNOCopyNew_BRX };
                        Messenger.Default.Send<string[]>(msg, "CopyExpBRXMessage");
                    }
                }

                #endregion


                #region 试验操作

                if (i == 1201)      //取消当前检测
                {
                    if (Dev.RunMode == Enums.RunMode.Exp_BRXMode || Dev.RunMode == RunMode.ID_Mode)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("取消检测将丢失已检测的数据，是否取消当前检测？", "提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.Yes)
                        {
                            Messenger.Default.Send<int>(1, "StopMessage");
                        }
                    }
                }

                else if (i == 1101)      //开始检测
                {
                    if (ExpBRXDQ.ExpNO == "DefaultExp")
                    {
                        MessageBox.Show("默认试验不能进行试验操作");
                        return;
                    }
                    if (ExpBRXDQ.ExpNO == "FactoryExp")
                    {
                        MessageBox.Show("工厂试验不能进行试验操作");
                        return;
                    }

                    if (Dev.IsBusy && ExpBRXDQ.SpDQ.TimeSp.IsStarted)
                    {
                        MessageBox.Show(ExpBRXDQ.SpNoDQ + "号试样正在检测，请耐心等待！");
                        return;
                    }

                    if (ExpBRXDQ.SpDQ.IsCompleted)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show(ExpBRXDQ.SpNoDQ + "号试样检测已完成。若重新开始试验，将清空已完成的数据，是否重新试验？", "数据清空提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.No)
                            return;
                    }
                    Messenger.Default.Send<int>(1, "StartTestMessage");
                }

                else if (i == 1301)      //放入试样
                {
                    if (Dev.RunMode == Enums.RunMode.Exp_BRXMode)
                    {
                        if (Dev.IsBusy && ExpBRXDQ.SpDQ.TimeTest30.IsStarted)
                        {
                            MessageBox.Show("正式检测已开始，无需重复操作！");
                            return;
                        }
                        if (!Dev.T1StbBalanceEst.IsStabilized)
                        {
                            MessageBox.Show("T1温度未满足控温误差要求，请等待！");
                            return;
                        }
                        if (!Dev.T1StbBalanceEst.IsDriftMeetsReqs)
                        {
                            MessageBox.Show("T1温度漂移未满足平衡要求，请等待！");
                            return;
                        }
                        if (!Dev.T1StbBalanceEst.IsDeviationMeetsReqs)
                        {
                            MessageBox.Show("T1温度最大偏差未满足平衡要求，请等待！");
                            return;
                        }

                        if (Dev.IsStd2023)
                        {
                            if (!Dev.T2StbBalanceEst.IsStabilized)
                            {
                                MessageBox.Show("T2温度未满足控温误差要求，请等待！");
                                return;
                            }
                            if (!Dev.T2StbBalanceEst.IsDriftMeetsReqs)
                            {
                                MessageBox.Show("T2温度漂移未满足平衡要求，请等待！");
                                return;
                            }
                            if (!Dev.T2StbBalanceEst.IsDeviationMeetsReqs)
                            {
                                MessageBox.Show("T2温度最大偏差未满足平衡要求，请等待！");
                                return;
                            }
                        }

                        Messenger.Default.Send<int>(2, "StartTestMessage");
                    }
                    else
                        MessageBox.Show("当前非检测试验模式！");

                }

                else if (i == 1401)      //出现火焰
                {
                    if (Dev.RunMode == Enums.RunMode.Exp_BRXMode)
                    {
                        if (Dev.IsBusy && !ExpBRXDQ.SpDQ.TimeTest30.IsStarted)
                        {
                            MessageBox.Show("试样未放入！");
                            return;
                        }

                        Messenger.Default.Send<int>(1, "FiredMessage");
                    }
                }

                else if (i == 1501)      //火焰熄灭
                {
                    if (Dev.RunMode == Enums.RunMode.Exp_BRXMode)
                    {
                        if (Dev.IsBusy && !ExpBRXDQ.SpDQ.TimeTest30.IsStarted)
                        {
                            MessageBox.Show("试样未放入！");
                            return;
                        }
                        if (Dev.IsBusy && !Dev.IsFired)
                        {
                            MessageBox.Show("尚未确认火焰出现！");
                            return;
                        }

                        Messenger.Default.Send<int>(0, "FiredMessage");
                    }
                }

                else if (i == 1601)      //上一个试样
                {
                    if (Dev.IsNotBusy)
                    {
                        int noNext;
                        int noDQ = ExpBRXDQ.SpNoDQ;
                        if (noDQ <= 1)
                            noNext = ExpBRXDQ.SpList.Count;
                        else
                            noNext = noDQ - 1;
                        ExpBRXDQ.SpDQ = ExpBRXDQ.SpList[noNext - 1];
                        ExpBRXDQ.SpNoDQ = ExpBRXDQ.SpDQ.SpNO;
                    }
                }

                else if (i == 1701)      //下一个试样
                {
                    if (Dev.IsNotBusy)
                    {
                        int noNext;
                        int noDQ = ExpBRXDQ.SpNoDQ;
                        if (noDQ >= ExpBRXDQ.SpList.Count)
                            noNext = 1;
                        else
                            noNext = noDQ + 1;
                        ExpBRXDQ.SpDQ = ExpBRXDQ.SpList[noNext - 1];
                        ExpBRXDQ.SpNoDQ = ExpBRXDQ.SpDQ.SpNO;
                    }
                }

                #endregion


                #region 调试指令

                else if (i == 4299)      //停止调试
                {
                    VO_SDTS = 0;
                    if ((Dev.RunMode == Enums.RunMode.SDDbg_Mode) || (Dev.RunMode == Enums.RunMode.TPID_Mode) || (Dev.RunMode == Enums.RunMode.PowerPID_Mode) || (Dev.RunMode == Enums.RunMode.ID_Mode))
                    {
                        Messenger.Default.Send<int>(1, "StopMessage");
                    }
                }

                //切换模式
                else if (i == 4301)
                {
                    if (Dev.IsBusy)
                    {
                        if (Dev.RunMode == RunMode.SDDbg_Mode)
                            MessageBox.Show("手动调试模式已启动，无需重复操作！");
                        else
                            MessageBox.Show("装置占用中，请先停止其它试验或调试！");
                        return;
                    }
                    Messenger.Default.Send<RunMode>(RunMode.SDDbg_Mode, "DevModeChangeMessage");
                }
                else if (i == 4302)
                {
                    if (Dev.IsBusy)
                    {
                        if (Dev.RunMode == RunMode.TPID_Mode)
                            MessageBox.Show("PID参数调试模式已启动，无需重复操作！");
                        else
                            MessageBox.Show("装置占用中，请先停止其它试验或调试！");
                        return;
                    }

                    Dev.PID_T_Err50Up.Reset();
                    Messenger.Default.Send<RunMode>(RunMode.TPID_Mode, "DevModeChangeMessage");
                }
                else if (i == 4304)
                {
                    if (Dev.IsBusy)
                    {
                        if (Dev.RunMode == RunMode.PowerPID_Mode)
                            MessageBox.Show("功率PID调试模式已启动，无需重复操作！");
                        else
                            MessageBox.Show("装置占用中，请先停止其它试验或调试！");
                        return;
                    }

                    Dev.PID_Power.Reset();
                    Messenger.Default.Send<RunMode>(RunMode.PowerPID_Mode, "DevModeChangeMessage");
                }

                //发送手动电压比例
                else if (i == 4401)
                {
                    if (Dev.RunMode == RunMode.SDDbg_Mode)
                    {
                        if (VO_SDTS < 0)
                            VO_SDTS = 0;
                        if (VO_SDTS > 100)
                            VO_SDTS = 100;

                        Messenger.Default.Send<double>(VO_SDTS, "SDDbgMessage");
                    }
                }

                //发送温控PID参数
                else if (i == 4402)
                {
                    if (Dev.RunMode == RunMode.TPID_Mode)
                    {
                        //给定值，kp,ki,kd,输出上限，积分限幅，积分分离误差，控制误差
                        SDPIDOrderArray[0] = TGiven;
                        SDPIDOrderArray[1] = Kp;
                        SDPIDOrderArray[2] = Ki;
                        SDPIDOrderArray[3] = Kd;
                        SDPIDOrderArray[4] = ULimit;
                        SDPIDOrderArray[5] = Dev.PID_T_Err50Up.PID_Param.U_IMax_Limit;
                        SDPIDOrderArray[6] = Dev.PID_T_Err50Up.PID_Param.ErrBound_IntegralSeparate;
                        SDPIDOrderArray[7] = Err;
                        Messenger.Default.Send<double[]>(SDPIDOrderArray, "PIDDbgMessage");
                    }
                }

                //发送功率PID参数
                else if (i == 4403)
                {
                    if (Dev.RunMode == RunMode.PowerPID_Mode)
                    {
                        //给定值，kp,ki,kd,输出上限，积分限幅，积分分离误差，控制误差
                        SDPIDOrderArrayPower[0] = GivenPower;
                        SDPIDOrderArrayPower[1] = KpPower;
                        SDPIDOrderArrayPower[2] = KiPower;
                        SDPIDOrderArrayPower[3] = KdPower;
                        SDPIDOrderArrayPower[4] = ULimitPower;
                        SDPIDOrderArrayPower[5] = Dev.PID_Power.PID_Param.U_IMax_Limit;
                        SDPIDOrderArrayPower[6] = Dev.PID_Power.PID_Param.ErrBound_IntegralSeparate;
                        SDPIDOrderArrayPower[7] = ErrPower;
                        Messenger.Default.Send<double[]>(SDPIDOrderArrayPower, "PowerPIDDbgMessage");
                    }
                }

                if (i == 4602)      //保存温控PID设置
                {
                    SetPIDParam();
                    var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Messenger.Default.Send<string>("SaveDevSettings", "DevDataRWMessage");
                    }
                    ));
                    task.Completed += new EventHandler(Task_Completed);

                }
                if (i == 4502)      //载入温控PID设置
                {
                    var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Messenger.Default.Send<string>("LoadDevSettings", "DevDataRWMessage");
                    }
                    ));
                    task.Completed += new EventHandler(Task_Completed);

                    LoadPIDParam();
                }

                if (i == 4603)      //保存功率PID设置
                {
                    SetPIDParamPower();
                    var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Messenger.Default.Send<string>("SaveDevSettings", "DevDataRWMessage");
                    }
                    ));
                    task.Completed += new EventHandler(Task_Completed);

                }
                if (i == 4503)      //载入功率PID设置
                {
                    var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Messenger.Default.Send<string>("LoadDevSettings", "DevDataRWMessage");
                    }
                    ));
                    task.Completed += new EventHandler(Task_Completed);

                    LoadPIDParamPower();
                }
                #endregion


                #region 试验数据相关指令


                //保存当前试验数据
                else if (i == 3101)
                {
                    Messenger.Default.Send<string>("SaveDataExpBRX", "SaveExpBRXMessage");
                }

                //计算测试试验数据
                if (i == 3301)
                {
                    var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Messenger.Default.Send<string>("ExpBRXData", "CalcExpDataMessage");
                    }));
                }

                //重置数据（清除数据）
                if (i == 3401)
                {
                    if (Dev.IsBusy)
                    {
                        MessageBox.Show("测试进行中，等测试完成后重试，或取消测试后重试。");
                        return;
                    }

                    MessageBoxResult msgBoxResult = MessageBox.Show("重置数据会删除所有机组的测试数据，请慎重操作。确认清除请按“是”，不清除请按“否”", "警告！", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                        return;

                    ExpBRXDQ.DataReset();
                    for (int j = 0; j < ExpBRXDQ.SpList.Count; j++)
                    {
                        ExpBRXDQ.SpList[j].DataReset();
                        Messenger.Default.Send<int>(ExpBRXDQ.SpList[j].SpNO, "SpInitRecClearMessage");
                    }
                    ;
                    Messenger.Default.Send<string>("SaveDataExpBRX", "SaveExpBRXMessage");
                }

                //导出测试试验报告
                if (i == 3501)
                {
                    Messenger.Default.Send<string>("BRXDQ", "ExportRPTMessage");
                }

                #endregion


                #region 装置设定

                //保存
                if (i == 8299)
                {
                    var task1 = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Messenger.Default.Send<string>("SaveDevSettings", "DevDataRWMessage");
                    }
                    ));
                    task1.Completed += new EventHandler(Task_Completed);
                }

                //取消修改（重新载入）
                if (i == 8199)
                {
                    var task1 = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Messenger.Default.Send<string>("LoadDevSettings", "DevDataRWMessage");
                    }
                    ));
                    task1.Completed += new EventHandler(Task_Completed);
                }

                //一键调零，质量流量检测
                if (i == 8501)
                {
                    Messenger.Default.Send<int>(1, "AutoZeroMessage");
                }
                if (i == 8502)
                {
                    Messenger.Default.Send<int>(2, "AutoZeroMessage");
                }
                if (i == 8503)
                {
                    Messenger.Default.Send<int>(3, "AutoZeroMessage");
                }
                if (i == 8504)
                {
                    Messenger.Default.Send<int>(4, "AutoZeroMessage");
                }
                //if (i == 8505)
                //{
                //    Messenger.Default.Send<int>(5, "AutoZeroMessage");
                //}
                if (i == 8506)
                {
                    Messenger.Default.Send<int>(6, "AutoZeroMessage");
                }
                if (i == 8507)
                {
                    Messenger.Default.Send<int>(7, "AutoZeroMessage");
                }
                if (i == 8508)
                {
                    Messenger.Default.Send<int>(8, "AutoZeroMessage");
                }
                if (i == 8509)
                {
                    Messenger.Default.Send<int>(9, "AutoZeroMessage");
                }
                if (i == 8510)
                {
                    Messenger.Default.Send<int>(10, "AutoZeroMessage");
                }
                if (i == 8511)
                {
                    Messenger.Default.Send<int>(11, "AutoZeroMessage");
                }
                if (i == 8512)
                {
                    Messenger.Default.Send<int>(12, "AutoZeroMessage");
                }
                if (i == 8513)
                {
                    Messenger.Default.Send<int>(13, "AutoZeroMessage");
                }
                if (i == 8514)
                {
                    Messenger.Default.Send<int>(14, "AutoZeroMessage");
                }
                if (i == 8515)
                {
                    Messenger.Default.Send<int>(15, "AutoZeroMessage");
                }

                #endregion


                #region 管理员登录



                //管理员登录
                else if (i == 8888)
                {
                    if (Dev.PassWord == "brt12345678")
                    {
                        Dev.IsAdmin = true;
                        MessageBox.Show("管理员登录成功，配置完成后请退出登录或重启软件！");
                    }
                    else
                    {
                        Dev.IsAdmin = false;
                        MessageBox.Show("管理员密码错误！");
                    }
                }
                //管理员退出登录
                else if (i == 8889)
                {
                    Dev.PassWord = "";
                    Messenger.Default.Send<string>("", "PasswordChanged");
                    Dev.IsAdmin = false;
                }

                #endregion

                else if (i == 4303)      //系统辨识
                {
                    Messenger.Default.Send<int>(1, "StartIDMessage");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region 管理员登录消息

        /// <summary>
        /// 管理员登录消息
        /// </summary>
        /// <param name="msg"></param>
        private void AdminLogginMessage(string msg)
        {
            if (Dev.PassWord == "brt12345678")
            {
                Dev.IsAdmin = true;
                MessageBox.Show("管理员登录成功，配置完成后请退出登录或重启软件！");
            }
            else
            {
                Dev.IsAdmin = false;
                MessageBox.Show("管理员密码错误！");
            }
        }

        #endregion

        #region 手动、温控调试用

        /// <summary>
        /// 输出电压百分比手动值
        /// </summary>
        private double _vo_SDTS = 0.0;
        /// <summary>
        /// 输出电压百分比手动值
        /// </summary>
        public double VO_SDTS
        {
            get { return _vo_SDTS; }
            set
            {
                _vo_SDTS = value;
                RaisePropertyChanged(() => VO_SDTS);
            }
        }

        /// <summary>
        /// 手动PID参数数据数组（给定值，kp,ki,kd,输出限幅，积分限幅，积分分离误差，控制误差）
        /// </summary>
        private double[] _sdPIDOrderArray = { 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 手动PID参数数据数组（给定值，kp,ki,kd,输出限幅，积分限幅，积分分离误差，控制误差）
        /// </summary>
        public double[] SDPIDOrderArray
        {
            get { return _sdPIDOrderArray; }
            set
            {
                _sdPIDOrderArray = value;
                RaisePropertyChanged(() => SDPIDOrderArray);
            }
        }

        /// <summary>
        /// 给定温度
        /// </summary>
        private double _tGiven = 0;
        /// <summary>
        /// 给定温度
        /// </summary>
        public double TGiven
        {
            get { return _tGiven; }
            set
            {
                _tGiven = value;
                RaisePropertyChanged(() => TGiven);
            }
        }

        /// <summary>
        /// kp
        /// </summary>
        private double _kp = 0;
        /// <summary>
        /// kp
        /// </summary>
        public double Kp
        {
            get { return _kp; }
            set
            {
                _kp = Math.Abs(value);
                RaisePropertyChanged(() => Kp);
            }
        }

        /// <summary>
        /// ki
        /// </summary>
        private double _ki = 0;
        /// <summary>
        /// ki
        /// </summary>
        public double Ki
        {
            get { return _ki; }
            set
            {
                _ki = Math.Abs(value);
                RaisePropertyChanged(() => Ki);
            }
        }

        /// <summary>
        /// kd
        /// </summary>
        private double _kd = 0;
        /// <summary>
        /// kd
        /// </summary>
        public double Kd
        {
            get { return _kd; }
            set
            {
                _kd = Math.Abs(value);
                RaisePropertyChanged(() => Kd);
            }
        }

        /// <summary>
        /// 输出上限
        /// </summary>
        private double _uLimit = 100;
        /// <summary>
        /// 输出上限
        /// </summary>
        public double ULimit
        {
            get { return _uLimit; }
            set
            {
                _uLimit = Math.Abs(value);
                RaisePropertyChanged(() => ULimit);
            }
        }

        /// <summary>
        /// 允许误差
        /// </summary>
        private double _err = 0;
        /// <summary>
        /// 允许误差
        /// </summary>
        public double Err
        {
            get { return _err; }
            set
            {
                _err = Math.Abs(value);
                RaisePropertyChanged(() => Err);
            }
        }

        /// <summary>
        /// 积分限幅
        /// </summary>
        private double _uiLimit = 0;
        /// <summary>
        /// 积分限幅
        /// </summary>
        public double UiLimit
        {
            get { return _uiLimit; }
            set
            {
                _uiLimit = Math.Abs(value);
                RaisePropertyChanged(() => UiLimit);
            }
        }

        /// <summary>
        /// 积分分离误差
        /// </summary>
        private double _err_JFFL = 0;
        /// <summary>
        /// 积分分离误差
        /// </summary>
        public double Err_JFFL
        {
            get { return _err_JFFL; }
            set
            {
                _err_JFFL = Math.Abs(value);
                RaisePropertyChanged(() => Err_JFFL);
            }
        }
        
        /// <summary>
        /// 载入PID参数
        /// </summary>
        private void LoadPIDParam()
        {
            Kp = Dev.PID_T_Err50Up.PID_Param.Kp;
            Ki = Dev.PID_T_Err50Up.PID_Param.Ki;
            Kd = Dev.PID_T_Err50Up.PID_Param.Kd;
            ULimit = Dev.PID_T_Err50Up.PID_Param.U_UpperBound;
            UiLimit = Dev.PID_T_Err50Up.PID_Param.U_IMax_Limit;
            Err_JFFL = Dev.PID_T_Err50Up.PID_Param.ErrBound_IntegralSeparate;
        }

        /// <summary>
        /// 设置PID参数
        /// </summary>
        private void SetPIDParam()
        {
            Dev.PID_T_Err50Up.PID_Param.Kp = Kp;
            Dev.PID_T_Err50Up.PID_Param.Ki = Ki;
            Dev.PID_T_Err50Up.PID_Param.Kd = Kd;
            Dev.PID_T_Err50Up.PID_Param.U_UpperBound = ULimit;
        }

        #endregion


        /// <summary>
        /// 切换试样
        /// </summary>
        /// <param name="msgWindow"></param>
        private void NextSpMessage(int msgNo)
        {
            if ((msgNo > 0) && (msgNo <= ExpBRXDQ.SpList.Count))
            {
                ExpBRXDQ.SpDQ = ExpBRXDQ.SpList[msgNo - 1];
                ExpBRXDQ.SpNoDQ = ExpBRXDQ.SpDQ.SpNO;
            }
        }

        #region 功率PID调试用

        /// <summary>
        /// 功率PID参数数据数组（给定值，kp,ki,kd,输出限幅，积分限幅，积分分离误差，控制误差）
        /// </summary>
        private double[] _sdPIDOrderArrayPower = { 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 功率PID参数数据数组（给定值，kp,ki,kd,输出限幅，积分限幅，积分分离误差，控制误差）
        /// </summary>
        public double[] SDPIDOrderArrayPower
        {
            get { return _sdPIDOrderArrayPower; }
            set
            {
                _sdPIDOrderArrayPower = value;
                RaisePropertyChanged(() => SDPIDOrderArrayPower);
            }
        }

        /// <summary>
        /// 给定功率
        /// </summary>
        private double _givenPower = 0;
        /// <summary>
        /// 给定功率
        /// </summary>
        public double GivenPower
        {
            get { return _givenPower; }
            set
            {
                _givenPower = value;
                RaisePropertyChanged(() => GivenPower);
            }
        }

        /// <summary>
        /// kp功率
        /// </summary>
        private double _kpPower = 0;
        /// <summary>
        /// kp功率
        /// </summary>
        public double KpPower
        {
            get { return _kpPower; }
            set
            {
                _kpPower = Math.Abs(value);
                RaisePropertyChanged(() => KpPower);
            }
        }

        /// <summary>
        /// ki功率
        /// </summary>
        private double _kiPower = 0;
        /// <summary>
        /// ki功率
        /// </summary>
        public double KiPower
        {
            get { return _kiPower; }
            set
            {
                _kiPower = Math.Abs(value);
                RaisePropertyChanged(() => KiPower);
            }
        }

        /// <summary>
        /// kd功率
        /// </summary>
        private double _kdPower = 0;
        /// <summary>
        /// kd功率
        /// </summary>
        public double KdPower
        {
            get { return _kdPower; }
            set
            {
                _kdPower = Math.Abs(value);
                RaisePropertyChanged(() => KdPower);
            }
        }

        /// <summary>
        /// 功率输出上限
        /// </summary>
        private double _uLimitPower = 100;
        /// <summary>
        /// 功率输出上限
        /// </summary>
        public double ULimitPower
        {
            get { return _uLimitPower; }
            set
            {
                _uLimitPower = Math.Abs(value);
                RaisePropertyChanged(() => ULimitPower);
            }
        }

        /// <summary>
        /// 功率允许误差
        /// </summary>
        private double _errPower = 0;
        /// <summary>
        /// 功率允许误差
        /// </summary>
        public double ErrPower
        {
            get { return _errPower; }
            set
            {
                _errPower = Math.Abs(value);
                RaisePropertyChanged(() => ErrPower);
            }
        }

        /// <summary>
        /// 功率积分限幅
        /// </summary>
        private double _uiLimitPower = 0;
        /// <summary>
        /// 功率积分限幅
        /// </summary>
        public double UiLimitPower
        {
            get { return _uiLimitPower; }
            set
            {
                _uiLimitPower = Math.Abs(value);
                RaisePropertyChanged(() => UiLimitPower);
            }
        }

        /// <summary>
        /// 功率积分分离误差
        /// </summary>
        private double _err_JFFLPower = 0;
        /// <summary>
        /// 功率积分分离误差
        /// </summary>
        public double Err_JFFLPower
        {
            get { return _err_JFFLPower; }
            set
            {
                _err_JFFLPower = Math.Abs(value);
                RaisePropertyChanged(() => Err_JFFLPower);
            }
        }

        /// <summary>
        /// 载入功率PID参数
        /// </summary>
        private void LoadPIDParamPower()
        {
            KpPower = Dev.PID_Power.PID_Param.Kp;
            KiPower = Dev.PID_Power.PID_Param.Ki;
            KdPower = Dev.PID_Power.PID_Param.Kd;
            ULimitPower = Dev.PID_Power.PID_Param.U_UpperBound;
            UiLimitPower = Dev.PID_Power.PID_Param.U_IMax_Limit;
            Err_JFFLPower = Dev.PID_Power.PID_Param.ErrBound_IntegralSeparate;
        }

        /// <summary>
        /// 设置功率PID参数
        /// </summary>
        private void SetPIDParamPower()
        {
            Dev.PID_Power.PID_Param.Kp = KpPower;
            Dev.PID_Power.PID_Param.Ki = KiPower;
            Dev.PID_Power.PID_Param.Kd = KdPower;
            Dev.PID_Power.PID_Param.U_UpperBound = ULimitPower;
        }

        #endregion


        #region 语音播放

        /// <summary>
        /// 播放提示语音
        /// </summary>
        private void PlaySoundMessage(int msgNo)
        {
            string voiceDir = Config.VoiceDir;

            switch (msgNo)
            {
                case -1:    //停止播放
                    SoundPlayer.Stop();
                    break;
                case 0:     //实验结束
                    SoundPlayer.Stop();
                    voiceDir += "0.wav";
                    SoundPlayer.SoundLocation = voiceDir;
                    SoundPlayer.LoadAsync();
                    SoundPlayer.PlayLooping();
                    break;
                case 1:     //继续进行1号试样
                    SoundPlayer.Stop();
                    voiceDir += "1.wav";
                    SoundPlayer.SoundLocation = voiceDir;
                    SoundPlayer.LoadAsync();
                    SoundPlayer.PlayLooping();
                    break;
                case 2:
                    SoundPlayer.Stop();
                    voiceDir += "2.wav";
                    SoundPlayer.SoundLocation = voiceDir;
                    SoundPlayer.LoadAsync();
                    SoundPlayer.PlayLooping();
                    break;
                case 3:
                    SoundPlayer.Stop();
                    voiceDir += "3.wav";
                    SoundPlayer.SoundLocation = voiceDir;
                    SoundPlayer.LoadAsync();
                    SoundPlayer.PlayLooping();
                    break;
                case 4:
                    SoundPlayer.Stop();
                    voiceDir += "4.wav";
                    SoundPlayer.SoundLocation = voiceDir;
                    SoundPlayer.LoadAsync();
                    SoundPlayer.PlayLooping();
                    break;
                case 5:
                    SoundPlayer.Stop();
                    voiceDir += "5.wav";
                    SoundPlayer.SoundLocation = voiceDir;
                    SoundPlayer.LoadAsync();
                    SoundPlayer.PlayLooping();
                    break;
                case 6:
                    SoundPlayer.Stop();
                    voiceDir += "6.wav";
                    SoundPlayer.SoundLocation = voiceDir;
                    SoundPlayer.LoadAsync();
                    SoundPlayer.PlayLooping();
                    break;
            }
        }

        /// <summary>
        /// 语音播放器
        /// </summary>
        private SoundPlayer _soundPlayer = new SoundPlayer();
        /// <summary>
        /// 绘图用曲线组
        /// </summary>
        public SoundPlayer SoundPlayer
        {
            get { return _soundPlayer; }
            set
            {
                _soundPlayer = value;
                RaisePropertyChanged(() => SoundPlayer);
            }
        }

        #endregion


        #region 绘图用

        /// <summary>
        /// 绘图用曲线组
        /// </summary>
        private List<LineModel> _plotLines;
        /// <summary>
        /// 绘图用曲线组
        /// </summary>
        public List<LineModel> PlotLines
        {
            get { return _plotLines; }
            set
            {
                _plotLines = value;
                RaisePropertyChanged(() => PlotLines);
            }
        }

        /// <summary>
        /// 试验数据温升用曲线组
        /// </summary>
        private List<LineModel> _plotLinesWS = new List<LineModel>();
        /// <summary>
        /// 试验数据温升用曲线组
        /// </summary>
        public List<LineModel> PlotLinesWS
        {
            get { return _plotLinesWS; }
            set
            {
                _plotLinesWS = value;
                RaisePropertyChanged(() => PlotLinesWS);
            }
        }

        /// <summary>
        /// 760、740上下界
        /// </summary>
        private List<LineModel> _lineMaxMin = new List<LineModel>();
        /// <summary>
        /// 760、740上下界
        /// </summary>
        public List<LineModel> LineMaxMin
        {
            get { return _lineMaxMin; }
            set
            {
                _lineMaxMin = value;
                RaisePropertyChanged(() => LineMaxMin);
            }
        }

        /// <summary>
        /// 绘图曲线初始化
        /// </summary>
        public void PlotInit()
        {
            PlotLines = new List<LineModel>();
            //炉内温度T1
            LineModel newLine = new LineModel
            {
                LineNO = "T1",
                LineName = "炉内温度1",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //炉内温度T2
            newLine = new LineModel
            {
                LineNO = "T2",
                LineName = "炉内温度2",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //试样中心温度Tsc
            newLine = new LineModel
            {
                LineNO = "Tsc",
                LineName = "试样中心温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //试样表面温度Tss
            newLine = new LineModel
            {
                LineNO = "Tss",
                LineName = "试样表面温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //标定炉壁温度Tw1
            newLine = new LineModel
            {
                LineNO = "Tw1",
                LineName = "标定炉壁温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //标定炉壁温度Tw2
            newLine = new LineModel
            {
                LineNO = "Tw2",
                LineName = "标定炉壁温度1",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //标定炉壁温度Tw3
            newLine = new LineModel
            {
                LineNO = "Tw3",
                LineName = "标定炉壁温度3",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //标定炉内温度Tfc
            newLine = new LineModel
            {
                LineNO = "Tfc",
                LineName = "标定炉内温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //输出电压百分比
            newLine = new LineModel
            {
                LineNO = "Vo",
                LineName = "电压百分比",
                VarUnit = "%",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //输出电压
            newLine = new LineModel
            {
                LineNO = "Vo",
                LineName = "电压",
                VarUnit = "V",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //输出功率
            newLine = new LineModel
            {
                LineNO = "Power",
                LineName = "功率",
                VarUnit = "W",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //PID输出Up
            newLine = new LineModel
            {
                LineNO = "Up",
                LineName = "Up",
                VarUnit = "",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //PID输出Ui
            newLine = new LineModel
            {
                LineNO = "Ui",
                LineName = "Ui",
                VarUnit = "",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //PID输出Ud
            newLine = new LineModel
            {
                LineNO = "Ud",
                LineName = "Ud",
                VarUnit = "",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);
            //PID输出Usum
            newLine = new LineModel
            {
                LineNO = "Usum",
                LineName = "Usum",
                VarUnit = "",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);

            //T2-T1
            newLine = new LineModel
            {
                LineNO = "T2-T1",
                LineName = "T2-T1",
                VarUnit = "",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);

            //采集电压U
            newLine = new LineModel
            {
                LineNO = "U",
                LineName = "U",
                VarUnit = "",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);


            //采集电流I
            newLine = new LineModel
            {
                LineNO = "I",
                LineName = "I",
                VarUnit = "",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);


            //采集功率P
            newLine = new LineModel
            {
                LineNO = "P",
                LineName = "P",
                VarUnit = "",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLines.Add(newLine);

            //755最大值==============================================
            newLine = new LineModel
            {
                LineNO = "755",
                LineName = "755",
                VarUnit = "",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            LineMaxMin.Add(newLine);
            //745最小值
            newLine = new LineModel
            {
                LineNO = "745",
                LineName = "745",
                VarUnit = "",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            LineMaxMin.Add(newLine);

            //试样1-炉内1温升===============
            LineModel newLineWS11 = new LineModel
            {
                LineNO = "Tn1",
                LineName = "炉内温度1",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS11);
            //试样1-炉内2温升
            LineModel newLineWS12 = new LineModel
            {
                LineNO = "Tn2",
                LineName = "炉内温度2",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS12);
            //试样1-试样中心温升
            LineModel newLineWS13 = new LineModel
            {
                LineNO = "Tsc",
                LineName = "试样中心温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS13);
            //试样1-试样表面温升
            LineModel newLineWS14 = new LineModel
            {
                LineNO = "Tsf",
                LineName = "试样表面温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS14);
            //试样2-炉内1温升===============
            LineModel newLineWS21 = new LineModel
            {
                LineNO = "Tn1",
                LineName = "炉内温度1",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS21);
            //试样2-炉内2温升
            LineModel newLineWS22 = new LineModel
            {
                LineNO = "Tn2",
                LineName = "炉内温度2",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS22);
            //试样2-试样中心温升
            LineModel newLineWS23 = new LineModel
            {
                LineNO = "Tsc",
                LineName = "试样中心温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS23);
            //试样2-试样表面温升
            LineModel newLineWS24 = new LineModel
            {
                LineNO = "Tsf",
                LineName = "试样表面温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS24);
            //试样3-炉内1温升===============
            LineModel newLineWS31 = new LineModel
            {
                LineNO = "Tn1",
                LineName = "炉内温度1",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS31);
            //试样3-炉内2温升
            LineModel newLineWS32 = new LineModel
            {
                LineNO = "Tn2",
                LineName = "炉内温度2",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS32);
            //试样3-试样中心温升
            LineModel newLineWS33 = new LineModel
            {
                LineNO = "Tsc",
                LineName = "试样中心温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS33);
            //试样3-试样表面温升
            LineModel newLineWS34 = new LineModel
            {
                LineNO = "Tsf",
                LineName = "试样表面温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS34);
            //试样4-炉内1温升===============
            LineModel newLineWS41 = new LineModel
            {
                LineNO = "Tn1",
                LineName = "炉内温度1",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS41);
            //试样4-炉内2温升
            LineModel newLineWS42 = new LineModel
            {
                LineNO = "Tn2",
                LineName = "炉内温度2",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS42);
            //试样4-试样中心温升
            LineModel newLineWS43 = new LineModel
            {
                LineNO = "Tsc",
                LineName = "试样中心温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS43);
            //试样4-试样表面温升
            LineModel newLineWS44 = new LineModel
            {
                LineNO = "Tsf",
                LineName = "试样表面温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS44);
            //试样5-炉内1温升===============
            LineModel newLineWS51 = new LineModel
            {
                LineNO = "Tn1",
                LineName = "炉内温度1",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS51);
            //试样5-炉内2温升
            LineModel newLineWS52 = new LineModel
            {
                LineNO = "Tn2",
                LineName = "炉内温度2",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS52);
            //试样5-试样中心温升
            LineModel newLineWS53 = new LineModel
            {
                LineNO = "Tsc",
                LineName = "试样中心温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS53);
            //试样5-试样表面温升
            LineModel newLineWS54 = new LineModel
            {
                LineNO = "Tsf",
                LineName = "试样表面温度",
                VarUnit = "℃",
                LineThickness = 0.8,
                LineColor = Color.Red,
                AxisYside = LineModel.AxisUse.Left,
                LineDataSource = new ObservableDataSource<System.Windows.Point>(),
                LineBackGroundPointList = new List<System.Windows.Point>(),
                MaxPointsQuantity = Dev.PointsPerLine,
                MaxBSPointsQuantity = Dev.PointsPerLine,
                IsBackStore = false
            };
            PlotLinesWS.Add(newLineWS54);
        }

        /// <summary>
        /// 曲线更新定时器
        /// </summary>
        DispatcherTimer PlotTimer = new DispatcherTimer();

        /// <summary>
        /// 曲线更新定时器回调函数。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlotTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan span = DateTime.Now - Dev.TimePowerOn;

            //炉内温度T1
            Point newPoint1 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[0].ValueFinal
            };
            PlotLines[0].AddPoint(newPoint1);
            //炉内温度T2
            Point newPoint2 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[1].ValueFinal
            };
            PlotLines[1].AddPoint(newPoint2);
            //试样中心温度Tsc
            Point newPoint3 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[2].ValueFinal
            };
            PlotLines[2].AddPoint(newPoint3);
            //试样表面温度Tss
            Point newPoint4 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[3].ValueFinal
            };
            PlotLines[3].AddPoint(newPoint4);
            //标定炉壁温度Tw1
            Point newPoint5 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[5].ValueFinal
            };
            PlotLines[4].AddPoint(newPoint5);
            //标定炉壁温度Tw2
            Point newPoint6 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[6].ValueFinal
            };
            PlotLines[5].AddPoint(newPoint6);
            //标定炉壁温度Tw3
            Point newPoint7 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[7].ValueFinal
            };
            PlotLines[6].AddPoint(newPoint7);
            //标定炉内温度Tfc
            Point newPoint8 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[4].ValueFinal
            };
            PlotLines[7].AddPoint(newPoint8);

            //输出电压比例
            Point newPoint9 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AOList[0].ValueFinal
            };
            PlotLines[8].AddPoint(newPoint9);
            //输出电压
            Point newPoint10 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.VO_110VBase
            };
            PlotLines[9].AddPoint(newPoint10);
            //输出功率
            Point newPoint11 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.Power_110VBase
            };
            PlotLines[10].AddPoint(newPoint11);

            //Up
            Point newPoint12 = new Point
            {
                X = span.TotalSeconds,
                Y = BLL.PID_T_BLL.UK_P
            };
            PlotLines[11].AddPoint(newPoint12);
            //Ui
            Point newPoint13 = new Point
            {
                X = span.TotalSeconds,
                Y = BLL.PID_T_BLL.UK_I
            };
            PlotLines[12].AddPoint(newPoint13);
            //Ud
            Point newPoint14 = new Point
            {
                X = span.TotalSeconds,
                Y = BLL.PID_T_BLL.UK_D
            };
            PlotLines[13].AddPoint(newPoint14);
            //Usum
            Point newPoint15 = new Point
            {
                X = span.TotalSeconds,
                Y = BLL.PID_T_BLL.UK
            };
            PlotLines[14].AddPoint(newPoint15);

            //760
            Point newPoint755 = new Point
            {
                X = span.TotalSeconds,
                Y = 760
            };
            LineMaxMin[0].AddPoint(newPoint755);
            //740
            Point newPoint745 = new Point
            {
                X = span.TotalSeconds,
                Y = 740
            };
            LineMaxMin[1].AddPoint(newPoint745);

            //T2-T1
            Point newPoint16 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[1].ValueFinal - Dev.AIList[0].ValueFinal
            };
            PlotLines[15].AddPoint(newPoint16);


            //U
            Point newPoint17 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[8].ValueFinal
            };
            PlotLines[16].AddPoint(newPoint17);
            //I
            Point newPoint18 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[9].ValueFinal
            };
            PlotLines[17].AddPoint(newPoint18);
            //P
            Point newPoint19 = new Point
            {
                X = span.TotalSeconds,
                Y = Dev.AIList[10].ValueFinal
            };
            PlotLines[18].AddPoint(newPoint19);
        }

        #endregion


        #region 温升数据曲线

        /// <summary>
        /// 更新试样温升曲线消息
        /// </summary>
        /// <param name="msgWindow"></param>
        private void UpdateSpWSPlotMessage(string msg)
        {
            try
            {
                int time;
                for (int i = 0; i < PlotLinesWS.Count; i++) //清空
                    PlotLinesWS[i].LineDataSource.Collection.Clear();

                for (int spnum = 0; spnum < ExpBRXDQ.SpList.Count; spnum++)
                {
                    if (ExpBRXDQ.SpList[spnum].IsCompleted)
                    {
                        time = 0;
                        for (int recnum = 0; recnum < ExpBRXDQ.SpList[spnum].RecList_Stb.Count; recnum++)
                        {
                            PlotLinesWS[spnum * 4 + 0].AddPoint(new Point { X = time, Y = ExpBRXDQ.SpList[spnum].RecList_Stb[recnum].T1 }); //炉内1

                            if (Dev.IsStd2023)
                                PlotLinesWS[spnum * 4 + 1].AddPoint(new Point { X = time, Y = ExpBRXDQ.SpList[spnum].RecList_Stb[recnum].T2 }); //炉内2

                            if (ExpBRXDQ.UseAddT)
                            {
                                PlotLinesWS[spnum * 4 + 2].AddPoint(new Point { X = time, Y = ExpBRXDQ.SpList[spnum].RecList_Stb[0].Tsc }); //试样中心
                                PlotLinesWS[spnum * 4 + 3].AddPoint(new Point { X = time, Y = ExpBRXDQ.SpList[spnum].RecList_Stb[0].Tss }); //试样表面
                            }
                            time++;
                        }

                        for (int recnum = 0; recnum < ExpBRXDQ.SpList[spnum].RecList_Test.Count; recnum++)
                        {
                            PlotLinesWS[spnum * 4 + 0].AddPoint(new Point { X = time, Y = ExpBRXDQ.SpList[spnum].RecList_Test[recnum].T1 }); //炉内1

                            if (Dev.IsStd2023)
                                PlotLinesWS[spnum * 4 + 1].AddPoint(new Point { X = time, Y = ExpBRXDQ.SpList[spnum].RecList_Test[recnum].T2 }); //炉内2

                            if (ExpBRXDQ.UseAddT)
                            {
                                PlotLinesWS[spnum * 4 + 2].AddPoint(new Point { X = time, Y = ExpBRXDQ.SpList[spnum].RecList_Test[recnum].Tsc }); //试样中心
                                PlotLinesWS[spnum * 4 + 3].AddPoint(new Point { X = time, Y = ExpBRXDQ.SpList[spnum].RecList_Test[recnum].Tss }); //试样表面
                            }

                            time++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        #endregion


        #region 试验窗口互锁与恢复

        /// <summary>
        /// 测试窗口可用
        /// </summary>
        private bool _isExpBRXWinUsable = true;
        /// <summary>
        /// 测试窗口可用
        /// </summary>
        public bool IsExpBRXWinUsable
        {
            get { return _isExpBRXWinUsable; }
            set
            {
                _isExpBRXWinUsable = value;
                RaisePropertyChanged(() => IsExpBRXWinUsable);
            }
        }

        /// <summary>
        /// 指定窗口关闭时，恢复按钮功能
        /// </summary>
        /// <param name="msg"></param>
        private void WindowClosedMessage(string msg)
        {
            if (msg == WinNames.BRXWinName)
            {
                IsExpBRXWinUsable = true;
            }
        }

        /// <summary>
        /// 试验窗口互锁，绘图数据更新
        /// </summary>
        /// <param name="msgWindow"></param>
        private void WindowCreatedMessage(Window msgWindow)
        {
            if (msgWindow.Name == WinNames.BRXWinName)
            {
                IsExpBRXWinUsable = true;
            }

        }

        #region 窗口可以打开状态属性

        /// <summary>
        /// ExpView可以打开状态
        /// </summary>
        private bool _isExpViewCanBeOpened = true;
        /// <summary>
        /// ExpView可以打开状态
        /// </summary>
        public bool IsExpViewCanBeOpened
        {
            get { return _isExpViewCanBeOpened; }
            set
            {
                _isExpViewCanBeOpened = value;
                RaisePropertyChanged(() => IsExpViewCanBeOpened);
            }
        }


        #endregion

        #endregion


        #region 试验数据学习




        #endregion


        #region ViewModel属性

        /// <summary>
        /// CalViewModel
        /// </summary>
        private CalViewModel _calViewModel;
        /// <summary>
        /// CalViewModel
        /// </summary>
        public CalViewModel CalViewModel
        {
            get { return _calViewModel; }
            set
            {
                _calViewModel = value;
                RaisePropertyChanged(() => CalViewModel);
            }
        }

        #endregion


        #region ViewModel传参创建

        /// <summary>
        /// CalViewModel传参并创建
        /// </summary>
        /// <returns></returns>
        public CalViewModel GetCalViewModel()
        {
            if (CalViewModel == null)
                CalViewModel = new CalViewModel(Dev, WallCalDQ, CenterCalDQ);
            return CalViewModel;
        }

        #endregion

        public void Task_Completed(object sender, EventArgs e)
        {
        }

    }
}

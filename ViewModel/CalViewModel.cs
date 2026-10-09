/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.ViewModel
 * 文件名：  MainViewModel
 * 版本号：  V1.0.0.0
 * 唯一标识：579a7638-2663-4ce7-8f57-2d146c0ae62d
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
using BRX.Model.Exp;
using BRX.DBDataSetTableAdapters;
using BRX.Model.Enums;
using Color = System.Drawing.Color;
using ChartModel;
using System.Windows.Threading;
using System.Collections.Generic;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using LiveCharts;
using System.Linq;
using LiveCharts.Defaults;
using BRX.DAL.CalDAL;
using BRX.DAL.CalRepDAL;

namespace BRX.ViewModel
{
    public class CalViewModel : ViewModelBase
    {
        public CalViewModel(DevModel dev, ExpModel_CalWall wallCal, ExpModel_CalCenter centerCall)
        {
            //有窗口新增或关闭
            Messenger.Default.Register<string>(this, "WindowClosed", WindowClosedMessage);
            Messenger.Default.Register<Window>(this, "NewWindowCreated", WindowCreatedMessage);

            //装置已修改
            Messenger.Default.Register<DevModel>(this, "DevSavedMessage", DevSavedMessage);

            //装置初始化
            Dev = dev;

            //试验初始化
            WallCalDQ = wallCal;
            CenterCalDQ = centerCall;

            //校准读写初始化
            CalDAL = new CalDAL(dev, WallCalDQ, CenterCalDQ);
            CalRepDAL = new CalRepDAL(dev, WallCalDQ, CenterCalDQ);

            if (Dev.IsLoadLastExpPowerOn)
            {
                Messenger.Default.Send<string>(Dev.WallCalNOLast, "LoadWallCalMessage");
                Messenger.Default.Send<string>(Dev.CenterCalNOLast, "LoadCenterCalMessage");
            }
            else
            {
                Messenger.Default.Send<string>("DefaultWallCal", "LoadWallCalMessage");
                Messenger.Default.Send<string>("DefaultCenterCal", "LoadCenterCalMessage");
            }

            CenterCalDQ.Std = Dev.StdSelected;

            A20TableAdapter.Fill(WallCalTable);
            A30TableAdapter.Fill(CenterCalTable);
            //试验列表已更新
            Messenger.Default.Register<DBDataSet.A20炉壁温度校准试验参数DataTable>(this, "WallCalTableChanged", WallCalTableChanged);
            Messenger.Default.Register<DBDataSet.A30炉内温度校准试验参数DataTable>(this, "CenterCalTableChanged", CenterCalTableChanged);
            

            //绘图数据初始化，绘图定时器初始化
            PlotInit();
            PlotTimer.Tick += new EventHandler(PlotTimer_Tick);
            PlotTimer.Interval = TimeSpan.FromMilliseconds(Dev.PlotPeriod);
            PlotTimer.Start();

            LivePloitInit();
            Messenger.Default.Register<string>(this, "CenterCalPointChanged", CenterCalPointChanged);
        }

        #region 装置、试验、数据读写属性

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
        /// 校准数据读写
        /// </summary>
        private CalDAL _calDAL;
        /// <summary>
        /// 校准数据读写
        /// </summary>
        private CalDAL CalDAL
        {
            get { return _calDAL; }
            set
            {
                _calDAL = value;
                RaisePropertyChanged(() => CalDAL);
            }
        }

        /// <summary>
        /// 报告数据读写
        /// </summary>
        private CalRepDAL _calRepDAL;
        /// <summary>
        /// 报告数据读写
        /// </summary>
        private CalRepDAL CalRepDAL
        {
            get { return _calRepDAL; }
            set
            {
                _calRepDAL = value;
                RaisePropertyChanged(() => CalRepDAL);
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


        #region 检测试验管理相关属性

        /// <summary>
        /// 炉壁校准试验列表Table
        /// </summary>
        private BRX.DBDataSet.A20炉壁温度校准试验参数DataTable _wallCalTable = new BRX.DBDataSet.A20炉壁温度校准试验参数DataTable();
        /// <summary>
        /// 炉壁校准试验列表Table
        /// </summary>
        public BRX.DBDataSet.A20炉壁温度校准试验参数DataTable WallCalTable
        {
            get { return _wallCalTable; }
            set
            {
                _wallCalTable = value;
                RaisePropertyChanged(() => WallCalTable);
            }
        }

        /// <summary>
        /// 炉内校准试验列表Table
        /// </summary>
        private BRX.DBDataSet.A30炉内温度校准试验参数DataTable _centerCalTable = new BRX.DBDataSet.A30炉内温度校准试验参数DataTable();
        /// <summary>
        /// 炉内校准试验列表Table
        /// </summary>
        public BRX.DBDataSet.A30炉内温度校准试验参数DataTable CenterCalTable
        {
            get { return _centerCalTable; }
            set
            {
                _centerCalTable = value;
                RaisePropertyChanged(() => CenterCalTable);
            }
        }

        /// <summary>
        /// A20炉壁温度校准试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A20炉壁温度校准试验参数TableAdapter _a20TableAdapter = new A20炉壁温度校准试验参数TableAdapter();

        /// <summary>
        /// A20炉壁温度校准试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A20炉壁温度校准试验参数TableAdapter A20TableAdapter
        {
            get { return _a20TableAdapter; }
            set
            {
                _a20TableAdapter = value;
                RaisePropertyChanged(() => A20TableAdapter);
            }
        }

        /// <summary>
        /// A20炉壁温度校准试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A30炉内温度校准试验参数TableAdapter _a30TableAdapter = new A30炉内温度校准试验参数TableAdapter();

        /// <summary>
        /// A20炉壁温度校准试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A30炉内温度校准试验参数TableAdapter A30TableAdapter
        {
            get { return _a30TableAdapter; }
            set
            {
                _a30TableAdapter = value;
                RaisePropertyChanged(() => A30TableAdapter);
            }
        }

        /// <summary>
        /// 列表中选中的炉壁校准试验编号
        /// </summary>
        private string _selectedNOStr_WallCal = "";
        /// <summary>
        /// 列表中选中的炉壁校准试验编号
        /// </summary>
        public string SelectedNOStr_WallCal
        {
            get { return _selectedNOStr_WallCal; }
            set
            {
                _selectedNOStr_WallCal = value;
                RaisePropertyChanged(() => SelectedNOStr_WallCal);
            }
        }

        /// <summary>
        /// 列表中选中的炉内校准试验编号
        /// </summary>
        private string _selectedNOStr_CenterCal = "";
        /// <summary>
        /// 列表中选中的炉内校准试验编号
        /// </summary>
        public string SelectedNOStr_CenterCal
        {
            get { return _selectedNOStr_CenterCal; }
            set
            {
                _selectedNOStr_CenterCal = value;
                RaisePropertyChanged(() => SelectedNOStr_CenterCal);
            }
        }

        /// <summary>
        /// 将要复制的新炉壁校准试验编号
        /// </summary>
        private string _expNOCopyNew_WallCal = "";
        /// <summary>
        /// 将要复制的新炉壁校准试验编号
        /// </summary>
        public string ExpNOCopyNew_WallCal
        {
            get { return _expNOCopyNew_WallCal; }
            set
            {
                _expNOCopyNew_WallCal = value;
                RaisePropertyChanged(() => ExpNOCopyNew_WallCal);
            }
        }

        /// <summary>
        /// 将要复制的新炉内校准试验编号
        /// </summary>
        private string _expNOCopyNew_CenterCal = "";
        /// <summary>
        /// 将要复制的新炉内校准试验编号
        /// </summary>
        public string ExpNOCopyNew_CenterCal
        {
            get { return _expNOCopyNew_CenterCal; }
            set
            {
                _expNOCopyNew_CenterCal = value;
                RaisePropertyChanged(() => ExpNOCopyNew_CenterCal);
            }
        }

        /// <summary>
        /// 将要新增的炉壁校准试验编号
        /// </summary>
        private string _expNONew_WallCal = "";
        /// <summary>
        /// 将要新增的炉壁校准试验编号
        /// </summary>
        public string ExpNONew_WallCal
        {
            get { return _expNONew_WallCal; }
            set
            {
                _expNONew_WallCal = value;
                RaisePropertyChanged(() => ExpNONew_WallCal);
            }
        }

        /// <summary>
        /// 将要新增的炉内校准试验编号
        /// </summary>
        private string _expNONew_CenterCal = "";
        /// <summary>
        /// 将要新增的炉内校准试验编号
        /// </summary>
        public string ExpNONew_CenterCal
        {
            get { return _expNONew_CenterCal; }
            set
            {
                _expNONew_CenterCal = value;
                RaisePropertyChanged(() => ExpNONew_CenterCal);
            }
        }

        #endregion


        /// <summary>
        ///更新炉壁校准试验列表消息消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void WallCalTableChanged(BRX.DBDataSet.A20炉壁温度校准试验参数DataTable msg)
        {
            WallCalTable = (DBDataSet.A20炉壁温度校准试验参数DataTable)msg.Copy();
        }

        /// <summary>
        ///更新炉内校准试验列表消息消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void CenterCalTableChanged(BRX.DBDataSet.A30炉内温度校准试验参数DataTable msg)
        {
            CenterCalTable = (DBDataSet.A30炉内温度校准试验参数DataTable)msg.Copy();
        }

        /// <summary>
        ///装置参数已修改消息处理
        /// </summary>
        /// <param name="msgWindow"></param>
        private void DevSavedMessage(DevModel msgDev)
        {
            CenterCalDQ.Std = Dev.StdSelected;
        }


        #region 按钮操作指令消息

        /// <summary>
        /// 传递校准窗口指令
        /// </summary>
        private RelayCommand<String> _calWinCommand;
        /// <summary>
        /// 传递校准窗口指令
        /// </summary>
        public RelayCommand<String> CalWinCommand
        {
            get
            {
                if (_calWinCommand == null)
                    _calWinCommand = new RelayCommand<String>((p) => ExecuteCalWinCMD(p));
                return _calWinCommand;

            }
            set { _calWinCommand = value; }
        }

        /// <summary>
        /// 校准窗口指令回调。根据操作传递消息
        /// </summary>
        /// <param name="num">按钮编号</param>
        private void ExecuteCalWinCMD(String num)
        {
            int i = Convert.ToInt16(num);

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

            //校准
            else if (i == 5211)     //炉壁校准
            {
                Messenger.Default.Send<string>(WinNames.WallCalWinName, "CloseGivenNameWin");
            }
            else if (i == 5212)     //炉内校准
            {
                Messenger.Default.Send<string>(WinNames.CenterCalWinName, "CloseGivenNameWin");
            }


            //所有子窗口
            else if (i == 5299)     //所有子窗口
            {
                Messenger.Default.Send<string>("All", "CloseGivenNameWin");
            }

            #endregion


            #region 炉壁校准试验管理

            //新建炉壁校准试验
            if (i == 2102)
            {
                if (ExpNONew_WallCal != "")
                    Messenger.Default.Send<string>(ExpNONew_WallCal, "NewWallCalMessage");
            }

            //删除选中的炉壁校准试验
            else if (i == 2202)
            {
                Messenger.Default.Send<string>(SelectedNOStr_WallCal, "DelWallCalMessage");
            }

            //载入选中的炉壁校准试验
            else if (i == 2302)
            {
                //装置忙，无法载入
                if (Dev.IsBusy)
                {
                    MessageBox.Show("装置忙，请先停止正在进行的检测或退出软件后重新打开！", "错误提示");
                }
                else
                {
                    Messenger.Default.Send<string>(SelectedNOStr_WallCal, "LoadWallCalMessage");
                }
            }

            //关闭当前炉壁校准试验
            else if (i == 2402)
            {
                MessageBoxResult msgBoxResult = MessageBox.Show("确认关闭当前校准试验并载入默认试验？", "提示", MessageBoxButton.YesNo);
                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    //装置忙
                    if (Dev.IsBusy)
                    {
                        MessageBox.Show("装置忙，请先停止正在进行的检测，或退出软件后重新打开！", "错误提示");
                    }
                    else
                    {
                        Messenger.Default.Send<string>("DefaultWallCal", "LoadWallCalMessage");
                    }
                }
            }

            //保存当前炉壁校准试验
            else if (i == 2502)
            {
                if (Dev.IsBusy)
                {
                    MessageBox.Show("装置忙，请先停止检测再修改试验参数！", "提示", MessageBoxButton.OK);
                    return;
                }
                Messenger.Default.Send<string>("SaveWallCal", "SaveWallCalMessage");
                Messenger.Default.Send<string>(WallCalDQ.ExpNO, "LoadWallCalMessage");

                MessageBox.Show("修改关键参数，需要退出后重新打开软件才能生效！", "提示", MessageBoxButton.OK);
            }

            //取消修改
            else if (i == 2602)
            {
                //装置忙，无法载入
                if (Dev.IsBusy)
                {
                    MessageBox.Show("装置忙，请先停止正在进行的检测或退出软件后重新打开！", "错误提示");
                }
                else
                {
                    Messenger.Default.Send<string>(WallCalDQ.ExpNO, "LoadWallCalMessage");
                }
            }

            //复制选中的炉壁校准试验
            else if (i == 2702)
            {
                if (ExpNOCopyNew_WallCal != "")
                {
                    string[] msg = new String[2] { SelectedNOStr_WallCal, ExpNOCopyNew_WallCal };
                    Messenger.Default.Send<string[]>(msg, "CopyWallCalMessage");
                }
            }

            #endregion


            #region 炉内校准试验管理

            //新建炉内校准试验
            if (i == 2103)
            {
                if (ExpNONew_CenterCal != "")
                    Messenger.Default.Send<string>(ExpNONew_CenterCal, "NewCenterCalMessage");
            }

            //删除选中的炉内校准试验
            else if (i == 2203)
            {
                Messenger.Default.Send<string>(SelectedNOStr_CenterCal, "DelCenterCalMessage");
            }

            //载入选中的炉内校准试验
            else if (i == 2303)
            {
                //装置忙，无法载入
                if (Dev.IsBusy)
                {
                    MessageBox.Show("装置忙，请先停止正在进行的检测或退出软件后重新打开！", "错误提示");
                }
                else
                {
                    Messenger.Default.Send<string>(SelectedNOStr_CenterCal, "LoadCenterCalMessage");
                }
            }

            //关闭当前炉内校准试验
            else if (i == 2403)
            {
                MessageBoxResult msgBoxResult = MessageBox.Show("确认关闭当前校准试验并载入默认试验？", "提示", MessageBoxButton.YesNo);
                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    //装置忙
                    if (Dev.IsBusy)
                    {
                        MessageBox.Show("装置忙，请先停止正在进行的检测，或退出软件后重新打开！", "错误提示");
                    }
                    else
                    {
                        Messenger.Default.Send<string>("DefaultCenterCal", "LoadCenterCalMessage");
                    }
                }
            }

            //保存当前炉内校准试验
            else if (i == 2503)
            {
                if (Dev.IsBusy)
                {
                    MessageBox.Show("装置忙，请先停止检测再修改试验参数！", "提示", MessageBoxButton.OK);
                    return;
                }
                Messenger.Default.Send<string>("SaveCenterCal", "SaveCenterCalMessage");
                Messenger.Default.Send<string>(CenterCalDQ.ExpNO, "LoadCenterCalMessage");

                MessageBox.Show("修改关键参数，需要退出后重新打开软件才能生效！", "提示", MessageBoxButton.OK);
            }

            //取消修改
            else if (i == 2603)
            {
                //装置忙，无法载入
                if (Dev.IsBusy)
                {
                    MessageBox.Show("装置忙，请先停止正在进行的检测或退出软件后重新打开！", "错误提示");
                }
                else
                {
                    Messenger.Default.Send<string>(CenterCalDQ.ExpNO, "LoadCenterCalMessage");
                }
            }

            //复制选中的炉内校准试验
            else if (i == 2703)
            {
                if (ExpNOCopyNew_CenterCal != "")
                {
                    string[] msg = new String[2] { SelectedNOStr_CenterCal, ExpNOCopyNew_CenterCal };
                    Messenger.Default.Send<string[]>(msg, "CopyCenterCalMessage");
                }
            }

            #endregion


            #region 炉壁校准操作

            if (i == 1202)      //取消当前检测
            {
                if (Dev.RunMode == Enums.RunMode.WallCal_Mode) 
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("取消检测将丢失已检测的数据，是否取消当前检测？", "提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                    if (msgBoxResult == MessageBoxResult.Yes)
                    {
                        Messenger.Default.Send<int>(1, "StopMessage");
                    }
                }
            }
            
            else if (i == 1102)      //开始检测
            {
                if (WallCalDQ.ExpNO == "DefaultWallCal")
                {
                    MessageBox.Show("默认试验不能进行试验操作");
                    return;
                }
                if (WallCalDQ.ExpNO == "FactoryWallCal")
                {
                    MessageBox.Show("工厂试验不能进行试验操作");
                    return;
                }

                if (Dev.IsBusy && WallCalDQ.HeightPointDQ.TimePoint.IsStarted)
                {
                    MessageBox.Show(WallCalDQ.HeightNoDQ + "点校准正在检测，请耐心等待！");
                    return;
                }

                if ((WallCalDQ.WallCalPointsList[0].IsReced) ||(WallCalDQ.WallCalPointsList[0].IsReced)||(WallCalDQ.WallCalPointsList[0].IsReced))
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show( "当前校准已有部分数据。若重新开始试验，将清空已完成的数据，是否重新试验？", "数据清空提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                    if (msgBoxResult == MessageBoxResult.No)
                        return;
                }
                Messenger.Default.Send<int>(1, "StartWallCalMessage");
            }

            else if (i == 1802)      //当前点准备就绪
            {
                if (Dev.RunMode == Enums.RunMode.WallCal_Mode)
                {
                    if (WallCalDQ.HeightPointDQ.IsReced)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("当前点已有数据数据。若重新开始试验，将清空已完成的数据，是否重新试验？", "数据清空提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.No)
                            return;
                    }
                    Messenger.Default.Send<int>(2, "StartWallCalMessage");
                }
            }

            else if (i == 1812)      //当前点停止
            {
                if (Dev.RunMode == Enums.RunMode.WallCal_Mode)
                {
                    if (WallCalDQ.IsTesting)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("停止当前点检测将丢失正在进行的试验数据，是否停止当前点检测？", "停止提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.No)
                            return;
                    }
                    Messenger.Default.Send<int>(4, "StartWallCalMessage");
                }
            }
            else if (i == 1902)      //记录当前炉壁校准点数据
            {
                if (Dev.RunMode == Enums.RunMode.WallCal_Mode)
                {
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

                    Messenger.Default.Send<int>(3, "StartWallCalMessage");
                }
                else
                    MessageBox.Show("当前非炉壁校准模式！");
            }


            else if (i == 1602)      //上一个高度
            {
                int noNext;
                int noDQ = WallCalDQ.HeightNoDQ;
                if (noDQ <= 1)
                    noNext = WallCalDQ.WallCalPointsList.Count;
                else
                    noNext = noDQ - 1;
                WallCalDQ.HeightPointDQ = WallCalDQ.WallCalPointsList[noNext - 1];
                WallCalDQ.HeightNoDQ = WallCalDQ.HeightPointDQ.LevelNO;
            }

            else if (i == 1702)      //下一个高度
            {
                int noNext;
                int noDQ = WallCalDQ.HeightNoDQ;
                if (noDQ >= WallCalDQ.WallCalPointsList.Count)
                    noNext = 1;
                else
                    noNext = noDQ + 1;
                WallCalDQ.HeightPointDQ = WallCalDQ.WallCalPointsList[noNext - 1];
                WallCalDQ.HeightNoDQ = WallCalDQ.HeightPointDQ.LevelNO;
            }

            #endregion


            #region 炉内温度校准操作


            if (i == 1203)      //取消当前检测
            {
                if (Dev.RunMode == Enums.RunMode.CenterCal_Mode)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("取消检测将丢失已检测的数据，是否取消当前检测？", "提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                    if (msgBoxResult == MessageBoxResult.Yes)
                    {
                        Messenger.Default.Send<int>(1, "StopMessage");
                    }
                }
            }
            else if (i == 1103)      //开始检测
            {
                if (CenterCalDQ.ExpNO == "DefaultCenterCal")
                {
                    MessageBox.Show("默认试验不能进行试验操作");
                    return;
                }
                if (CenterCalDQ.ExpNO == "FactoryCenterCal")
                {
                    MessageBox.Show("工厂试验不能进行试验操作");
                    return;
                }

                if (Dev.IsBusy && CenterCalDQ.HeightPointDQ.TimePoint.IsStarted)
                {
                    MessageBox.Show(CenterCalDQ.HeightNoDQ + "点校准正在检测，请耐心等待！");
                    return;
                }

                bool existRec = false;
                for (int j = 0; j < CenterCalDQ.CenterCalPointsList.Count; j++)
                {
                    if (CenterCalDQ.CenterCalPointsList[j].IsReced)
                        existRec = true;
                }
                if (existRec)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("当前校准已有部分数据。若重新开始试验，将清空已完成的数据，是否重新试验？", "数据清空提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                    if (msgBoxResult == MessageBoxResult.No)
                        return;
                }
                Messenger.Default.Send<int>(1, "StartCenterCalMessage");
            }

            else if (i == 1803)      //当前点准备就绪
            {
                if (Dev.RunMode == Enums.RunMode.CenterCal_Mode)
                {
                    if (CenterCalDQ.HeightPointDQ.IsReced)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("当前点已有数据数据。若重新开始试验，将清空已完成的数据，是否重新试验？", "数据清空提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.No)
                            return;
                    }
                    Messenger.Default.Send<int>(2, "StartCenterCalMessage");
                }
            }

            else if (i == 1813)      //当前点停止
            {
                if (Dev.RunMode == Enums.RunMode.CenterCal_Mode)
                {
                    if (CenterCalDQ.IsTesting)
                    {
                        MessageBoxResult msgBoxResult = MessageBox.Show("停止当前点检测将丢失正在进行的试验数据，是否停止当前点检测？", "停止提示", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No, MessageBoxOptions.ServiceNotification);
                        if (msgBoxResult == MessageBoxResult.No)
                            return;
                    }
                    Messenger.Default.Send<int>(4, "StartCenterCalMessage");
                }
            }
            else if (i == 1903)      //记录当前炉内校准点数据
            {
                if (Dev.RunMode == Enums.RunMode.CenterCal_Mode)
                {
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

                    Messenger.Default.Send<int>(3, "StartCenterCalMessage");
                }
                else
                    MessageBox.Show("当前非炉壁校准模式！");
            }

            else if (i == 1603)      //上一个高度
            {
                int noNext;
                int noDQ = CenterCalDQ.HeightNoDQ;
                if (noDQ <= 0)
                    noNext = CenterCalDQ.CenterCalPointsList.Count - 1;
                else
                    noNext = noDQ - 1;
                CenterCalDQ.HeightPointDQ = CenterCalDQ.CenterCalPointsList[noNext];
                CenterCalDQ.HeightNoDQ = CenterCalDQ.HeightPointDQ.PointNO;
            }

            else if (i == 1703)      //下一个高度
            {
                int noNext;
                int noDQ = CenterCalDQ.HeightNoDQ;
                if (noDQ >= (CenterCalDQ.CenterCalPointsList.Count - 1))
                    noNext = 1;
                else
                    noNext = noDQ + 1;
                CenterCalDQ.HeightPointDQ = CenterCalDQ.CenterCalPointsList[noNext];
                CenterCalDQ.HeightNoDQ = CenterCalDQ.HeightPointDQ.PointNO;
            }

            #endregion


            #region 炉壁校准数据相关指令

            //保存当前炉壁校准数据
            else if (i == 3102)
            {
                Messenger.Default.Send<string>("SaveWallCal", "SaveWallCalMessage");
            }

            //计算炉壁校准数据
            if (i == 3302)
            {
                Messenger.Default.Send<string>("WallCalData", "CalcCalDataMessage");
            }
            //重置炉壁校准数据（清除数据）
            if (i == 3402)
            {
                if (Dev.IsBusy)
                {
                    MessageBox.Show("测试进行中，等测试完成后重试，或取消测试后重试。");
                    return;
                }

                MessageBoxResult msgBoxResult = MessageBox.Show("重置数据后无法恢复，请慎重操作。确认清除请按“是”，不清除请按“否”", "警告！", MessageBoxButton.YesNo);
                if (msgBoxResult == MessageBoxResult.No)
                    return;

                WallCalDQ.WallCalDataReset(1);
                WallCalDQ.WallCalDataReset(2);
                WallCalDQ.WallCalDataReset(3);
                Messenger.Default.Send<string>("SaveWallCal", "SaveWallCalMessage");
            }
            //导出炉壁校准报告
            if (i == 3502)
            {
                Messenger.Default.Send<string>("WallCalDQ", "ExportRPTMessage");
            }

            #endregion


            #region 炉内校准数据相关指令

            //保存当前炉内校准数据
            else if (i == 3103)
            {
                Messenger.Default.Send<string>("SaveCenterCal", "SaveCenterCalMessage");
            }

            //计算炉内校准数据
            if (i == 3303)
            {
                Messenger.Default.Send<string>("CenterCalData", "CalcCalDataMessage");
            }
            //重置炉内校准数据（清除数据）
            if (i == 3403)
            {
                if (Dev.IsBusy)
                {
                    MessageBox.Show("测试进行中，等测试完成后重试，或取消测试后重试。");
                    return;
                }

                MessageBoxResult msgBoxResult = MessageBox.Show("重置数据后无法恢复，请慎重操作。确认清除请按“是”，不清除请按“否”", "警告！", MessageBoxButton.YesNo);
                if (msgBoxResult == MessageBoxResult.No)
                    return;

                for(int j=0;j<CenterCalDQ.CenterCalPointsList.Count;j++)
                    CenterCalDQ.CenterCalDataReset(j);
                Messenger.Default.Send<string>("SaveCenterCal", "SaveCenterCalMessage");
            }
            //导出炉内校准报告
            if (i == 3503)
            {
                Messenger.Default.Send<string>("CenterCalDQ", "ExportRPTMessage");
            }

            #endregion

        }

        #endregion
        

        #region 动态绘图用

        /// <summary>
        /// 动态绘图用曲线组
        /// </summary>
        private List<LineModel> _plotLines;
        /// <summary>
        /// 动态绘图用曲线组
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
        /// 炉壁校准试验窗口已打开
        /// </summary>
        private bool _isWallCalWinOpened = false;
        /// <summary>
        /// 炉壁校准试验窗口已打开
        /// </summary>
        public bool IsWallCalWinOpened
        {
            get { return _isWallCalWinOpened; }
            set
            {
                _isWallCalWinOpened = value;
                RaisePropertyChanged(() => IsWallCalWinOpened);
            }
        }

        /// <summary>
        /// 炉内校准试验窗口已打开
        /// </summary>
        private bool _isCenterCalWinOpened = false;
        /// <summary>
        /// 炉内校准试验窗口已打开
        /// </summary>
        public bool IsCenterCalWinOpened
        {
            get { return _isCenterCalWinOpened; }
            set
            {
                _isCenterCalWinOpened = value;
                RaisePropertyChanged(() => IsCenterCalWinOpened);
            }
        }

        /// <summary>
        /// 绘图曲线初始化
        /// </summary>
        public void PlotInit()
        {
            PlotLines = new List<LineModel>();
            //炉内温度T1
            LineModel newLine = new LineModel();
            newLine.LineNO = "T1";
            newLine.LineName = "炉内温度1";
            newLine.VarUnit = "℃";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //炉内温度T2
            newLine = new LineModel();
            newLine.LineNO = "T2";
            newLine.LineName = "炉内温度2";
            newLine.VarUnit = "℃";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //试样中心温度Tsc
            newLine = new LineModel();
            newLine.LineNO = "Tsc";
            newLine.LineName = "试样中心温度";
            newLine.VarUnit = "℃";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //试样表面温度Tss
            newLine = new LineModel();
            newLine.LineNO = "Tss";
            newLine.LineName = "试样表面温度";
            newLine.VarUnit = "℃";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //标定炉壁温度Tw1
             newLine = new LineModel();
            newLine.LineNO = "Tw1";
            newLine.LineName = "标定炉壁温度";
            newLine.VarUnit = "℃";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //标定炉壁温度Tw2
            newLine = new LineModel();
            newLine.LineNO = "Tw2";
            newLine.LineName = "标定炉壁温度1";
            newLine.VarUnit = "℃";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //标定炉壁温度Tw3
            newLine = new LineModel();
            newLine.LineNO = "Tw3";
            newLine.LineName = "标定炉壁温度3";
            newLine.VarUnit = "℃";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //标定炉内温度Tfc
            newLine = new LineModel();
            newLine.LineNO = "Tfc";
            newLine.LineName = "标定炉内温度";
            newLine.VarUnit = "℃";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //输出电压百分比
            newLine = new LineModel();
            newLine.LineNO = "Vo";
            newLine.LineName = "电压百分比";
            newLine.VarUnit = "%";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //输出电压
            newLine = new LineModel();
            newLine.LineNO = "Vo";
            newLine.LineName = "电压";
            newLine.VarUnit = "V";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);
            //输出功率
            newLine = new LineModel();
            newLine.LineNO = "Power";
            newLine.LineName = "功率";
            newLine.VarUnit = "W";
            newLine.LineThickness = 0.8;
            newLine.LineColor = Color.Red;
            newLine.AxisYside = LineModel.AxisUse.Left;
            newLine.LineDataSource = new ObservableDataSource<System.Windows.Point>();
            newLine.LineBackGroundPointList = new List<System.Windows.Point>();
            newLine.MaxPointsQuantity = Dev.PointsPerLine;
            newLine.MaxBSPointsQuantity = Dev.PointsPerLine;
            newLine.IsBackStore = false;
            PlotLines.Add(newLine);


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

            if (IsWallCalWinOpened || IsCenterCalWinOpened)
            {
                //炉内温度T1
                System.Windows.Point newPoint1 = new System.Windows.Point();
                newPoint1.X = span.TotalSeconds;
                newPoint1.Y = Dev.AIList[0].ValueFinal;
                PlotLines[0].AddPoint(newPoint1);
                //炉内温度T2
                System.Windows.Point newPoint2 = new System.Windows.Point();
                newPoint2.X = span.TotalSeconds;
                newPoint2.Y = Dev.AIList[1].ValueFinal;
                PlotLines[1].AddPoint(newPoint2);
                //标定炉壁温度Tw1
                System.Windows.Point newPoint5 = new System.Windows.Point();
                newPoint5.X = span.TotalSeconds;
                newPoint5.Y = Dev.AIList[5].ValueFinal;
                PlotLines[4].AddPoint(newPoint5);
                //标定炉壁温度Tw2
                System.Windows.Point newPoint6 = new System.Windows.Point();
                newPoint6.X = span.TotalSeconds;
                newPoint6.Y = Dev.AIList[6].ValueFinal;
                PlotLines[5].AddPoint(newPoint6);
                //标定炉壁温度Tw3
                System.Windows.Point newPoint7 = new System.Windows.Point();
                newPoint7.X = span.TotalSeconds;
                newPoint7.Y = Dev.AIList[7].ValueFinal;
                PlotLines[6].AddPoint(newPoint7);
                //标定炉内温度Tfc
                System.Windows.Point newPoint8 = new System.Windows.Point();
                newPoint8.X = span.TotalSeconds;
                newPoint8.Y = Dev.AIList[4].ValueFinal;
                PlotLines[7].AddPoint(newPoint8);

                //输出电压比例
                System.Windows.Point newPoint9 = new System.Windows.Point();
                newPoint9.X = span.TotalSeconds;
                newPoint9.Y = Dev.AOList[0].ValueFinal;
                PlotLines[8].AddPoint(newPoint9);
                //输出电压
                System.Windows.Point newPoint10 = new System.Windows.Point();
                newPoint10.X = span.TotalSeconds;
                newPoint10.Y = Dev.VO_110VBase;
                PlotLines[9].AddPoint(newPoint10);
                //输出功率
                System.Windows.Point newPoint11 = new System.Windows.Point();
                newPoint11.X = span.TotalSeconds;
                newPoint11.Y = Dev.Power_110VBase;
                PlotLines[10].AddPoint(newPoint11);
            }
        }

        #endregion
        

        #region livechart绘图用

        /// <summary>
        /// 绘布
        /// </summary>
        private ChartModel_LiveChart _chartLive = new ChartModel_LiveChart()
        {
            ChartType = ChartType.Line,
        };
        /// <summary>
        /// 绘布
        /// </summary>
        public ChartModel_LiveChart ChartLive
        {
            get { return _chartLive; }
            set
            {
                _chartLive = value;
                RaisePropertyChanged(() => ChartLive);
            }
        }


        #region 绘制Chart

        /// <summary>
        /// 最大值
        /// </summary>
        public IChartValues _maxValues =new ChartValues<ObservablePoint>();
        /// <summary>
        /// 最大值
        /// </summary>
        public IChartValues MaxValues
        {
            get { return _maxValues; }
            set
            {
                _maxValues = value;
                RaisePropertyChanged(() => MaxValues);
            }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public IChartValues _minValues = new ChartValues<ObservablePoint>();
        /// <summary>
        /// 最小值
        /// </summary>
        public IChartValues MinValues
        {
            get { return _minValues; }
            set
            {
                _minValues = value;
                RaisePropertyChanged(() => MinValues);
            }
        }

        /// <summary>
        /// 检测值
        /// </summary>
        public IChartValues _testValues = new ChartValues<ObservablePoint>();
        /// <summary>
        /// 检测值
        /// </summary>
        public IChartValues TestValues
        {
            get { return _testValues; }
            set
            {
                _testValues = value;
                RaisePropertyChanged(() => TestValues);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void LivePloitInit()
        {
            MaxValues = new ChartValues<ObservablePoint>();
            MinValues = new ChartValues<ObservablePoint>();
            TestValues = new ChartValues<ObservablePoint>();
        }

        
        /// <summary>
        /// 更新校准曲线
        /// </summary>
        /// <param name="msg">检测点列表</param>
        private void CenterCalPointChanged(string msg)
        {
            MaxValues.Clear();
            MinValues.Clear();
            TestValues.Clear();
            for (int i = 0; i < CenterCalDQ.HList.Count; i++)
            {
                if (Dev.IsStd2023)
                    MaxValues.Add(new ObservablePoint(CenterCalDQ.TMaxLimitList_2023[i], CenterCalDQ.HList[i]));
                else
                    MaxValues.Add(new ObservablePoint(CenterCalDQ.TMaxLimitList_2010[i], CenterCalDQ.HList[i]));

                if (Dev.IsStd2023)
                    MinValues.Add(new ObservablePoint( CenterCalDQ.TMinLimitList_2023[i], CenterCalDQ.HList[i]));
                else
                    MinValues.Add(new ObservablePoint(CenterCalDQ.TMinLimitList_2010[i], CenterCalDQ.HList[i]));

                TestValues.Add(new ObservablePoint(CenterCalDQ.TAvgList[i], CenterCalDQ.HList[i]));
            }
        }

        #endregion

        #endregion


        #region 窗口打开关闭消息

        /// <summary>
        /// 窗口已关闭消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void WindowClosedMessage(string msg)
        {
            if (msg == WinNames.WallCalWinName)
            {
                IsWallCalWinOpened = false;
                if (IsCenterCalWinOpened)
                {

                    for (int i = 0; i < PlotLines.Count; i++)
                    {
                        PlotLines[i].LineDataSource.Collection.Clear();
                    }
                }
            }

            if (msg == WinNames.CenterCalWinName)
            {
                IsCenterCalWinOpened= false;
                if (IsWallCalWinOpened)
                {

                    for (int i = 0; i < PlotLines.Count; i++)
                    {
                        PlotLines[i].LineDataSource.Collection.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// 窗口已新增消息处理
        /// </summary>
        /// <param name="msgWindow"></param>
        private void WindowCreatedMessage(Window msgWindow)
        {
            if (msgWindow.Name == WinNames.WallCalWinName)
                IsWallCalWinOpened = true;
            if (msgWindow.Name == WinNames.CenterCalWinName)
                IsCenterCalWinOpened = true;
        }

        #endregion


        /// <summary>
        /// 线程内保存数据方法
        /// </summary>
        public void SaveExpData()
        {
            var task = System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {

                }
            ));
            task.Completed += new EventHandler(Task_Completed);
        }
        public void Task_Completed(object sender, EventArgs e)
        {
        }

    }
}

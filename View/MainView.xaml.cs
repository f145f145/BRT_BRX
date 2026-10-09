using GalaSoft.MvvmLight.Messaging;
using BRX.View.DevSet;
using System;
using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Research.DynamicDataDisplay.Charts;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BRX.View
{

    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();

            //炉内温度
            Legend.SetDescription(Line11, "T1 ℃");
            Legend.SetDescription(Line12, "T2 ℃");
            //试样温度
            Legend.SetDescription(Line21, "中心 ℃");
            Legend.SetDescription(Line22, "表面 ℃");
            //输出电压
           // Legend.SetDescription(Line23, "T1 ℃");
            Legend.SetDescription(Line24, "电压 V");
            //755.745
            Legend.SetDescription(Line755, "760");
            Legend.SetDescription(Line745, "740");
            Legend.SetDescription(Line7452, "760");
            Legend.SetDescription(Line7552, "740");
            Legend.SetDescription(Line7453, "760");
            Legend.SetDescription(Line7553, "740");
            Legend.SetDescription(Line7454, "760");
            Legend.SetDescription(Line7554, "740");

            //炉内温度
            Legend.SetDescription(LineDb11, "T1 ℃");
            Legend.SetDescription(LineDb12, "T2 ℃");
            ////电压
            //Legend.SetDescription(LineDb13, "电压 %");
            //Legend.SetDescription(LineDb14, "电压 V");


            //炉内温度
            Legend.SetDescription(LineDb21, "T1 ℃");
            Legend.SetDescription(LineDb22, "T2 ℃");
            

            //PID输出
            Legend.SetDescription(LineDb31, "Usum");
            Legend.SetDescription(LineDb32, "Up");
            Legend.SetDescription(LineDb33, "Ui");
            Legend.SetDescription(LineDb34, "Ud");


            //炉内温度
            Legend.SetDescription(LineDb41, "T1 ℃");
            Legend.SetDescription(LineDb42, "T2 ℃");
            //T2-T1
            Legend.SetDescription(LineDb43, "T2-T1");

            //功率P
            Legend.SetDescription(LineDb51, "P");

            //电压，电流，功率
            Legend.SetDescription(Line61, "U");
            Legend.SetDescription(Line62, "I");
            Legend.SetDescription(Line63, "P");

            //温升
            Legend.SetDescription(LineWSTn11, "Tn1");
            Legend.SetDescription(LineWSTn12, "Tn2");
            Legend.SetDescription(LineWSTc1, "Tsc");
            Legend.SetDescription(LineWSTs1, "Tss");
            Legend.SetDescription(LineWSTn21, "Tn1");
            Legend.SetDescription(LineWSTn22, "Tn2");
            Legend.SetDescription(LineWSTc2, "Tsc");
            Legend.SetDescription(LineWSTs2, "Tss");
            Legend.SetDescription(LineWSTn31, "Tn1");
            Legend.SetDescription(LineWSTn32, "Tn2");
            Legend.SetDescription(LineWSTc3, "Tsc");
            Legend.SetDescription(LineWSTs3, "Tss");
            Legend.SetDescription(LineWSTn41, "Tn1");
            Legend.SetDescription(LineWSTn42, "Tn2");
            Legend.SetDescription(LineWSTc4, "Tsc");
            Legend.SetDescription(LineWSTs4, "Tss")                ;
            Legend.SetDescription(LineWSTn51, "Tn1");
            Legend.SetDescription(LineWSTn52, "Tn2");
            Legend.SetDescription(LineWSTc5, "Tsc");
            Legend.SetDescription(LineWSTs5, "Tss");

            //卸载当前(this)对象注册的所有MVVMLight消息
            this.Unloaded += (sender, e) => Messenger.Default.Unregister(this);

            //注册窗口关闭、新增处理方法
            Messenger.Default.Register<Window>(this, "NewWindowCreated", NewWinCreatedMessage);
            Messenger.Default.Register<string>(this, "WindowClosed", WinClosedMessage);
            Messenger.Default.Register<string>(this, "OpenGivenNameWin", OpenWinMessage);
            Messenger.Default.Register<string>(this, "CloseGivenNameWin", CloseGivenNameWinMessage);

            //试验完成提示消息
            Messenger.Default.Register<int>(this, "ExpEndedMessage", ExpEndedMessage);
            //异步弹窗提示
            Messenger.Default.Register<string>(this, "PopMessage", PopMessage);

        }
        #region 窗口属性，新增、关闭窗口消息处理

        /// <summary>
        /// 已打开子窗口列表
        /// </summary>
        private List<Window> _childWindowList = new List<Window>();
        /// <summary>
        /// 已打开子窗口列表
        /// </summary>
        private List<Window> ChildWindowList
        {
            get { return _childWindowList; }
            set { _childWindowList = value; }
        }

        //校准窗口
        public WallCalView WallCalWin;
        public CenterCalView CenterCalWin;

        //装置设定
        public DevSetView_AIOCal DevAIOCalWin;
        public DevSetView_AIOParam DevAIOParamWin;
        public DevSetView_AIOChannelParam DevAIOChannelParamWin;
        public DevSetView_BisicInfo DevBisicInfoWin;
        public DevSetView_BisicParam DevBisicParamWin;
        public DevSetView_CoInfo DevCoInfoWin;
        public DevSetView_ComSet DevComSetWin;
        public DevSetView_PidSet DevPIDSetWin;

        /// <summary>
        /// 窗口已关闭消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void WinClosedMessage(string msg)
        {
            for (int i = 0; i < ChildWindowList.Count; i++)
            {
                if (ChildWindowList[i].Name == msg)
                {
                    ChildWindowList.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// 窗口已新增消息处理
        /// </summary>
        /// <param name="msgWindow"></param>
        private void NewWinCreatedMessage(Window msgWindow)
        {
            if (!ChildWindowList.Exists(o => o.Name == msgWindow.Name))
            {
                ChildWindowList.Add(msgWindow);
            }
        }

        /// <summary>
        /// 打开指定窗口消息
        /// </summary>
        /// <param name="msg"></param>
        private void OpenWinMessage(string msg)
        {
            //校准窗口
            if (msg == WinNames.WallCalWinName)
                WallCalWin = WindowsManager<WallCalView>.Show(new object(), WinNames.WallCalWinName, this);
            if (msg == WinNames.CenterCalWinName) 
                CenterCalWin = WindowsManager<CenterCalView>.Show(new object(), WinNames.CenterCalWinName, this);

            //装置设定
            if (msg == WinNames.AIOCalWinName)
                DevAIOCalWin = WindowsManager<DevSetView_AIOCal>.Show(new object(), WinNames.AIOCalWinName, this);
            if (msg == WinNames.AIOParamWinName)
                DevAIOParamWin = WindowsManager<DevSetView_AIOParam>.Show(new object(), WinNames.AIOParamWinName, this); 
            if (msg == WinNames.AIOChannelWinName)
                DevAIOChannelParamWin = WindowsManager<DevSetView_AIOChannelParam>.Show(new object(), WinNames.AIOChannelWinName, this);
            if (msg == WinNames.BasicInfoWinName)
                DevBisicInfoWin = WindowsManager<DevSetView_BisicInfo>.Show(new object(), WinNames.BasicInfoWinName, this);
            if (msg == WinNames.BasicParamWinName)
                DevBisicParamWin = WindowsManager<DevSetView_BisicParam>.Show(new object(), WinNames.BasicParamWinName, this);
            if (msg == WinNames.CoInfoWinName)
                DevCoInfoWin = WindowsManager<DevSetView_CoInfo>.Show(new object(), WinNames.CoInfoWinName, this);
            if (msg == WinNames.ComSetWinName)
                DevComSetWin = WindowsManager<DevSetView_ComSet>.Show(new object(), WinNames.ComSetWinName, this);
            if (msg == WinNames.PidSetWinName)
                DevPIDSetWin = WindowsManager<DevSetView_PidSet>.Show(new object(), WinNames.PidSetWinName, this);
        }

        /// <summary>
        /// 关闭指定窗口消息
        /// </summary>
        /// <param name="msg"></param>
        private void CloseGivenNameWinMessage(string msg)
        {
            //校准
            if (msg == WinNames.WallCalWinName)
            {
                if (WallCalWin != null)
                {
                    WallCalWin.Close();
                    WallCalWin = null;
                }
            }
            if (msg == WinNames.CenterCalWinName)
            {
                if (CenterCalWin != null)
                {
                    CenterCalWin.Close();
                    CenterCalWin = null;
                }
            }

            //装置设定
            if (msg == WinNames.AIOCalWinName)
            {
                if (DevAIOCalWin != null)
                {
                    DevAIOCalWin.Close();
                    DevAIOCalWin = null;
                }
            }
            if (msg == WinNames.AIOParamWinName)
            {
                if (DevAIOParamWin != null)
                {
                    DevAIOParamWin.Close();
                    DevAIOParamWin = null;
                }
            }
            if (msg == WinNames.AIOChannelWinName)
            {
                if (DevAIOChannelParamWin != null)
                {
                    DevAIOChannelParamWin.Close();
                    DevAIOChannelParamWin = null;
                }
            }
            if (msg == WinNames.BasicInfoWinName)
            {
                if (DevBisicInfoWin != null)
                {
                    DevBisicInfoWin.Close();
                    DevBisicInfoWin = null;
                }
            }
            if (msg == WinNames.BasicParamWinName)
            {
                if (DevBisicParamWin != null)
                {
                    DevBisicParamWin.Close();
                    DevBisicParamWin = null;
                }
            }
            if (msg == WinNames.CoInfoWinName)
            {
                if (DevCoInfoWin != null)
                {
                    DevCoInfoWin.Close();
                    DevCoInfoWin = null;
                }
            }
            if (msg == WinNames.ComSetWinName)
            {
                if (DevComSetWin != null)
                {
                    DevComSetWin.Close();
                    DevComSetWin = null;
                }
            }
            if (msg == WinNames.PidSetWinName)
            {
                if (DevPIDSetWin != null)
                {
                    DevPIDSetWin.Close();
                    DevPIDSetWin = null;
                }
            }

            if (msg == "All")
            {
                CloseChildWindows();
            }
        }

        #endregion


        /// <summary>
        /// 退出菜单
        /// </summary>
        private void Button_Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        /// <summary>
        /// 更新温升曲线
        /// </summary>
        private void Button_UpdatePlotWS_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            Task.Run(() =>
            {
                dispatcher.Invoke(new Action(() => { Messenger.Default.Send<string>("UpdateSpWSPlotMessage", "UpdateSpWSPlotMessage"); 
                }));
            });
        }

        ///// <summary>
        ///// 异步请求，返回请求结果
        ///// </summary>
        ///// <param name="Url">请求地址</param>
        ///// <returns>参数列表</returns>
        //public Task<string> GetHttpPostStringAsync()
        //{
        //    Task msg = new Task(() => { Messenger.Default.Send<string>("UpdateSpWSPlotMessage", "UpdateSpWSPlotMessage"); });

        //    Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        //    Task.Run(() =>
        //    {
        //        dispatcher.Invoke(new Action(() => { }));
        //    });


        #region 关闭窗口事件处理

        /// <summary>
        /// 关闭窗口前检查子窗口是否全部关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (ChildWindowList.Count == 0)
                {
                    CloseChildWindows();
                    Messenger.Default.Send<string>("Quit", "Quit");
                }
                else
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("有子窗口未关闭，强行退出可能会丢失数据。继续退出程序?", "关闭子窗口提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        Messenger.Default.Send<string>("Quit", "Quit");
                    }
                }
            }
            catch (Exception ex)

            {
                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// 强行关闭子窗口
        /// </summary>
        private void CloseChildWindows()
        {
            //校准
            if (WallCalWin != null)
            {
                WallCalWin.Close();
                WallCalWin = null;
            }
            if (CenterCalWin != null)
            {
                CenterCalWin.Close();
                CenterCalWin = null;
            }

            //装置设定
            if (DevAIOCalWin != null)
            {
                DevAIOCalWin.Close();
                DevAIOCalWin = null;
            }
            if (DevAIOParamWin != null)
            {
                DevAIOParamWin.Close();
                DevAIOParamWin = null;
            }
            if (DevAIOChannelParamWin != null)
            {
                DevAIOChannelParamWin.Close();
                DevAIOChannelParamWin = null;
            }
            if (DevBisicInfoWin != null)
            {
                DevBisicInfoWin.Close();
                DevBisicInfoWin = null;
            }
            if (DevBisicParamWin != null)
            {
                DevBisicParamWin.Close();
                DevBisicParamWin = null;
            }
            if (DevCoInfoWin != null)
            {
                DevCoInfoWin.Close();
                DevCoInfoWin = null;
            }
            if (DevComSetWin != null)
            {
                DevComSetWin.Close();
                DevComSetWin = null;
            }
        }

        #endregion


        #region 试验结束消息

        /// <summary>
        /// 试验完成弹窗消息回调
        /// </summary>
        /// <param name="msg"></param>
        private async void ExpEndedMessage(int msg)
        {
            string strShown;
            if (msg < 0)
                strShown = "试验已结束，请称量残余、碎落的试样，将重量输入试验参数后保存、重新计算！";
            else
            {
                strShown = "上一个试样检测已结束，已自动进行" + msg + "号试样的检测。请尽快称量残余、碎落的试样后并手动记录（所有试验结束后再统一输入），请尽快将底部的残渣收集盘装回！！！";
            }

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "确认",
                NegativeButtonText = "",
                FirstAuxiliaryButtonText = "",
                ColorScheme = MetroDialogColorScheme.Accented,
                DialogMessageFontSize = 24,
                DialogTitleFontSize = 36
            };

            //播放语音
            if (msg < 0)
                Messenger.Default.Send<int>(0, "PlaySoundMessage");
            else
                Messenger.Default.Send<int>(msg, "PlaySoundMessage");

            await this.ShowMessageAsync("重要提示：", strShown, MessageDialogStyle.Affirmative, mySettings);

            //停止语音
            Messenger.Default.Send<int>(-1, "PlaySoundMessage");

        }


        /// <summary>
        /// 试验完成弹窗消息回调
        /// </summary>
        /// <param name="msg"></param>
        private async void PopMessage(string msg)
        {
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "确认",
                NegativeButtonText = "",
                FirstAuxiliaryButtonText = "",
                ColorScheme = MetroDialogColorScheme.Accented,
                DialogMessageFontSize = 24,
                DialogTitleFontSize=36
            };
            await this.ShowMessageAsync("重要提示：", msg, MessageDialogStyle.Affirmative, mySettings);
        }

        #endregion
    }
}
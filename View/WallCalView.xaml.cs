using GalaSoft.MvvmLight.Messaging;
using BRX.View.DevSet;
using System;
using System.Collections.Generic;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Research.DynamicDataDisplay.Charts;
using System.Windows.Controls;
using System.Drawing;

namespace BRX.View
{

    public partial class WallCalView : MetroWindow
    {
        public WallCalView()
        {
            InitializeComponent();

            //温度
            Legend.SetDescription(LineWall1, "T1 ℃");
            Legend.SetDescription(LineWall2, "T2 ℃");
            Legend.SetDescription(LineWall3, "Tw1 ℃");
            Legend.SetDescription(LineWall4, "Tw2 ℃");
            Legend.SetDescription(LineWall5, "Tw3 ℃");
        }


        #region 相关状态

        /// <summary>
        /// 参数变更需要存盘标志
        /// </summary>
        private bool _isNeedSave = false;
        /// <summary>
        /// 参数变更需要存盘标志
        /// </summary>
        public bool IsNeedSave
        {
            get { return _isNeedSave; }
            set
            {
                _isNeedSave = value;
            }
        }

        #endregion


        #region 关闭窗口处理方法

        /// <summary>
        /// 窗口退出处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (IsNeedSave)
                {
                    MessageBoxResult msgBoxResult = MessageBox.Show("参数变更后尚未存盘，确认退出程序?", "提示", MessageBoxButton.YesNo);
                    if (msgBoxResult == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        Messenger.Default.Send<string>("LoadDevSettings", "DevDataRWMessage");

                        //注销消息
                        Messenger.Default.Unregister(this);
                        //通知主窗口已关闭某窗口
                        Messenger.Default.Send<string>(this.Name, "WindowClosed");
                    }
                }
                else
                {
                    Messenger.Default.Send<string>("LoadDevSettings", "DevDataRWMessage");
                    //注销消息
                    Messenger.Default.Unregister(this);
                    //通知主窗口已关闭某窗口
                    Messenger.Default.Send<string>(this.Name, "WindowClosed");
                }
            }
            catch (Exception)

            {

            }
        }

        #endregion 
    }

}


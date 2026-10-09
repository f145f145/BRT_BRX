using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Input;

namespace BRX.View.DevSet
{
    public partial class DevSetView_BisicParam : MetroWindow
    {
        public DevSetView_BisicParam()
        {
            InitializeComponent();

            //注册管理员登录消息
            Messenger.Default.Register<string>(this, "PasswordChanged", PasswordChangedMessage);
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

        private void SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Messenger.Default.Send<string>("ModelChangedMessage", "ModelChangedMessage");
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<string>("WithPowerChangedMessage", "WithPowerChangedMessage");
        }


        /// <summary>
        /// 更新密码消息处理
        /// </summary>
        /// <param name="msgWindow"></param>
        private void PasswordChangedMessage(string msg)
        {
            try
            {
                AdminPasswordBox.Password = msg;
            }
            catch (Exception ex)
            {
                ;
            }
        }


        /// <summary>
        /// 输入密码然后回车键处理
        /// </summary>
        private void AdminPasswordBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Messenger.Default.Send<string>("AdminLogginMessage", "AdminLogginMessage");
        }
    }
}
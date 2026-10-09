using System;
using System.IO;
using System.Windows;
using Authorize;
using BRX.DAL;
using BRX.View.Authorize;

namespace BRX.View
{
    /// <summary>
    /// LoadingView.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingView : Window
    {

        public LoadingView()
        {
            InitializeComponent();

            //授权检查
            Authorize.AppType = 22;  //新不燃性22
            Authorize.AuthDAL=new AuthorizeDAL(Authorize.AppType);
            if (!CheckIn())
            {
                Authorize.AuthDAL.SaveSN("");
                Info.Content = "授权验证失败，请重新注册！";
                ReAuthorize.Visibility = Visibility.Visible;
                return;
            }
            else
                ReAuthorize.Visibility = Visibility.Hidden;
            //授权验证通过后，更新最后运行时间。
            Authorize.AuthDAL.SaveLastTime();

            //文件检查
            if (!CheckFiles())
            {
                Info .Content = "文件检查失败！";
                return;
            }
            //进入系统
            GetIn();
            this.Close();
        }

        #region 交互按钮

        public SN_InputView SN_InputWin;

        /// <summary>
        /// 重新注册按钮
        /// </summary>
        private void Button_ReReg_Click(object sender, RoutedEventArgs e)
        {
            SN_InputWin = new SN_InputView();
            SN_InputWin.Name = WinNames.SN_InputWinName;

            SN_InputWin.Show();
            SN_InputWin.Activate();
            SN_InputWin.Focus();
            Close();
        }
        
        /// <summary>
        /// 退出
        /// </summary>
        private void Button_Quit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        #endregion


        #region 启动成功处理

        private string mqzh_MainWinName = "MainView";

        public MainView MainView;

        /// <summary>
        /// 进入系统
        /// </summary>
        private void GetIn()
        {
            double screenWidth = System.Windows.SystemParameters.WorkArea.Width;
            double screenHeight = System.Windows.SystemParameters.WorkArea.Height;
            MainView = new MainView();
            MainView.Name = mqzh_MainWinName;
            MainView.Show();
            MainView.Activate();
            MainView.Focus();
            MainView.Top = screenHeight / 2 - MainView.Height / 2;
            MainView.Left = screenWidth / 2 - MainView.Width / 2;
        }

        #endregion


        #region 授权测试

        private AuthorizeBLL _authorize = new AuthorizeBLL();
        public AuthorizeBLL Authorize
        {
            get { return _authorize; }
            set { _authorize = value; }
        }

        /// <summary>
        /// 授权测试
        /// </summary>
        private bool CheckIn()
        {
            //测试是否已注册
            if (!Authorize.AuthDAL.CheckRegExist())
            {
                MessageBox.Show("未测试到授权信息！");
                return false;
            }

            //测试注册码有效性
            string tempSN = Authorize.AuthDAL.GetRegSN();
            int tempRet = Authorize.CheckSN(tempSN, Authorize.AppType);
            if (tempRet == 1)
            {
                MessageBox.Show("未测试到授权信息！");
                return false;
            }
            if (tempRet == 2)
            {
                MessageBox.Show("授权无效1！");
                return false;
            }
            if (tempRet == 3)
            {
                MessageBox.Show("授权无效2！");
                return false;
            }
            if (tempRet == 4)
            {
                MessageBox.Show("授权无效3！");
                return false;
            }
            if (tempRet == 5)
            {
                MessageBox.Show("授权无效4！");
                return false;
            }
            
            //测试时间有效性
            DateTime tempTime = Authorize.AuthDAL.GetLastTime();
            if (tempTime > DateTime.Now)
            {
                MessageBox.Show("系统时间有误！");
                Close();
            }

            return true;
        }

        #endregion


        #region 文件检查

        private bool CheckFiles()
        {
            bool tempRet = true;

            if (!File.Exists(Config.MainDBFile))
            {
                MessageBox.Show("文件'" + Config.MainDBFile + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }

            if (!File.Exists(Config.DevDBFile))
            {
                MessageBox.Show("文件'" + Config.DevDBFile + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }

            if (!File.Exists(Config.InitDBFile))
            {
                MessageBox.Show("文件'" + Config.InitDBFile + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }

            if (!File.Exists(Config.LogDBFile))
            {
                MessageBox.Show("文件'" + Config.LogDBFile + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }

            if (!File.Exists(Config.FormDir + Config.ExpRep10FormName))
            {
                MessageBox.Show("文件'" + Config.ExpRep10FormName + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }

            if (!File.Exists(Config.FormDir + Config.ExpRep22FormName))
            {
                MessageBox.Show("文件'" + Config.ExpRep22FormName + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }

            if (!File.Exists(Config.FormDir + Config.WallCalRepFormName))
            {
                MessageBox.Show("文件'" + Config.WallCalRepFormName + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }

            if (!File.Exists(Config.FormDir + Config.CenterCalRepFormName))
            {
                MessageBox.Show("文件'" + Config.CenterCalRepFormName + "' 不存在，请检查文件是否丢失！");
                tempRet = false;
            }

            return tempRet;
        }

        #endregion
    }
}
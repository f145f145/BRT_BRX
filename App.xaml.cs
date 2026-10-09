using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BRX
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 只打开一个进程
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            Process existProcess = Process.GetCurrentProcess();

            foreach (Process newProcessItem in Process.GetProcessesByName(existProcess.ProcessName))
            {
                if (newProcessItem.Id != existProcess.Id &&
                    (newProcessItem.StartTime - existProcess.StartTime).TotalMilliseconds <= 0)
                {
                    MessageBox.Show("请勿重复启动程序，\r\n若未找到程序窗口，请检查显示分辨率或重启计算机。", "提示");
                    existProcess.Kill();
                    break;
                }
            }
            //设置数据库路径
            string dataDir = AppDomain.CurrentDomain.BaseDirectory;
            dataDir = System.IO.Directory.GetParent(dataDir).Parent.Parent.FullName;
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDir);


            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            base.OnStartup(e);
        }

        /// <summary>
        /// UI线程未捕获异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            // MessageBox.Show("系统异常1" + e.Exception);
        }

        /// <summary>
        /// Task线程内未捕获异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            // MessageBox.Show("系统异常2" + e.Exception);
        }

        /// <summary>
        /// 非UI线程未捕获异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            //  MessageBox.Show("系统异常3");

        }
    }
}

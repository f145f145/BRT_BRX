/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.View
 * 文件名：  WindowsManager
 * 版本号：  V1.0.0.0
 * 唯一标识：8fd19f7e-b76f-455e-a5f2-5f8929afa0f1
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/2/27 8:45:03
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/2/27 8:45:03		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace BRX.View
{
    public class WindowsManager<TWindow> where TWindow : Window, new()
    {
        static TWindow window;

        public static TWindow Show(object vm, string name, Window owner)
        {
            bool existAlready = true;

            if ((window == null) || (!window.IsLoaded))
            {
                existAlready = false;
                window = null;
                window = new TWindow() { Name = name, Owner = owner };

                //通知主窗口已创建新窗口
                Messenger.Default.Send<Window>(window, "NewWindowCreated");
            }
            window.Show();
            window.Activate();
            window.Focus();

            if (existAlready)
            {
                window.WindowState = WindowState.Normal;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            return window;
        }
    }
}


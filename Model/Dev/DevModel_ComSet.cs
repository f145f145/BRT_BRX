/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.Model.Dev
 * 文件名：  DevModel_ComSet
 * 版本号：  V1.0.0.0
 * 唯一标识：69884726-fac6-40e4-879a-66fee69147e2
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/3 22:45:36
 * 描述：
 * 装置Model，串口通讯部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/8/22 22:45:36		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BRX.Model.Dev
{
    public partial class DevModel: ObservableObject
    {
        #region 热电偶采集模块通讯

        /// <summary>
        /// 热电偶采集模块通讯设定
        /// </summary>
        private ComSetParaModel _tCom = new ComSetParaModel()
        {
            ComName = "TCOM",
            PhyPortNO = "COM7",
            BoundRate = 9600,
            StartBits = 1,
            DataBits = 8,
            StopBits = StopBits.One,
            Parity = System.IO.Ports.Parity.None,

            Addr = 1,
            PeriodRW = 500,
            WatchDogPeriod = 20000,
            Timeout = 300,
            Time_BusyDealy = 50,
            CMDRepeat = 1
        };

        /// <summary>
        /// 热电偶采集模块通讯设定
        /// </summary>
        public ComSetParaModel TCom
        {
            get { return _tCom; }
            set
            {
                _tCom = value;
                RaisePropertyChanged(() => TCom);
            }
        }

        /// <summary>
        /// 热电偶采集模块通讯状态
        /// </summary>
        private CommStatusModel _tComStatus = new CommStatusModel();
        /// <summary>
        /// 热电偶采集模块通讯状态
        /// </summary>
        public CommStatusModel TComStatus
        {
            get { return _tComStatus; }
            set
            {
                _tComStatus = value;
                RaisePropertyChanged(() => TComStatus);
            }
        }

        #endregion


        #region 功率采集模块通讯

        /// <summary>
        /// 功率采集模块通讯设定
        /// </summary>
        private ComSetParaModel _powerCom = new ComSetParaModel()
        {
            ComName = "PowerCOM",
            PhyPortNO = "COM7",
            BoundRate = 9600,
            StartBits = 1,
            DataBits = 8,
            StopBits = StopBits.One,
            Parity = System.IO.Ports.Parity.None,

            Addr = 1,
            PeriodRW = 500,
            WatchDogPeriod = 20000,
            Timeout = 300,
            Time_BusyDealy = 50,
            CMDRepeat = 1
        };

        /// <summary>
        /// 功率采集模块通讯设定
        /// </summary>
        public ComSetParaModel PowerCom
        {
            get { return _powerCom; }
            set
            {
                _powerCom = value;
                RaisePropertyChanged(() => PowerCom);
            }
        }

        /// <summary>
        /// 功率采集模块通讯状态
        /// </summary>
        private CommStatusModel _powerComStatus = new CommStatusModel();
        /// <summary>
        /// 功率采集模块通讯状态
        /// </summary>
        public CommStatusModel PowerComStatus
        {
            get { return _powerComStatus; }
            set
            {
                _powerComStatus = value;
                RaisePropertyChanged(() => PowerComStatus);
            }
        }

        #endregion


        #region AO模块通讯

        /// <summary>
        /// AO模块通讯设定
        /// </summary>
        private ComSetParaModel _aoCom = new ComSetParaModel()
            {
                ComName = "AOCom",
                PhyPortNO = "COM4",
                BoundRate = 9600,
                StartBits = 2,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = System.IO.Ports.Parity.None,

                Addr = 1,
                PeriodRW = 500,
                WatchDogPeriod = 20000,
                Timeout = 300,
                Time_BusyDealy = 50,
                CMDRepeat = 1
            };
        /// <summary>
        /// AO模块通讯设定
        /// </summary>
        public ComSetParaModel AOCom
        {
            get { return _aoCom; }
            set
            {
                _aoCom = value;
                RaisePropertyChanged(() => AOCom);
            }
        }

        /// <summary>
        /// AO模块通讯状态
        /// </summary>
        private CommStatusModel _aoComStatus = new CommStatusModel();
        /// <summary>
        /// AO模块通讯状态
        /// </summary>
        public CommStatusModel AOComStatus
        {
            get { return _aoComStatus; }
            set
            {
                _aoComStatus = value;
                RaisePropertyChanged(() => AOComStatus);
            }
        }
        
        #endregion


        #region UI选择用列表清单

        /// <summary>
        /// 获取串口列表
        /// </summary>
        public void GetComList()
        {
            SerialPortNames.Clear();
            try
            {
                Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey software11 = hklm.OpenSubKey("HARDWARE");
                //打开"HARDWARE"子健
                Microsoft.Win32.RegistryKey software = software11.OpenSubKey("DEVICEMAP");
                Microsoft.Win32.RegistryKey sitekey = software.OpenSubKey("SERIALCOMM");
                //获取当前子健
                string[] Str2 = sitekey.GetValueNames();
                //获得当前子健下面所有健组成的字符串数组
                int ValueCount = sitekey.ValueCount;
                //获得当前子健存在的健值
                int i;
                for (i = 0; i < ValueCount; i++)
                {
                    SerialPortNames.Add((string)sitekey.GetValue(Str2[i]));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("读取串口列表时出错"+e.ToString(), "错误提示");
            }
        }

        /// <summary>
        /// 计算机串口列表
        /// </summary>
        private ObservableCollection<string> _serialPortNames = new ObservableCollection<string>() { "COM1" };
        /// <summary>
        /// 计算机串口列表
        /// </summary>
        public ObservableCollection<string> SerialPortNames
        {
            get { return _serialPortNames; }
            set
            {
                _serialPortNames = value;
                RaisePropertyChanged(() => SerialPortNames);
            }
        }

        /// <summary>
        /// 波特率列表
        /// </summary>
        public ObservableCollection<int> _baudRateList = new ObservableCollection<int>() { 75, 110, 134, 150, 300, 600, 1200, 1800, 2400, 4800, 7200, 9600, 14400, 19200, 38400, 57600, 115200, 128000 };
        /// <summary>
        /// 波特率列表
        /// </summary>
        public ObservableCollection<int> BaudRateList
        {
            get { return _baudRateList; }
        }

        /// <summary>
        /// 数据位列表
        /// </summary>
        public ObservableCollection<int> _dataBitsList = new ObservableCollection<int>() { 4, 5, 6, 7, 8 };
        /// <summary>
        /// 数据位列表
        /// </summary>
        public ObservableCollection<int> DataBitsList
        {
            get { return _dataBitsList; }
        }

        /// <summary>
        /// 停止位列表
        /// </summary>
        public ObservableCollection<string> _stopBitsList = new ObservableCollection<string>() { "0", "1", "1.5", "2" };
        /// <summary>
        /// 停止位列表
        /// </summary>
        public ObservableCollection<string> StopBitsList
        {
            get { return _stopBitsList; }
        }

        /// <summary>
        /// 校验位列表
        /// </summary>
        public ObservableCollection<string> _parityList = new ObservableCollection<string>() { "奇", "偶", "无", "标志", "空格" };
        /// <summary>
        /// 计算机串口列表
        /// </summary>
        public ObservableCollection<string> ParityList
        {
            get { return _parityList; }
        }

        #endregion
    }
}

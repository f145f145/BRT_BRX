/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.Communication
 * 文件名：  CommMain
 * 版本号：  V1.0.0.0
 * 唯一标识：3e8ec6ec-2932-43d2-acd9-59cb6541512a
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/12 15:57:57
 * 描述：
 * 通讯程序。主体部分。
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/12 15:57:57		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.IO.Ports;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using BRX.Model.Dev;
using NPOI.SS.Formula.Atp;

namespace BRX.Communication
{
    public partial class Comm : ObservableObject
    {
        public Comm(DevModel dev)
        {
            //订阅发送指令消息
            Messenger.Default.Register<string>(this, "NeedOutPutMessage", NeedOutPutMessage);
            //订阅软件退出消息（关闭串口）
            Messenger.Default.Register<string>(this, "Quit", Quit);

            Dev = dev;

            //检查热电偶模块串口
            switch (Dev.TModelType)
            {
                case "DAM3138":
                    TestPort_T();
                    break;
                case "DAM3134":
                    TestPort_T3134();
                    break;
                default:
                    TestPort_T();
                    break;
            }


            //检查AO模块串口
            switch (Dev.AOModelType)
            {
                case "DAM3060C":
                    TestPort_3060C();
                    break;
                case "UT5564A":
                    TestPort_5564A();
                    break;
                case "Modbus-4AI4AO":
                    TestPort_4AI4AO();
                    break;
                default:
                    TestPort_3060C();
                    break;
            }

            //检查功率模块串口
            if (Dev.WithPowerSenser)
                TestPort_Power();

            //数据读取定时器初始化
            SPATimerInit();
        }


        #region 基本属性

        /// <summary>
        /// 装置参数
        /// </summary>
        private DevModel _dev;
        /// <summary>
        /// 装置参数
        /// </summary>
        private DevModel Dev
        {
            get { return _dev; }
            set
            {
                _dev = value;
                RaisePropertyChanged(() => Dev);
            }
        }

        /// <summary>
        /// 通讯串口A
        /// </summary>
        private SerialPort _serialPortA = new SerialPort("COM1");
        /// <summary>
        /// 通讯串口A
        /// </summary>
        private SerialPort SerialPortA
        {
            get { return _serialPortA; }
            set
            {
                _serialPortA = value;
                RaisePropertyChanged(() => SerialPortA);
            }
        }

        /// <summary>
        /// 串口A忙状态
        /// </summary>
        private bool _isBusy_SerialPortA = false;
        /// <summary>
        /// 串口A忙状态
        /// </summary>
        private bool IsBusy_SerialPortA
        {
            get { return _isBusy_SerialPortA; }
            set
            {
                _isBusy_SerialPortA = value;
                RaisePropertyChanged(() => IsBusy_SerialPortA);
            }
        }

        /// <summary>
        /// 需要更新输出状态
        /// </summary>
        private bool _needOutPut = false;
        /// <summary>
        /// 需要更新输出状态
        /// </summary>
        private bool NeedOutPut
        {
            get { return _needOutPut; }
            set
            {
                _needOutPut = value;
                RaisePropertyChanged(() => NeedOutPut);
            }
        }

        #endregion


        #region 运行属性

        /// <summary>
        /// 热电偶模块通讯测试结果
        /// </summary>
        private bool _comTTestOk = false;
        /// <summary>
        /// 热电偶模块通讯测试结果
        /// </summary>
        private bool ComTTestOk
        {
            get { return _comTTestOk; }
            set
            {
                _comTTestOk = value;
                RaisePropertyChanged(() => ComTTestOk);
            }
        }
        
        /// <summary>
        /// AO模块通讯测试结果
        /// </summary>
        private bool _comAOTestOk = false;
        /// <summary>
        /// AO模块通讯测试结果
        /// </summary>
        private bool ComAOTestOk
        {
            get { return _comAOTestOk; }
            set
            {
                _comAOTestOk = value;
                RaisePropertyChanged(() => ComAOTestOk);
            }
        }

        /// <summary>
        /// AO模块喂狗计数器
        /// </summary>
        private int _aoKickingCounter = 0;
        /// <summary>
        /// AO模块喂狗计数器
        /// </summary>
        private int AOKickingCounter
        {
            get { return _aoKickingCounter; }
            set
            {
                _aoKickingCounter = value;
                RaisePropertyChanged(() => AOKickingCounter);
            }
        }

        /// <summary>
        /// 功率模块通讯测试结果
        /// </summary>
        private bool _comPowerTestOk = false;
        /// <summary>
        /// 功率模块通讯测试结果
        /// </summary>
        private bool ComPowerTestOk
        {
            get { return _comPowerTestOk; }
            set
            {
                _comPowerTestOk = value;
                RaisePropertyChanged(() => ComPowerTestOk);
            }
        }
        #endregion


        #region 读写定时器（串口A）

        /// <summary>
        /// 综合读写定时器
        /// </summary>
        private System.Threading.Timer SPATimer;

        /// <summary>
        /// 综合读写定时器初始化函数
        /// </summary>
        private void SPATimerInit()
        {
            SPATimer = new System.Threading.Timer(new System.Threading.TimerCallback(SPATimerTick), this, 0, Dev.Period_Comm);
        }

        /// <summary>
        /// 综合读写定时器回调函数
        /// </summary>
        /// <param name="state"></param>
        private void SPATimerTick(object state)
        {
            //如果通讯占用，则延迟后再判断。若仍占用则退出此次收发。
            if (IsBusy_SerialPortA)
            {
                Thread.Sleep(Dev.AOCom.Time_BusyDealy);
                if (IsBusy_SerialPortA)
                    return;
            }
            //通讯口锁定
            IsBusy_SerialPortA = true;

            //温度数据读取
            if (ComTTestOk)
                switch (Dev.TModelType)
                {
                    case "DAM3138":
                        T_RW();
                        break;
                    case "DAM3134":
                        T_RW3134();
                        break;
                    default:
                        T_RW();
                        break;
                }

            //功率数据读写
            if (ComPowerTestOk)
                Power_RW();

            //模拟量输出读写
            if (ComAOTestOk)
                switch (Dev.AOModelType)
                {
                    case "DAM3060C":
                        AO_RW();
                        break;
                    case "UT5564A":
                        AO_RW5564A();
                        break;
                    case "Modbus-4AI4AO":
                        AO_RW4AI4AO();
                        break;
                }

            //通讯口解锁
            IsBusy_SerialPortA = false;
        }

        #endregion


        #region 需要发送指令消息处理

        /// <summary>
        /// 发送指令消息回调（接收到BLL需要发送指令）
        /// </summary>
        private void NeedOutPutMessage(string msg)
        {
            if (msg == "NeedOutPut")
            {
                NeedOutPut = true;
            }
        }

        #endregion


        #region 软件退出时装置复位

        /// <summary>
        /// 软件退出时，关闭串口。
        /// </summary>
        private void Quit(string msg)
        {
            //发送复位指令
            try
            {

                if (SerialPortA.IsOpen)
                    SerialPortA.Close();
            }
            catch (Exception)
            {
                Messenger.Default.Send<string>("通讯异常3，请检查通讯配置及硬件连接！", "ComError1");
            }
        }

        #endregion
    }
}

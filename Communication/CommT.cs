/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.Communication
 * 文件名：  CommDVP
 * 版本号：  V1.0.0.0
 * 唯一标识：4f87f5b1-e14b-4b48-993f-9e1d0f05e308
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/13 16:40:09
 * 描述：
 * 通讯程序。阿尔泰3138H热电偶模块。ModbusRTU模式
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/7/4 15:57:57		郝正强			V1.0.0.0
 *
 ************************************************************************************/
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Modbus.Device;
using static BRX.Model.Enums.Enums;

namespace BRX.Communication
{
    public partial class Comm : ObservableObject
    {

        #region 读写长度及数组属性
        
        /// <summary>
        /// 温度模块，AI返回数据长度最大值
        /// </summary>
        static int DataLenTRead = 20;

        /// <summary>
        /// DAM3134温度模块，AI返回数据长度最大值
        /// </summary>
        static int DataLenTRead3134 = 20;

        /// <summary>
        /// 温度模块，AI返回数值（ushort）
        /// </summary>
        private ushort[] _dataTRead =Enumerable.Repeat((ushort)0, DataLenTRead).ToArray();
        /// <summary>
        /// 温度模块，AI返回数值（ushort）
        /// </summary>
        public ushort[] DataTRead
        {
            get { return _dataTRead; }
            set
            {
                _dataTRead = value;
                RaisePropertyChanged(() => DataTRead);
            }
        }

        /// <summary>
        /// DAM3134温度模块，AI返回数值（ushort）
        /// </summary>
        private ushort[] _dataTRead3134 = Enumerable.Repeat((ushort)0, DataLenTRead3134).ToArray();
        /// <summary>
        /// DAM3134温度模块，AI返回数值（ushort）
        /// </summary>
        public ushort[] DataTRead3134
        {
            get { return _dataTRead3134; }
            set
            {
                _dataTRead3134 = value;
                RaisePropertyChanged(() => DataTRead3134);
            }
        }


        #endregion


        #region 开机测试、端口设定

        /// <summary>
        /// 3138H热电偶模块通讯参数设定
        /// </summary>
        private void PortSetT()
        {
            SerialPortA.PortName = Dev.TCom.PhyPortNO;
            SerialPortA.BaudRate = Dev.TCom.BoundRate;
            SerialPortA.DataBits = Dev.TCom.DataBits;
            SerialPortA.Parity = Dev.TCom.Parity;
            SerialPortA.StopBits = Dev.TCom.StopBits;
            SerialPortA.ReadTimeout = Dev.TCom.Timeout;
        }

        /// <summary>
        /// 3134热电偶模块通讯参数设定
        /// </summary>
        private void PortSetT3134()
        {
            SerialPortA.PortName = Dev.TCom.PhyPortNO;
            SerialPortA.BaudRate = Dev.TCom.BoundRate;
            SerialPortA.DataBits = Dev.TCom.DataBits;
            SerialPortA.Parity = Dev.TCom.Parity;
            SerialPortA.StopBits = Dev.TCom.StopBits;
            SerialPortA.ReadTimeout = Dev.TCom.Timeout;
        }

        /// <summary>
        /// 3138H热电偶模块端口测试
        /// </summary>
        private bool TestPort_T()
        {
            bool isSuccess;
            String[] Portname = SerialPort.GetPortNames();
            bool exist = false;
            foreach (string str in Portname)
            {
                if (str == Dev.TCom.PhyPortNO)
                    exist = true;
            }
            if (exist)
            {
                try
                {
                    isSuccess = T_RW();
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                    Messenger.Default.Send<string>("Err" + Dev.ComTErrTimes + "  热电偶模块通讯异常" + e.Message, "ComErrorT");
                    isSuccess = false;
                }
            }
            else
            {
                MessageBox.Show("热电偶模块通讯" + Dev.TCom.PhyPortNO + "不可用！");
                Messenger.Default.Send<string>("Err" + Dev.ComTErrTimes + "  热电偶模块通讯端口不可用", "ComErrorT");
                isSuccess = false;
            }

            if (!isSuccess)
            {
                Dev.TComStatus.IsFailure = true;
            }
            else
            {
                Dev.TComStatus.IsFailure = false;
            }

            ComTTestOk = isSuccess;
            isSuccess = false;
            return isSuccess;
        }


        /// <summary>
        /// 3134热电偶模块端口测试
        /// </summary>
        private bool TestPort_T3134()
        {
            bool isSuccess;
            String[] Portname = SerialPort.GetPortNames();
            bool exist = false;
            foreach (string str in Portname)
            {
                if (str == Dev.TCom.PhyPortNO)
                    exist = true;
            }
            if (exist)
            {
                try
                {
                    isSuccess = T_RW3134();
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                    Messenger.Default.Send<string>("Err" + Dev.ComTErrTimes + "  热电偶模块通讯异常" + e.Message, "ComErrorT");
                    isSuccess = false;
                }
            }
            else
            {
                MessageBox.Show("热电偶模块通讯" + Dev.TCom.PhyPortNO + "不可用！");
                Messenger.Default.Send<string>("Err" + Dev.ComTErrTimes + "  热电偶模块通讯端口不可用", "ComErrorT");
                isSuccess = false;
            }

            if (!isSuccess)
            {
                Dev.TComStatus.IsFailure = true;
            }
            else
            {
                Dev.TComStatus.IsFailure = false;
            }

            ComTTestOk = isSuccess;
            isSuccess = false;
            return isSuccess;
        }

        #endregion


        #region 读写方法

        /// <summary>
        /// 读取3138H热电偶模块
        /// </summary>
        private bool T_RW()
        {
            if (Dev.ComTStop)
                return false;

            DateTime startR = DateTime.Now;

            bool isSuccess = true;
            ushort[] aiTransData = new ushort[DataLenTRead];                   //热电偶参数读取传输数组

            try
            {
                if (SerialPortA.IsOpen)
                    SerialPortA.Close();
                PortSetT();
                SerialPortA.Open();

                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPortA);
                master.Transport.ReadTimeout = Dev.TCom.Timeout;
                master.Transport.Retries = Dev.TCom.CMDRepeat;
                byte slaveId = Convert.ToByte(Dev.TCom.Addr);
                ushort startAddress = Convert.ToUInt16(RegAddr_DAM3138H.All);
                ushort numRegisters = Convert.ToUInt16(DataQty_DAM3138HL.All);
                aiTransData = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                SerialPortA.Close();
            }
            catch (Exception e)
            {
                isSuccess = false;
                // MessageBox.Show(e.Message);
                Messenger.Default.Send<string>("Err" + Dev.ComTErrTimes + "  热电偶模块" + "读取异常" + e.Message, "ComErrorT");
            }

            
            //结果处理
            if (isSuccess)
            {
                Dev.TComStatus.IsFailure = false;
                DataTRead = (ushort[])aiTransData.Clone();

                //数据转发
                Messenger.Default.Send<ushort[]>(DataTRead, "TDatasUpdated");

                Dev.ComTErrTimes = 0;
            }
            else
            {
                Dev.TComStatus.IsFailure = true;

                Dev.ComTErrTimes++;
                if (Dev.ComTErrTimes >= 30)
                {
                    Dev.ComTStop = true;
                    Messenger.Default.Send<string>("热电偶模块通讯连续多次异常，已暂停通讯，如需恢复请重新打开软件！", "ComErrorT");
                }
            }
            
            TimeSpan r = DateTime.Now - startR;
            Trace.Write("T read：" + r.TotalMilliseconds + "\r\n");
            return isSuccess;
        }

        /// <summary>
        /// 读取3134热电偶模块
        /// </summary>
        private bool T_RW3134()
        {
            if (Dev.ComTStop)
                return false;

            DateTime startR = DateTime.Now;

            bool isSuccess = true;
            ushort[] aiTransData = new ushort[DataLenTRead3134];                   //热电偶参数读取传输数组

            try
            {
                if (SerialPortA.IsOpen)
                    SerialPortA.Close();
                PortSetT3134();
                SerialPortA.Open();

                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPortA);
                master.Transport.ReadTimeout = Dev.TCom.Timeout;
                master.Transport.Retries = Dev.TCom.CMDRepeat;
                byte slaveId = Convert.ToByte(Dev.TCom.Addr);
                ushort startAddress = Convert.ToUInt16(RegAddr_DAM3134.All);
                ushort numRegisters = Convert.ToUInt16(DataQty_DAM3134.All);
                aiTransData = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                SerialPortA.Close();
            }
            catch (Exception e)
            {
                isSuccess = false;
                // MessageBox.Show(e.Message);
                Messenger.Default.Send<string>("Err" + Dev.ComTErrTimes + "  热电偶模块" + "读取异常" + e.Message, "ComErrorT");
            }


            //结果处理
            if (isSuccess)
            {
                Dev.TComStatus.IsFailure = false;
                DataTRead3134 = (ushort[])aiTransData.Clone();

                //数据转发
                Messenger.Default.Send<ushort[]>(DataTRead3134, "T3134DatasUpdated");

                Dev.ComTErrTimes = 0;
            }
            else
            {
                Dev.TComStatus.IsFailure = true;

                Dev.ComTErrTimes++;
                if (Dev.ComTErrTimes >= 30)
                {
                    Dev.ComTStop = true;
                    Messenger.Default.Send<string>("热电偶模块通讯连续多次异常，已暂停通讯，如需恢复请重新打开软件！", "ComErrorT");
                }
            }

            TimeSpan r = DateTime.Now - startR;
            Trace.Write("T read：" + r.TotalMilliseconds + "\r\n");
            return isSuccess;
        }

        #endregion
    }
}

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
 * 通讯程序。阿尔泰3060C 4AO模块。ModbusRTU模式
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
using System.Threading;
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
        /// 3060C，AI返回数据单次长度
        /// </summary>
        static int DataLenSingle_3060C = 8;

        /// <summary>
        /// 5564A，AI返回数据单次长度
        /// </summary>
        static int DataLenSingle_5564A = 8;

        /// <summary>
        /// 4AI4AO，AO写数据单次长度
        /// </summary>
        static int DataLenSingle_4AI4AO = 4;

        /// <summary>
        /// 3060C，读写数据（ushort）
        /// </summary>
        private ushort[] _dataread_3060C = Enumerable.Repeat((ushort)0, DataLenSingle_3060C).ToArray();
        /// <summary>
        /// 3060C，读写数据（ushort）
        /// </summary>
        public ushort[] DataRW_3060C
        {
            get { return _dataread_3060C; }
            set
            {
                _dataread_3060C = value;
                RaisePropertyChanged(() => DataRW_3060C);
            }
        }

        /// <summary>
        /// 4AI4AO，读写数据（ushort）
        /// </summary>
        private ushort[] _dataread_4AI4AO = Enumerable.Repeat((ushort)0, DataLenSingle_4AI4AO).ToArray();
        /// <summary>
        /// 4AI4AO，读写数据（ushort）
        /// </summary>
        public ushort[] DataRW_4AI4AO
        {
            get { return _dataread_4AI4AO; }
            set
            {
                _dataread_4AI4AO = value;
                RaisePropertyChanged(() => DataRW_4AI4AO);
            }
        }

        /// <summary>
        /// 5564A，读写数据（ushort）
        /// </summary>
        private ushort[] _dataread_5564A = Enumerable.Repeat((ushort)0, DataLenSingle_5564A).ToArray();
        /// <summary>
        /// 5564A，读写数据（ushort）
        /// </summary>
        public ushort[] DataRW_5564A
        {
            get { return _dataread_5564A; }
            set
            {
                _dataread_5564A = value;
                RaisePropertyChanged(() => DataRW_5564A);
            }
        }
        #endregion


        #region 开机测试、端口设定

        /// <summary>
        /// 3060C通讯参数设定
        /// </summary>
        private void PortSet_3060C()
        {
            SerialPortA.PortName = Dev.AOCom.PhyPortNO;
            SerialPortA.BaudRate = Dev.AOCom.BoundRate;
            SerialPortA.DataBits = Dev.AOCom.DataBits;
            SerialPortA.Parity = Dev.AOCom.Parity;
            SerialPortA.StopBits = Dev.AOCom.StopBits;
            SerialPortA.ReadTimeout = Dev.AOCom.Timeout;
        }

        /// <summary>
        /// 5564A通讯参数设定
        /// </summary>
        private void PortSet_5564A()
        {
            SerialPortA.PortName = Dev.AOCom.PhyPortNO;
            SerialPortA.BaudRate = Dev.AOCom.BoundRate;
            SerialPortA.DataBits = Dev.AOCom.DataBits;
            SerialPortA.Parity = Dev.AOCom.Parity;
            SerialPortA.StopBits = Dev.AOCom.StopBits;
            SerialPortA.ReadTimeout = Dev.AOCom.Timeout;
        }

        /// <summary>
        /// 4AI4AO通讯参数设定
        /// </summary>
        private void PortSet_4AI4AO()
        {
            SerialPortA.PortName = Dev.AOCom.PhyPortNO;
            SerialPortA.BaudRate = Dev.AOCom.BoundRate;
            SerialPortA.DataBits = Dev.AOCom.DataBits;
            SerialPortA.Parity = Dev.AOCom.Parity;
            SerialPortA.StopBits = Dev.AOCom.StopBits;
            SerialPortA.ReadTimeout = Dev.AOCom.Timeout;
        }

        /// <summary>
        /// 3060C端口测试
        /// </summary>
        private bool TestPort_3060C()
        {
            bool isSuccess;
            String[] Portname = SerialPort.GetPortNames();
            bool exist = false;
            foreach (string str in Portname)
            {
                if (str == Dev.AOCom.PhyPortNO)
                    exist = true;
            }
            if (exist)
            {
                try
                {
                    isSuccess = AO_RW();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块通讯异常" + e.Message, "ComErrorAO");
                    isSuccess = false;
                }
            }
            else
            {
                MessageBox.Show("AO模块通讯" + Dev.AOCom.PhyPortNO + "不可用！");
                Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块通讯端口不可用", "ComErrorAO");
                isSuccess = false;
            }

            if (!isSuccess)
            {
                Dev.AOComStatus.IsFailure = true;
            }
            else
            {
                //看门狗设定
            //    Dog_Set();

                Dev.AOComStatus.IsFailure = false;
            }

            ComAOTestOk = isSuccess;
            return isSuccess;
        }

        /// <summary>
        /// 5564A端口测试
        /// </summary>
        private bool TestPort_5564A()
        {
            bool isSuccess;
            String[] Portname = SerialPort.GetPortNames();
            bool exist = false;
            foreach (string str in Portname)
            {
                if (str == Dev.AOCom.PhyPortNO)
                    exist = true;
            }
            if (exist)
            {
                try
                {
                    isSuccess = AO_RW5564A();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块通讯异常" + e.Message, "ComErrorAO");
                    isSuccess = false;
                }
            }
            else
            {
                MessageBox.Show("AO模块通讯" + Dev.AOCom.PhyPortNO + "不可用！");
                Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块通讯端口不可用", "ComErrorAO");
                isSuccess = false;
            }

            if (!isSuccess)
            {
                Dev.AOComStatus.IsFailure = true;
            }
            else
            {
                Dev.AOComStatus.IsFailure = false;
            }

            ComAOTestOk = isSuccess;
            return isSuccess;
        }

        /// <summary>
        /// 4AI4AO端口测试
        /// </summary>
        private bool TestPort_4AI4AO()
        {
            bool isSuccess;
            String[] Portname = SerialPort.GetPortNames();
            bool exist = false;
            foreach (string str in Portname)
            {
                if (str == Dev.AOCom.PhyPortNO)
                    exist = true;
            }
            if (exist)
            {
                try
                {
                    isSuccess = AO_RW4AI4AO();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块通讯异常" + e.Message, "ComErrorAO");
                    isSuccess = false;
                }
            }
            else
            {
                MessageBox.Show("AO模块通讯" + Dev.AOCom.PhyPortNO + "不可用！");
                Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块通讯端口不可用", "ComErrorAO");
                isSuccess = false;
            }

            if (!isSuccess)
            {
                Dev.AOComStatus.IsFailure = true;
            }
            else
            {
                Dev.AOComStatus.IsFailure = false;
            }

            ComAOTestOk = isSuccess;
            return isSuccess;
        }

        #endregion


        #region 读写方法


        /// <summary>
        /// 3060C写输出
        /// </summary>
        private bool AO_RW()
        {
            if (Dev.ComAOStop)
                return false;

            bool isRWSuccess = true;
            UInt16[] regData = { 0, 0, 0, 0, 0, 0, 0, 0 };

            try
            {
                if (SerialPortA.IsOpen)
                    SerialPortA.Close();
                PortSet_3060C();
                SerialPortA.Open();

                UInt16 regAddr;

                switch (Dev.AOList[0].ChannelSerialNO)
                {
                    case 1:
                        regAddr = Convert.ToUInt16(RegAddr_DAM3060C.AO1);
                        break;
                    case 2:
                        regAddr = Convert.ToUInt16(RegAddr_DAM3060C.AO2);
                        break;
                    case 3:
                        regAddr = Convert.ToUInt16(RegAddr_DAM3060C.AO3);
                        break;
                    case 4:
                        regAddr = Convert.ToUInt16(RegAddr_DAM3060C.AO4);
                        break;
                    default:
                        regAddr = Convert.ToUInt16(RegAddr_DAM3060C.AO1);
                        break;
                }
                regData[Dev.AOList[0].ChannelSerialNO * 2 - 2] = Convert.ToUInt16(Dev.Mod_AO.Channels[Dev.AOList[0].ChannelSerialNO - 1].DataRealTime);

                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPortA);
                master.Transport.ReadTimeout = Dev.AOCom.Timeout;
                byte slaveId = Convert.ToByte(Dev.AOCom.Addr);
                ushort regAddress = Convert.ToUInt16(RegAddr_DAM3060C.All);

                master.WriteMultipleRegisters(slaveId, regAddress, regData);

                //每10次（10秒）喂狗1次
                if (AOKickingCounter > 8)
                {
                    //    AO_KickingDog();
                    AOKickingCounter = 0;
                    Trace.Write("Kicking /r/n");
                }
                else
                    AOKickingCounter++;

                SerialPortA.Close();
            }
            catch (Exception e)
            {
                isRWSuccess = false;
                Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块写异常" + e.Message, "ComErrorAO");
            }

            if (isRWSuccess)
            {
                Dev.AOComStatus.IsFailure = false;
                Dev.ComAOErrTimes = 0;
                //数据转发
                Messenger.Default.Send<ushort[]>(DataRW_3060C, "AODatasUpdated");
            }
            else
            {
                Dev.AOComStatus.IsFailure = true;

                Dev.ComAOErrTimes++;
                if (Dev.ComAOErrTimes >= 30)
                {
                    Dev.ComAOStop = true;
                    Messenger.Default.Send<string>("AO模块连续多次异常，已暂停通讯，如需恢复请重新打开软件！", "ComErrorAO");
                }
            }

            return isRWSuccess;
        }

        /// <summary>
        /// 5564A写输出
        /// </summary>
        private bool AO_RW5564A()
        {
            if (Dev.ComAOStop)
                return false;

            bool isRWSuccess = true;
            UInt16[] regData = { 0, 0, 0, 0, 0, 0, 0, 0 };

            try
            {
                if (SerialPortA.IsOpen)
                    SerialPortA.Close();
                PortSet_5564A();
                SerialPortA.Open();

                UInt16 regAddr;

                switch (Dev.AOList[0].ChannelSerialNO)
                {
                    case 1:
                        regAddr = Convert.ToUInt16(RegAddr_UT5564A.AO1);
                        break;
                    case 2:
                        regAddr = Convert.ToUInt16(RegAddr_UT5564A.AO2);
                        break;
                    case 3:
                        regAddr = Convert.ToUInt16(RegAddr_UT5564A.AO3);
                        break;
                    case 4:
                        regAddr = Convert.ToUInt16(RegAddr_UT5564A.AO4);
                        break;
                    default:
                        regAddr = Convert.ToUInt16(RegAddr_UT5564A.AO1);
                        break;
                }
                regData[Dev.AOList[0].ChannelSerialNO * 2 - 2] = Convert.ToUInt16(Dev.Mod_AO.Channels[Dev.AOList[0].ChannelSerialNO - 1].DataRealTime);

                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPortA);
                master.Transport.ReadTimeout = Dev.AOCom.Timeout;
                byte slaveId = Convert.ToByte(Dev.AOCom.Addr);
                ushort regAddress = Convert.ToUInt16(RegAddr_UT5564A.All);

                master.WriteMultipleRegisters(slaveId, regAddress, regData);
                
                SerialPortA.Close();
            }
            catch (Exception e)
            {
                isRWSuccess = false;
                Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块写异常" + e.Message, "ComErrorAO");
            }

            if (isRWSuccess)
            {
                Dev.AOComStatus.IsFailure = false;
                Dev.ComAOErrTimes = 0;
                //数据转发
                Messenger.Default.Send<ushort[]>(DataRW_3060C, "AODatasUpdated");
            }
            else
            {
                Dev.AOComStatus.IsFailure = true;

                Dev.ComAOErrTimes++;
                if (Dev.ComAOErrTimes >= 30)
                {
                    Dev.ComAOStop = true;
                    Messenger.Default.Send<string>("AO模块连续多次异常，已暂停通讯，如需恢复请重新打开软件！", "ComErrorAO");
                }
            }

            return isRWSuccess;
        }

        /// <summary>
        /// 4AI4AO写输出
        /// </summary>
        private bool AO_RW4AI4AO()
        {
            if (Dev.ComAOStop)
                return false;

            bool isRWSuccess = true;
            UInt16[] regData = { 0, 0, 0, 0 };

            try
            {
                if (SerialPortA.IsOpen)
                    SerialPortA.Close();
                PortSet_4AI4AO();
                SerialPortA.Open();

                UInt16 regAddr;

                switch (Dev.AOList[0].ChannelSerialNO)
                {
                    case 1:
                        regAddr = Convert.ToUInt16(RegAddr_4AI4AO.AO1);
                        break;
                    case 2:
                        regAddr = Convert.ToUInt16(RegAddr_4AI4AO.AO2);
                        break;
                    case 3:
                        regAddr = Convert.ToUInt16(RegAddr_4AI4AO.AO3);
                        break;
                    case 4:
                        regAddr = Convert.ToUInt16(RegAddr_4AI4AO.AO4);
                        break;
                    default:
                        regAddr = Convert.ToUInt16(RegAddr_4AI4AO.AO1);
                        break;
                }
                regData[Dev.AOList[0].ChannelSerialNO  - 1] = Convert.ToUInt16(Dev.Mod_AO.Channels[Dev.AOList[0].ChannelSerialNO - 1].DataRealTime);

                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPortA);
                master.Transport.ReadTimeout = Dev.AOCom.Timeout;
                byte slaveId = Convert.ToByte(Dev.AOCom.Addr);
                ushort regAddress = Convert.ToUInt16(RegAddr_4AI4AO.All);

                master.WriteMultipleRegisters(slaveId, regAddress, regData);

                SerialPortA.Close();
            }
            catch (Exception e)
            {
                isRWSuccess = false;
                Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块写异常" + e.Message, "ComErrorAO");
            }

            if (isRWSuccess)
            {
                Dev.AOComStatus.IsFailure = false;
                Dev.ComAOErrTimes = 0;
                //数据转发
                Messenger.Default.Send<ushort[]>(DataRW_4AI4AO, "AODatasUpdated");
            }
            else
            {
                Dev.AOComStatus.IsFailure = true;

                Dev.ComAOErrTimes++;
                if (Dev.ComAOErrTimes >= 30)
                {
                    Dev.ComAOStop = true;
                    Messenger.Default.Send<string>("AO模块连续多次异常，已暂停通讯，如需恢复请重新打开软件！", "ComErrorAO");
                }
            }

            return isRWSuccess;
        }


        /// <summary>
        /// 读取3060C
        /// </summary>
        private bool AO_Get()
        {
            DateTime startR = DateTime.Now;
            bool isSuccess = true;
            ushort[] aiTransData = new ushort[DataLenSingle_3060C];                   //AI参数单次读取传输数组

            try
            {
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPortA);
                master.Transport.ReadTimeout = Dev.AOCom.Timeout;
                master.Transport.Retries = Dev.AOCom.CMDRepeat;
                byte slaveId = Convert.ToByte(Dev.AOCom.Addr);
                ushort startAddress = Convert.ToUInt16(RegAddr_DAM3060C.All);
                ushort numRegisters = Convert.ToUInt16(DataQty_DAM3060C.All);
                aiTransData = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
            }
            catch (Exception e)
            {
                isSuccess = false;
                // MessageBox.Show(e.Message);
                Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块读取异常" + e.Message, "ComErrorAO");
            }
            if (isSuccess)
            {
                Dev.AOComStatus.IsFailure = false;
                aiTransData.CopyTo(DataRW_3060C, 0);

                //数据转发
                Messenger.Default.Send<ushort[]>(DataRW_3060C, "AODatasUpdated");
            }
            else
            {
                Dev.AOComStatus.IsFailure = true;
                isSuccess = false;
            }
           
            TimeSpan r = DateTime.Now - startR;
            Trace.Write("AO read：" + r.TotalMilliseconds + "\r\n");
            return isSuccess;
        }

        #endregion


        #region 看门狗相关


        /// <summary>
        /// 3060C喂狗
        /// </summary>
        private bool AO_KickingDog()
        {
            bool isRWSuccess = true;
            UInt16[] regDataDog = {1};

            try
            {
                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPortA);
                master.Transport.ReadTimeout = Dev.AOCom.Timeout;
                byte slaveId = Convert.ToByte(Dev.AOCom.Addr);
                ushort regAddress = Convert.ToUInt16(RegAddr_DAM3060C_Dog.DogOverFlow);
                master.WriteMultipleRegisters(slaveId, regAddress, regDataDog);
            }
            catch (Exception e)
            {
                isRWSuccess = false;
                Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块喂狗异常" + e.Message, "ComErrorAO");
            }

            if (isRWSuccess)
            {
                Dev.AOComStatus.IsFailure = false;
                Dev.ComAOErrTimes = 0;
            }
            else
            {
                Dev.AOComStatus.IsFailure = true;
                Dev.ComAOErrTimes++;
                if (Dev.ComAOErrTimes >= 30)
                {
                    Dev.ComAOStop = true;
                    Messenger.Default.Send<string>("AO模块连续多次异常，已暂停通讯，如需恢复请重新打开软件！", "ComErrorAO");
                }
            }

            return isRWSuccess;
        }



        /// <summary>
        /// 3060C看门狗设定
        /// </summary>
        private void Dog_Set()
        {
            try
            {
                if (SerialPortA.IsOpen)
                    SerialPortA.Close();
                Thread.Sleep(500);
                SerialPortA.Open();

               ushort regAddress;
                UInt16[] regData={0};

                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPortA);
                master.Transport.ReadTimeout = Dev.AOCom.Timeout;
                master.Transport.Retries = Dev.AOCom.CMDRepeat;
                byte slaveId = Convert.ToByte(Dev.AOCom.Addr);

                //看门狗停用
                regAddress = Convert.ToUInt16(RegAddr_DAM3060C_Dog.DogEnable);
                regData[0] =0;
                master.WriteMultipleRegisters(slaveId, regAddress, regData);
                Thread.Sleep(500);
                master.WriteMultipleRegisters(slaveId, regAddress, regData);
                Thread.Sleep(500);

                //设定定时器
                regAddress = Convert.ToUInt16(RegAddr_DAM3060C_Dog.DogTimer);
                regData[0] = Convert.ToUInt16(Dev.TCom.WatchDogPeriod / 100);
                if (regData[0] > 255)
                    regData[0] = 255;
                if (regData[0] < 100)
                    regData[0] = 100;
                master.WriteMultipleRegisters(slaveId, regAddress, regData);
                Thread.Sleep(500);
                master.WriteMultipleRegisters(slaveId, regAddress, regData);
                Thread.Sleep(500);

                //溢出复位
                regAddress = Convert.ToUInt16(RegAddr_DAM3060C_Dog.DogOverFlow);
                regData[0] = 0;
                master.WriteMultipleRegisters(slaveId, regAddress, regData);
                Thread.Sleep(500);
                master.WriteMultipleRegisters(slaveId, regAddress, regData);
                Thread.Sleep(500);

                //喂狗
                regAddress = Convert.ToUInt16(RegAddr_DAM3060C_Dog.DogReset);
                regData[0] = 0x55AA;
                master.WriteMultipleRegisters(slaveId, regAddress, regData);
                Thread.Sleep(500);
                master.WriteMultipleRegisters(slaveId, regAddress, regData);
                Thread.Sleep(500);

                //启用看门狗
                regAddress = Convert.ToUInt16(RegAddr_DAM3060C_Dog.DogEnable);
                regData[0] = 1;
                master.WriteMultipleRegisters(slaveId, regAddress, regData);
                Thread.Sleep(500);
                master.WriteMultipleRegisters(slaveId, regAddress, regData);

                SerialPortA.Close();
            }
            catch (Exception e)
            {
                Messenger.Default.Send<string>("Err" + Dev.ComAOErrTimes + "  AO模块看门狗设定异常" + e.Message, "ComErrorAO");
            }
        }

        #endregion
    }
}

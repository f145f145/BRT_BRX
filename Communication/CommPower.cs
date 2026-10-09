/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 通讯程序。中测功率采集模块。ModbusRTU模式
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2023/5/10 15:57:57		郝正强			V1.0.0.0
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
        /// 功率模块，AI返回数据长度最大值
        /// </summary>
        static int DataLenPowerRead = 20;

        /// <summary>
        /// 功率模块，AI返回数值（ushort）
        /// </summary>
        private ushort[] _dataPowerRead = Enumerable.Repeat((ushort)0, DataLenPowerRead).ToArray();
        /// <summary>
        /// 功率模块，AI返回数值（ushort）
        /// </summary>
        public ushort[] DataPowerRead
        {
            get { return _dataPowerRead; }
            set
            {
                _dataPowerRead = value;
                RaisePropertyChanged(() => DataPowerRead);
            }
        }
        #endregion


        #region 开机测试、端口设定

        /// <summary>
        /// 功率模块通讯参数设定
        /// </summary>
        private void PortSetPower()
        {
            SerialPortA.PortName = Dev.PowerCom.PhyPortNO;
            SerialPortA.BaudRate = Dev.PowerCom.BoundRate;
            SerialPortA.DataBits = Dev.PowerCom.DataBits;
            SerialPortA.Parity = Dev.PowerCom.Parity;
            SerialPortA.StopBits = Dev.PowerCom.StopBits;
            SerialPortA.ReadTimeout = Dev.PowerCom.Timeout;
        }

        /// <summary>
        /// 功率模块端口测试
        /// </summary>
        private bool TestPort_Power()
        {
            bool isSuccess;
            String[] Portname = SerialPort.GetPortNames();
            bool exist = false;
            foreach (string str in Portname)
            {
                if (str == Dev.PowerCom.PhyPortNO)
                    exist = true;
            }
            if (exist)
            {
                try
                {
                    isSuccess = Power_RW();
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message);
                    Messenger.Default.Send<string>("Err" + Dev.ComPowerErrTimes + "  功率模块通讯异常" + e.Message, "ComErrorPower");
                    isSuccess = false;
                }
            }
            else
            {
                MessageBox.Show("功率模块通讯" + Dev.PowerCom.PhyPortNO + "不可用！");
                Messenger.Default.Send<string>("Err" + Dev.ComPowerErrTimes + "  功率模块通讯端口不可用", "ComErrorPower");
                isSuccess = false;
            }

            if (!isSuccess)
            {
                Dev.PowerComStatus.IsFailure = true;
            }
            else
            {
                Dev.PowerComStatus.IsFailure = false;
            }

            ComPowerTestOk = isSuccess;
            isSuccess = false;
            return isSuccess;
        }

        #endregion


        #region 读写方法

        /// <summary>
        /// 功率模块
        /// </summary>
        private bool Power_RW()
        {
            if (Dev.ComPowerStop)
                return false;

            DateTime startR = DateTime.Now;

            bool isSuccess = true;
            ushort[] aiTransData = new ushort[DataLenPowerRead];                   //功率参数读取传输数组

            try
            {
                if (SerialPortA.IsOpen)
                    SerialPortA.Close();
                PortSetPower();
                SerialPortA.Open();

                IModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPortA);
                master.Transport.ReadTimeout = Dev.PowerCom.Timeout;
                master.Transport.Retries = Dev.PowerCom.CMDRepeat;
                byte slaveId = Convert.ToByte(Dev.PowerCom.Addr);
                ushort startAddress = Convert.ToUInt16(RegAddr_Power.All);
                ushort numRegisters = Convert.ToUInt16(DataQty_Power.All);
                aiTransData = master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);

                SerialPortA.Close();
            }
            catch (Exception e)
            {
                isSuccess = false;
                // MessageBox.Show(e.Message);
                Messenger.Default.Send<string>("Err" + Dev.ComPowerErrTimes + "  功率模块" + "读取异常" + e.Message, "ComErrorPower");
            }

            
            //结果处理
            if (isSuccess)
            {
                Dev.PowerComStatus.IsFailure = false;
                DataPowerRead = (ushort[])aiTransData.Clone();

                //数据转发
                Messenger.Default.Send<ushort[]>(DataPowerRead, "PowerDatasUpdated");

                Dev.ComPowerErrTimes = 0;
            }
            else
            {
                Dev.PowerComStatus.IsFailure = true;

                Dev.ComPowerErrTimes++;
                if (Dev.ComPowerErrTimes >= 30)
                {
                    Dev.ComPowerStop = true;
                    Messenger.Default.Send<string>("功率模块通讯连续多次异常，已暂停通讯，如需恢复请重新打开软件！", "ComErrorPower");
                }
            }
            
            TimeSpan r = DateTime.Now - startR;
            Trace.Write("Power read：" + r.TotalMilliseconds + "\r\n");
            return isSuccess;
        }

        #endregion
    }
}

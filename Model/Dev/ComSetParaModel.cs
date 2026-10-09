/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.Model.Dev
 * 文件名：  CommSettingsModel
 * 版本号：  V1.0.0.0
 * 唯一标识：8acc340b-e2b4-45c7-8f6e-846bfc34b7a5
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com-+
 * 创建时间：2021/12/10 8:33:41
 * 描述：
 * 串口通讯设定参数Modelo
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2021/12/10 8:33:41		郝正强			V1.0.0.0
 * 
 ************************************************************************************/

using System.IO.Ports;
using GalaSoft.MvvmLight;

namespace BRX.Model.Dev
{
    /// <summary>
    /// 通讯参数设定类
    /// </summary>
    public class ComSetParaModel : ObservableObject 
    {
        #region 通讯设定参数属性

        /// <summary>
        /// 串口连接名称
        /// </summary>
        private string _comName = "DVPCom";
        /// <summary>
        /// 串口连接名称
        /// </summary>
        public string ComName
        {
            get { return _comName; }
            set
            {
                _comName = value;
                RaisePropertyChanged(() => ComName);
            }
        }

        /// <summary>
        /// 物理串口编号
        /// </summary>
        private string _phyPortNO = "COM7";
        /// <summary>
        /// 物理串口编号
        /// </summary>
        public string PhyPortNO
        {
            get { return _phyPortNO; }
            set
            {
                _phyPortNO = value;
                RaisePropertyChanged(() => PhyPortNO);
            }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        private int _boundRate = 19200;
        /// <summary>
        /// 波特率
        /// </summary>
        public int BoundRate
        {
            get { return _boundRate; }
            set
            {
                _boundRate = value;
                RaisePropertyChanged(() => BoundRate);
            }
        }

        /// <summary>
        /// 起始位长
        /// </summary>
        private int _startBits=1;
        /// <summary>
        /// 起始位长
        /// </summary>
        public int StartBits
        {
            get { return _startBits; }
            set
            {
                _startBits = value;
                RaisePropertyChanged(() => StartBits);
            }
        }

        /// <summary>
        /// 数据位长
        /// </summary>
        private int _dataBits = 8;
        /// <summary>
        /// 数据位长
        /// </summary>
        public int DataBits
        {
            get { return _dataBits; }
            set
            {
                _dataBits = value;
                RaisePropertyChanged(() => DataBits);
            }
        }

        /// <summary>
        /// 停止位长
        /// </summary>
        private System.IO.Ports.StopBits _stopBits=StopBits.One ;
        /// <summary>
        /// 停止位长
        /// </summary>
        public System.IO.Ports.StopBits StopBits
        {
            get { return _stopBits; }
            set
            {
                _stopBits = value;
                RaisePropertyChanged(() => StopBits);
            }
        }

        /// <summary>
        /// 校验位
        /// </summary>
        private System.IO.Ports.Parity _parity=Parity.Even ;
        /// <summary>
        /// 校验位
        /// </summary>
        public System.IO.Ports.Parity Parity
        {
            get { return _parity; }
            set
            {
                _parity = value;
                RaisePropertyChanged(() => Parity);
            }
        }

        /// <summary>
        /// 器件地址
        /// </summary>
        private int _addr=2;
        /// <summary>
        /// 器件地址
        /// </summary>
        public int Addr
        {
            get { return _addr; }
            set
            {
                _addr = value;
                RaisePropertyChanged(() => Addr);
            }
        }

        /// <summary>
        /// 定时读写周期(ms)
        /// </summary>
        private int _periodRW=300;
        /// <summary>
        /// 定时读写周期(ms)
        /// </summary>
        public int PeriodRW
        {
            get { return _periodRW; }
            set
            {
                _periodRW = value;
                RaisePropertyChanged(() => PeriodRW);
            }
        }

        /// <summary>
        /// 看门狗置位周期(ms)
        /// </summary>
        private int _watchDogPeriod=2000;
        /// <summary>
        /// 看门狗置位周期(ms)
        /// </summary>
        public int WatchDogPeriod
        {
            get { return _watchDogPeriod; }
            set
            {
                _watchDogPeriod = value;
                RaisePropertyChanged(() => WatchDogPeriod);
            }
        }

        /// <summary>
        /// 读写通讯超时时长
        /// </summary>
        private int _timeout=100;
        /// <summary>
        /// 读写通讯超时时长
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
            set
            {
                _timeout = value;
                RaisePropertyChanged(() => Timeout);
            }
        }

        /// <summary>
        /// 通讯忙时延迟时长(ms)
        /// </summary>
        private int _time_BusyDealy=40;
        /// <summary>
        /// 通讯忙时延迟时长(ms)
        /// </summary>
        public int Time_BusyDealy
        {
            get { return _time_BusyDealy; }
            set
            {
                _time_BusyDealy = value;
                RaisePropertyChanged(() => Time_BusyDealy);
            }
        }

        /// <summary>
        /// 指令重复次数
        /// </summary>
        private int _cmdRepeat=1;
        /// <summary>
        /// 指令重复次数
        /// </summary>
        public int CMDRepeat
        {
            get { return _cmdRepeat; }
            set
            {
                _cmdRepeat = value;
                RaisePropertyChanged(() => CMDRepeat);
            }
        }

        #endregion

    }
}

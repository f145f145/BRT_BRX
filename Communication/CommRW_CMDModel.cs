/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Communication
 * 文件名：  MQZH_CommRW_CMDModel
 * 版本号：  V1.0.0.0
 * 唯一标识：b0233101-240b-4694-a3b7-58511c4b2979
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/12/11 21:31:20
 * 描述：
 * 通讯指令Model。
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/12/11       21:31:20		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using Modbus.Device;
using static BRX.Model.Enums.Enums ;

namespace BRX.Communication
{
    public class CommRW_CMDModel : ObservableObject
    {
        ///<summary>
        ///指令类别
        ///</summary>
        private CommCMDType _cmdType = CommCMDType.None;
        /// <summary>
        /// 指令类别
        /// </summary>
        public CommCMDType CMDType
        {
            get { return _cmdType; }
            set
            {
                _cmdType = value;
                RaisePropertyChanged(() => CMDType);
            }
        }

        /// <summary>
        /// 寄存器起始地址
        /// </summary>
        private ushort _startAddress = 0;
        /// <summary>
        /// 寄存器起始地址
        /// </summary>
        public ushort StartAddress
        {
            get { return _startAddress; }
            set
            {
                _startAddress = value;
                RaisePropertyChanged(() => StartAddress);
            }
        }

        /// <summary>
        /// 读写数量
        /// </summary>
        private ushort _number=0;
        /// <summary>
        /// 读写数量
        /// </summary>
        public ushort Number
        {
            get { return _number; }
            set
            {
                _number = value;
                RaisePropertyChanged(() => Number);
            }
        }

        /// <summary>
        /// 写入的单个布尔值
        /// </summary>
        private bool _boolValue_Single = true;
        /// <summary>
        /// 写入的单个布尔值
        /// </summary>
        public bool BoolValue_Single
        {
            get { return _boolValue_Single; }
            set
            {
                _boolValue_Single = value;
                RaisePropertyChanged(() => _boolValue_Single);
            }
        }

        /// <summary>
        /// 写入的多个布尔值
        /// </summary>
        private bool[] _boolValues_Multi = new bool[1];
        /// <summary>
        /// 写入的多个布尔值
        /// </summary>
        public bool[] BoolValues_Multi
        {
            get { return _boolValues_Multi; }
            set
            {
                _boolValues_Multi = value;
                RaisePropertyChanged(() => BoolValues_Multi);
            }
        }

        /// <summary>
        /// 写入的单个寄存器值
        /// </summary>
        private ushort _ushortValue_Single=0;
        /// <summary>
        /// 写入的单个寄存器值
        /// </summary>
        public ushort UshortValue_Single
        {
            get { return _ushortValue_Single; }
            set
            {
                _ushortValue_Single = value;
                RaisePropertyChanged(() => UshortValue_Single);
            }
        }

        /// <summary>
        /// 写入的多个寄存器值
        /// </summary>
        private ushort[] _ushortValues_Multi = new ushort[1];
        /// <summary>
        /// 写入的多个寄存器值
        /// </summary>
        public ushort[] UshortValues_Multi
        {
            get { return _ushortValues_Multi; }
            set
            {
                _ushortValues_Multi = value;
                RaisePropertyChanged(() => UshortValues_Multi);
            }
        }

        /// <summary>
        /// 收发次数
        /// </summary>
        private int _repeatTimes = 1;
        /// <summary>
        /// 收发次数
        /// </summary>
        public int RepeatTimes
        {
            get { return _repeatTimes; }
            set
            {
                _repeatTimes = value;
                RaisePropertyChanged(() => RepeatTimes);
            }
        }

    }
}

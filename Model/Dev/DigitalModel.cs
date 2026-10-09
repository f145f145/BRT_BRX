/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.Model.Dev
 * 文件名：  DigitalModel
 * 版本号：  V1.0.0.0
 * 唯一标识：85df1ec1-7e4c-4fdf-96a4-212321641cb7
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/14 0:08:26
 * 描述：
 * 数字量Model
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/14 0:08:26		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace BRX.Model.Dev
{
    public class DigitalModel:ObservableObject 
    {
        #region 数字量信号基本设定属性

        /// <summary>
        /// 信号编号
        /// </summary>
        private string _singalNO = "";
        /// <summary>
        /// 信号编号
        /// </summary>
        public string SingalNO
        {
            get { return _singalNO; }
            set
            {
                _singalNO = value;
                RaisePropertyChanged(() => SingalNO);
            }
        }

        /// <summary>
        /// 信号名称
        /// </summary>
        private string _singalName = "";
        /// <summary>
        /// 信号名称
        /// </summary>
        public string SingalName
        {
            get { return _singalName; }
            set
            {
                _singalName = value;
                RaisePropertyChanged(() => SingalName);
            }
        }
        
        /// <summary>
        /// 输入输出类型（true为输出）
        /// </summary>
        private bool _isOutType = false;
        /// <summary>
        /// 输入输出类型（true为输出）
        /// </summary>
        public bool IsOutType
        {
            get { return _isOutType; }
            set
            {
                _isOutType = value;
                RaisePropertyChanged(() => IsOutType);
            }
        }

        /// <summary>
        /// 接入模块编号
        /// </summary>
        private string _modulNO = "08SN-1";
        /// <summary>
        /// 接入模块编号
        /// </summary>
        public string ModulNO
        {
            get { return _modulNO; }
            set
            {
                _modulNO = value;
                RaisePropertyChanged(() => ModulNO);
            }
        }

        /// <summary>
        /// 接入通道在模块中的编号（从1开始）
        /// </summary>
        private int _channelSerialNO = 1;
        /// <summary>
        /// 接入通道在模块中的编号（从1开始）
        /// </summary>
        public int ChannelSerialNO
        {
            get { return _channelSerialNO; }
            set
            {
                _channelSerialNO = value;
                RaisePropertyChanged(() => ChannelSerialNO);
            }
        }

        #endregion

        
        #region 推导及运行属性

        /// <summary>
        /// 信号对应通道
        /// </summary>
        private DigitalChannelModel _digitalChannel = new DigitalChannelModel();
        /// <summary>
        /// 通道列表
        /// </summary>
        public DigitalChannelModel DigitalChannel
        {
            get { return _digitalChannel; }
            set
            {
                _digitalChannel = value;
                RaisePropertyChanged(() => DigitalChannel);
            }
        }

        /// <summary>
        /// 参数开关状态（闭合为ON，断开为OFF）
        /// </summary>
        private bool _isOn = false;
        /// <summary>
        /// 参数开关状态（闭合为ON，断开为OFF）
        /// </summary>
        public bool IsOn
        {
            get
            {
                return _isOn;
            }
            set
            {
                _isOn = value;
                SetDOStatus();
                RaisePropertyChanged(() => IsOn);
                RaisePropertyChanged(() => WZ);
            }
        }

        /// <summary>
        /// 状态文字(开启时)
        /// </summary>
        public string _wzOn = "";
        /// <summary>
        /// 状态文字（关闭时）
        /// </summary>
        public string _wzOff = "";
        /// <summary>
        /// 状态文字（显示）
        /// </summary>
        public string WZ
        {
            get { return IsOn ? _wzOn : _wzOff; }
        }

        #endregion


        #region 更新方法

        /// <summary>
        /// 更新DI参数状态
        /// </summary>
        public void GetDIStatus()
        {
            if (!IsOutType)
                IsOn = DigitalChannel.IsOn;
        }

        /// <summary>
        /// 更新DO通道输出状态
        /// </summary>
        public void SetDOStatus()
        {
            if (IsOutType)
                DigitalChannel.IsOn = IsOn;
        }

        #endregion
    }



    //-----------------------------------------------------------------------------------------------------
    /// <summary>
    /// 数字量模块Model类
    /// </summary>
    public class DigitalModuleModel : ObservableObject
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        private string _moduleNO = "08SN-1";
        /// <summary>
        /// 模块编号
        /// </summary>
        public string ModuleNO
        {
            get { return _moduleNO; }
            set
            {
                _moduleNO = value;
                RaisePropertyChanged(() => ModuleNO);
            }
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        private string _moduleName = "08SN模块1";
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName
        {
            get { return _moduleName; }
            set
            {
                _moduleName = value;
                RaisePropertyChanged(() => ModuleName);
            }
        }

        /// <summary>
        /// 通道列表
        /// </summary>
        private ObservableCollection<DigitalChannelModel> _channels = new ObservableCollection<DigitalChannelModel>();
        /// <summary>
        /// 通道列表
        /// </summary>
        public ObservableCollection<DigitalChannelModel> Channels
        {
            get { return _channels; }
            set
            {
                _channels = value;
                RaisePropertyChanged(() => Channels);
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------
    /// <summary>
    ///  数字量通道Model类
    /// </summary>
    public class DigitalChannelModel : ObservableObject
    {
        /// <summary>
        /// 通道编号
        /// </summary>
        private string _channelNO = "08SN-1-1";
        /// <summary>
        /// 通道编号
        /// </summary>
        public string ChannelNO
        {
            get { return _channelNO; }
            set
            {
                _channelNO = value;
                RaisePropertyChanged(() => ChannelNO);
            }
        }
        
        /// <summary>
        /// 通道占用标志
        /// </summary>
        private bool _isUsed = false;
        /// <summary>
        /// 通道占用标志
        /// </summary>
        public bool IsUsed
        {
            get { return _isUsed; }
            set
            {
                _isUsed = value;
                RaisePropertyChanged(() => IsUsed);
            }
        }

        /// <summary>
        /// 通道输出状态（导通为ON，断开为OFF）
        /// </summary>
        private bool _isOn = false;
        /// <summary>
        /// 通道输出状态（导通为ON，断开为OFF）
        /// </summary>
        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                _isOn = value;
                RaisePropertyChanged(() => IsOn);
            }
        }

        /// <summary>
        /// 输入输出类型（true为输出）
        /// </summary>
        private bool _isOutType = false;
        /// <summary>
        /// 输入输出类型（true为输出）
        /// </summary>
        public bool IsOutType
        {
            get { return _isOutType; }
            set
            {
                _isOutType = value;
                RaisePropertyChanged(() => IsOutType);
            }
        }
    }
}
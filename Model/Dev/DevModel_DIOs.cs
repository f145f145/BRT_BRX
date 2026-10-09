/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.Model.Dev
 * 文件名：  DevModel_DIOs
 * 版本号：  V1.0.0.0
 * 唯一标识：fed127a8-0cdc-4411-b9c5-4555e0a53adf
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/14 8:24:56
 * 描述：
 * 装置Model，装置数字量信号部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/3 22:45:36		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace BRX.Model.Dev
{
    public partial class DevModel : ObservableObject
    {
        #region 数字量模块

        /// <summary>
        /// 多功能模块DI通道
        /// </summary>
        private DigitalModuleModel _mod_DI = new DigitalModuleModel()
        {
            ModuleNO = "DI",
            ModuleName = "艾莫迅DI",
            Channels = new ObservableCollection<DigitalChannelModel>()
            {
                new DigitalChannelModel()
            }
        };
        /// <summary>
        /// DVP-多功能模块DI通道
        /// </summary>
        public DigitalModuleModel Mod_DI
        {
            get { return _mod_DI; }
            set
            {
                _mod_DI = value;
                RaisePropertyChanged(() => Mod_DI);
            }
        }

        /// <summary>
        /// 多功能模块DO通道
        /// </summary>
        private DigitalModuleModel _mod_DO = new DigitalModuleModel()
        {
            ModuleNO = "DO",
            ModuleName = "艾莫迅DO",
            Channels = new ObservableCollection<DigitalChannelModel>()
            {
                new DigitalChannelModel()
            }
        };
        /// <summary>
        /// DVP-多功能模块DO通道
        /// </summary>
        public DigitalModuleModel Mod_DO
        {
            get { return _mod_DO; }
            set
            {
                _mod_DO = value;
                RaisePropertyChanged(() => Mod_DO);
            }
        }

        #endregion


        #region 数字量参数

        /// <summary>
        /// 数字量输入参数列表
        /// </summary>
        /// <remarks>自动调速</remarks>
        private ObservableCollection<DigitalModel> _diList = new ObservableCollection<DigitalModel>()
        {
            new DigitalModel(){_wzOn ="自动调速"}
        };
        /// <summary>
        /// 数字量输入参数列表
        /// </summary>
        /// <remarks>自动调速</remarks>
        public ObservableCollection<DigitalModel> DIList
        {
            get { return _diList; }
            set
            {
                _diList = value;
                RaisePropertyChanged(() => DIList);
            }
        }

        /// <summary>
        /// 数字量输出参数列表
        /// </summary>
        /// <remarks>变频运行</remarks>
        private ObservableCollection<DigitalModel> _doList = new ObservableCollection<DigitalModel>()
        {
            new DigitalModel(){_wzOn ="关闭变频"}
        };
        /// <summary>
        /// 数字量输出参数列表
        /// </summary>
        /// <remarks>变频运行</remarks>
        public ObservableCollection<DigitalModel> DOList
        {
            get { return _doList; }
            set
            {
                _doList = value;
                RaisePropertyChanged(() => DOList);
            }
        }

        #endregion

    }
}

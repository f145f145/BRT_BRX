/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 装置Model，装置模拟量信号部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/3 22:45:36		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRX.Model.Dev
{
    public partial class DevModel : ObservableObject
    {
        #region 模拟量模块

        /// <summary>
        /// 热电偶模块
        /// </summary>
        private AnalogModuleModel _mod_AI_T = new AnalogModuleModel()
        {
            ModuleNO = "Module_T",
            ModuleName = "热电偶采集模块",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(), new AnalogChannelModel(), new AnalogChannelModel(), new AnalogChannelModel(),
                new AnalogChannelModel(), new AnalogChannelModel(), new AnalogChannelModel(), new AnalogChannelModel()
            }
        };
        /// <summary>
        /// 热电偶模块
        /// </summary>
        public AnalogModuleModel Mod_AI_T
        {
            get { return _mod_AI_T; }
            set
            {
                _mod_AI_T = value;
                RaisePropertyChanged(() => Mod_AI_T);
            }
        }


        /// <summary>
        /// 模拟量输出模块
        /// </summary>
        private AnalogModuleModel _mod_AO = new AnalogModuleModel()
        {
            ModuleNO = "Module_AO",
            ModuleName = "模拟量输出模块",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(),new AnalogChannelModel(),
                new AnalogChannelModel(),new AnalogChannelModel()
            }
        };
        /// <summary>
        /// 模拟量输出模块
        /// </summary>
        public AnalogModuleModel Mod_AO
        {
            get { return _mod_AO; }
            set
            {
                _mod_AO = value;
                RaisePropertyChanged(() => Mod_AO);
            }
        }


        /// <summary>
        /// 功率采集模块（电压、电流、有功功率、无功功率、视在功率、功率因数、频率）
        /// </summary>
        private AnalogModuleModel _mod_AI_Power = new AnalogModuleModel()
        {
            ModuleNO = "Module_Power",
            ModuleName = "功率变送器",
            Channels = new ObservableCollection<AnalogChannelModel>()
            {
                new AnalogChannelModel(), new AnalogChannelModel(), new AnalogChannelModel(), new AnalogChannelModel(),
                new AnalogChannelModel(), new AnalogChannelModel(), new AnalogChannelModel()
            }
        };
        /// <summary>
        /// 功率采集模块（电压、电流、有功功率、无功功率、视在功率、功率因数、频率）
        /// </summary>
        public AnalogModuleModel Mod_AI_Power
        {
            get { return _mod_AI_Power; }
            set
            {
                _mod_AI_Power = value;
                RaisePropertyChanged(() => Mod_AI_Power);
            }
        }

        #endregion


        #region 模拟量参数

        /// <summary>
        /// 模拟量输入参数列表（炉内温度1，炉内温度2，试样中心温度，试样表面温度，标定中心温度，标定炉壁温度1，标定炉壁温度2，标定炉壁温度3
        /// 电压、电流、有功功率、无功功率、视在功率、功率因数、频率）
        /// </summary>
        private ObservableCollection<AnalogModel> _aiList = new ObservableCollection<AnalogModel>()
        {
            new AnalogModel(),new AnalogModel(),new AnalogModel(),new AnalogModel(),
            new AnalogModel(),new AnalogModel(),new AnalogModel(),new AnalogModel(),
            new AnalogModel(),new AnalogModel(),new AnalogModel(),new AnalogModel(),
            new AnalogModel(),new AnalogModel(),new AnalogModel()
        };

        /// <summary>
        /// 模拟量输入参数列表（炉内温度1，炉内温度2，试样中心温度，试样表面温度，标定中心温度，标定炉壁温度1，标定炉壁温度2，标定炉壁温度3
        /// 电压、电流、有功功率、无功功率、视在功率、功率因数、频率）
        /// </summary>
        public ObservableCollection<AnalogModel> AIList
        {
            get { return _aiList; }
            set
            {
                _aiList = value;
                RaisePropertyChanged(() => AIList);
            }
        }

        /// <summary>
        /// 模拟量输出参数列表（控温电压百分比）
        /// </summary>
        private ObservableCollection<AnalogModel> _aoList = new ObservableCollection<AnalogModel>()
        {
            new AnalogModel()
        };

        /// <summary>
        /// 模拟量输出参数列表（控温电压百分比）
        /// </summary>
        public ObservableCollection<AnalogModel> AOList
        {
            get { return _aoList; }
            set
            {
                _aoList = value;
                RaisePropertyChanged(() => AOList);
            }
        }


        #endregion


        #region 一键调零

        /// <summary>
        ///  AI一键调零。y=k*x+b。b=y0-k*x0
        /// </summary>
        private void AutoZeroMessage(int msg)
        {
            switch (msg)
            {
                // 新标准下，炉内温度1、炉内温度2平均
                case 1:
                case 2:
                    if (IsStd2023)
                    {
                        AIList[0].ZeroCalValue = ((AIList[0].ValueCaledNonZero + AIList[1].ValueCaledNonZero) / 2 - AIList[0].ValueCaledNonZero) * AIList[0].KCalValue;
                        AIList[1].ZeroCalValue = ((AIList[0].ValueCaledNonZero + AIList[1].ValueCaledNonZero) / 2 - AIList[1].ValueCaledNonZero) * AIList[1].KCalValue;
                    }
                    break;
                //试样中心、试样表面平均
                case 3:
                case 4:
                    AIList[2].ZeroCalValue = ((AIList[2].ValueCaledNonZero + AIList[3].ValueCaledNonZero) / 2 - AIList[2].ValueCaledNonZero) * AIList[2].KCalValue;
                    AIList[3].ZeroCalValue = ((AIList[2].ValueCaledNonZero + AIList[3].ValueCaledNonZero) / 2 - AIList[3].ValueCaledNonZero) * AIList[3].KCalValue;
                    break;
                //3个标定炉壁平均
                //case 5:
                case 6:
                case 7:
                case 8:
                   // AIList[4].ZeroCalValue = ((AIList[4].ValueCaledNonZero + AIList[5].ValueCaledNonZero + AIList[6].ValueCaledNonZero + AIList[7].ValueCaledNonZero) / 2 - AIList[4].ValueCaledNonZero) * AIList[4].KCalValue;
                    AIList[5].ZeroCalValue = (( AIList[5].ValueCaledNonZero + AIList[6].ValueCaledNonZero + AIList[7].ValueCaledNonZero) / 3 - AIList[5].ValueCaledNonZero) * AIList[5].KCalValue;
                    AIList[6].ZeroCalValue = (( AIList[5].ValueCaledNonZero + AIList[6].ValueCaledNonZero + AIList[7].ValueCaledNonZero) / 3 - AIList[6].ValueCaledNonZero) * AIList[6].KCalValue;
                    AIList[7].ZeroCalValue = (( AIList[5].ValueCaledNonZero + AIList[6].ValueCaledNonZero + AIList[7].ValueCaledNonZero) / 3 - AIList[7].ValueCaledNonZero) * AIList[7].KCalValue;
                    break;
                //电压U
                case 9:
                    AIList[8].ZeroCalValue = (AIList[8].ValueCaledNonZero - AIList[8].ValueCaledNonZero) * AIList[8].KCalValue;
                    break;
                //电流I
                case 10:
                    AIList[9].ZeroCalValue = (AIList[9].ValueCaledNonZero - AIList[9].ValueCaledNonZero) * AIList[9].KCalValue;
                    break;
                //有功功率P
                case 11:
                    AIList[10].ZeroCalValue = (AIList[10].ValueCaledNonZero - AIList[10].ValueCaledNonZero) * AIList[10].KCalValue;
                    break;
                //无功功率Q
                case 12:
                    AIList[11].ZeroCalValue = (AIList[11].ValueCaledNonZero - AIList[11].ValueCaledNonZero) * AIList[11].KCalValue;
                    break;
                //视在功率S
                case 13:
                    AIList[12].ZeroCalValue = (AIList[12].ValueCaledNonZero - AIList[12].ValueCaledNonZero) * AIList[12].KCalValue;
                    break;
                //功率因数PF
                case 14:
                    AIList[8].ZeroCalValue = (AIList[13].ValueCaledNonZero - AIList[13].ValueCaledNonZero) * AIList[13].KCalValue;
                    break;
                //频率F
                case 15:
                    AIList[14].ZeroCalValue = (AIList[14].ValueCaledNonZero - AIList[14].ValueCaledNonZero) * AIList[14].KCalValue;
                    break;

                case 99:

                    break;
            }
        }

        #endregion

        
        #region 下拉列表项

        /// <summary>
        /// 接口类型列表
        /// </summary>
        public ObservableCollection<string> _infTypeList = new ObservableCollection<string>() { "Voltage", "Current", "Digital", "Couple" };
        /// <summary>
        /// 接口类型列表
        /// </summary>
        public ObservableCollection<string> InfTypeList
        {
            get { return _infTypeList; }
        }
        /// <summary>
        /// 电信号单位列表
        /// </summary>
        public ObservableCollection<string> _elecSigUnitList = new ObservableCollection<string>() { "mA", "A", "V", "mV" };
        /// <summary>
        /// 标定点启用标志
        /// </summary>
        public ObservableCollection<string> ElecSigUnitList
        {
            get { return _elecSigUnitList; }
        }

        /// <summary>
        /// 滤波器类型列表
        /// </summary>
        public ObservableCollection<string> _filterTypeList = new ObservableCollection<string>() { "中值", "平均", "抗干扰中值", "惯性" };
        /// <summary>
        /// 滤波器类型列表
        /// </summary>
        public ObservableCollection<string> FilterTypeList
        {
            get { return _filterTypeList; }
        }
        #endregion
    }
}

/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.Model.Dev
 * 文件名：  DevModel_PIDs
 * 版本号：  V1.0.0.0
 * 唯一标识：cb34e812-5cb2-4cd8-9878-af81653cff04
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/4 8:00:07
 * 描述：
 * 装置Model，PID参数部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/3 22:45:36		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using CtrlMethod;
using GalaSoft.MvvmLight;

namespace BRX.Model.Dev
{
    public partial class DevModel : ObservableObject
    {
        /// <summary>
        /// 温度PID控制器(15≤Err<50)
        /// </summary>
        private PID_CtrlModel _pid_T_Err15_50 = new PID_CtrlModel() { PID_Param = new PID_ParamModel() { ControllerName = "温度控制PID", ControllerNo = "PID_T_Err15_50" } };
        /// <summary>
        /// 温度PID控制器(15≤Err<50)
        /// </summary>
        public PID_CtrlModel PID_T_Err15_50
        {
            get { return _pid_T_Err15_50; }
            set
            {
                _pid_T_Err15_50 = value;
                RaisePropertyChanged(() => PID_T_Err15_50);
            }
        }


        /// <summary>
        /// 温度PID控制器（误差大于50）
        /// </summary>
        private PID_CtrlModel _pid_T_Err50Up = new PID_CtrlModel() { PID_Param = new PID_ParamModel() { ControllerName = "温度控制PID", ControllerNo = "PID_T_Err50Up" } };
        /// <summary>
        /// 温度PID控制器（误差大于50）
        /// </summary>
        public PID_CtrlModel PID_T_Err50Up
        {
            get { return _pid_T_Err50Up; }
            set
            {
                _pid_T_Err50Up = value;
                RaisePropertyChanged(() => PID_T_Err50Up);
            }
        }


        /// <summary>
        /// 温度PID控制器(0≤Err<15)
        /// </summary>
        private PID_CtrlModel _pid_T_Err0_15 = new PID_CtrlModel() { PID_Param = new PID_ParamModel() { ControllerName = "温度控制PID", ControllerNo = "PID_T_Err50Up" } };
        /// <summary>
        /// 温度PID控制器(0≤Err<15)
        /// </summary>
        public PID_CtrlModel PID_T_Err0_15
        {
            get { return _pid_T_Err0_15; }
            set
            {
                _pid_T_Err0_15 = value;
                RaisePropertyChanged(() => PID_T_Err0_15);
            }
        }


        /// <summary>
        /// 功率PID控制器
        /// </summary>
        private PID_CtrlModel _pid_Power = new PID_CtrlModel() { PID_Param = new PID_ParamModel() { ControllerName = "功率控制PID", ControllerNo = "PID_Power" } };
        /// <summary>
        /// 功率PID控制器
        /// </summary>
        public PID_CtrlModel PID_Power
        {
            get { return _pid_Power; }
            set
            {
                _pid_Power = value;
                RaisePropertyChanged(() => PID_Power);
            }
        }
    }
}

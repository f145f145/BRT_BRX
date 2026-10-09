/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
  * 。BLL，功率PID参数调试部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/18 16:22:39		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using BRX.Model.Enums;
using static BRX.Model.Enums.Enums;
using System;
using System.Diagnostics;

namespace BRX.BLL
{
    public partial class Bll : ObservableObject
    {
        /// <summary>
        /// 功率PID调试事务
        /// </summary>
        private  void PowerPIDDbgBLLAsync()
        {
            if (!(Dev.RunMode == RunMode.PowerPID_Mode))
                return;
            
            //给定值，kp,ki,kd,输出上限，积分限幅，积分分离误差，控制误差
            double sv, pv, err;
            sv = Math.Abs(PowerPIDCMD[0]) - Math.Abs(PowerPIDCMD[7]) / 2;

            //当前值
            pv = Dev.AIList[10].ValueFinal;
            err = sv - pv;

            PID_Power_BLL.PID_Param.ControllerType = Dev.PID_Power.PID_Param.ControllerType;
            PID_Power_BLL.PID_Param.Is_U_Limit = Dev.PID_Power.PID_Param.Is_U_Limit;
            PID_Power_BLL.PID_Param.Is_UILimit_Used = Dev.PID_Power.PID_Param.Is_UILimit_Used;
            PID_Power_BLL.PID_Param.Is_ISeparate_Used = Dev.PID_Power.PID_Param.Is_ISeparate_Used;
            PID_Power_BLL.PID_Param.T = Dev.PID_Power.PID_Param.T;
            PID_Power_BLL.PID_Param.Kp = PowerPIDCMD[1];
            PID_Power_BLL.PID_Param.Ki = PowerPIDCMD[2];
            PID_Power_BLL.PID_Param.Kd = PowerPIDCMD[3];
            PID_Power_BLL.PID_Param.U_LowerBound = 0;
            PID_Power_BLL.PID_Param.U_UpperBound = PowerPIDCMD[4];
            PID_Power_BLL.PID_Param.U_IMax_Limit = PowerPIDCMD[5];
            PID_Power_BLL.PID_Param.ErrBound_IntegralSeparate = PowerPIDCMD[6];
            PID_Power_BLL.PID_Param.ControllerEnable = true;

            PID_Power_BLL.CalculatePID(err);
          //   Trace.Write("\r\n Err:"+ PID_Power_BLL.ErrK+"  Usum:"+PID_Power_BLL.UK+"\r\n UkP:"+ PID_Power_BLL.UK_P+"  UkI:"+ PID_Power_BLL.UK_I+"  UkD:"+ PID_Power_BLL.UK_D);
            //AO输出
            //软启动
            double aoOutMax=Dev.RatioAoOutMax;//= SoftBoot();
            double aoOutShould = PID_Power_BLL.UK;
            if (aoOutShould >= aoOutMax)
                aoOutShould = aoOutMax;
            Dev.AOList[0].ValueFinal = aoOutShould;
            Dev.AOList[0].CalcAODatas();
        }

        /// <summary>
        /// 功率PID调试消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void PowerPIDDbgMessage(double[] msg)
        {
            if (Dev.RunMode == Enums.RunMode.PowerPID_Mode)
            {
                PowerPIDCMD = (double[])msg.Clone();
                StopCMD = false;
                PID_Power_BLL.PID_Param.ControllerEnable = true;
            }
        }

        /// <summary>
        /// 功率PID调试指令数据（给定值，kp,ki,kd,输出限幅，积分限幅，积分分离误差，控制误差）
        /// </summary>
        private double[] _powerPIDCMD = { 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 功率PID调试指令数据（给定值，kp,ki,kd,输出限幅，积分限幅，积分分离误差，控制误差）
        /// </summary>
        public double[] PowerPIDCMD
        {
            get { return _powerPIDCMD; }
            set
            {
                _powerPIDCMD = value;
                RaisePropertyChanged(() => PowerPIDCMD);
            }
        }

    }
}
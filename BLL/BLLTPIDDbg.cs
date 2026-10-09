/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
  * 。BLL，温控PID参数调试部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/18 16:22:39		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System.Diagnostics;
using GalaSoft.MvvmLight;
using BRX.Model.Enums;
using static BRX.Model.Enums.Enums;

namespace BRX.BLL
{
    public partial class Bll : ObservableObject
    {
        /// <summary>
        /// 温控PID调试事务
        /// </summary>
        private  void PIDDbgBLLAsync()
        {
            if (!(Dev.RunMode == RunMode.TPID_Mode))
                return;
            
            //给定值，kp,ki,kd,输出上限，积分限幅，积分分离误差，控制误差
            double sv, pv, err;
            if (PIDCMD[0] >= 0)
                sv = PIDCMD[0] + PIDCMD[7] / 2;
            else 
              sv = PIDCMD[0] - PIDCMD[7] / 2;

            //当前值
            if (Dev.IsStd2023)
                pv = (Dev.AIList[0].ValueFinal + Dev.AIList[1].ValueFinal) / 2;
            else
                pv = Dev.AIList[0].ValueFinal;
            err = sv - pv;

            PID_T_BLL.PID_Param.ControllerType = Dev.PID_T_Err50Up.PID_Param.ControllerType;
            PID_T_BLL.PID_Param.Is_U_Limit = Dev.PID_T_Err50Up.PID_Param.Is_U_Limit;
            PID_T_BLL.PID_Param.Is_UILimit_Used = Dev.PID_T_Err50Up.PID_Param.Is_UILimit_Used;
            PID_T_BLL.PID_Param.Is_ISeparate_Used = Dev.PID_T_Err50Up.PID_Param.Is_ISeparate_Used;
            PID_T_BLL.PID_Param.T = Dev.PID_T_Err50Up.PID_Param.T;
            PID_T_BLL.PID_Param.Kp = PIDCMD[1];
            PID_T_BLL.PID_Param.Ki = PIDCMD[2];
            PID_T_BLL.PID_Param.Kd = PIDCMD[3];
            PID_T_BLL.PID_Param.U_LowerBound = 0;
            PID_T_BLL.PID_Param.U_UpperBound = PIDCMD[4];
            PID_T_BLL.PID_Param.U_IMax_Limit = PIDCMD[5];
            PID_T_BLL.PID_Param.ErrBound_IntegralSeparate = PIDCMD[6];
            PID_T_BLL.PID_Param.ControllerEnable = true;

            PID_T_BLL.CalculatePID(err);
            // Trace.Write("\r\n Err:"+ PID_T_BLL.ErrK+"  Usum:"+PID_T_BLL.UK+"\r\n UkP:"+ PID_T_BLL.UK_P+"  UkI:"+ PID_T_BLL.UK_I+"  UkD:"+ PID_T_BLL.UK_D);
            //AO输出
            //软启动
            double aoOutMax = Dev.RatioAoOutMax;
            double aoOutShould = PID_T_BLL.UK;
            if (aoOutShould >= aoOutMax)
                aoOutShould = aoOutMax;
            Dev.AOList[0].ValueFinal = aoOutShould;
            Dev.AOList[0].CalcAODatas();

            //自动均值调零
            AutoAverage();
        }

        /// <summary>
        /// 温控PID调试消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void PIDDbgMessage(double[] msg)
        {
            if (Dev.RunMode == Enums.RunMode.TPID_Mode)
            {
                PIDCMD = (double[])msg.Clone();
                StopCMD = false;
                PID_T_BLL.PID_Param.ControllerEnable = true;
            }
        }

        /// <summary>
        /// 温控PID调试指令数据（给定值，kp,ki,kd,输出限幅，积分限幅，积分分离误差，控制误差）
        /// </summary>
        private double[] _pidCMD = { 0, 0, 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 温控PID调试指令数据（给定值，kp,ki,kd,输出限幅，积分限幅，积分分离误差，控制误差）
        /// </summary>
        public double[] PIDCMD
        {
            get { return _pidCMD; }
            set
            {
                _pidCMD = value;
                RaisePropertyChanged(() => PIDCMD);
            }
        }

    }
}
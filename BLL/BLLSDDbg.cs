/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.BLL
 * 文件名：  BLLExp
 * 版本号：  V1.0.0.0
 * 唯一标识：2951152b-32bb-4f70-861a-78ea2f2d2012
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/23 18:51:29
 * 描述：
  * 。BLL，手动调试部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/18 16:22:39		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using static BRX.Model.Enums.Enums;

namespace BRX.BLL
{
    public partial class Bll : ObservableObject
    {
        /// <summary>
        /// 手动调试事务
        /// </summary>
        private  void SDDbgBLLAsync()
        {
            if (Dev.RunMode != RunMode.SDDbg_Mode)
                return;

            if (!ExistSDCMD)
                return;

            Dev.AOList[0].ValueFinal = SDCMD;
            Dev.RaisePropertyChanged(() => Dev.VO_110VBase);
            Dev.RaisePropertyChanged(() => Dev.Power_110VBase);

            ExistSDCMD = false;
        }
        
        /// <summary>
        /// 手动调试消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void SDDbgMessage(double msg)
        {
            if (Dev.RunMode == RunMode.SDDbg_Mode)
            {
                SDCMD = msg;
                ExistSDCMD = true;
                StopCMD = false;
            }
        }
        
        /// <summary>
        /// 手动指令数据（指令值）
        /// </summary>
        private double _sdCMD = 0;
        /// <summary>
        /// 手动指令数据（指令类型，指令值）
        /// </summary>
        public double SDCMD
        {
            get { return _sdCMD; }
            set
            {
                _sdCMD = value;
                RaisePropertyChanged(() => SDCMD);
            }
        }

        /// <summary>
        /// 存在手动指令
        /// </summary>
        private bool _existSDCMD = false;
        /// <summary>
        /// 存在手动指令
        /// </summary>
        private bool ExistSDCMD
        {
            get { return _existSDCMD; }
            set
            {
                _existSDCMD = value;
                RaisePropertyChanged(() => ExistSDCMD);
            }
        }
    }
}
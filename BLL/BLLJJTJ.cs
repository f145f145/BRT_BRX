/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.BLL
 * 文件名：  BLLDbg
 * 版本号：  V1.0.0.0
 * 唯一标识：e6633d4f-46df-4580-941a-3af92d8b5cb8
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/18 16:22:39
 * 描述：
 * 。BLL，紧急停机部分
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
    public partial  class Bll:ObservableObject 
    {
        /// <summary>
        /// 紧急停机事务
        /// </summary>
        private void JJTJBLL()
        {

        }

        /// <summary>
        /// 紧急停机消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void JJTJMessage(int msg)
        {
            int cmd = msg;

            switch (cmd)
            {
                //紧急停机指令
                case 911:
                    Dev.RunMode = RunMode.JJTJ_Mode;
                    BllRst();
                    Dev.IsBusy = false;
                    break;

                //紧急停机复位指令
                case 918:
                    if (Dev.RunMode == RunMode.JJTJ_Mode)
                    {
                        Dev.RunMode = RunMode.Wait_Mode;
                        BllRst();
                        Dev.IsBusy = false;
                    }
                    break;
            }
        }
    }
}

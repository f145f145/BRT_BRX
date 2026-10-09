/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.Model.Enums
 * 文件名：  Enums
 * 版本号：  V1.0.0.0
 * 唯一标识：a50b8dcc-4ba6-4065-8708-579a9031125e
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/20 16:01:28
 * 描述：
 * 枚举，通讯相关
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/20       16:01:28		郝正强			V1.0.0.0
 *
 ************************************************************************************/

namespace BRX.Model.Enums
{
    public static partial class Enums
    {

        /// <summary>
        /// 装置运行模式类型
        /// </summary>
        public enum RunMode
        {
            /// <summary>
            /// 待机模式
            /// </summary>
            Wait_Mode = 0,

            /// <summary>
            /// 测试试验
            /// </summary>
            Exp_BRXMode = 1,

            /// <summary>
            /// 手动调试模式
            /// </summary>
            SDDbg_Mode = 2,

            /// <summary>
            /// 温控PID调试模式
            /// </summary>
            TPID_Mode = 3,

            /// <summary>
            /// 功率PID调试模式
            /// </summary>
            PowerPID_Mode = 4,

            /// <summary>
            /// 炉壁温度校准模式
            /// </summary>
            WallCal_Mode = 5,

            /// <summary>
            /// 炉内温度校准模式
            /// </summary>
            CenterCal_Mode = 6,

            /// <summary>
            /// 系统辨识模式
            /// </summary>
            ID_Mode = 333,

            /// <summary>
            /// 紧急停机
            /// </summary>
            JJTJ_Mode = 99
        }


        /// <summary>
        /// 测试阶段
        /// </summary>
        public enum TestStage
        {
            /// <summary>
            /// 待机
            /// </summary>
            Wait_Stage = 0,

            /// <summary>
            /// 温度准备
            /// </summary>
            TCtl_Stage = 1,
            
            /// <summary>
            /// 试样检测
            /// </summary>
            Test_Stage = 2,

            /// <summary>
            /// 检测完成
            /// </summary>
            TestEnd_Stage = 3,
        }
    }
}
/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.Model
 * 文件名：  MQZH_Enums
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
        /// 通讯收发数据命令类型
        /// </summary>
        public enum CommCMDType
        {
            None = 0,                             //未定义

            ReadCoils = 1,                      //读取多个线圈DO
            WriteSingleCoil = 5,                //写单个线圈DO
            WriteMultipleCoils = 15,            //写多个线圈DO

            ReadInputs = 2,                      //读取多个DI
            ReadInputRegisters = 4,             //读取多个AI输入寄存器

            ReadHoldingRegisters = 3,            //读取多个AO保持寄存器
            WriteSingleRegister = 6,              //写单个AO寄存器
            WriteMultipleRegisters = 16,          //写多个AO保持寄存器
            ReadWriteMultipleRegisters = 23,      //读写多个保持寄存器AO
        }


        /// <summary>
        /// 仁科风速读寄存器地址
        /// </summary>
        public enum RKRegAddr_R
        {
            FS = 0                   //风速
        }

        /// <summary>
        /// 仁科风速数据长度
        /// </summary>
        public enum RKDataQty
        {
            FS = 1                   //风速
        }

        /// <summary>
        ///艾莫迅多功能模块DI点地址
        /// </summary>
        public enum AMXRegAddr_DI
        {
            DI1 = 0,
            DI2 = 1,
            DI3 = 2,
            DI4 = 3,
            DI5 = 4,
            DI6 = 5,
            DI7 = 6,
            DI8 = 7,
            All = 0
        }

        /// <summary>
        /// 艾莫迅多功能模块DI点数据长度
        /// </summary>
        public enum AMXDataQty_DI
        {
            Single = 1,
            All = 8
        }

        /// <summary>
        ///艾莫迅多功能模块DO点地址
        /// </summary>
        public enum AMXRegAddr_DO
        {
            DO1 = 0,
            DO2 = 1,
            DO3 = 2,
            DO4 = 3,
            DO5 = 4,
            DO6 = 5,
            DO7 = 6,
            DO8 = 7,
            All = 0
        }

        /// <summary>
        /// 艾莫迅多功能模块DO点数据长度
        /// </summary>
        public enum AMXDataQty_DO
        {
            Single = 1,
            All = 8
        }

        /// <summary>
        ///艾莫迅多功能模块输入寄存器地址
        /// </summary>
        public enum AMXRegAddr_AI
        {
            AI1 = 0,
            AI2=1,
            AI3=2,
            AI4=3,
            AI5=4,
            AI6=5,
            I=0,
            V=3,
            All=0
        }

        /// <summary>
        /// 艾莫迅多功能模块输入寄存器数据长度
        /// </summary>
        public enum AMXDataQty_AI
        {
            Single = 1,
            I_All=3,
            V_All=3,
            All=6
        }
        
        /// <summary>
        ///艾莫迅多功能模块保持寄存器地址
        /// </summary>
        public enum AMXRegAddr_AO
        {
            AO1 = 0,
            All = 0
        }

        /// <summary>
        /// 艾莫迅多功能模块保持寄存器数据长度
        /// </summary>
        public enum AMXDataQty_AO
        {
            AO1 = 1,
            All = 1
        }

        /// <summary>
        /// 安科瑞PZ电力表寄存器地址
        /// </summary>
        public enum AcrelRegAddr_PZ
        {
            Ua = 37,
            Ub = 38,
            Uc = 39,

            Uab = 40,
            Ubc = 41,
            Uac = 45,

           Ia = 43,
            Ib = 44,
            Ic = 45,

            Pa = 46,
            Pb = 47,
            Pc = 48,
            Pall = 49,

            Qa = 50,
            Qb = 51,
            Qc = 52,
            Qall = 53,

            PFa = 54,
            PFb = 55,
            PFc = 56,
            PFall = 57,

            PSa = 58,
            PSb = 59,
            PSc = 60,
            PSall = 61,

            F=62,

            W=71,
            All = 37
        }

        /// <summary>
        /// 安科瑞PZ电力表寄存器数据长度
        /// </summary>
        public enum AcrelDataQty_PZ
        {
            Ua = 1,
            Ub = 1,
            Uc = 1,

            Uab = 1,
            Ubc = 1,
            Uac = 1,

            Ia = 1,
            Ib = 1,
            Ic = 1,

            Pa = 1,
            Pb = 1,
            Pc = 1,
            Pall = 1,

            Qa = 1,
            Qb = 1,
            Qc = 1,
            Qall = 1,

            PFa =1,
            PFb = 1,
            PFc = 1,
            PFall = 1,

            PSa = 1,
            PSb = 1,
            PSc = 1,
            PSall = 1,

            F = 1,

            W = 2,
            All = 36
        }


        /// <summary>
        /// 大连华峰仪表发展热量表寄存器数据地址
        /// </summary>
        public enum HFRegAddr_RL
        {
            Tin = 33,       //供水温度
            Tout = 35,      //回水温度
            T = 33,

            LLs = 1,       //瞬时流量
            RLs = 3,       //瞬时热流量
            LL = 1,

            LLjlj = 113,
            RLjlj = 119,
            JLJ = 113,
        }

        /// <summary>
        /// 大连华峰仪表发展热量表寄存器数据长度
        /// </summary>
        public enum HFDataQty_RL
        {
            Tin = 2,       //供水温度
            Tout = 2,      //回水温度
            T = 4,

            LLs = 2,       //瞬时流量
            RLs = 2,       //瞬时热流量
            LL = 4,

            LLjlj = 2,
            RLjlj = 2,
            JLJ = 4,
        }

        
        /// <summary>
        /// 阿尔泰3138H热电偶模块寄存器地址
        /// </summary>
        public enum RegAddr_DAM3138H
        {
            T1 = 0,       //1通道
            T2 = 1,       //2通道
            T3 = 2,       //3通道
            T4 = 3,       //4通道
            T5 = 4,       //5通道
            T6 = 5,       //6通道
            T7 = 6,       //7通道
            T8 = 7,       //8通道

            All = 0,
        }
        /// <summary>
        /// 阿尔泰3138H热电偶模块寄存器数据长度
        /// </summary>
        public enum DataQty_DAM3138HL
        {
            T1 = 1,       //1通道
            T2 = 1,       //2通道
            T3 = 1,       //3通道
            T4 = 1,       //4通道
            T5 = 1,       //5通道
            T6 = 1,       //6通道
            T7 = 1,       //7通道
            T8 = 1,       //8通道

            All = 8,
        }


        /// <summary>
        /// 阿尔泰3134热电偶模块寄存器地址
        /// </summary>
        public enum RegAddr_DAM3134
        {
            T1 = 0,       //1通道
            T2 = 1,       //2通道
            T3 = 2,       //3通道
            T4 = 3,       //4通道

            All = 0,
        }
        /// <summary>
        /// 阿尔泰3134热电偶模块寄存器数据长度
        /// </summary>
        public enum DataQty_DAM3134
        {
            T1 = 1,       //1通道
            T2 = 1,       //2通道
            T3 = 1,       //3通道
            T4 = 1,       //4通道

            All = 4,
        }

        /// <summary>
        /// 阿尔泰3060C模拟量输出模块寄存器地址
        /// </summary>
        public enum RegAddr_DAM3060C
        {
            AO1 = 352,       //1通道
            AO2 = 354,       //2通道
            AO3 = 356,       //3通道
            AO4 = 358,       //4通道

            All = 352,
        }
        /// <summary>
        /// 阿尔泰3060C模拟量输出模块寄存器数据长度
        /// </summary>
        public enum DataQty_DAM3060C
        {
            AO1 = 2,       //1通道
            AO2 = 2,       //2通道
            AO3 = 2,       //3通道
            AO4 = 2,       //4通道

            All = 8,
        }

        /// <summary>
        /// UT5564A模拟量输出模块寄存器地址
        /// </summary>
        public enum RegAddr_UT5564A
        {
            AO1 = 0,       //1通道
            AO2 = 1,       //2通道
            AO3 = 2,       //3通道
            AO4 = 3,       //4通道

            All = 0,
        }
        /// <summary>
        /// UT5564A模拟量输出模块寄存器数据长度
        /// </summary>
        public enum DataQty_UT5564A
        {
            AO1 = 2,       //1通道
            AO2 = 2,       //2通道
            AO3 = 2,       //3通道
            AO4 = 2,       //4通道

            All = 8,
        }

        /// <summary>
        /// 艾莫迅4AI4AO模拟量输出模块寄存器地址
        /// </summary>
        public enum RegAddr_4AI4AO
        {
            AO1 = 0,       //1通道
            AO2 = 1,       //2通道
            AO3 = 2,       //3通道
            AO4 = 3,       //4通道

            All = 0,
        }
        /// <summary>
        /// 艾莫迅4AI4AO模拟量输出模块寄存器数据长度
        /// </summary>
        public enum DataQty_4AI4AO
        {
            AO1 = 1,       //1通道
            AO2 = 1,       //2通道
            AO3 = 1,       //3通道
            AO4 = 1,       //4通道

            All = 4,
        }

        /// <summary>
        /// 阿尔泰3060C看门狗寄存器地址
        /// </summary>
        public enum RegAddr_DAM3060C_Dog
        {
            DogEnable = 512,       //看门狗使能
            DogOverFlow = 513,       //看门狗溢出
            DogTimer = 514,       //看门狗定时器
            DogReset = 515,       //看门狗复位

            All = 512,
        }
        /// <summary>
        /// 阿尔泰3060C看门狗寄存器数据长度
        /// </summary>
        public enum DataQty_DAM3060C_Dog
        {
            DogEnable = 1,       //看门狗使能
            DogOverFlow = 1,       //看门狗溢出
            DogTimer = 1,       //看门狗定时器
            DogReset = 1,       //看门狗复位

            All = 4,
        }



        /// <summary>
        /// 中测单相功率采集模块寄存器地址
        /// </summary>
        public enum RegAddr_Power
        {
            U = 32,       //1通道
            I = 33,       //2通道
            P = 34,       //3通道
            Q = 35,       //4通道
            S = 36,       //5通道
            PF =37,       //6通道
            F = 38,       //7通道

            All = 32,
        }
        /// <summary>
        /// 中测单相功率采集模块寄存器数据长度
        /// </summary>
        public enum DataQty_Power
        {
            U = 1,       //1通道
            I = 1,       //2通道
            P = 1,       //3通道
            Q = 1,       //4通道
            S = 1,       //5通道
            PF = 1,       //6通道
            F = 1,       //7通道

            All = 7,
        }
    }
}

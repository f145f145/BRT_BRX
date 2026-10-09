/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.View
 * 文件名：  WinNames
 * 版本号：  V1.0.0.0
 * 唯一标识：a50b8dcc-4ba6-4065-8708-579a9031125e
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/11/20 16:01:28
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/11/20       16:01:28		郝正强			V1.0.0.0
 *
 ************************************************************************************/

namespace BRX.View
{
    /// <summary>
    /// 系统各种常量字符串
    /// </summary>
    public static class WinNames
    {
        //主窗口、加载窗口
        public static string LoadingWinName = "程序加载";
        public static string MainWinName = "主界面";
        public static string SN_InputWinName = "授权输入";

        //调试窗口
        public static string DbgWinName = "调试";

        //校准窗口
        public static string WallCalWinName = "炉壁校准";
        public static string CenterCalWinName = "炉内校准";

        //试验窗口
        public static string BRXWinName = "冷水热泵测试";
        public static string QDWinName = "风管强度测试";

        //装置设定窗口
        public static string AIOCalWinName = "模拟量调零标定";
        public static string AIOParamWinName = "模拟量参数设定";
        public static string AIOChannelWinName = "模拟量通道参数";
        public static string BasicInfoWinName = "装置基本信息";
        public static string BasicParamWinName = "装置基本参数";
        public static string CoInfoWinName = "公司信息";
        public static string ComSetWinName = "串口设定";
        public static string PidSetWinName = "PID参数设定";
    }
}
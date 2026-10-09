/************************************************************************************
 * Copyright (c) 2021  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：MQZHWL.View
 * 文件名：  BoolToVisibilityConverter
 * 版本号：  V1.0.0.0
 * 唯一标识：a260e1ec-7bf5-4f5c-bedd-f6166604e876
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2021/12/11 19:17:58
 * 描述：
 * DIDO状态转换为Visibility属性
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2021/12/11       19:17:58		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SqlSugar.Extensions;
using static BRX.Model.Enums.Enums;

namespace BRX.View
{
    /// <summary>
    /// Bool取反转换器
    /// </summary>
    public class BoolToNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool到visibility的转换器
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Visibility)value== Visibility.Visible);
        }
    }

    /// <summary>
    /// Bool到visibility的反向转换器
    /// </summary>
    public class BoolNotToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Visibility)value == Visibility.Hidden);
        }
    }

    /// <summary>
    /// INT到方向的转换器
    /// </summary>
    public class IntToDirConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value==1) ? "↑" : "↓";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value == "↑") ? 1 : -1;
        }
    }

    /// <summary>
    /// Bool到对号的转换器
    /// </summary>
    public class BoolToTickConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "√" : "×";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value == "√");
        }
    }

    /// <summary>
    /// Bool到有无的转换器
    /// </summary>
    public class BoolToYWConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "有" : "无";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value == "有");
        }
    }

    /// <summary>
    /// Bool到正负压的转换器
    /// </summary>
    public class BoolToNegativeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "负压" : "正压";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value == "负压");
        }
    }

    /// <summary>
    /// 时间字符串的转换器
    /// </summary>
    public class TimeToStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str;
            DateTime time = (DateTime) value;
            str = time.ToShortTimeString();
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// int到分秒字符串的转换器
    /// </summary>
    public class IntToMMSSStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str;
            int time = (int) value;
            if (time >= 3600)
            {
                int hh = time / 3600;
                int mm = (time -hh*3600)/60;
                int ss = time % 60;
                str = hh+"时 "+mm.ToString() + "分 " + ss.ToString() + "秒";
                return str;
            }
            else if (time >= 60)
            {
                int mm = time / 60;
                int ss = time % 60;
                str = mm.ToString()+"分 "+ss.ToString()+"秒";
                return str;
            }
            else
                str = time.ToString() + "秒";
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Double
    /// </summary>
    public class DoubleToMMSSStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str;
            int time = (int)(double)value;
            if (time >= 3600)
            {
                int hh = time / 3600;
                int mm = (time - hh * 3600) / 60;
                int ss = time % 60;
                str = hh + "时 " + mm.ToString() + "分 " + ss.ToString() + "秒";
                return str;
            }
            else if (time >= 60)
            {
                int mm = time / 60;
                int ss = time % 60;
               str = mm.ToString() + "分 " + ss.ToString() + "秒";
                return str;
            }
            else
                str = time.ToString() + "秒";
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 风速仪类型的文字转换器
    /// </summary>
    public class FSYTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 2)
                return "微压计式";
            return "数字热线式";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "微压计式")
                return 2;
            return 1;
        }
    }


    /// <summary>
    /// 机组类别的文字转换器
    /// </summary>
    public class UnitTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int) value == 1)
                return "蒸气压缩";
            if ((int) value == 2)
                return "燃气溴化锂";
            if ((int) value == 3)
                return "燃油溴化锂";
            return "未知";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "蒸气压缩")
                return 1;
            if ((string)value == "燃气溴化锂")
                return 2;
            if ((string)value == "燃油溴化锂")
                return 3;
            return 0;
        }
    }

    /// <summary>
    /// 圆管方管类型的文字转换器
    /// </summary>
    public class IsRoundnessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return "圆形风管";
            return "矩形风管";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "圆形风管")
                return true;
            return false;
        }
    }

    /// <summary>
    /// 热线风速仪的显示转换器
    /// </summary>
    public class IsRXConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 1)
                return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 微压差风速仪的显示转换器
    /// </summary>
    public class IsWYCConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 2)
                return Visibility.Visible;
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// int到collor的普通状态转换器
    /// </summary>
    public class IntToCollorOConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return "DarkOrange" ;
                case 1:
                    return "Chartreuse";
                case 2:
                    return "DarkGray";
                default: return "DarkGray";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool到collor的普通状态转换器（灰绿）
    /// </summary>
    public class BoolToCollorGConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Chartreuse" : "DarkGray";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool到collor的普通状态转换器（灰橙）
    /// </summary>
    public class BoolToCollorOConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "DarkOrange" : "DarkGray";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    /// <summary>
    /// Bool到collor的普通状态转换器（绿橙）
    /// </summary>
    public class BoolToCollorNotOConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Chartreuse" : "DarkOrange";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Bool到collor的普通状态转换器（灰红）
    /// </summary>
    public class BoolToCollorRConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Red" : "DarkGray";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    /// <summary>
    /// 串口校验位转换器
    /// </summary>
    public class StrToParityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((System.IO.Ports.Parity)value == System.IO.Ports.Parity.Odd)
                return "奇";
            if ((System.IO.Ports.Parity)value == System.IO.Ports.Parity.Even)
                return "偶";
            if ((System.IO.Ports.Parity)value == System.IO.Ports.Parity.None)
                return "无";
            if ((System.IO.Ports.Parity)value == System.IO.Ports.Parity.Mark)
                return "标志";
            if ((System.IO.Ports.Parity)value == System.IO.Ports.Parity.Space)
                return "空格";
            return "无";

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "奇")
                return System.IO.Ports.Parity.Odd;
            if ((string)value == "偶")
                return System.IO.Ports.Parity.Even;
            if ((string)value == "无")
                return System.IO.Ports.Parity.None;
            if ((string)value == "标志")
                return System.IO.Ports.Parity.Mark;
            if ((string)value == "空格")
                return System.IO.Ports.Parity.Space;
            return System.IO.Ports.Parity.None;
        }
    }

    /// <summary>
    /// 串口停止位转换器
    /// </summary>
    public class StrToStopBitsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((System.IO.Ports.StopBits)value == System.IO.Ports.StopBits.None)
                return "0";
            if ((System.IO.Ports.StopBits)value == System.IO.Ports.StopBits.One )
                return "1";
            if ((System.IO.Ports.StopBits)value == System.IO.Ports.StopBits.OnePointFive )
                return "1.5";
            if ((System.IO.Ports.StopBits)value == System.IO.Ports.StopBits.Two )
                return "2";
            return "无";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "0")
                return System.IO.Ports.StopBits.None;
            if ((string)value == "1")
                return System.IO.Ports.StopBits.One;
            if ((string)value == "1.5")
                return System.IO.Ports.StopBits.OnePointFive;
            if ((string)value == "2")
                return System.IO.Ports.StopBits.Two;
            return System.IO.Ports.StopBits.None;
        }
    }
    
    /// <summary>
    /// 装置运行模式文字
    /// </summary>
    public class RunModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((RunMode)value == RunMode.Wait_Mode)
                return "待机模式";
            if ((RunMode)value == RunMode.SDDbg_Mode)
                return "手动调试模式";
            if ((RunMode)value == RunMode.TPID_Mode)
                return "PID调试模式";
            if ((RunMode)value == RunMode.PowerPID_Mode)
                return "功率PID调试";
            if ((RunMode)value == RunMode.Exp_BRXMode)
                return "检测试验模式";
            if ((RunMode)value == RunMode.WallCal_Mode)
                return "炉壁校准模式";
            if ((RunMode)value == RunMode.CenterCal_Mode)
                return "炉内校准模式";
            if ((RunMode)value == RunMode.JJTJ_Mode)
                return "紧急停机模式";
            if ((RunMode)value == RunMode.ID_Mode)
                return "系统辨识模式";
            return "待机模式";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 测试阶段文字
    /// </summary>
    public class TestStageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((TestStage)value == TestStage.TCtl_Stage)
                return "温度准备";
            if ((TestStage)value == TestStage.Test_Stage)
                return "30min测试";
            if ((TestStage)value == TestStage.TestEnd_Stage)
                return "测试完成";

            return "待机阶段";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "温度准备")
                return TestStage.TCtl_Stage;
            if ((string)value == "30min测试")
                return TestStage.Test_Stage;
            if ((string)value == "测试完成")
                return TestStage.TestEnd_Stage;

            return TestStage.Wait_Stage;
        }
    }


    /// <summary>
    /// 炉壁校准高度
    /// </summary>
    public class WallCalHeithtConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1:
                    return "a:30 mm";
                case 2:
                    return "b:0 mm";
                case 3:
                    return "c:-30 mm";
                default:
                    return "0 mm";

            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using LiveCharts;

namespace BRX.View
{
    public class ChartModel_LiveChart:ObservableObject
    {
        #region 数据属性

        /// <summary>
        /// 图表所用数据
        /// </summary>
        private SeriesCollection _seriesCollection = new SeriesCollection();
        /// <summary>
        /// 图表所用数据
        /// </summary>
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                RaisePropertyChanged(() => SeriesCollection);
            }
        }

        /// <summary>
        /// 横轴数据
        /// </summary>
        private string[] _labels = null;
        /// <summary>
        /// 设置折线颜色
        /// </summary>
        public string[] Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                RaisePropertyChanged(() => Labels);
            }
        }

        /// <summary>
        /// 图表类型
        /// </summary>
        private ChartType _chartType = ChartType.Line;
        /// <summary>
        /// 图表类型
        /// </summary>
        public ChartType ChartType
        {
            get { return _chartType; }
            set
            {
                _chartType = value;
                RaisePropertyChanged(() => ChartType);
            }
        }

        #endregion

        #region 未知

        public Func<double, string> _formatter;
        /// <summary>
        /// 设置折线颜色
        /// </summary>
        public Func<double, string> Formatter
        {
            get { return _formatter; }
            set
            {
                _formatter = value;
                RaisePropertyChanged(() => Formatter);
            }
        }


        public string _contentToolTip = "";
        /// <summary>
        /// 设置折线粗细
        /// </summary>
        public string ContentToolTip
        {
            get { return _contentToolTip; }
            set
            {
                _contentToolTip = value;
                RaisePropertyChanged(() => ContentToolTip);
            }
        }

        public ImageSource _icon;
        /// <summary>
        /// 设置折线颜色
        /// </summary>
        public ImageSource Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                RaisePropertyChanged(() => Icon);
            }
        }

        #endregion

        #region 绘布

        /// <summary>
        /// 绘布标题
        /// </summary>
        private string _title = "校准曲线";
        /// <summary>
        /// 绘布标题
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        /// <summary>
        /// 绘布的背景颜色
        /// </summary>
        public SolidColorBrush _chartBackground = System.Windows.Media.Brushes.White;
        /// <summary>
        /// 绘布的背景颜色
        /// </summary>
        public SolidColorBrush ChartBackground
        {
            get { return _chartBackground; }
            set
            {
                _chartBackground = value;
                RaisePropertyChanged(() => ChartBackground);
            }
        }

        /// <summary>
        /// 图例位置
        /// </summary>
        public LegendLocation _legendLocation = LegendLocation.Right;
        /// <summary>
        /// 图例位置
        /// </summary>
        public LegendLocation LegendLocation
        {
            get { return _legendLocation; }
            set
            {
                _legendLocation = value;
                RaisePropertyChanged(() => LegendLocation);
            }
        }


        /// <summary>
        /// 网格线粗细
        /// </summary>
        public int _separatorThickness = 1;
        /// <summary>
        /// 网格线粗细
        /// </summary>
        public int SeparatorThickness
        {
            get { return _separatorThickness; }
            set
            {
                _separatorThickness = value;
                RaisePropertyChanged(() => SeparatorThickness);
            }
        }

        /// <summary>
        /// 网格线颜色
        /// </summary>
        public SolidColorBrush _separatorStroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240, 240, 240));
        /// <summary>
        /// 网格线颜色
        /// </summary>
        public SolidColorBrush SeparatorStroke
        {
            get { return _separatorStroke; }
            set
            {
                _separatorStroke = value;
                RaisePropertyChanged(() => SeparatorStroke);
            }
        }

        #endregion

        #region 坐标轴线属性

        /// <summary>
        /// X轴名称
        /// </summary>
        private string _axisXName = "高度 mm";

        /// <summary>
        /// X轴名称
        /// </summary>
        public string AxisXName
        {
            get { return _axisXName; }
            set
            {
                _axisXName = value;
                RaisePropertyChanged(() => AxisXName);
            }
        }

        /// <summary>
        /// Y轴名称
        /// </summary>
        private string _axisYName = "温度";
        /// <summary>
        /// Y轴名称
        /// </summary>
        public string AxisYName
        {
            get { return _axisYName; }
            set
            {
                _axisYName = value;
                RaisePropertyChanged(() => AxisYName);
            }
        }
        
        /// <summary>
        /// 设置轴线标题位置
        /// </summary>
        public AxisPosition _axisPosition = AxisPosition.LeftBottom;
        /// <summary>
        /// 设置轴线标题位置
        /// </summary>
        public AxisPosition AxisPosition
        {
            get { return _axisPosition; }
            set
            {
                _axisPosition = value;
                RaisePropertyChanged(() => AxisPosition);
            }
        }

        /// <summary>
        /// 设置坐标轴标签旋转角度
        /// </summary>
        public int _axisLabelsRotation = 45;
        /// <summary>
        /// 设置坐标轴标签旋转角度
        /// </summary>
        public int AxisLabelsRotation
        {
            get { return _axisLabelsRotation; }
            set
            {
                _axisLabelsRotation = value;
                RaisePropertyChanged(() => AxisLabelsRotation);
            }
        }

        #endregion




        #region 设置提示

        /// <summary>
        /// 提示背景颜色
        /// </summary>
        public SolidColorBrush _tooltipBackground = System.Windows.Media.Brushes.LightCyan;
        /// <summary>
        /// 提示背景颜色
        /// </summary>
        public SolidColorBrush TooltipBackground
        {
            get { return _tooltipBackground; }
            set
            {
                _tooltipBackground = value;
                RaisePropertyChanged(() => TooltipBackground);
            }
        }

        /// <summary>
        /// 提示选择模式
        /// </summary>
        public TooltipSelectionMode _tooltipSelectionMode = TooltipSelectionMode.OnlySender;
        /// <summary>
        /// 提示选择模式
        /// </summary>
        public TooltipSelectionMode TooltipSelectionMode
        {
            get { return _tooltipSelectionMode; }
            set
            {
                _tooltipSelectionMode = value;
                RaisePropertyChanged(() => TooltipSelectionMode);
            }
        }

        /// <summary>
        /// 提示圆角半径
        /// </summary>
        public CornerRadius _tooltipCornerRadius = new CornerRadius(5);
        /// <summary>
        /// 提示圆角半径
        /// </summary>
        public CornerRadius TooltipCornerRadius
        {
            get { return _tooltipCornerRadius; }
            set
            {
                _tooltipCornerRadius = value;
                RaisePropertyChanged(() => TooltipCornerRadius);
            }
        }

        /// <summary>
        /// 提示边框颜色
        /// </summary>
        public SolidColorBrush _tooltipBorderBrush = System.Windows.Media.Brushes.Yellow;
        /// <summary>
        /// 提示边框颜色
        /// </summary>
        public SolidColorBrush TooltipBorderBrush
        {
            get { return _tooltipBorderBrush; }
            set
            {
                _tooltipBorderBrush = value;
                RaisePropertyChanged(() => TooltipBorderBrush);
            }
        }

        /// <summary>
        /// 提示边框粗细
        /// </summary>
        public Thickness _tooltipBorderThickness = new Thickness(2);
        /// <summary>
        /// 提示边框粗细
        /// </summary>
        public Thickness TooltipBorderThickness
        {
            get { return _tooltipBorderThickness; }
            set
            {
                _tooltipBorderThickness = value;
                RaisePropertyChanged(() => TooltipBorderThickness);
            }
        }

        #endregion

    }

    /// <summary>
    /// 图表类型
    /// </summary>
    public enum ChartType
    {
        [Description("折线图 ")]
        Line,
        [Description("柱状图")]
        Bar,
        [Description("饼状图")]
        Pie
    }
}
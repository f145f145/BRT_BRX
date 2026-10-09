using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using LiveCharts.Defaults;

namespace BRX.View
{
    public class LineModel_LiveChart:ObservableObject
    {
        #region 折线属性

        /// <summary>
        /// 设置折线颜色
        /// </summary>
        public SolidColorBrush _lineStroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 149, 242));
        /// <summary>
        /// 设置折线颜色
        /// </summary>
        public SolidColorBrush LineStroke
        {
            get { return _lineStroke; }
            set
            {
                _lineStroke = value;
                RaisePropertyChanged(() => LineStroke);
            }
        }

        /// <summary>
        /// 设置折线粗细
        /// </summary>
        public int _lineThickness = 2;
        /// <summary>
        /// 设置折线粗细
        /// </summary>
        public int LineThickness
        {
            get { return _lineThickness; }
            set
            {
                _lineThickness = value;
                RaisePropertyChanged(() => LineThickness);
            }
        }

        /// <summary>
        /// 设置折线光滑
        /// </summary>
        public int _lineSmoothness = 1;
        /// <summary>
        /// 设置折线光滑
        /// </summary>
        public int LineSmoothness
        {
            get { return _lineSmoothness; }
            set
            {
                _lineSmoothness = value;
                RaisePropertyChanged(() => LineSmoothness);
            }
        }

        /// <summary>
        /// 设置折线填充颜色
        /// </summary>
        public SolidColorBrush _lineFill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(222, 239, 253));
        /// <summary>
        /// 设置折线填充颜色
        /// </summary>
        public SolidColorBrush LineFill
        {
            get { return _lineFill; }
            set
            {
                _lineFill = value;
                RaisePropertyChanged(() => LineFill);
            }
        }

        #endregion

    }
}
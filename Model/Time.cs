using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace BRX.Model
{
    /// <summary>
    /// 开始结束标志及时间
    /// </summary>
    public class Time : ObservableObject
    {
        /// <summary>
        /// 已开始标志
        /// </summary>
        private bool _isStarted = false;
        /// <summary>
        ///  已开始标志
        /// </summary>
        public bool IsStarted
        {
            get { return _isStarted; }
            set
            {
                _isStarted = value;
                RaisePropertyChanged(() => IsStarted);
            }
        }

        /// <summary>
        ///  开始时刻
        /// </summary>
        private DateTime _startTime = DateTime.MinValue;
        /// <summary>
        ///  开始时刻
        /// </summary>
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                RaisePropertyChanged(() => StartTime);
            }
        }


        /// <summary>
        ///  已进行时间（从开始试验,s）
        /// </summary>
        private double _timeFromStart =0;
        /// <summary>
        ///  已进行时间（从开始试验,s）
        /// </summary>
        public double TimeFromStart
        {
            get
            {
                return _timeFromStart;
            }
            set
            {
                _timeFromStart = value;
                RaisePropertyChanged(() => TimeFromStart);
            }
        }

        /// <summary>
        /// 已结束标志
        /// </summary>
        private bool _isEnded = false;
        /// <summary>
        ///  已结束标志
        /// </summary>
        public bool IsEnded
        {
            get { return _isEnded; }
            set
            {
                _isEnded = value;
                RaisePropertyChanged(() => IsEnded);
            }
        }

        /// <summary>
        ///  结束时刻
        /// </summary>
        private DateTime _endTime = DateTime.MinValue;
        /// <summary>
        ///  结束时刻
        /// </summary>
        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                RaisePropertyChanged(() => EndTime);
            }
        }

        /// <summary>
        ///  已结束时间（从结束时刻,s）
        /// </summary>
        private double _timeFromEnd = 0;
        /// <summary>
        ///  已结束时间（从结束时刻,s）
        /// </summary>
        public double TimeFromEnd
        {
            get
            {
                return _timeFromEnd;
            }
            set
            {
                _timeFromEnd = value;
                RaisePropertyChanged(() => TimeFromEnd);
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            StartTime=DateTime.MinValue;
            EndTime = DateTime.MinValue;
            TimeFromStart = 0;
            TimeFromEnd = 0;
            IsStarted = false;
            IsEnded = false;
        }
    }
}
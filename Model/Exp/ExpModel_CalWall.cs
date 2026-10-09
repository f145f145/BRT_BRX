/************************************************************************************
 * Copyright @ DESKTOP-SB8IL0P 2022. All rights reserved.
 * CLR 版本：4.0.30319.42000
 * 项目名称：BRX.Model.Exp
 * 命名空间：BRX.Model.Exp
 * 类 名 称：ExpModel_WallCal 
 * 创 建 人：郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/8/23 9:46:46 
 * 描    述：
 * 炉壁温度校准试验Model。
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/8/23 9:46:46  		郝正强			V1.0.0.0
 *
 ************************************************************************************/
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using BRX.DAL.CalDAL.CalDALModel;

namespace BRX.Model.Exp
{
    public class ExpModel_CalWall : ObservableObject
    {
        public ExpModel_CalWall()
        {
            //注册计算试验数据消息
            Messenger.Default.Register<string>(this, "CalcCalDataMessage", CalcCalDataMessage);
            HeightPointDQ = WallCalPointsList[1];
            HeightNoDQ = 1;
        }


        #region 校准试验基本信息

        /// <summary>
        /// 试验编号
        /// </summary>
        private string _expNO = "Default";
        /// <summary>
        /// 试验编号
        /// </summary>
        public string ExpNO
        {
            get
            {
                return _expNO;
            }
            set
            {
                _expNO = value;
                RaisePropertyChanged(() => ExpNO);
            }
        }

        /// <summary>
        /// 试验补充说明
        /// </summary>
        private string _expDetail = "//";
        /// <summary>
        /// 试验补充说明
        /// </summary>
        public string ExpDetail
        {
            get
            {
                return _expDetail;
            }
            set
            {
                _expDetail = value;
                RaisePropertyChanged(() => ExpDetail);
            }
        }

        /// <summary>
        /// 报告编号
        /// </summary>
        private string _repNO = "Rpt001_Exp";
        /// <summary>
        /// 报告编号
        /// </summary>
        public string RepNO
        {
            get
            {
                return _repNO;
            }
            set
            {
                _repNO = value;
                RaisePropertyChanged(() => RepNO);
            }
        }

        /// <summary>
        /// 创建日期时间
        /// </summary>
        private DateTime _creatTime = DateTime.Now;
        /// <summary>
        /// 创建日期时间
        /// </summary>
        public DateTime CreatTime
        {
            get
            {
                return _creatTime;
            }
            set
            {
                _creatTime = value;
                RaisePropertyChanged(() => CreatTime);
            }
        }

        /// <summary>
        /// 报告日期时间
        /// </summary>
        private DateTime _repTime = DateTime.Now;
        /// <summary>
        /// 报告日期时间
        /// </summary>
        public DateTime RepTime
        {
            get
            {
                return _repTime;
            }
            set
            {
                _repTime = value;
                RaisePropertyChanged(() => RepTime);
            }
        }

        /// <summary>
        /// 原始试验标志
        /// </summary>
        private bool _isReal = false;
        /// <summary>
        /// 原始试验标志
        /// </summary>
        public bool IsReal
        {
            get { return _isReal; }
            set
            {
                _isReal = value;
                RaisePropertyChanged(() => IsReal);
            }
        }

        #endregion


        #region 数据参数

        /// <summary>
        /// 已完成标志
        /// </summary>
        private bool _isCompleted =false;
        /// <summary>
        /// 已完成标志
        /// </summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                RaisePropertyChanged(() => IsCompleted);
            }
        }

        /// <summary>
        /// 校准点列表
        /// </summary>
        private ObservableCollection<WallCalPoint> _wallCalPointsList = new ObservableCollection<WallCalPoint>()
        {
            new WallCalPoint(1,30,0,0,0,0,0),
            new WallCalPoint(2,0,0,0,0,0,0),
            new WallCalPoint(3,-30,0,0,0,0,0)
        };
        /// <summary>
        /// 校准点列表
        /// </summary>
        public ObservableCollection<WallCalPoint> WallCalPointsList
        {
            get { return _wallCalPointsList; }
            set
            {
                _wallCalPointsList = value;
                RaisePropertyChanged(() => WallCalPointsList);

                //RaisePropertyChanged(() => QtyComplete);
                //RaisePropertyChanged(() => QtyLeft);
                //RaisePropertyChanged(() => IsCompleted);
            }
        }

        /// <summary>
        /// 校准结果
        /// </summary>
        private string _result = "合格";
        /// <summary>
        /// 校准结果
        /// </summary>
        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged(() => Result);
            }
        }

        /// <summary>
        /// 炉壁平均温度（℃）
        /// </summary>
        private double _tavg = 0;
        /// <summary>
        /// 炉壁平均温度（℃）
        /// </summary>
        public double Tavg
        {
            get
            {
                return _tavg;
            }
            set
            {
                _tavg = value;
                RaisePropertyChanged(() => Tavg);
            }
        }

        /// <summary>
        /// 垂轴线1平均温度（℃）
        /// </summary>
        private double _tavg_axis1 = 0;
        /// <summary>
        /// 垂轴线1平均温度（℃）
        /// </summary>
        public double Tavg_axis1
        {
            get
            {
                return _tavg_axis1;
            }
            set
            {
                _tavg_axis1 = value;
                RaisePropertyChanged(() => Tavg_axis1);
            }
        }

        /// <summary>
        /// 垂轴线2平均温度（℃）
        /// </summary>
        private double _tavg_axis2 = 0;
        /// <summary>
        /// 垂轴线2平均温度（℃）
        /// </summary>
        public double Tavg_axis2
        {
            get
            {
                return _tavg_axis2;
            }
            set
            {
                _tavg_axis2 = value;
                RaisePropertyChanged(() => Tavg_axis2);
            }
        }

        /// <summary>
        /// 垂轴线3平均温度（℃）
        /// </summary>
        private double _tavg_axis3 = 0;
        /// <summary>
        /// 垂轴线3平均温度（℃）
        /// </summary>
        public double Tavg_axis3
        {
            get
            {
                return _tavg_axis3;
            }
            set
            {
                _tavg_axis3 = value;
                RaisePropertyChanged(() => Tavg_axis3);
            }
        }

        /// <summary>
        /// 垂轴线1相对总平均温度的偏差百分数（%）
        /// </summary>
        private double _tdev_axis1 = 0;
        /// <summary>
        /// 垂轴线1相对总平均温度的偏差百分数（%）
        /// </summary>
        public double Tdev_axis1
        {
            get
            {
                return _tdev_axis1;
            }
            set
            {
                _tdev_axis1 = value;
                RaisePropertyChanged(() => Tdev_axis1);
            }
        }

        /// <summary>
        /// 垂轴线2相对总平均温度的偏差百分数（%）
        /// </summary>
        private double _tdev_axis2 = 0;
        /// <summary>
        /// 垂轴线2相对总平均温度的偏差百分数（%）
        /// </summary>
        public double Tdev_axis2
        {
            get
            {
                return _tdev_axis2;
            }
            set
            {
                _tdev_axis2 = value;
                RaisePropertyChanged(() => Tdev_axis2);
            }
        }

        /// <summary>
        /// 垂轴线3相对总平均温度的偏差百分数（%）
        /// </summary>
        private double _tdev_axis3 = 0;
        /// <summary>
        /// 垂轴线3相对总平均温度的偏差百分数（%）
        /// </summary>
        public double Tdev_axis3
        {
            get
            {
                return _tdev_axis3;
            }
            set
            {
                _tdev_axis3 = value;
                RaisePropertyChanged(() => Tdev_axis3);
            }
        }

        /// <summary>
        /// 三垂轴线平均温度相对总平均温度的偏差平均值（℃）
        /// </summary>
        private double _tavg_dev_axis = 0;
        /// <summary>
        /// 三垂轴线平均温度相对总平均温度的偏差平均值（℃）
        /// </summary>
        public double Tavg_dev_axis
        {
            get
            {
                return _tavg_dev_axis;
            }
            set
            {
                _tavg_dev_axis = value;
                RaisePropertyChanged(() => Tavg_dev_axis);
            }
        }

        /// <summary>
        /// 30mm高度平均温度（℃）
        /// </summary>
        private double _tavg_levela = 0;
        /// <summary>
        /// 30mm高度平均温度（℃）
        /// </summary>
        public double Tavg_levela
        {
            get
            {
                return _tavg_levela;
            }
            set
            {
                _tavg_levela = value;
                RaisePropertyChanged(() => Tavg_levela);
            }
        }

        /// <summary>
        /// 0mm高度平均温度（℃）
        /// </summary>
        private double _tavg_levelb = 0;
        /// <summary>
        /// 0mm高度平均温度（℃）
        /// </summary>
        public double Tavg_levelb
        {
            get
            {
                return _tavg_levelb;
            }
            set
            {
                _tavg_levelb = value;
                RaisePropertyChanged(() => Tavg_levelb);
            }
        }

        /// <summary>
        /// -30mm高度平均温度（℃）
        /// </summary>
        private double _tavg_levelc = 0;
        /// <summary>
        /// -30mm高度平均温度（℃）
        /// </summary>
        public double Tavg_levelc
        {
            get
            {
                return _tavg_levelc;
            }
            set
            {
                _tavg_levelc = value;
                RaisePropertyChanged(() => Tavg_levelc);
            }
        }

        /// <summary>
        /// 30mm同高度相对总平均温度偏差的百分数（%）
        /// </summary>
        private double _tdev_levela = 0;
        /// <summary>
        /// 30mm同高度相对总平均温度偏差的百分数（%）
        /// </summary>
        public double Tdev_levela
        {
            get
            {
                return _tdev_levela;
            }
            set
            {
                _tdev_levela = value;
                RaisePropertyChanged(() => Tdev_levela);
            }
        }

        /// <summary>
        /// 0mm高度相对总平均温度偏差的百分数（%）
        /// </summary>
        private double _tdev_levelb = 0;
        /// <summary>
        /// 0mm高度相对总平均温度偏差的百分数（%）
        /// </summary>
        public double Tdev_levelb
        {
            get
            {
                return _tdev_levelb;
            }
            set
            {
                _tdev_levelb = value;
                RaisePropertyChanged(() => Tdev_levelb);
            }
        }

        /// <summary>
        /// -30mm高度相对总平均温度偏差的百分数（%）
        /// </summary>
        private double _tdev_levelc = 0;
        /// <summary>
        /// -30mm高度相对总平均温度偏差的百分数（%）
        /// </summary>
        public double Tdev_levelc
        {
            get
            {
                return _tdev_levelc;
            }
            set
            {
                _tdev_levelc = value;
                RaisePropertyChanged(() => Tdev_levelc);
            }
        }
        
        /// <summary>
        /// 三轴线同高度的平均温度偏差（℃）
        /// </summary>
        private double _tavg_dev_level = 0;
        /// <summary>
        /// -三轴线同高度的平均温度偏差（℃）
        /// </summary>
        public double Tavg_dev_level
        {
            get
            {
                return _tavg_dev_level;
            }
            set
            {
                _tavg_dev_level = value;
                RaisePropertyChanged(() => Tavg_dev_level);
            }
        }

        #endregion


        #region 运行参数

        /// <summary>
        /// 当前高度点序号
        /// </summary>
        private int _heightNoDQ = 1;
        /// <summary>
        /// 当前高度点序号
        /// </summary>
        public int HeightNoDQ
        {
            get { return _heightNoDQ; }
            set
            {
                _heightNoDQ = value;
                RaisePropertyChanged(() => HeightNoDQ);
            }
        }

        /// <summary>
        /// 当前高度点检测
        /// </summary>
        private WallCalPoint _heightPointDQ = new WallCalPoint();
        /// <summary>
        /// 当前高度点检测
        /// </summary>
        public WallCalPoint HeightPointDQ
        {
            get { return _heightPointDQ; }
            set
            {
                _heightPointDQ = value;
                RaisePropertyChanged(() => HeightPointDQ);
            }
        }

        /// <summary>
        /// 完成数量
        /// </summary>
        public int QtyComplete
        {
            get
            {
                int num = 0;
                for (int i = 0; i < WallCalPointsList.Count; i++)
                {
                    if (WallCalPointsList[i].IsReced)
                        num++;
                }
                return num;
            }
        }

        /// <summary>
        /// 测试中状态
        /// </summary>
        private bool _isTesting = false;
        /// <summary>
        /// 测试中状态
        /// </summary>
        public bool IsTesting
        {
            get { return _isTesting; }
            set
            {
                _isTesting = value;
                RaisePropertyChanged(() => IsTesting);
                RaisePropertyChanged(() => IsNotTesting);
            }
        }
        /// <summary>
        /// 未测试中状态
        /// </summary>
        public bool IsNotTesting
        {
            get { return !_isTesting; }
        }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int QtyLeft
        {
            get { return WallCalPointsList.Count - QtyComplete; }
        }

        #endregion


        #region 计算方法

        /// <summary>
        /// 计算炉壁校准数据
        /// </summary>
        public void CalcWallCal()
        {
            //更新全部完成标志
            bool allCompleted = true;
            for (int i = 0; i < WallCalPointsList.Count; i++)
            {
                if (!WallCalPointsList[i].IsReced)
                    allCompleted = false;
            }
            IsCompleted= allCompleted;
            RaisePropertyChanged(() => QtyComplete);
            RaisePropertyChanged(() => QtyLeft);

            //总平均值
            Tavg = (WallCalPointsList[0].T1 + WallCalPointsList[0].T2 + WallCalPointsList[0].T3 +
                    WallCalPointsList[1].T1 + WallCalPointsList[1].T2 + WallCalPointsList[1].T3 +
                    WallCalPointsList[2].T1 + WallCalPointsList[2].T2 + WallCalPointsList[2].T3) / 9;
            //同垂轴线平均值
            Tavg_axis1 = (WallCalPointsList[0].T1 + WallCalPointsList[1].T1 + WallCalPointsList[2].T1) / 3;
            Tavg_axis2 = (WallCalPointsList[0].T2 + WallCalPointsList[1].T2 + WallCalPointsList[2].T2) / 3;
            Tavg_axis3 = (WallCalPointsList[0].T3 + WallCalPointsList[1].T3 + WallCalPointsList[2].T3) / 3;
            //同垂轴线相对总平均温度的偏差百分数
            if (Tavg == 0)
            {
                Tdev_axis1 = 0;
                Tdev_axis2 = 0;
                Tdev_axis3 = 0;
            }
            else
            {
                Tdev_axis1 = 100 * Math.Abs(Tavg - Tavg_axis1) / Tavg;
                Tdev_axis2 = 100 * Math.Abs(Tavg - Tavg_axis2) / Tavg;
                Tdev_axis3 = 100 * Math.Abs(Tavg - Tavg_axis3) / Tavg;
            }
            //三垂轴线平均温度相对总平均温度的偏差平均值
            Tavg_dev_axis = (Tdev_axis1 + Tdev_axis2 + Tdev_axis3) / 3;
            
            //同高度的温度平均值
            Tavg_levela = (WallCalPointsList[0].T1 + WallCalPointsList[0].T2 + WallCalPointsList[0].T3) / 3;
            Tavg_levelb = (WallCalPointsList[1].T1 + WallCalPointsList[1].T2 + WallCalPointsList[1].T3) / 3;
            Tavg_levelc = (WallCalPointsList[2].T1 + WallCalPointsList[2].T2 + WallCalPointsList[2].T3) / 3;
            //同高度相对总平均温度的偏差百分数
            if (Tavg == 0)
            {
                Tdev_levela = 0;
                Tdev_levelb = 0;
                Tdev_levelc = 0;
            }
            else
            {
                Tdev_levela = 100 * Math.Abs(Tavg - Tavg_levela) / Tavg;
                Tdev_levelb = 100 * Math.Abs(Tavg - Tavg_levelb) / Tavg;
                Tdev_levelc = 100 * Math.Abs(Tavg - Tavg_levelc) / Tavg;
            }
            //同高度相对偏差的平均值
            Tavg_dev_level = (Tdev_levela + Tdev_levelb + Tdev_levelc) / 3;

            if ((Tavg_dev_axis <= 0.5) && (Tavg_dev_level <= 1.5)&& Tavg!=0)
                Result = "合格";
            else
                Result = "不合格";

        }

        /// <summary>
        /// 计算试验数据消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void CalcCalDataMessage(string msg)
        {
            string order = msg.Clone().ToString();
            if (order == "WallCalData")
            {
                CalcWallCal();
            }
        }
        #endregion


        #region 初始化、复位方法

        /// <summary>
        /// 炉壁校准试验数据重置
        /// </summary>
        public void WallCalDataReset(int no)
        {
            IsTesting = false;

            if ((no >0) && (no <= WallCalPointsList.Count))
                WallCalPointsList[no - 1].DataReset();

            RaisePropertyChanged(() => QtyComplete);
            RaisePropertyChanged(() => QtyLeft);
            RaisePropertyChanged(() => IsCompleted);
        }

        #endregion
    }


    /// <summary>
    /// 炉壁温度校准点类
    /// </summary>
    public class WallCalPoint : ObservableObject
    {
        /// <summary>
        /// 构造函数0
        /// </summary>
        public WallCalPoint()
        {
        }

        /// <summary>
        /// 构造函数1
        /// </summary>
        public WallCalPoint(int NO, int h,double t1, double t2, double t3, double tf1, double tf2)
        {
            LevelNO = NO;
            H = h;
            T1 = t1;
            T2 = t2;
            T3 = t3;
            Tf1 = tf1;
            Tf2 = tf2;
        }

        /// <summary>
        /// 构造函数2
        /// </summary>
        public WallCalPoint(int NO, int h)
        {
            LevelNO = NO;
            H = h;
        }

        /// <summary>
        /// 测试试验数据重置
        /// </summary>
        public void DataReset()
        {
            T1 = 0;
            T2 = 0;
            T3 = 0;
            Tf1 = 0;
            Tf2 = 0;
            IsReced = false;
            TimePoint.Reset();
            TimeKeepT.Reset();
            RecList_WallCal.Clear();
            RecPointNow = 0;
        }

        #region 数据参数

        /// <summary>
        /// 高度编号
        /// </summary>
        private int _levelNO = 1;
        /// <summary>
        /// 高度编号
        /// </summary>
        public int LevelNO
        {
            get
            {
                return _levelNO;
            }
            set
            {
                _levelNO = value;
                RaisePropertyChanged(() => LevelNO);
            }
        }

        /// <summary>
        /// 高度
        /// </summary>
        private int _h = 0;
        /// <summary>
        /// 高度
        /// </summary>
        public int H
        {
            get
            {
                return _h;
            }
            set
            {
                _h = value;
                RaisePropertyChanged(() => H);
            }
        }

        /// <summary>
        /// 校准点温度1（0°）
        /// </summary>
        private double _t1 = 0;
        /// <summary>
        /// 校准点温度1（0°）
        /// </summary>
        public double T1
        {
            get
            {
                return _t1;
            }
            set
            {
                _t1 = value;
                RaisePropertyChanged(() => T1);
            }
        }


        /// <summary>
        /// 校准点温度2（120°）
        /// </summary>
        private double _t2 = 0;
        /// <summary>
        /// 校准点温度2（120°）
        /// </summary>
        public double T2
        {
            get
            {
                return _t2;
            }
            set
            {
                _t2 = value;
                RaisePropertyChanged(() => T2);
            }
        }

        /// <summary>
        /// 校准点温度3（240°）
        /// </summary>
        private double _t3 = 0;
        /// <summary>
        /// 校准点温度3（240°）
        /// </summary>
        public double T3
        {
            get
            {
                return _t3;
            }
            set
            {
                _t3 = value;
                RaisePropertyChanged(() => T3);
            }
        }

        /// <summary>
        /// 炉内温度1
        /// </summary>
        private double _tf1 = 0;
        /// <summary>
        /// 炉内温度1
        /// </summary>
        public double Tf1
        {
            get
            {
                return _tf1;
            }
            set
            {
                _tf1 = value;
                RaisePropertyChanged(() => Tf1);
            }
        }

        /// <summary>
        /// 炉内温度2
        /// </summary>
        private double _tf2 = 0;
        /// <summary>
        /// 炉内温度2
        /// </summary>
        public double Tf2
        {
            get
            {
                return _tf2;
            }
            set
            {
                _tf2 = value;
                RaisePropertyChanged(() => Tf2);
            }
        }

        /// <summary>
        /// 记录时间
        /// </summary>
        private DateTime _recTime = DateTime.MinValue;
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecTime
        {
            get { return _recTime; }
            set
            {
                _recTime = value;
                RaisePropertyChanged(() => RecTime);
            }
        }

        /// <summary>
        /// 已记录标志
        /// </summary>
        private bool _isReced = false;
        /// <summary>
        /// 已记录标志
        /// </summary>
        public bool IsReced
        {
            get { return _isReced; }
            set
            {
                _isReced = value;
                RaisePropertyChanged(() => IsReced);
            }
        }

        /// <summary>
        /// 校准数据原始记录列表
        /// </summary>
        private ObservableCollection<WallCalInitRec> _recList_WallCal = new ObservableCollection<WallCalInitRec>() { };
        /// <summary>
        /// 校准数据原始记录列表
        /// </summary>
        public ObservableCollection<WallCalInitRec> RecList_WallCal
        {
            get { return _recList_WallCal; }
            set
            {
                _recList_WallCal = value;
                RaisePropertyChanged(() => RecList_WallCal);
            }
        }

        #endregion


        #region 运行参数

        /// <summary>
        /// 当前点校准总体测试时间
        /// </summary>
        private Time _timePoint = new Time();
        /// <summary>
        /// 当前点校准总体测试时间
        /// </summary>
        public Time TimePoint
        {
            get { return _timePoint; }
            set
            {
                _timePoint = value;
                RaisePropertyChanged(() => TimePoint);
            }
        }

        /// <summary>
        /// 温度平衡时间
        /// </summary>
        private Time _timeKeepT = new Time();
        /// <summary>
        /// 温度平衡时间
        /// </summary>
        public Time TimeKeepT
        {
            get { return _timeKeepT; }
            set
            {
                _timeKeepT = value;
                RaisePropertyChanged(() => TimeKeepT);
            }
        }

        /// <summary>
        /// 当前记录序号
        /// </summary>
        private int _recPointNow = 0;
        /// <summary>
        ///  当前记录序号
        /// </summary>
        public int RecPointNow
        {
            get { return _recPointNow; }
            set
            {
                _recPointNow = value;
                RaisePropertyChanged(() => RecPointNow);
            }
        }
        #endregion
    }
}
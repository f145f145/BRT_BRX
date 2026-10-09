/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 平衡判定Model
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/8/25 0:08:26		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;

namespace BRX.Model.Dev
{
    public class StabilizationModel: ObservableObject
    {
        public StabilizationModel()
        {
            DataReset();
        }

        #region 控制设定参数

        /// <summary>
        /// 控制目标值
        /// </summary>
        private double _valueAim = 750;
        /// <summary>
        /// 控制目标值
        /// </summary>
        public double ValueAim
        {
            get
            {
                return _valueAim;
            }
            set
            {
                _valueAim = value;
                RaisePropertyChanged(() => ValueAim);
            }
        }

        /// <summary>
        /// 误差允许值
        /// </summary>
        private double _ePermit = 5;
        /// <summary>
        /// 误差允许值
        /// </summary>
        public double EPermit
        {
            get
            {
                return _ePermit;
            }
            set
            {
                _ePermit = Math.Abs(value);
                RaisePropertyChanged(() => EPermit);
            }
        }

        /// <summary>
        /// 漂移允许值
        /// </summary>
        private double _driftPermit = 2;
        /// <summary>
        /// 漂移允许值
        /// </summary>
        public double DriftPermit
        {
            get
            {
                return _driftPermit;
            }
            set
            {
                _driftPermit = Math.Abs(value);
                RaisePropertyChanged(() => DriftPermit);
            }
        }

        /// <summary>
        /// 最大偏差允许值
        /// </summary>
        private double _deviationPermit = 10;
        /// <summary>
        /// 最大偏差允许值
        /// </summary>
        public double DeviationPermit
        {
            get
            {
                return _deviationPermit;
            }
            set
            {
                _deviationPermit =Math.Abs(value);
                RaisePropertyChanged(() => DeviationPermit);
            }
        }

        /// <summary>
        /// 平衡时间
        /// </summary>
        private int _timeEqu = 600;
        /// <summary>
        /// 平衡时间
        /// </summary>
        public int TimeEqu
        {
            get
            {
                return _timeEqu;
            }
            set
            {
                _timeEqu = value;
                RaisePropertyChanged(() => TimeEqu);
            }
        }

        /// <summary>
        /// 样本数量（-1时不限数量）
        /// </summary>
        private int _countLimit = -1;
        /// <summary>
        /// 样本数量（-1时不限数量）
        /// </summary>
        public int CountLimit
        {
            get
            {
                return _countLimit;
            }
            set
            {
                _countLimit = value;
                RaisePropertyChanged(() => CountLimit);
            }
        }

        #endregion


        #region 计算参数

        /// <summary>
        /// 稳定符合要求
        /// </summary>
        private bool _isStabilized = false;
        /// <summary>
        /// 稳定符合要求
        /// </summary>
        public bool IsStabilized
        {
            get
            {
                return _isStabilized;
            }
            set
            {
                _isStabilized = value;
                RaisePropertyChanged(() => IsStabilized);
            }
        }


        /// <summary>
        /// 漂移符合要求
        /// </summary>
        private bool _isDriftMeetsReqs = false;
        /// <summary>
        /// 漂移符合要求
        /// </summary>
        public bool IsDriftMeetsReqs
        {
            get
            {
                return _isDriftMeetsReqs;
            }
            set
            {
                _isDriftMeetsReqs = value;
                RaisePropertyChanged(() => IsDriftMeetsReqs);
            }
        }


        /// <summary>
        /// 最大偏差符合要求
        /// </summary>
        private bool _isDeviationMeetsReqs = false;
        /// <summary>
        /// 最大偏差符合要求
        /// </summary>
        public bool IsDeviationMeetsReqs
        {
            get
            {
                return _isDeviationMeetsReqs;
            }
            set
            {
                _isDeviationMeetsReqs = value;
                RaisePropertyChanged(() => IsDeviationMeetsReqs);
            }
        }


        /// <summary>
        /// 全部指标符合要求
        /// </summary>
        private bool _isFitAll = false;
        /// <summary>
        /// 全部指标符合要求
        /// </summary>
        public bool IsFitAll
        {
            get
            {
                return _isFitAll;
            }
            set
            {
                _isFitAll = value;
                RaisePropertyChanged(() => IsFitAll);
            }
        }


        /// <summary>
        /// 当前样本数量
        /// </summary>
        private int _counts = 0;
        /// <summary>
        /// 当前样本数量
        /// </summary>
        public int Counts
        {
            get
            {
                return _counts;
            }
            set
            {
                _counts = value;
                RaisePropertyChanged(() => Counts);
            }
        }

        /// <summary>
        /// 漂移
        /// </summary>
        private double _eDrift = 0;
        /// <summary>
        /// 漂移
        /// </summary>
        public double EDrift
        {
            get
            {
                return _eDrift;
            }
            set
            {
                _eDrift = value;
                RaisePropertyChanged(() => EDrift);
            }
        }

        /// <summary>
        /// Y平均值
        /// </summary>
        private double _y_Avg = 0;
        /// <summary>
        /// Y平均值
        /// </summary>
        public double Y_Avg
        {
            get
            {
                return _y_Avg;
            }
            set
            {
                _y_Avg = value;
                RaisePropertyChanged(() => Y_Avg);
            }
        }

        /// <summary>
        /// 最大偏差（相对平均值）
        /// </summary>
        private double _deviationMax = 0;
        /// <summary>
        /// 最大偏差（相对平均值）
        /// </summary>
        public double DeviationMax
        {
            get
            {
                return _deviationMax;
            }
            set
            {
                _deviationMax = value;
                RaisePropertyChanged(() => DeviationMax);
            }
        }


        /// <summary>
        /// Y最大值
        /// </summary>
        private double _y_Max = 0;
        /// <summary>
        /// Y最大值
        /// </summary>
        public double Y_Max
        {
            get
            {
                return _y_Max;
            }
            set
            {
                _y_Max = value;
                RaisePropertyChanged(() => Y_Max);
            }
        }

        /// <summary>
        /// Y最小值
        /// </summary>
        private double _y_Min = 0;
        /// <summary>
        /// Y最小值
        /// </summary>
        public double Y_Min
        {
            get
            {
                return _y_Min;
            }
            set
            {
                _y_Min = value;
                RaisePropertyChanged(() => Y_Min);
            }
        }

        #endregion


        #region 数据列表

        /// <summary>
        /// x列表
        /// </summary>
        private List<double> _dataListX = new List<double>();
        /// <summary>
        /// x列表
        /// </summary>
        private List<double> DataListX
        {
            get
            {
                return _dataListX;
            }
            set
            {
                _dataListX = value;
                RaisePropertyChanged(() => DataListX);
            }
        }

        /// <summary>
        /// y列表
        /// </summary>
        private List<double> _dataListY = new List<double>();
        /// <summary>
        /// y列表
        /// </summary>
        private List<double> DataListY
        {
            get
            {
                return _dataListY;
            }
            set
            {
                _dataListY = value;
                RaisePropertyChanged(() => DataListY);
            }
        }

        /// <summary>
        /// x的平方列表
        /// </summary>
        private List<double> _dataListXPow2 = new List<double>();
        /// <summary>
        /// x的平方列表
        /// </summary>
        private List<double> DataListXPow2
        {
            get
            {
                return _dataListXPow2;
            }
            set
            {
                _dataListXPow2 = value;
                RaisePropertyChanged(() => DataListXPow2);
            }
        }

        /// <summary>
        /// y的平方列表
        /// </summary>
        private List<double> _dataListYPow2 = new List<double>();
        /// <summary>
        /// y的平方列表
        /// </summary>
        private List<double> DataListYPow2
        {
            get
            {
                return _dataListYPow2;
            }
            set
            {
                _dataListYPow2 = value;
                RaisePropertyChanged(() => DataListYPow2);
            }
        }

        /// <summary>
        /// xy乘积列表
        /// </summary>
        private List<double> _dataListXY = new List<double>();
        /// <summary>
        /// y列xy乘积列表表
        /// </summary>
        private List<double> DataListXY
        {
            get
            {
                return _dataListXY;
            }
            set
            {
                _dataListXY = value;
                RaisePropertyChanged(() => DataListXY);
            }
        }

        /// <summary>
        /// 偏差列表（相对于平均值的差值）
        /// </summary>
        private List<double> _dataListDeviation = new List<double>();
        /// <summary>
        /// 偏差列表（相对于平均值的差值）
        /// </summary>
        private List<double> DataListDeviation
        {
            get
            {
                return _dataListDeviation;
            }
            set
            {
                _dataListDeviation = value;
                RaisePropertyChanged(() => DataListDeviation);
            }
        }

        /// <summary>
        /// 偏差的模列表（相对于平均值的差值）
        /// </summary>
        private List<double> _dataListDeviationMod = new List<double>();
        /// <summary>
        /// 偏差的模列表（相对于平均值的差值）
        /// </summary>
        private List<double> DataListDeviationMod
        {
            get
            {
                return _dataListDeviationMod;
            }
            set
            {
                _dataListDeviationMod = value;
                RaisePropertyChanged(() => DataListDeviationMod);
            }
        }

        /// <summary>
        /// 误差列表（回归值与真实值的差）
        /// </summary>
        private List<double> _dataListNE = new List<double>();
        /// <summary>
        /// 误差列表（回归值与真实值的差）
        /// </summary>
        private List<double> DataListNE
        {
            get
            {
                return _dataListNE;
            }
            set
            {
                _dataListNE = value;
                RaisePropertyChanged(() => DataListNE);
            }
        }

        /// <summary>
        /// 误差的模列表（回归值与真实值的差）
        /// </summary>
        private List<double> _dataListNEmod = new List<double>();
        /// <summary>
        /// 误差的模列表（回归值与真实值的差）
        /// </summary>
        private List<double> DataListNEmod
        {
            get
            {
                return _dataListNEmod;
            }
            set
            {
                _dataListNEmod = value;
                RaisePropertyChanged(() => DataListNEmod);
            }
        }

        #endregion


        #region 过程参数

        /// <summary>
        /// 数据计算中
        /// </summary>
        private bool _isAdding = false;
        /// <summary>
        /// 数据计算中
        /// </summary>
        public bool IsAdding
        {
            get
            {
                return _isAdding;
            }
            set
            {
                _isAdding = value;
                RaisePropertyChanged(() => IsAdding);
            }
        }

        /// <summary>
        /// 初始数据
        /// </summary>
        private bool _isInitial = true;
        /// <summary>
        /// 初始数据
        /// </summary>
        private bool IsInitial
        {
            get
            {
                return _isInitial;
            }
            set
            {
                _isInitial = value;
                RaisePropertyChanged(() => IsInitial);
            }
        }

        /// <summary>
        /// ΣX²-n(Xav)²
        /// </summary>
        private double _lxx = 0;
        /// <summary>
        /// ΣX²-n(Xav)²
        /// </summary>
        private double LXX
        {
            get
            {
                return _lxx;
            }
            set
            {
                _lxx = value;
                RaisePropertyChanged(() => LXX);
            }
        }

        /// <summary>
        /// Σ(XY)-n(Xav*Yav)
        /// </summary>
        private double _lxy = 0;
        /// <summary>
        /// Σ(XY)-n(Xav*Yav)
        /// </summary>
        private double LXY
        {
            get
            {
                return _lxy;
            }
            set
            {
                _lxy = value;
                RaisePropertyChanged(() => LXY);
            }
        }

        /// <summary>
        /// 斜率
        /// </summary>
        private double _a = 0;
        /// <summary>
        /// 斜率
        /// </summary>
        private double A
        {
            get
            {
                return _a;
            }
            set
            {
                _a = value;
                RaisePropertyChanged(() => A);
            }
        }

        /// <summary>
        /// 截距
        /// </summary>
        private double _b = 0;
        /// <summary>
        /// 截距
        /// </summary>
        private double B
        {
            get
            {
                return _b;
            }
            set
            {
                _b = value;
                RaisePropertyChanged(() => B);
            }
        }

        /// <summary>
        /// X最大值
        /// </summary>
        private double _x_Max = 0;
        /// <summary>
        /// X最大值
        /// </summary>
        private double X_Max
        {
            get
            {
                return _x_Max;
            }
            set
            {
                _x_Max = value;
                RaisePropertyChanged(() => X_Max);
            }
        }

        /// <summary>
        /// X最小值
        /// </summary>
        private double _x_Min = 0;
        /// <summary>
        /// X最小值
        /// </summary>
        private double X_Min
        {
            get
            {
                return _x_Min;
            }
            set
            {
                _x_Min = value;
                RaisePropertyChanged(() => X_Min);
            }
        }

        /// <summary>
        /// X平均值
        /// </summary>
        private double _x_Avg = 0;
        /// <summary>
        /// X平均值
        /// </summary>
        private double X_Avg
        {
            get
            {
                return _x_Avg;
            }
            set
            {
                _x_Avg = value;
                RaisePropertyChanged(() => X_Avg);
            }
        }

        #endregion


        #region 计算方法

        /// <summary>
        /// 添加数据并自动计算（x默认为序号）
        /// </summary>
        /// <param name="tempy">y值</param>
        public void ADD(double tempy)
        {
            try
            {
                double xWillAdd = 0;
                if (DataListX.Count == 0)
                    xWillAdd = 1;
                else
                    xWillAdd = DataListX[DataListX.Count - 1] + 1;
                ADDXY(xWillAdd, tempy);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 添加数据并自动计算
        /// </summary>
        /// <param name="tempx">x值</param>
        /// <param name="tempy">y值</param>
        public void ADDXY(double tempx, double tempy)
        {
            try
            {
                lock (this)
                {
                    while (IsAdding)
                        Thread.Sleep(10);
                    IsAdding = true;
                }                

                //列表增减数据
                DataListX.Add(tempx);
                DataListY.Add(tempy);
                DataListXPow2.Add(Math.Pow(tempx, 2));
                DataListYPow2.Add(Math.Pow(tempy, 2));
                while (DataListX.Count > CountLimit)
                {
                    DataListX.RemoveAt(0);
                }
                while (DataListXPow2.Count > CountLimit)
                {
                    DataListXPow2.RemoveAt(0);
                }
                while (DataListY.Count > CountLimit)
                {
                    DataListY.RemoveAt(0);
                }
                while (DataListYPow2.Count > CountLimit)
                {
                    DataListYPow2.RemoveAt(0);
                }

                DataListXY.Clear();
                for (int i = 0; i < DataListX.Count; i++)
                {
                    if ((DataListY.Count > i) && (DataListX.Count > i))
                        DataListXY.Add(DataListX[i] * DataListY[i]);
                }

                Counts = DataListY.Count;

                if (DataListX.Count > 0)
                {
                    //计算极值、均值
                    X_Max = DataListX.Max();
                    X_Min = DataListX.Min();
                    X_Avg = DataListX.Average(); ;
                    Y_Max = DataListY.Max();
                    Y_Min = DataListY.Min();
                    Y_Avg = DataListY.Average();

                    //计算分母
                    LXX = DataListXPow2.Sum() - DataListX.Count * Math.Pow(DataListX.Average(), 2);
                    //计算分子
                    LXY = DataListXY.Sum() - DataListXY.Count * DataListX.Average() * DataListY.Average();

                    //计算a、b
                    if (!(LXX == 0))
                        A = LXY / LXX;
                    else
                        A = 0;
                    B = DataListY.Average() - A * DataListX.Average();

                    //计算漂移
                    EDrift = A * DataListX[DataListX.Count - 1] + B - (A * DataListX[0] + B);

                    //计算偏差（相对平均值）
                    DataListDeviation.Clear();
                    DataListDeviationMod.Clear();
                    for (int i = 0; i < DataListY.Count; i++)
                    {
                        if ((DataListY.Count > i) && (DataListX.Count > i))
                            DataListDeviation.Add(DataListY[i] - DataListY.Average());
                        if (DataListDeviation.Count > i)
                            DataListDeviationMod.Add(Math.Abs(DataListDeviation[i]));
                    }
                    DeviationMax = DataListDeviationMod.Max();

                    //计算误差
                    DataListNE.Clear();
                    DataListNEmod.Clear();
                    for (int i = 0; i < DataListY.Count; i++)
                    {
                        if ((DataListY.Count > i) && (DataListX.Count > i))
                            DataListNE.Add(DataListY[i] - (A * DataListX[i] + B));
                        if (DataListNE.Count > i)
                            DataListNEmod.Add(Math.Abs(DataListNE[i]));
                    }

                    IsInitial = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //解锁状态
                IsAdding = false;
            }
        }

        /// <summary>
        /// 分析结果
        /// </summary>
        /// <param name="data"></param>
        public void Evaluate()
        {
            try
            {
                //稳定
                if ((CountLimit > 0 && Counts < CountLimit) || Counts < 1 || IsInitial)
                    IsStabilized = false;
                else
                {
                    if (Math.Abs(Y_Avg) >= Math.Abs(ValueAim - EPermit) && Math.Abs(Y_Avg) <= Math.Abs(ValueAim + EPermit))
                        IsStabilized = true;
                    else
                        IsStabilized = false;
                }

                //漂移
                if ((CountLimit > 0 && Counts < CountLimit) || Counts < 1 || IsInitial)
                    IsDriftMeetsReqs = false;
                else
                {
                    if (Math.Abs(EDrift) <= DriftPermit)
                        IsDriftMeetsReqs = true;
                    else
                        IsDriftMeetsReqs = false;
                }

                //最大偏差
                if ((CountLimit > 0 && Counts < CountLimit) || Counts < 1 || IsInitial)
                    IsDeviationMeetsReqs = false;
                else
                {
                    if (Math.Abs(DeviationMax) <= DeviationPermit)
                        IsDeviationMeetsReqs = true;
                    else
                        IsDeviationMeetsReqs = false;
                }

                //所有符合
                IsFitAll = IsStabilized && IsDriftMeetsReqs && IsDeviationMeetsReqs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 复位重置方法

        /// <summary>
        /// 清空数据
        /// </summary>
        public void DataReset()
        {
            while (IsAdding)
                Thread.Sleep(10);

            IsAdding = false;
            DataListX.Clear();
            DataListY.Clear();
            DataListYPow2.Clear();
            DataListXPow2.Clear();
            DataListXY.Clear();
            DataListDeviation.Clear();
            DataListNE.Clear();
            DataListDeviationMod.Clear();
            DataListNEmod.Clear();

            EDrift = 0;
            DeviationMax = 0;
            LXX = 0;
            LXY = 0;
            A = 0;
            B = 0;
            X_Max = 0;
            Y_Max = 0;
            X_Min = 0;
            Y_Min = 0;
            X_Avg = 0;
            Y_Avg = 0;
            IsInitial = true;

            Evaluate();
        }

        #endregion
    }
}
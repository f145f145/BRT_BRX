/************************************************************************************
 * Copyright @ DESKTOP-SB8IL0P 2022. All rights reserved.
 * CLR 版本：4.0.30319.42000
 * 项目名称：BRX.Model.Exp
 * 命名空间：BRX.Model.Exp
 * 类 名 称：ExpModel_CenterCal 
 * 创 建 人：郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/8/23 9:46:46 
 * 描    述：
 * 炉内温度校准试验Model。
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/8/23 9:46:46  		郝正强			V1.0.0.0
 *
 ************************************************************************************/
using System;
using System.Collections.ObjectModel;
using BRX.DAL.CalDAL.CalDALModel;
using BRX.Model;
using BRX.Model.Exp;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace BRX.Model.Exp
{
    public class ExpModel_CalCenter : ObservableObject
    {
        public ExpModel_CalCenter()
        {
            //注册计算试验数据消息
            Messenger.Default.Register<string>(this, "CalcCalDataMessage", CalcCalDataMessage);
            HeightPointDQ = CenterCalPointsList[0];
            HeightNoDQ = 0;
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
        private bool _isCompleted = false;
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
        ///<remarks>0-75下,1-65下,2-55下,3-45下,4-35下,5-25下,6-15下,7-5下，</remarks>
        ///<remarks>8-5上,9-15上,10-25上,11-35上,12-45上,13-55上,14-65上,15-75上,</remarks>
        ///<remarks>16-85上,17-95上,18-105上,19-115上,20-125上,21-135上,22-145上,</remarks>
        ///<remarks>23-145下,24-135下,25-125下,26-115下,27-105下,28-95下,29-85下，</remarks>
        private ObservableCollection<CenterCalPoint> _centerCalPointsList = new ObservableCollection<CenterCalPoint>()
        {
            new CenterCalPoint(0,75,-1),

            new CenterCalPoint(1,65,-1),
            new CenterCalPoint(2,55,-1),
            new CenterCalPoint(3,45,-1),
            new CenterCalPoint(4,35,-1),
            new CenterCalPoint(5,25,-1),
            new CenterCalPoint(6,15,-1),
            new CenterCalPoint(7,5,-1),
        
            new CenterCalPoint(8,5,1),
            new CenterCalPoint(9,15,1),
            new CenterCalPoint(10,25,1),
            new CenterCalPoint(11,35,1),
            new CenterCalPoint(12,45,1),
            new CenterCalPoint(13,55,1),
            new CenterCalPoint(14,65,1),

            new CenterCalPoint(15,75,1),

            new CenterCalPoint(16,85,1),
            new CenterCalPoint(17,95,1),
            new CenterCalPoint(18,105,1),
            new CenterCalPoint(19,115,1),
            new CenterCalPoint(20,125,1),
            new CenterCalPoint(21,135,1),
            new CenterCalPoint(22,145,1),

            new CenterCalPoint(23,145,-1),
            new CenterCalPoint(24,135,-1),
            new CenterCalPoint(25,125,-1),
            new CenterCalPoint(26,115,-1),
            new CenterCalPoint(27,105,-1),
            new CenterCalPoint(28,95,-1),
            new CenterCalPoint(29,85,-1)
        };
        /// <summary>
        /// 校准点列表
        /// </summary>
        /// <remarks>   0-75下,1-65下,2-55下,3-45下,4-35下,5-25下,6-15下,7-5下，
        ///             8-5上,9-15上,10-25上,11-35上,12-45上,13-55上,14-65上,15-75上,
        ///             16-85上,17-95上,18-105上,19-115上,20-125上,21-135上,22-145上,
        ///             23-145下,24-135下,25-125下,26-115下,27-105下,28-95下,29-85下，</remarks>
        public ObservableCollection<CenterCalPoint> CenterCalPointsList
        {
            get { return _centerCalPointsList; }
            set
            {
                _centerCalPointsList = value;
                RaisePropertyChanged(() => CenterCalPointsList);
            }
        }
        
        /// <summary>
        /// 温度平均值列表（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        private ObservableCollection<double> _tAvgList = new ObservableCollection<double>()
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        };
        /// <summary>
        /// 温度平均值列表（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        public ObservableCollection<double> TAvgList
        {
            get
            {
                return _tAvgList;
            }
            set
            {
                _tAvgList = value;
                RaisePropertyChanged(() => TAvgList);
            }
        }

        /// <summary>
        /// 符合要求标志列表（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        private ObservableCollection<bool> _isFitStdList = new ObservableCollection<bool>()
        {
            false, false, false, false, false, false, false, false, false, false, false, false, false, false, false
        };
        /// <summary>
        /// 符合要求标志列表（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        public ObservableCollection<bool> IsFitStdList
        {
            get
            {
                return _isFitStdList;
            }
            set
            {
                _isFitStdList = value;
                RaisePropertyChanged(() => IsFitStdList);
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
        /// 高度点列表（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        private ObservableCollection<double> _hList = new ObservableCollection<double>()
        {
            5,15,25,35,45,55,65,75,85,95,105,115,125,135,145
        };
        /// <summary>
        /// 高度点列表（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        public ObservableCollection<double> HList
        {
            get
            {
                return _hList;
            }
        }

        /// <summary>
        /// 最大温度限值（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        private ObservableCollection<double> _tMaxLimitList_2023 = new ObservableCollection<double>()
        {
            GetTmax2023(5),
            GetTmax2023(15),
            GetTmax2023(25),
            GetTmax2023(35),
            GetTmax2023(45),
            GetTmax2023(55),
            GetTmax2023(65),

            GetTmax2023(75),

            GetTmax2023(85),
            GetTmax2023(95),
            GetTmax2023(105),
            GetTmax2023(115),
            GetTmax2023(125),
            GetTmax2023(135),
            GetTmax2023(145)
        };
        /// <summary>
        /// 最大温度限值（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        public ObservableCollection<double> TMaxLimitList_2023
        {
            get
            {
                return _tMaxLimitList_2023;
            }
        }

        /// <summary>
        /// 最低温度限值（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        private ObservableCollection<double> _tMinLimitList_2023 = new ObservableCollection<double>()
        {
            GetTmin2023(5),
            GetTmin2023(15),
            GetTmin2023(25),
            GetTmin2023(35),
            GetTmin2023(45),
            GetTmin2023(55),
            GetTmin2023(65),

            GetTmin2023(75),

            GetTmin2023(85),
            GetTmin2023(95),
            GetTmin2023(105),
            GetTmin2023(115),
            GetTmin2023(125),
            GetTmin2023(135),
            GetTmin2023(145)
        };
        /// <summary>
        /// 最低温度限值（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        public ObservableCollection<double> TMinLimitList_2023
        {
            get
            {
                return _tMinLimitList_2023;
            }
        }

        /// <summary>
        /// 最大温度限值（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        private ObservableCollection<double> _tMaxLimitList_2010 = new ObservableCollection<double>()
        {
            GetTmax2010(5),
            GetTmax2010(15),
            GetTmax2010(25),
            GetTmax2010(35),
            GetTmax2010(45),
            GetTmax2010(55),
            GetTmax2010(65),

            GetTmax2010(75),

            GetTmax2010(85),
            GetTmax2010(95),
            GetTmax2010(105),
            GetTmax2010(115),
            GetTmax2010(125),
            GetTmax2010(135),
            GetTmax2010(145)
        };
        /// <summary>
        /// 最大温度限值（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        public ObservableCollection<double> TMaxLimitList_2010
        {
            get
            {
                return _tMaxLimitList_2010;
            }
        }

        /// <summary>
        /// 最低温度限值（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        private ObservableCollection<double> _tMinLimitList_2010 = new ObservableCollection<double>()
        {
            GetTmin2010(5),
            GetTmin2010(15),
            GetTmin2010(25),
            GetTmin2010(35),
            GetTmin2010(45),
            GetTmin2010(55),
            GetTmin2010(65),

            GetTmin2010(75),

            GetTmin2010(85),
            GetTmin2010(95),
            GetTmin2010(105),
            GetTmin2010(115),
            GetTmin2010(125),
            GetTmin2010(135),
            GetTmin2010(145)
        };
        /// <summary>
        /// 最低温度限值（5，15，25，35，45，55，65，75，85，95，105，115，125，135，145）
        /// </summary>
        public ObservableCollection<double> TMinLimitList_2010
        {
            get
            {
                return _tMinLimitList_2010;
            }
        }

        ///// <summary>
        ///// 计算最高温度2023
        ///// </summary>
        static public double GetTmax2023(double pointH)
        {
            return 614.167 + (5.347 * pointH) - (0.081 * pointH * pointH) + (5.826 * pointH * pointH * pointH * 0.0001) - (1.772 * pointH * pointH * pointH * pointH * 0.000001);
        }

        ///// <summary>
        ///// 计算最低温度2023
        ///// </summary>
        static public double GetTmin2023(double pointH)
        {
            return 541.653 + (5.901 * pointH) - (0.067 * pointH * pointH) + (3.375 * pointH * pointH * pointH * 0.0001) - (8.553 * pointH * pointH * pointH * pointH * 0.0000001);
        }

        ///// <summary>
        ///// 计算最高温度2010
        ///// </summary>
        static public double GetTmax2010(double pointH)
        {
            return 613.906 + (5.333 * pointH) - (0.081 * pointH * pointH) + (5.779 * pointH * pointH * pointH * 0.0001) - (1.767 * pointH * pointH * pointH * pointH * 0.000001);
        }

        ///// <summary>
        ///// 计算最低温度2010
        ///// </summary>
        static public double GetTmin2010(double pointH)
        {
            return 541.653 + (5.901 * pointH) - (0.067 * pointH * pointH) + (3.375 * pointH * pointH * pointH * 0.0001) - (8.553 * pointH * pointH * pointH * pointH * 0.0000001);
        }

        /// <summary>
        /// 执行标准
        /// </summary>
        private string _std = "GB/T 5464-2010";
        /// <summary>
        /// 执行标准
        /// </summary>
        public string Std
        {
            get { return _std; }
            set
            {
                _std = value;
                RaisePropertyChanged(() => Std);
            }
        }

        #endregion


        #region 运行参数

        /// <summary>
        /// 温度稳定时间(min，稳定时间后可以采集)
        /// </summary>
        private int _steadyTime = 5;
        /// <summary>
        /// 温度稳定时间(min)
        /// </summary>
        public int SteadyTime
        {
            get { return _steadyTime; }
        }

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
        private CenterCalPoint _heightPointDQ = new CenterCalPoint();
        /// <summary>
        /// 当前高度点检测
        /// </summary>
        public CenterCalPoint HeightPointDQ
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
                for (int i = 0; i < CenterCalPointsList.Count; i++)
                {
                    if (CenterCalPointsList[i].IsReced)
                        num++;
                }
                return num;
            }
        }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int QtyLeft
        {
            get { return CenterCalPointsList.Count - QtyComplete; }
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

        #endregion


        #region 计算方法

        /// <summary>
        /// 计算炉内校准数据
        /// </summary>
        public void CalcCenterCal()
        {
            bool IsFit = true;
            bool allComplete = true;

            TAvgList[0] = (CenterCalPointsList[7].Tfc + CenterCalPointsList[8].Tfc) / 2;      //5mm
            TAvgList[1] = (CenterCalPointsList[6].Tfc + CenterCalPointsList[9].Tfc) / 2;      //15mm
            TAvgList[2] = (CenterCalPointsList[5].Tfc + CenterCalPointsList[10].Tfc) / 2;     //25mm
            TAvgList[3] = (CenterCalPointsList[4].Tfc + CenterCalPointsList[11].Tfc) / 2;     //35mm
            TAvgList[4] = (CenterCalPointsList[3].Tfc + CenterCalPointsList[12].Tfc) / 2;     //45mm
            TAvgList[5] = (CenterCalPointsList[2].Tfc + CenterCalPointsList[13].Tfc) / 2;     //55mm
            TAvgList[6] = (CenterCalPointsList[1].Tfc + CenterCalPointsList[14].Tfc) / 2;     //65mm
            TAvgList[7] = (CenterCalPointsList[0].Tfc + CenterCalPointsList[15].Tfc) / 2;     //75mm
            TAvgList[8] = (CenterCalPointsList[29].Tfc + CenterCalPointsList[16].Tfc) / 2;    //85mm
            TAvgList[9] = (CenterCalPointsList[28].Tfc + CenterCalPointsList[17].Tfc) / 2;    //95mm
            TAvgList[10] = (CenterCalPointsList[27].Tfc + CenterCalPointsList[18].Tfc) / 2;   //105mm
            TAvgList[11] = (CenterCalPointsList[26].Tfc + CenterCalPointsList[19].Tfc) / 2;   //115mm
            TAvgList[12] = (CenterCalPointsList[25].Tfc + CenterCalPointsList[20].Tfc) / 2;   //125mm
            TAvgList[13] = (CenterCalPointsList[24].Tfc + CenterCalPointsList[21].Tfc) / 2;   //135mm
            TAvgList[14] = (CenterCalPointsList[23].Tfc + CenterCalPointsList[22].Tfc) / 2;   //135mm


            for (int i = 0; i < CenterCalPointsList.Count; i++)
            {
                if (!CenterCalPointsList[i].IsReced)
                {
                    allComplete = false;
                    IsFit = false;
                }
            }

            for (int i = 0; i < TAvgList.Count; i++)
            {
                if (Std == "GB/T 5464-2010")
                {
                    if ((TAvgList[i] >= TMinLimitList_2010[i]) && (TAvgList[i] <= TMaxLimitList_2010[i]))
                        IsFitStdList[i] = true;
                    else
                    {
                        IsFitStdList[i] = false;
                        IsFit = false;
                    }
                }
                else
                {
                    if ((TAvgList[i] >= TMinLimitList_2023[i]) && (TAvgList[i] <= TMaxLimitList_2023[i]))
                        IsFitStdList[i] = true;
                    else
                    {
                        IsFitStdList[i] = false;
                        IsFit = false;
                    }
                }
            }

            IsCompleted = allComplete;
            Result = IsFit ? "合格" : "不合格";
            RaisePropertyChanged(() => QtyComplete);
            RaisePropertyChanged(() => QtyLeft);
            Messenger.Default.Send<string>("CenterCalPointChanged", "CenterCalPointChanged");
        }


        /// <summary>
        /// 计算试验数据消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void CalcCalDataMessage(string msg)
        {
            string order = msg.Clone().ToString();
            if (order == "CenterCalData")
            {
                CalcCenterCal();
            }
        }
        
        #endregion


        #region 初始化、复位方法

        /// <summary>
        /// 炉内校准试验数据重置
        /// </summary>
        public void CenterCalDataReset(int no)
        {
            IsTesting = false;

            if ((no>=0)&&(no<CenterCalPointsList.Count))
                CenterCalPointsList[no].DataReset();

            CalcCenterCal();

            RaisePropertyChanged(() => QtyComplete);
            RaisePropertyChanged(() => QtyLeft);
            RaisePropertyChanged(() => IsCompleted);
        }
    }

    #endregion
}


/// <summary>
/// 炉内温度校准点类
/// </summary>
public class CenterCalPoint : ObservableObject
{
    /// <summary>
    /// 构造函数0
    /// </summary>
    public CenterCalPoint()
    {
    }

    /// <summary>
    /// 构造函数1
    /// </summary>
    public CenterCalPoint(int NO, int h, int dir)
    {
        PointNO = NO;
        PointH = h;
        Dir = dir;
    }

    #region 数据参数
    
    /// <summary>
    /// 校准点编号
    /// </summary>
    private int _pointNO = 0;
    /// <summary>
    /// 校准点编号
    /// </summary>
    public int PointNO
    {
        get
        {
            return _pointNO;
        }
        set
        {
            _pointNO = value;
            RaisePropertyChanged(() => PointNO);
        }
    }

    
    /// <summary>
    /// 校准点高度
    /// </summary>
    private int _pointH = 5;
    /// <summary>
    /// 校准点高度
    /// </summary>
    public int PointH
    {
        get
        {
            return _pointH;
        }
        set
        {
            _pointH = value;
            RaisePropertyChanged(() => PointH);
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
    /// 校准点温度
    /// </summary>
    private double _tfc = 0;
    /// <summary>
    /// 校准点温度
    /// </summary>
    public double Tfc
    {
        get
        {
            return _tfc;
        }
        set
        {
            _tfc = value;
            RaisePropertyChanged(() => Tfc);
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
    /// 运行方向（1向上，-1向下）
    /// </summary>
    private int _dir = 1;
    /// <summary>
    /// 运行方向（1向上，-1向下）
    /// </summary>
    public int Dir
    {
        get
        {
            return _dir;
        }
        set
        {
            _dir = value;
            RaisePropertyChanged(() => Dir);
        }
    }

    /// <summary>
    /// 校准数据原始记录列表
    /// </summary>
    private ObservableCollection<CenterCalInitRec> _recList_CenterCal = new ObservableCollection<CenterCalInitRec>() { };
    /// <summary>
    /// 校准数据原始记录列表
    /// </summary>
    public ObservableCollection<CenterCalInitRec> RecList_CenterCal
    {
        get { return _recList_CenterCal; }
        set
        {
            _recList_CenterCal = value;
            RaisePropertyChanged(() => RecList_CenterCal);
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


    /// <summary>
    /// 数据重置
    /// </summary>
    public void DataReset()
    {
        Tf1 = 0;
        Tf2 = 0;
        Tfc = 0;
        IsReced = false;
        TimePoint.Reset();
        TimeKeepT.Reset();
        RecList_CenterCal.Clear();
        RecPointNow = 0;
        RecTime = DateTime.MinValue;
    }
}
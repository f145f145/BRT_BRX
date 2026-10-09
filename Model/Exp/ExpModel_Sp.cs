/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 试样检测试验Model
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-8-22 11:12:00		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using NPOI.Util;
using SqlSugar;
using static BRX.Model.Enums.Enums;

namespace BRX.Model.Exp
{
    public class ExpModel_Sp : ObservableObject
    {
        public ExpModel_Sp()
        {
            //计算周期变更消息
            Messenger.Default.Register<int>(this, "PllPeriodChanged", PllPeriodChangedMessage);
        }
        /// <summary>
        /// 试样序号
        /// </summary>
        private int _spNO = 1;
        /// <summary>
        /// 试样序号
        /// </summary>
        public int SpNO
        {
            get
            {
                return _spNO;
            }
            set
            {
                _spNO = value;
                RaisePropertyChanged(() => SpNO);
            }
        }

        #region 试样检测试验数据

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
        /// 检测功率值/电压百分比
        /// </summary>
        private double _testPower = 0;
        /// <summary>
        /// 检测功率值/电压百分比
        /// </summary>
        public double TestPower
        {
            get
            {
                return _testPower;
            }
            set
            {
                _testPower = value;
                RaisePropertyChanged(() => TestPower);
            }
        }

        /// <summary>
        /// 试样初始质量（g）
        /// </summary>
        private double _weightBefore = 0;
        /// <summary>
        /// 试样初始质量（g）
        /// </summary>
        public double WeightBefore
        {
            get { return _weightBefore; }
            set
            {
                _weightBefore = value;
                RaisePropertyChanged(() => WeightBefore);
                RaisePropertyChanged(() => LostRatio);
            }
        }

        /// <summary>
        /// 试样最终质量（g）
        /// </summary>
        private double _weightFinal = 0;
        /// <summary>
        /// 试样最终质量（g）
        /// </summary>
        public double WeightFinal
        {
            get { return _weightFinal; }
            set
            {
                _weightFinal = value;
                RaisePropertyChanged(() => WeightFinal);
                RaisePropertyChanged(() => LostRatio);
            }
        }

        /// <summary>
        /// 试样质量损失率（%）
        /// </summary>
        public double LostRatio
        {
            get
            {
                if (WeightBefore == 0)
                    return 0;
                else
                {
                    double ratio = (WeightBefore - WeightFinal) / WeightBefore * 100;
                    return ratio;
                }

            }
        }

        /// <summary>
        /// 初始温度1（℃）
        /// </summary>
        private double _tStart1 = 0;
        /// <summary>
        /// 初始温度1（℃）
        /// </summary>
        public double TStart1
        {
            get { return _tStart1; }
            set
            {
                _tStart1 = value;
                RaisePropertyChanged(() => TStart1);
                RaisePropertyChanged(() => TStartAvg);
                RaisePropertyChanged(() => TUp1);
                RaisePropertyChanged(() => TUpAvg);
            }
        }

        /// <summary>
        /// 初始温度2（℃）
        /// </summary>
        private double _tStart2 = 0;
        /// <summary>
        /// 初始温度2（℃）
        /// </summary>
        public double TStart2
        {
            get { return _tStart2; }
            set
            {
                _tStart2 = value;
                RaisePropertyChanged(() => TStart2);
                RaisePropertyChanged(() => TStartAvg);
                RaisePropertyChanged(() => TUp2);
                RaisePropertyChanged(() => TUpAvg);
            }
        }

        /// <summary>
        /// 初始平均温度（℃）
        /// </summary>
        public double TStartAvg
        {
            get { return (TStart1 + TStart2) / 2; }
        }

        /// <summary>
        /// 最终温度1（℃）
        /// </summary>
        private double _tFinal1 = 0;
        /// <summary>
        /// 最终温度1（℃）
        /// </summary>
        public double TFinal1
        {
            get { return _tFinal1; }
            set
            {
                _tFinal1 = value;
                RaisePropertyChanged(() => TFinal1);
                RaisePropertyChanged(() => TFinalAvg);
                RaisePropertyChanged(() => TUp1);
                RaisePropertyChanged(() => TUpAvg);
            }
        }

        /// <summary>
        /// 最终温度2（℃）
        /// </summary>
        private double _tFinal2 = 0;
        /// <summary>
        /// 最终温度2（℃）
        /// </summary>
        public double TFinal2
        {
            get { return _tFinal2; }
            set
            {
                _tFinal2 = value;
                RaisePropertyChanged(() => TFinal2);
                RaisePropertyChanged(() => TFinalAvg);
                RaisePropertyChanged(() => TUp2);
                RaisePropertyChanged(() => TUpAvg);
            }
        }

        /// <summary>
        /// 最终平均温度（℃）
        /// </summary>
        public double TFinalAvg
        {
            get { return (TFinal1 + TFinal2) / 2; }
        }

        /// <summary>
        /// 温升1（℃）
        /// </summary>
        public double TUp1
        {
            get { return (TMax1 - TFinal1); }
        }

        /// <summary>
        /// 温升2（℃）
        /// </summary>
        public double TUp2
        {
            get { return (TMax2 - TFinal2); }
        }

        /// <summary>
        /// 平均温升（℃）
        /// </summary>
        public double TUpAvg
        {
            get { return (TUp1 + TUp2) / 2; }
        }

        /// <summary>
        /// 最高温度1（℃）
        /// </summary>
        private double _tMax1 = 0;
        /// <summary>
        /// 最高温度1（℃）
        /// </summary>
        public double TMax1
        {
            get { return _tMax1; }
            set
            {
                _tMax1 = value;
                RaisePropertyChanged(() => TMax1);
                RaisePropertyChanged(() => TUp1);
                RaisePropertyChanged(() => TUpAvg);
            }
        }

        /// <summary>
        /// 最高温度2（℃）
        /// </summary>
        private double _tMax2 = 0;
        /// <summary>
        /// 最高温度2（℃）
        /// </summary>
        public double TMax2
        {
            get { return _tMax2; }
            set
            {
                _tMax2 = value;
                RaisePropertyChanged(() => TMax2);
                RaisePropertyChanged(() => TUp2);
                RaisePropertyChanged(() => TUpAvg);
            }
        }

        /// <summary>
        /// 试样中心最终温度（℃）
        /// </summary>
        private double _tscFinal = 0;
        /// <summary>
        /// 试样中心最终温度（℃）
        /// </summary>
        public double TscFinal
        {
            get { return _tscFinal; }
            set
            {
                _tscFinal = value;
                RaisePropertyChanged(() => TscFinal);
                RaisePropertyChanged(() => TscUp);
            }
        }

        /// <summary>
        /// 试样表面最终温度（℃）
        /// </summary>
        private double _tssFinal = 0;
        /// <summary>
        /// 试样表面最终温度（℃）
        /// </summary>
        public double TssFinal
        {
            get { return _tssFinal; }
            set
            {
                _tssFinal = value;
                RaisePropertyChanged(() => TssFinal);
                RaisePropertyChanged(() => TssUp);
            }
        }

        /// <summary>
        /// 试样中心最高温度（℃）
        /// </summary>
        private double _tscMax = 0;
        /// <summary>
        /// 试样中心最高温度（℃）
        /// </summary>
        public double TscMax
        {
            get { return _tscMax; }
            set
            {
                _tscMax = value;
                RaisePropertyChanged(() => TscMax);
                RaisePropertyChanged(() => TscUp);
            }
        }

        /// <summary>
        /// 试样表面最高温度（℃）
        /// </summary>
        private double _tssMax = 0;
        /// <summary>
        /// 试样表面最高温度（℃）
        /// </summary>
        public double TssMax
        {
            get { return _tssMax; }
            set
            {
                _tssMax = value;
                RaisePropertyChanged(() => TssMax);
                RaisePropertyChanged(() => TssUp);
            }
        }

        /// <summary>
        /// 试样中心温升（℃）
        /// </summary>
        public double TscUp
        {
            get { return TscMax - TscFinal; }
        }

        /// <summary>
        /// 试样表面温升（℃）
        /// </summary>
        public double TssUp
        {
            get { return TssMax - TssFinal; }
        }

        /// <summary>
        /// 火焰持续总时间(s，持续火焰超过5s的总时间)
        /// </summary>
        private int _fireTimeSum = 0;
        /// <summary>
        /// 火焰持续总时间(s，持续火焰超过5s的总时间)
        /// </summary>
        public int FireTimeSum
        {
            get { return _fireTimeSum; }
            set
            {
                _fireTimeSum = value;
                RaisePropertyChanged(() => FireTimeSum);
            }
        }

        /// <summary>
        /// 测试时长(min，正式试验开始后的时间)
        /// </summary>
        private double _timeTest = 0;
        /// <summary>
        /// 测试时长(min，正式试验开始后的时间)
        /// </summary>
        public double TimeTest
        {
            get { return _timeTest; }
            set
            {
                _timeTest = value;
                RaisePropertyChanged(() => TimeTest);
            }
        }

        /// <summary>
        /// 数据记录总列表
        /// </summary>
        private ObservableCollection<TestRec> _recList_All = new ObservableCollection<TestRec>() { };
        /// <summary>
        /// 数据记录总列表
        /// </summary>
        public ObservableCollection<TestRec> RecList_All
        {
            get { return _recList_All; }
            set
            {
                _recList_All = value;
                RaisePropertyChanged(() => RecList_All);
            }
        }

        /// <summary>
        /// 控温最后10分钟记录列表
        /// </summary>
        private ObservableCollection<TestRec> _recList_Stb = new ObservableCollection<TestRec>() { };
        /// <summary>
        /// 控温最后10分钟记录列表
        /// </summary>
        public ObservableCollection<TestRec> RecList_Stb
        {
            get { return _recList_Stb; }
            set
            {
                _recList_Stb = value;
                RaisePropertyChanged(() => RecList_Stb);
            }
        }

        /// <summary>
        /// 正式试验记录列表
        /// </summary>
        private ObservableCollection<TestRec> _recList_Test = new ObservableCollection<TestRec>() { };
        /// <summary>
        /// 正式试验记录列表
        /// </summary>
        public ObservableCollection<TestRec> RecList_Test
        {
            get { return _recList_Test; }
            set
            {
                _recList_Test = value;
                RaisePropertyChanged(() => RecList_Test);
            }
        }


        #endregion


        #region 运行参数

        /// <summary>
        /// 最后控温输出
        /// </summary>
        private double _voFinal = 0;
        /// <summary>
        ///  最后控温输出
        /// </summary>
        public double VoFinal
        {
            get { return _voFinal; }
            set
            {
                _voFinal = value;
                RaisePropertyChanged(() => VoFinal);
            }
        }

        /// <summary>
        /// 已复制数据
        /// </summary>
        private bool _copyed = false;
        /// <summary>
        ///  已复制数据
        /// </summary>
        public bool Copyed
        {
            get { return _copyed; }
            set
            {
                _copyed = value;
                RaisePropertyChanged(() => Copyed);
            }
        }

        /// <summary>
        /// 当前点序号
        /// </summary>
        private int _recPointNow = 0;
        /// <summary>
        ///  当前点序号
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
        
        /// <summary>
        /// 试样总体测试时间
        /// </summary>
        private Time _TimeSp = new Time();
        /// <summary>
        /// 试样总体测试时间
        /// </summary>
        public Time TimeSp
        {
            get { return _TimeSp; }
            set
            {
                _TimeSp = value;
                RaisePropertyChanged(() => _TimeSp);
            }
        }

        /// <summary>
        /// 初始温度平衡时间
        /// </summary>
        private Time _timeKeepT = new Time();
        /// <summary>
        /// 初始温度平衡时间
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
        /// 30min测试时间（从插入试样开始）
        /// </summary>
        private Time _timeTest30 = new Time();
        /// <summary>
        ///  30min测试时间（从插入试样开始）
        /// </summary>
        public Time TimeTest30
        {
            get { return _timeTest30; }
            set
            {
                _timeTest30 = value;
                RaisePropertyChanged(() => TimeTest30);
            }
        }

        /// <summary>
        /// 最终平衡判定时间上一次
        /// </summary>
        private DateTime _timeBefore_FinalEquEval = DateTime.MinValue;
        /// <summary>
        /// 最终平衡判定时间上一次
        /// </summary>
        public DateTime TimeBefore_FinalEquEval
        {
            get { return _timeBefore_FinalEquEval; }
            set
            {
                _timeBefore_FinalEquEval = value;
                RaisePropertyChanged(() => TimeBefore_FinalEquEval);
            }
        }

        /// <summary>
        /// 控温稳定判定时间上一次
        /// </summary>
        private DateTime _timeBefore_StabilizeEval = DateTime.MinValue;
        /// <summary>
        /// 控温稳定判定时间上一次
        /// </summary>
        public DateTime TimeBefore_StabilizeEval
        {
            get { return _timeBefore_StabilizeEval; }
            set
            {
                _timeBefore_StabilizeEval = value;
                RaisePropertyChanged(() => TimeBefore_StabilizeEval);
            }
        }

        /// <summary>
        /// 当前阶段
        /// </summary>
        private TestStage _stageDQ = TestStage.Wait_Stage;
        /// <summary>
        /// 当前阶段
        /// </summary>
        public TestStage StageDQ
        {
            get { return _stageDQ; }
            set
            {
                _stageDQ = value;
                RaisePropertyChanged(() => StageDQ);
            }
        }

        /// <summary>
        /// 10分钟的数据点数
        /// </summary>
        private int _pointsQty10Min = 600;
        /// <summary>
        /// 10分钟的数据点数
        /// </summary>
        public int PointsQty10Min
        {
            get { return _pointsQty10Min; }
            set
            {
                _pointsQty10Min = value;
                RaisePropertyChanged(() => PointsQty10Min);
            }
        }

        /// <summary>
        /// 1分钟的数据点数
        /// </summary>
        private int _pointsQtyPerMin = 60;
        /// <summary>
        /// 1分钟的数据点数
        /// </summary>
        public int PointsQtyPerMin
        {
            get { return _pointsQtyPerMin; }
            set
            {
                _pointsQtyPerMin = value;
                RaisePropertyChanged(() => PointsQtyPerMin);
            }
        }
        #endregion


        #region 计算

        /// <summary>
        /// 结果结算
        /// </summary>
        public void CalcExpData()
        {
            //判定完成后再计算，否则不计算
            if (IsCompleted)
            {
                if (RecList_Stb.Count > 0)
                {
                    List<double> t1List1 = new List<double>();  //炉内温度1稳定阶段列表
                    List<double> t2List1 = new List<double>();  //炉内温度2稳定阶段列表
                    for (int i = 0; i < RecList_Stb.Count; i++)
                    {
                        t1List1.Add(RecList_Stb[i].T1);
                        t2List1.Add(RecList_Stb[i].T2);
                    }

                    //初始温度
                    TStart1 = t1List1.Average();
                    TStart2 = t2List1.Average();
                }

                if (RecList_Test.Count > 0)
                {
                    List<double> t1List2 = new List<double>();  //炉内温度1三十分钟测试列表
                    List<double> t2List2 = new List<double>();  //炉内温度1三十分钟测试列表
                    List<double> tscList = new List<double>();  //试样中心温度30分钟测试列表
                    List<double> tssList = new List<double>();  //试样表面温度30分钟测试列表
                    for (int i = 0; i < RecList_Test.Count; i++)
                    {
                        t1List2.Add(RecList_Test[i].T1);
                        t2List2.Add(RecList_Test[i].T2);
                        tscList.Add(RecList_Test[i].Tsc);
                        tssList.Add(RecList_Test[i].Tss);
                    }

                    //检测时长
                    TimeSpan span = RecList_Test[RecList_Test.Count - 1].RecTime - RecList_Test[0].RecTime;
                    TimeTest = span.TotalMinutes;
                    //最高温度
                    TMax1 = t1List2.Max();
                    TMax2 = t2List2.Max();
                    TscMax = tscList.Max();
                    TssMax = tssList.Max();

                    //最终温度
                    while (t1List2.Count>PointsQtyPerMin)
                        t1List2.RemoveAt(0);
                    TFinal1 = t1List2.Average();
                    while (t2List2.Count > PointsQtyPerMin)
                        t2List2.RemoveAt(0);
                    TFinal2 = t2List2.Average();
                    while (tscList.Count > PointsQtyPerMin)
                        tscList.RemoveAt(0);
                    TscFinal = tscList.Average();
                    while (tssList.Count > PointsQtyPerMin )
                        tssList.RemoveAt(0);
                    TssFinal = tssList.Average();

                    //持续火焰总时长
                    int fireTimeSum = 0;
                    int fireTimeLong = 0;
                    bool isFiredBefor = false;
                    for (int i = 0; i < RecList_Test.Count; i++)
                    {
                        if (RecList_Test[i].IsFired)
                            fireTimeLong++;
                        else
                        {
                            if (isFiredBefor && (fireTimeLong >= 5))
                            {
                                fireTimeSum +=  fireTimeLong;
                            }
                            fireTimeLong = 0;
                        }

                        isFiredBefor = RecList_Test[i].IsFired;
                    }
                    FireTimeSum = fireTimeSum;
                }
            }
        }

        /// <summary>
        /// 形成控温最后10分钟记录
        /// </summary>
        public void CopyToRecListStb()
        {
            List<TestRec> recListStbAll = RecList_All.Copy().Where(x => x.Stage == TestStage.TCtl_Stage).ToList();
            while (recListStbAll.Count > PointsQty10Min)
                recListStbAll.RemoveAt(0);
            RecList_Stb = new ObservableCollection<TestRec>((IEnumerable<TestRec>)recListStbAll);
        }

        /// <summary>
        /// 形成正式测试记录
        /// </summary>
        public void CopyToRecListTest()
        {
            List<TestRec> recListTest = RecList_All.Copy().Where(x => x.Stage == TestStage.Test_Stage).ToList();
            List<TestRec> recListOK = RecList_All.Copy().Where(x => x.Stage == TestStage.TestEnd_Stage).ToList();
            if (recListOK.Count > 0)
                recListTest.Add(recListOK[0]);
            RecList_Test = new ObservableCollection<TestRec>((IEnumerable<TestRec>)recListTest);
        }

        #endregion


        #region 初始化、复位方法

        /// <summary>
        /// 测试试验数据重置
        /// </summary>
        public void DataReset()
        {
            IsCompleted = false;
            TStart1 = 0;
            TStart2 = 0;
            TFinal1 = 0;
            TFinal2 = 0;
            TMax1 = 0;
            TMax2 = 0;
            TscFinal = 0;
            TssFinal = 0;
            TscMax = 0;
            TssMax = 0;
            FireTimeSum = 0;
            TimeTest = 0;

            RecList_All.Clear();
            RecList_Stb.Clear();
            RecList_Test.Clear();

            VoFinal = 0;
            RecPointNow = 0;
            TimeSp.Reset();
            TimeKeepT.Reset();
            TimeTest30.Reset();
            TimeBefore_FinalEquEval = DateTime.MinValue;
            TimeBefore_StabilizeEval = DateTime.MinValue;
            StageDQ = TestStage.Wait_Stage;

            Copyed = false;
        }

        #endregion


        /// <summary>
        /// 程序计算周期变更消息处理（更改每分钟和10分钟的数据数量）
        /// </summary>
        /// <param name="msg">新建试验编号</param>
        private void PllPeriodChangedMessage(int period_BLL)
        {
            PointsQtyPerMin = 60 * 1000 / period_BLL;
            PointsQty10Min = 600 * 1000 / period_BLL;
        }
    }


    /// <summary>
    /// 试验记录类
    /// </summary>
    public class TestRec : ObservableObject
    {
        /// <summary>
        /// 已记录标志
        /// </summary>
        private bool _isRecorded = false;
        /// <summary>
        /// 已记录标志
        /// </summary>
        public bool IsRecorded
        {
            get { return _isRecorded; }
            set
            {
                _isRecorded = value;
                RaisePropertyChanged(() => IsRecorded);
            }
        }

        /// <summary>
        /// 记录序号
        /// </summary>
        private int _no = 1;
        /// <summary>
        /// 记录序号
        /// </summary>
        public int No
        {
            get { return _no; }
            set
            {
                _no = value;
                RaisePropertyChanged(() => No);
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
        /// 试验阶段
        /// </summary>
        private TestStage _stage = TestStage.Wait_Stage;
        /// <summary>
        /// 试验阶段
        /// </summary>
        public TestStage Stage
        {
            get { return _stage; }
            set
            {
                _stage = value;
                RaisePropertyChanged(() => Stage);
            }
        }

        /// <summary>
        /// 炉内温度1
        /// </summary>
        private double _t1 = 0;
        /// <summary>
        /// 炉内温度1
        /// </summary>
        public double T1
        {
            get { return _t1; }
            set
            {
                _t1 = value;
                RaisePropertyChanged(() => T1);
            }
        }

        /// <summary>
        /// 炉内温度2
        /// </summary>
        private double _t2 = 0;
        /// <summary>
        /// 炉内温度2
        /// </summary>
        public double T2
        {
            get { return _t2; }
            set
            {
                _t2 = value;
                RaisePropertyChanged(() => T2);
            }
        }

        /// <summary>
        /// 试样中心温度
        /// </summary>
        private double _tsc = 0;
        /// <summary>
        /// 试样中心温度
        /// </summary>
        public double Tsc
        {
            get { return _tsc; }
            set
            {
                _tsc = value;
                RaisePropertyChanged(() => Tsc);
            }
        }

        /// <summary>
        /// 试样表面温度
        /// </summary>
        private double _tss = 0;
        /// <summary>
        /// 试样表面温度
        /// </summary>
        public double Tss
        {
            get { return _tss; }
            set
            {
                _tss = value;
                RaisePropertyChanged(() => Tss);
            }
        }

        /// <summary>
        /// 输出电压百分比或输出功率
        /// </summary>
        private double _vo = 0;
        /// <summary>
        /// 输出电压百分比或输出功率
        /// </summary>
        public double Vo
        {
            get { return _vo; }
            set
            {
                _vo = value;
                RaisePropertyChanged(() => Vo);
            }
        }

        /// <summary>
        /// 有火焰标志
        /// </summary>
        private bool _isFired = false;
        /// <summary>
        /// 有火焰标志
        /// </summary>
        public bool IsFired
        {
            get { return _isFired; }
            set
            {
                _isFired = value;
                RaisePropertyChanged(() => IsFired);
            }
        }
    }
}
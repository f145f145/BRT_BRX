/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 单断面采集及数据Model
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-5-12 11:12:00		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using static BRX.Model.Enums.Enums;

namespace BRX.Model.Exp
{
    public class ExpModel_Test : ObservableObject
    {
        public ExpModel_Test()
        {
            //注册计算试验数据消息
            Messenger.Default.Register<string>(this, "CalcExpDataMessage", CalcExpDataMessage);

            SpDQ = SpList[0];
        }

        #region 试验基本信息

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
        /// 委托单位
        /// </summary>
        private string _wtdw = "//";
        /// <summary>
        /// 委托单位
        /// </summary>
        public string WTDW
        {
            get
            {
                return _wtdw;
            }
            set
            {
                _wtdw = value;
                RaisePropertyChanged(() => WTDW);
            }
        }

        /// <summary>
        /// 委托单位地址
        /// </summary>
        private string _wtdwdz = "//";
        /// <summary>
        /// 委托单位地址
        /// </summary>
        public string WTDWDZ
        {
            get
            {
                return _wtdwdz;
            }
            set
            {
                _wtdwdz = value;
                RaisePropertyChanged(() => WTDWDZ);
            }
        }

        /// <summary>
        /// 委托时间
        /// </summary>
        private DateTime _wtTime = DateTime.Now;
        /// <summary>
        /// 委托时间
        /// </summary>
        public DateTime WTTime
        {
            get
            {
                return _wtTime;
            }
            set
            {
                _wtTime = value;
                RaisePropertyChanged(() => WTTime);
            }
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        private string _xmmc = "//";
        /// <summary>
        /// 项目名称
        /// </summary>
        public string XMMC
        {
            get
            {
                return _xmmc;
            }
            set
            {
                _xmmc = value;
                RaisePropertyChanged(() => XMMC);
            }
        }

        /// <summary>
        /// 项目地址
        /// </summary>
        private string _xmdz = "//";
        /// <summary>
        /// 项目地址
        /// </summary>
        public string XMDZ
        {
            get
            {
                return _xmdz;
            }
            set
            {
                _xmdz = value;
                RaisePropertyChanged(() => XMDZ);
            }
        }

        /// <summary>
        /// 生产厂家
        /// </summary>
        private string _sccj = "//";
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string SCCJ
        {
            get
            {
                return _sccj;
            }
            set
            {
                _sccj = value;
                RaisePropertyChanged(() => SCCJ);
            }
        }

        /// <summary>
        /// 厂家厂址
        /// </summary>
        private string _cjcz = "//";
        /// <summary>
        /// 厂家厂址
        /// </summary>
        public string CJCZ
        {
            get
            {
                return _cjcz;
            }
            set
            {
                _cjcz = value;
                RaisePropertyChanged(() => CJCZ);
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


        #region 设定参数

        /// <summary>
        /// 使用附加热电偶
        /// </summary>
        private bool _useAddT = true;
        /// <summary>
        /// 使用附加热电偶
        /// </summary>
        public bool UseAddT
        {
            get { return _useAddT; }
            set
            {
                _useAddT = value;
                RaisePropertyChanged(() => UseAddT);
            }
        }

        #endregion


        #region 样品参数

        /// <summary>
        /// 试样列表
        /// </summary>
        private ObservableCollection<ExpModel_Sp> _spList = new ObservableCollection<ExpModel_Sp>()
        {
            new ExpModel_Sp(){SpNO=1},
            new ExpModel_Sp(){SpNO=2},
            new ExpModel_Sp(){SpNO=3},
            new ExpModel_Sp(){SpNO=4},
            new ExpModel_Sp(){SpNO=5}
        };
        /// <summary>
        /// 试样列表
        /// </summary>
        public ObservableCollection<ExpModel_Sp> SpList
        {
            get { return _spList; }
            set
            {
                _spList = value;
                RaisePropertyChanged(() => SpList);
            }
        }


        /// <summary>
        /// 样品编号
        /// </summary>
        private string _ypNO = "A0001";
        /// <summary>
        /// 样品编号
        /// </summary>
        public string YPNO
        {
            get
            {
                return _ypNO;
            }
            set
            {
                _ypNO = value;
                RaisePropertyChanged(() => YPNO);
            }
        }


        /// <summary>
        /// 样品名称
        /// </summary>
        private string _ypmc = "岩棉板";
        /// <summary>
        /// 样品名称
        /// </summary>
        public string YPMC
        {
            get
            {
                return _ypmc;
            }
            set
            {
                _ypmc = value;
                RaisePropertyChanged(() => YPMC);
            }
        }


        /// <summary>
        /// 样品数量
        /// </summary>
        private int _ypsl = 5;
        /// <summary>
        /// 样品数量
        /// </summary>
        public int YPSL
        {
            get { return _ypsl; }
            set
            {
                _ypsl = value;
                RaisePropertyChanged(() => YPSL);
            }
        }

        /// <summary>
        /// 到样日期
        /// </summary>
        private DateTime _dyrq = DateTime.Now;
        /// <summary>
        /// 报告日期时间
        /// </summary>
        public DateTime DYRQ
        {
            get
            {
                return _dyrq;
            }
            set
            {
                _dyrq = value;
                RaisePropertyChanged(() => DYRQ);
            }
        }

        /// <summary>
        /// 制品标识
        /// </summary>
        private string _zpbs = "//";
        /// <summary>
        /// 制品标识
        /// </summary>
        public string ZPBS
        {
            get
            {
                return _zpbs;
            }
            set
            {
                _zpbs = value;
                RaisePropertyChanged(() => ZPBS);
            }
        }

        /// <summary>
        /// 抽样程序说明
        /// </summary>
        private string _cycx = "//";
        /// <summary>
        /// 抽样程序说明
        /// </summary>
        public string CYCX
        {
            get
            {
                return _cycx;
            }
            set
            {
                _cycx = value;
                RaisePropertyChanged(() => CYCX);
            }
        }

        /// <summary>
        /// 状态调节说明
        /// </summary>
        private string _zttj = "//";
        /// <summary>
        /// 状态调节说明
        /// </summary>
        public string ZTTJ
        {
            get
            {
                return _zttj;
            }
            set
            {
                _zttj = value;
                RaisePropertyChanged(() => ZTTJ);
            }
        }

        /// <summary>
        /// 密度
        /// </summary>
        private double _md = 0;
        /// <summary>
        /// 密度
        /// </summary>
        public double MD
        {
            get
            {
                return _md;
            }
            set
            {
                _md = value;
                RaisePropertyChanged(() => MD);
            }
        }

        /// <summary>
        /// 面密度
        /// </summary>
        private double _mmd = 0;
        /// <summary>
        /// 面密度
        /// </summary>
        public double MMD
        {
            get
            {
                return _mmd;
            }
            set
            {
                _mmd = value;
                RaisePropertyChanged(() => MMD);
            }
        }

        /// <summary>
        /// 厚度
        /// </summary>
        private double _hd = 0;
        /// <summary>
        /// 厚度
        /// </summary>
        public double HD
        {
            get
            {
                return _hd;
            }
            set
            {
                _hd = value;
                RaisePropertyChanged(() => HD);
            }
        }

        /// <summary>
        /// 结构信息
        /// </summary>
        private string _jgxx = "//";
        /// <summary>
        /// 结构信息
        /// </summary>
        public string JGXX
        {
            get
            {
                return _jgxx;
            }
            set
            {
                _jgxx = value;
                RaisePropertyChanged(() => JGXX);
            }
        }

        #endregion


        #region 运行参数

        /// <summary>
        /// 已全部完成标志
        /// </summary>
        private bool _isCompleted = false;
        /// <summary>
        /// 已全部完成标志
        /// </summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                RaisePropertyChanged(() => IsCompleted);
                RaisePropertyChanged(() => QtyComplete);
            }
        }

        /// <summary>
        /// 当前试样序号
        /// </summary>
        private int _spNoDQ = 1;
        /// <summary>
        /// 当前试样序号
        /// </summary>
        public int SpNoDQ
        {
            get { return _spNoDQ; }
            set
            {
                _spNoDQ = value;
                RaisePropertyChanged(() => SpNoDQ);
            }
        }
        
        /// <summary>
        /// 当前试样
        /// </summary>
        private ExpModel_Sp _spDQ = new ExpModel_Sp();
        /// <summary>
        /// 当前试样
        /// </summary>
        public ExpModel_Sp SpDQ
        {
            get { return _spDQ; }
            set
            {
                _spDQ = value;
                RaisePropertyChanged(() => SpDQ);
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
                for (int i = 0; i<SpList.Count; i++)
                {
                    if (SpList[i].IsCompleted)
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
            get { return YPSL- QtyComplete; }
        }

        #endregion


        #region 计算

        /// <summary>
        /// 结果结算
        /// </summary>
        public void CalcExpData()
        {
            bool isAllComplete = true;
            for (int i = 0; i < SpList.Count; i++)
            {
                if (!SpList[i].IsCompleted)
                    isAllComplete = false;

                SpList[i].CalcExpData();
            }

            IsCompleted = isAllComplete;

            RaisePropertyChanged(() => QtyComplete);
        }


        /// <summary>
        /// 计算试验数据消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void CalcExpDataMessage(string msg)
        {
            string order = msg.Clone().ToString();
            if (order == "ExpBRXData")
            {
                CalcExpData();
          //      Messenger.Default.Send<string>("UpdateSpWSPlotMessage", "UpdateSpWSPlotMessage");
            }
        }


        #endregion


        #region 初始化、复位方法

        /// <summary>
        /// 测试试验数据重置
        /// </summary>
        public void DataReset()
        {
            IsCompleted = false;
        }

        #endregion


        /// <summary>
        /// 阶段转化
        /// </summary>
        public string Converter<T>(TestStage stage)
        {
            if ((TestStage)stage == TestStage.TCtl_Stage)
                return "温度准备";
            if ((TestStage)stage == TestStage.Test_Stage)
                return "30min测试";
            if ((TestStage)stage == TestStage.TestEnd_Stage)
                return "测试完成";

            return "待机阶段";
        }


        /// <summary>
        /// 阶段反向转化
        /// </summary>
        public TestStage Converter<T>( string expStage)
        {
            if ((string)expStage == "温度准备")
                return TestStage.TCtl_Stage;
            if ((string)expStage == "30min测试")
                return TestStage.Test_Stage;
            if ((string)expStage == "测试完成")
                return TestStage.TestEnd_Stage;

            return TestStage.Wait_Stage;
        }
    }
}
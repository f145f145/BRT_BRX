/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 试验读写。主体部分。
 * ==================================================================================
 * 修改标记
 * 修改时间				修改人			版本号			描述
 * 2022/3/22 23:14:24		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using BRX.Model.Dev;
using BRX.Model.Exp;
using BRX.DBDataSetTableAdapters;
using SqlSugar;
using System.Linq;
using BRX.DAL.ExpDAL.ExpDALModel;
using BRX.Model.Enums;
using BRX.View;
using NPOI.Util;
using static BRX.Model.Enums.Enums;
using System.Collections.Generic;

namespace BRX.DAL.ExpDAL
{
    public partial class ExpDAL:ObservableObject 
    {
        public ExpDAL(DevModel dev, ExpModel_Test exp)
        {
            ExpBRXDQ = exp;
            Dev = dev;

            //Dataset、DataTable、TableAdapter初始化
            SetTableAdapterInit_Exp();
            Messenger.Default.Send(A10Table, "ExpBRXTableChanged");


            //需要更新试验列表消息
            Messenger.Default.Register<string>(this, "UpdateExpListMessage", UpdateExpListMessage);

            //新建测试试验消息
            Messenger.Default.Register<string>(this, "NewExpBRXMessage", NewExpBRXMessage);
            //复制测试试验消息
            Messenger.Default.Register<string[]>(this, "CopyExpBRXMessage", CopyExpBRXMessage);
            //删除测试试验消息
            Messenger.Default.Register<string>(this, "DelExpBRXMessage", DelExpBRXMessage);
            //加载测试试验消息
            Messenger.Default.Register<string>(this, "LoadExpBRXMessage", LoadExpBRXMessage);
            //保存测试试验消息
            Messenger.Default.Register<string>(this, "SaveExpBRXMessage", SaveExpBRXMessage);
            //保存测试原始数据记录（单条）
            Messenger.Default.Register<TestRec>(this, "SaveExpInitRecMessage", SaveExpInitRecMessage);
            //清空指定试样编号的原始测试数据记录
            Messenger.Default.Register<int>(this, "SpInitRecClearMessage", SpInitRecClearMessage);

            //保存辨识数据记录（单条）
            Messenger.Default.Register<IDRec>(this, "SaveIDRecMessage", SaveIDRecMessage);
            creatTable("Persent20");
        }


        #region dev、EXP属性

        /// <summary>
        /// 装置
        /// </summary>
        private DevModel _dev;
        /// <summary>
        /// 装置
        /// </summary>
        public DevModel Dev
        {
            get { return _dev; }
            set
            {
                _dev = value;
                RaisePropertyChanged(() => Dev);
            }
        }

        /// <summary>
        /// 当前测试试验
        /// </summary>
        private ExpModel_Test _expBRXDQ;
        /// <summary>
        /// 当前测试试验
        /// </summary>
        public ExpModel_Test ExpBRXDQ
        {
            get { return _expBRXDQ; }
            set
            {
                _expBRXDQ = value;
                RaisePropertyChanged(() => ExpBRXDQ);
            }
        }

        #endregion


        #region DataTable属性

        /// <summary>
        /// A10检测试验参数DataTable
        /// </summary>
        private BRX.DBDataSet.A10检测试验参数DataTable _a10Table;

        /// <summary>
        /// A10检试验参数DataTable
        /// </summary>
        private BRX.DBDataSet.A10检测试验参数DataTable A10Table
        {
            get { return _a10Table; }
            set
            {
                _a10Table = value;
                RaisePropertyChanged(() => A10Table);
            }
        }
        
        /// <summary>
        /// B11试样1检测数据DataTable
        /// </summary>
        private BRX.DBDataSet.B11试样1检测数据DataTable _b11Table;

        /// <summary>
        /// B11试样1检测数据DataTable
        /// </summary>
        private BRX.DBDataSet.B11试样1检测数据DataTable B11Table
        {
            get { return _b11Table; }
            set
            {
                _b11Table = value;
                RaisePropertyChanged(() => B11Table);
            }
        }

        /// <summary>
        /// B12试样2检测数据DataTable
        /// </summary>
        private BRX.DBDataSet.B12试样2检测数据DataTable _b12Table;

        /// <summary>
        /// B12试样2检测数据DataTable
        /// </summary>
        private BRX.DBDataSet.B12试样2检测数据DataTable B12Table
        {
            get { return _b12Table; }
            set
            {
                _b12Table = value;
                RaisePropertyChanged(() => B12Table);
            }
        }

        /// <summary>
        /// B13试样3检测数据DataTable
        /// </summary>
        private BRX.DBDataSet.B13试样3检测数据DataTable _b13Table;

        /// <summary>
        /// B13试样3检测数据DataTable
        /// </summary>
        private BRX.DBDataSet.B13试样3检测数据DataTable B13Table
        {
            get { return _b13Table; }
            set
            {
                _b13Table = value;
                RaisePropertyChanged(() => B13Table);
            }
        }

        /// <summary>
        /// B14试样4检测数据DataTable
        /// </summary>
        private BRX.DBDataSet.B14试样4检测数据DataTable _b14Table;

        /// <summary>
        /// B14试样4检测数据DataTable
        /// </summary>
        private BRX.DBDataSet.B14试样4检测数据DataTable B14Table
        {
            get { return _b14Table; }
            set
            {
                _b14Table = value;
                RaisePropertyChanged(() => B14Table);
            }
        }

        /// <summary>
        /// B15试样5检测数据DataTable
        /// </summary>
        private BRX.DBDataSet.B15试样5检测数据DataTable _b15Table;

        /// <summary>
        /// B15试样5检测数据DataTable
        /// </summary>
        private BRX.DBDataSet.B15试样5检测数据DataTable B15Table
        {
            get { return _b15Table; }
            set
            {
                _b15Table = value;
                RaisePropertyChanged(() => B15Table);
            }
        }
        
        #endregion


        #region TableAdapter属性

        /// <summary>
        /// A10检测试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A10检测试验参数TableAdapter _a10TableAdapter;

        /// <summary>
        /// A10检测试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A10检测试验参数TableAdapter A10TableAdapter
        {
            get { return _a10TableAdapter; }
            set
            {
                _a10TableAdapter = value;
                RaisePropertyChanged(() => A10TableAdapter);
            }
        }

        /// <summary>
        /// B11试样1检测数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B11试样1检测数据TableAdapter _b11TableAdapter;

        /// <summary>
        /// B11试样1检测数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B11试样1检测数据TableAdapter B11TableAdapter
        {
            get { return _b11TableAdapter; }
            set
            {
                _b11TableAdapter = value;
                RaisePropertyChanged(() => B11TableAdapter);
            }
        }

        /// <summary>
        /// B12试样2检测数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B12试样2检测数据TableAdapter _b12TableAdapter;

        /// <summary>
        /// B12试样2检测数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B12试样2检测数据TableAdapter B12TableAdapter
        {
            get { return _b12TableAdapter; }
            set
            {
                _b12TableAdapter = value;
                RaisePropertyChanged(() => B12TableAdapter);
            }
        }

        /// <summary>
        /// B13试样3检测数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B13试样3检测数据TableAdapter _b13TableAdapter;

        /// <summary>
        /// B13试样3检测数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B13试样3检测数据TableAdapter B13TableAdapter
        {
            get { return _b13TableAdapter; }
            set
            {
                _b13TableAdapter = value;
                RaisePropertyChanged(() => B13TableAdapter);
            }
        }

        /// <summary>
        /// B14试样4检测数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B14试样4检测数据TableAdapter _b14TableAdapter;

        /// <summary>
        /// B14试样4检测数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B14试样4检测数据TableAdapter B14TableAdapter
        {
            get { return _b14TableAdapter; }
            set
            {
                _b14TableAdapter = value;
                RaisePropertyChanged(() => B14TableAdapter);
            }
        }

        /// <summary>
        /// B15试样5检测数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B15试样5检测数据TableAdapter _b15TableAdapter;

        /// <summary>
        /// B15试样5检测数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B15试样5检测数据TableAdapter B15TableAdapter
        {
            get { return _b15TableAdapter; }
            set
            {
                _b15TableAdapter = value;
                RaisePropertyChanged(() => B15TableAdapter);
            }
        }
        
        #endregion


        #region 原始数据DB属性
        
        /// <summary>
        /// 试验原始数据记录数据库
        /// </summary>
        private SqlSugarClient InitDB;
        
        #endregion


        #region DataTable、TableAdapter初始化、更新

        /// <summary>
        /// Dataset、DataTable、TableAdapter初始化、读取数据
        /// </summary>
        private void SetTableAdapterInit_Exp()
        {
            //TableAdapter初始化
            A10TableAdapter = new A10检测试验参数TableAdapter();
           B11TableAdapter = new B11试样1检测数据TableAdapter();
            B12TableAdapter = new B12试样2检测数据TableAdapter();
            B13TableAdapter = new B13试样3检测数据TableAdapter();
            B14TableAdapter = new B14试样4检测数据TableAdapter();
            B15TableAdapter = new B15试样5检测数据TableAdapter();
           
            //DataTable初始化
            A10Table = new DBDataSet.A10检测试验参数DataTable();
            B11Table = new DBDataSet.B11试样1检测数据DataTable();
            B12Table = new DBDataSet.B12试样2检测数据DataTable();
            B13Table = new DBDataSet.B13试样3检测数据DataTable();
            B14Table = new DBDataSet.B14试样4检测数据DataTable();
            B15Table = new DBDataSet.B15试样5检测数据DataTable();
           
            //数据读取
            A10TableAdapter.Fill(A10Table);
            B11TableAdapter.Fill(B11Table);
            B12TableAdapter.Fill(B12Table);
            B13TableAdapter.Fill(B13Table);
            B14TableAdapter.Fill(B14Table);
            B15TableAdapter.Fill(B15Table);
           

            //原始数据创建连接
            InitDB = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.ConnString_Init,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
            });
            //创建sql打印输出，调试用
            InitDB.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql);//输出sql
                Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数

                //5.0.8.2 获取无参数化 SQL 
                //UtilMethods.GetSqlString(DbType.SqlServer,sql,pars)
            };
            //查询原始记录表是否存在，若不存在则新建
            for (int i = 0; i<A10Table.Rows.Count; i++)
            {
                string tableName = A10Table[i].试验编号.Copy();
                bool isExists = InitDB.DbMaintenance.IsAnyTable(tableName, false);
                if (!isExists)
                    InitDB.CodeFirst.As<InitRec>(tableName).InitTables<InitRec>();
            }
        }

        #endregion


        #region 试验管理、操作消息回调

        /// <summary>
        /// 根据消息更新试验列表
        /// </summary>
        /// <param name="msg">新建试验编号</param>
        private void UpdateExpListMessage(string msg)
        {
            Messenger.Default.Send(A10Table, "ExpBRXTableChanged");
        }


        /// <summary>
        /// 根据指定编号新建试验（从默认试验复制）
        /// </summary>
        /// <param name="msg">新建试验编号</param>
        private void NewExpBRXMessage(string msg)
        {
            string oldExpName = "DefaultExp";
            string newExpName = msg;
            if (CopyBRX(oldExpName, newExpName, true))
            {
                Messenger.Default.Send<BRX.DBDataSet.A10检测试验参数DataTable>(A10Table, "ExpBRXTableChanged");
                MessageBox.Show(newExpName+"号试验已新建完毕！", "提示");
            }
            else
            {
                MessageBox.Show("新建试验失败，数据库中可能残存信息！", "错误提示");
            }
            Messenger.Default.Send(A10Table, "ExpBRXTableChanged");
        }

        /// <summary>
        /// 复制指定名称的试验至新试验
        /// </summary>
        /// <param name="msg">被复制试验编号和新试验编号</param>
        private void CopyExpBRXMessage(string[] msg)
        {
            string oldExpName = msg[0];
            string newExpName = msg[1];
            if (CopyBRX(oldExpName, newExpName,false))
            {
                Messenger.Default.Send<BRX.DBDataSet.A10检测试验参数DataTable>(A10Table, "ExpBRXTableChanged");
                MessageBox.Show("已复制" + oldExpName + "号试验至" + newExpName + "号试验。", "提示");
            }
            else
            {
                MessageBox.Show("复制试验失败，数据库中可能残存信息！", "错误提示");
            }
        }

        /// <summary>
        ///根据编号删除试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void DelExpBRXMessage(string msg)
        {
            if (DelBRX(msg))
            {
                Messenger.Default.Send<BRX.DBDataSet.A10检测试验参数DataTable>(A10Table, "ExpBRXTableChanged");
                MessageBox.Show(msg + "号试验已删除！", "提示");
            }
            else
            {
                MessageBox.Show("删除试验失败！", "错误提示");
            }
        }

        /// <summary>
        ///根据编号载入试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void LoadExpBRXMessage(string msg)
        {
            LoadBRX(msg);
            for (int i = 0; i < ExpBRXDQ.SpList.Count; i++)
            {
                ExpBRXDQ.SpList[i].CopyToRecListStb();
                ExpBRXDQ.SpList[i].CopyToRecListTest();
            }
        }

        /// <summary>
        ///保存试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void SaveExpBRXMessage(string msg)
        {
            string str = Convert.ToString(msg);
            if (str == "SaveExpBRX")
            {
                SaveA10();
                SaveB11();
                SaveB12();
                SaveB13();
                SaveB14();
                SaveB15();
                A10TableAdapter.Fill(A10Table);

                Messenger.Default.Send(A10Table, "ExpBRXTableChanged");
            }
            if (str == "SaveDataExpBRX")
            {
                SaveA10();
                SaveB11();
                SaveB12();
                SaveB13();
                SaveB14();
                SaveB15();
            }
        }


        /// <summary>
        /// 保存一条试验检测原始记录
        /// </summary>
        /// <param name="rec"></param>
        private void SaveExpInitRecMessage(TestRec rec)
        {
            SaveInitData(rec);
        }
        

        /// <summary>
        /// 清空当前试验的某个试样的测试原始数据记录
        /// </summary>
        /// <param name="spNo">试样编号</param>
        private void SpInitRecClearMessage(int spNo)
        {
            SpRecReset(spNo);
        }

        #endregion

        
        #region 系统辨识相关

        /// <summary>
        /// 辨识记录数据库
        /// </summary>
        private SqlSugarClient IDDB;

        /// <summary>
        /// 保存一条辨识原始记录
        /// </summary>
        /// <param name="rec"></param>
        private void SaveIDRecMessage(IDRec rec)
        {
            SaveIDData(rec,"Persent20");
        }


        /// <summary>
        /// 保存系统辨识记录
        /// </summary>
        private void SaveIDData(IDRec rec,string tableName)
        {
            try
            {
                IDRec recWillSave = new IDRec
                {
                    RecNum = rec.RecNum,
                    RecTime = rec.RecTime,
                    T1 = rec.T1,
                    T2 = rec.T2,
                    Vo = rec.Vo,
                    Vo_V = rec.Vo_V,
                    UseOut = rec.UseOut,
                    Detail = (string)rec.Detail.Clone()
                };

                List<IDRec> tempRecList = IDDB.Queryable<IDRec>().AS(tableName).Where(it => it.RecNum == recWillSave.RecNum).ToList();
                if (tempRecList.Count > 0)
                    IDDB.Deleteable<IDRec>().AS(tableName).Where(it => it.RecNum == recWillSave.RecNum).ExecuteCommand();
                IDDB.Insertable<IDRec>(recWillSave).AS(tableName).ExecuteCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 创建辨识数据表
        /// </summary>
        /// <param name="tableName"></param>
        private void creatTable(string tableName)
        {
            //原始数据创建连接
            IDDB = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.ConnString_ID,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
            });
            //创建sql打印输出，调试用
            IDDB.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql);//输出sql
                Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数
            };
           try
            {
                //创建新表
                bool isExists = IDDB.DbMaintenance.IsAnyTable(tableName, false);
                if (isExists)
                    IDDB.DbMaintenance.DropTable(tableName);
                IDDB.CodeFirst.As<IDRec>(tableName).InitTables<IDRec>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
    }
}

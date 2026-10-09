/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/22 23:14:24
 * 描述：
 * 炉壁温度校准原始数据记录表Model（sqlite—sqlsugar）
 * ==================================================================================
 * 修改标记
 * 修改时间				修改人			版本号			描述
 * 2022/11/12 23:14:24		郝正强			V1.0.0.0
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
using BRX.DAL.CalDAL.CalDALModel;

namespace BRX.DAL.CalDAL
{
    public partial class CalDAL : ObservableObject 
    {
        public CalDAL(DevModel dev, ExpModel_CalWall wallCal, ExpModel_CalCenter centerCal)
        {
            Dev = dev;
            WallCalDQ = wallCal;
            CenterCalDQ = centerCal;

            //Dataset、DataTable、TableAdapter初始化
            SetTableAdapterInit_Exp();
            Messenger.Default.Send(A20Table, "WallCalTableChanged");
            Messenger.Default.Send(A30Table, "CenterCalTableChanged");

            //新建校准试验消息
            Messenger.Default.Register<string>(this, "NewWallCalMessage", NewWallCalMessage);
            Messenger.Default.Register<string>(this, "NewCenterCalMessage", NewCenterCalMessage);
            //复制校准试验消息
            Messenger.Default.Register<string[]>(this, "CopyWallCalMessage", CopyWallCalMessage);
            Messenger.Default.Register<string[]>(this, "CopyCenterCalMessage", CopyCenterCalMessage);
            //删除校准试验消息
            Messenger.Default.Register<string>(this, "DelWallCalMessage", DelWallCalMessage);
            Messenger.Default.Register<string>(this, "DelCenterCalMessage", DelCenterCalMessage);
            //加载校准试验消息
            Messenger.Default.Register<string>(this, "LoadWallCalMessage", LoadWallCalMessage);
            Messenger.Default.Register<string>(this, "LoadCenterCalMessage", LoadCenterCalMessage);
            //保存校准试验消息
            Messenger.Default.Register<string>(this, "SaveWallCalMessage", SaveWallCalMessage);
            Messenger.Default.Register<string>(this, "SaveCenterCalMessage", SaveCenterCalMessage);

            //保存校准原始数据记录（单条）
            Messenger.Default.Register<WallCalInitRec>(this, "SaveWallCalInitRecMessage", SaveWallCalInitRecMessage);            //保存校准原始数据记录（单条）
            Messenger.Default.Register<CenterCalInitRec>(this, "SaveCenterCalInitRecMessage", SaveCenterCalInitRecMessage);
            //清空指定高度编号的原始测试数据记录
            Messenger.Default.Register<int>(this, "WallCalInitRecClearMessage", WallCalInitRecClearMessage);
            Messenger.Default.Register<int>(this, "CenterCalInitRecClearMessage", CenterCalInitRecClearMessage);
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
        /// 当前炉壁校准试验
        /// </summary>
        private ExpModel_CalWall _wallCalDQ;
        /// <summary>
        /// 当前炉壁校准试验
        /// </summary>
        public ExpModel_CalWall WallCalDQ
        {
            get { return _wallCalDQ; }
            set
            {
                _wallCalDQ = value;
                RaisePropertyChanged(() => WallCalDQ);
            }
        }

        /// <summary>
        /// 当前炉内校准试验
        /// </summary>
        private ExpModel_CalCenter _centerCalDQ;
        /// <summary>
        /// 当前炉内校准试验
        /// </summary>
        public ExpModel_CalCenter CenterCalDQ
        {
            get { return _centerCalDQ; }
            set
            {
                _centerCalDQ = value;
                RaisePropertyChanged(() => CenterCalDQ);
            }
        }

        #endregion


        #region DataTable属性
        
        /// <summary>
        /// A20炉壁温度校准试验参数DataTable
        /// </summary>
        private BRX.DBDataSet.A20炉壁温度校准试验参数DataTable _a20Table;

        /// <summary>
        /// A20炉壁温度校准试验参数DataTable
        /// </summary>
        private BRX.DBDataSet.A20炉壁温度校准试验参数DataTable A20Table
        {
            get { return _a20Table; }
            set
            {
                _a20Table = value;
                RaisePropertyChanged(() => A20Table);
            }
        }

        /// <summary>
        /// A30炉内温度校准试验参数DataTable
        /// </summary>
        private BRX.DBDataSet.A30炉内温度校准试验参数DataTable _a30Table;

        /// <summary>
        /// A30炉内温度校准试验参数DataTable
        /// </summary>
        private BRX.DBDataSet.A30炉内温度校准试验参数DataTable A30Table
        {
            get { return _a30Table; }
            set
            {
                _a30Table = value;
                RaisePropertyChanged(() => A30Table);
            }
        }
        
        /// <summary>
        /// B20炉壁温度校准数据DataTable
        /// </summary>
        private BRX.DBDataSet.B20炉壁温度校准数据DataTable _b20Table;

        /// <summary>
        /// B20炉壁温度校准数据DataTable
        /// </summary>
        private BRX.DBDataSet.B20炉壁温度校准数据DataTable B20Table
        {
            get { return _b20Table; }
            set
            {
                _b20Table = value;
                RaisePropertyChanged(() => B20Table);
            }
        }

        /// <summary>
        /// B30炉内温度校准数据DataTable
        /// </summary>
        private BRX.DBDataSet.B30炉内温度校准数据DataTable _b30Table;

        /// <summary>
        /// B30炉内温度校准数据DataTable
        /// </summary>
        private BRX.DBDataSet.B30炉内温度校准数据DataTable B30Table
        {
            get { return _b30Table; }
            set
            {
                _b30Table = value;
                RaisePropertyChanged(() => B30Table);
            }
        }

        /// <summary>
        /// B31炉内温度校准数据2DataTable
        /// </summary>
        private BRX.DBDataSet.B31炉内温度校准数据2DataTable _b31Table;

        /// <summary>
        /// B31炉内温度校准数据2DataTable
        /// </summary>
        private BRX.DBDataSet.B31炉内温度校准数据2DataTable B31Table
        {
            get { return _b31Table; }
            set
            {
                _b31Table = value;
                RaisePropertyChanged(() => B31Table);
            }
        }
        #endregion


        #region TableAdapter属性

        /// <summary>
        /// A20炉壁温度校准试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A20炉壁温度校准试验参数TableAdapter _a20TableAdapter;

        /// <summary>
        /// A20炉壁温度校准试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A20炉壁温度校准试验参数TableAdapter A20TableAdapter
        {
            get { return _a20TableAdapter; }
            set
            {
                _a20TableAdapter = value;
                RaisePropertyChanged(() => A20TableAdapter);
            }
        }

        /// <summary>
        /// A30炉内温度校准试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A30炉内温度校准试验参数TableAdapter _a30TableAdapter;

        /// <summary>
        /// A30炉内温度校准试验参数TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.A30炉内温度校准试验参数TableAdapter A30TableAdapter
        {
            get { return _a30TableAdapter; }
            set
            {
                _a30TableAdapter = value;
                RaisePropertyChanged(() => A30TableAdapter);
            }
        }

        /// <summary>
        /// B20炉壁温度校准数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B20炉壁温度校准数据TableAdapter _b20TableAdapter;

        /// <summary>
        /// B20炉壁温度校准数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B20炉壁温度校准数据TableAdapter B20TableAdapter
        {
            get { return _b20TableAdapter; }
            set
            {
                _b20TableAdapter = value;
                RaisePropertyChanged(() => B20TableAdapter);
            }
        }

        /// <summary>
        /// B30炉内温度校准数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B30炉内温度校准数据TableAdapter _b30TableAdapter;

        /// <summary>
        /// B30炉内温度校准数据TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B30炉内温度校准数据TableAdapter B30TableAdapter
        {
            get { return _b30TableAdapter; }
            set
            {
                _b30TableAdapter = value;
                RaisePropertyChanged(() => B30TableAdapter);
            }
        }

        /// <summary>
        /// B31炉内温度校准数据2TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B31炉内温度校准数据2TableAdapter _b31TableAdapter;

        /// <summary>
        /// B31炉内温度校准数据2TableAdapter
        /// </summary>
        private BRX.DBDataSetTableAdapters.B31炉内温度校准数据2TableAdapter B31TableAdapter
        {
            get { return _b31TableAdapter; }
            set
            {
                _b31TableAdapter = value;
                RaisePropertyChanged(() => B31TableAdapter);
            }
        }

        #endregion


        #region 原始数据DB属性

        /// <summary>
        /// 炉壁校准原始数据记录数据库
        /// </summary>
        private SqlSugarClient WallCalInitDB;

        /// <summary>
        /// 炉内校准原始数据记录数据库
        /// </summary>
        private SqlSugarClient CenterCalInitDB;

        #endregion


        #region DataTable、TableAdapter初始化、更新

        /// <summary>
        /// Dataset、DataTable、TableAdapter初始化、读取数据
        /// </summary>
        private void SetTableAdapterInit_Exp()
        {
            //TableAdapter初始化
            A20TableAdapter = new A20炉壁温度校准试验参数TableAdapter();
            A30TableAdapter = new A30炉内温度校准试验参数TableAdapter();
            B20TableAdapter = new B20炉壁温度校准数据TableAdapter();
            B30TableAdapter = new B30炉内温度校准数据TableAdapter();
            B31TableAdapter = new B31炉内温度校准数据2TableAdapter();

            //DataTable初始化
            A20Table = new DBDataSet.A20炉壁温度校准试验参数DataTable();
            A30Table = new DBDataSet.A30炉内温度校准试验参数DataTable();
            B20Table = new DBDataSet.B20炉壁温度校准数据DataTable();
            B30Table = new DBDataSet.B30炉内温度校准数据DataTable();
            B31Table = new DBDataSet.B31炉内温度校准数据2DataTable();

            //数据读取
            A20TableAdapter.Fill(A20Table);
            A30TableAdapter.Fill(A30Table);
            B20TableAdapter.Fill(B20Table);
            B30TableAdapter.Fill(B30Table);
            B31TableAdapter.Fill(B31Table);

            //炉壁校准原始数据创建连接
            WallCalInitDB = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.ConnString_WallCalInit,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
            });
            //创建sql打印输出，调试用
            WallCalInitDB.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql);//输出sql
                Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数

                //5.0.8.2 获取无参数化 SQL 
                //UtilMethods.GetSqlString(DbType.SqlServer,sql,pars)
            };
            //查询原始记录表是否存在，若不存在则新建
            for (int i = 0; i < A20Table.Rows.Count; i++)
            {
                string tableName = A20Table[i].试验编号.Clone().ToString();
                bool isExists = WallCalInitDB.DbMaintenance.IsAnyTable(tableName, false);
                if (!isExists)
                    WallCalInitDB.CodeFirst.As<WallCalInitRec>(tableName).InitTables<WallCalInitRec>();
            }


            //炉内校准原始数据创建连接
            CenterCalInitDB = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = Config.ConnString_CenterCalInit,
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
            });
            //创建sql打印输出，调试用
            CenterCalInitDB.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql);//输出sql
                Console.WriteLine(string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value)));//参数

                //5.0.8.2 获取无参数化 SQL 
                //UtilMethods.GetSqlString(DbType.SqlServer,sql,pars)
            };
            //查询原始记录表是否存在，若不存在则新建
            for (int i = 0; i < A30Table.Rows.Count; i++)
            {
                string tableName = A30Table[i].试验编号.Clone().ToString();
                bool isExists = CenterCalInitDB.DbMaintenance.IsAnyTable(tableName, false);
                if (!isExists)
                    CenterCalInitDB.CodeFirst.As<CenterCalInitRec>(tableName).InitTables<CenterCalInitRec>();
            }
        }

        #endregion


        #region 炉壁校准试验管理、操作消息回调

        /// <summary>
        /// 根据指定编号新建炉壁校准试验（从默认试验复制）
        /// </summary>
        /// <param name="msg">新建试验编号</param>
        private void NewWallCalMessage(string msg)
        {
            string oldExpName = "DefaultWallCal";
            string newExpName = msg;
            if (CopyWallCal(oldExpName, newExpName, true))
            {
                MessageBox.Show(newExpName+"号试验已新建完毕！", "提示");
            }
            else
            {
                MessageBox.Show("新建试验失败，数据库中可能残存信息！", "错误提示");
            }
            Messenger.Default.Send(A20Table, "WallCalTableChanged");
        }

        /// <summary>
        /// 复制指定名称的炉壁校准试验至新试验
        /// </summary>
        /// <param name="msg">被复制试验编号和新试验编号</param>
        private void CopyWallCalMessage(string[] msg)
        {
            string oldExpName = msg[0];
            string newExpName = msg[1];
            if (CopyWallCal(oldExpName, newExpName,false))
            {
                Messenger.Default.Send<BRX.DBDataSet.A20炉壁温度校准试验参数DataTable>(A20Table, "WallCalTableChanged");
                MessageBox.Show("已复制" + oldExpName + "号试验至" + newExpName + "号试验。", "提示");
            }
            else
            {
                MessageBox.Show("复制试验失败，数据库中可能残存信息！", "错误提示");
            }
        }

        /// <summary>
        ///根据编号删除炉壁校准试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void DelWallCalMessage(string msg)
        {
            if (DelWallCal(msg))
            {
                Messenger.Default.Send<BRX.DBDataSet.A20炉壁温度校准试验参数DataTable>(A20Table, "WallCalTableChanged");
                MessageBox.Show(msg + "号试验已删除！", "提示");
            }
            else
            {
                MessageBox.Show("删除试验失败！", "错误提示");
            }
        }

        /// <summary>
        ///根据编号载入炉壁校准试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void LoadWallCalMessage(string msg)
        {
          LoadWallCall(msg);
        }

        /// <summary>
        ///保存炉壁校准试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void SaveWallCalMessage(string msg)
        {
            string str = Convert.ToString(msg);
            if (str == "SaveWallCal")
            {
                SaveA20();
                SaveB20();
                A20TableAdapter.Fill(A20Table);
                Messenger.Default.Send<BRX.DBDataSet.A20炉壁温度校准试验参数DataTable>(A20Table, "WallCalTableChanged");
            }
        }
        
        /// <summary>
        /// 保存一条炉壁校准试验检测原始记录
        /// </summary>
        /// <param name="rec"></param>
        private void SaveWallCalInitRecMessage(WallCalInitRec rec)
        {
            SaveWallCalInitData(rec);
        }

        /// <summary>
        /// 清空当前炉壁校准试验的某个点的测试原始数据记录
        /// </summary>
        /// <param name="heightPointNo">高度点编号</param>
        private void WallCalInitRecClearMessage(int heightNO)
        {
            WallCalPointRecReset(heightNO);
        }

        #endregion


        #region 炉内校准试验管理、操作消息回调

        /// <summary>
        /// 根据指定编号新建炉内校准试验（从默认试验复制）
        /// </summary>
        /// <param name="msg">新建试验编号</param>
        private void NewCenterCalMessage(string msg)
        {
            string oldExpName = "DefaultCenterCal";
            string newExpName = msg;
            if (CopyCenterCal(oldExpName, newExpName, true))
            {
                MessageBox.Show(newExpName + "号试验已新建完毕！", "提示");
            }
            else
            {
                MessageBox.Show("新建试验失败，数据库中可能残存信息！", "错误提示");
            }
            Messenger.Default.Send(A30Table, "CenterCalTableChanged");
        }

        /// <summary>
        /// 复制指定名称的炉内校准试验至新试验
        /// </summary>
        /// <param name="msg">被复制试验编号和新试验编号</param>
        private void CopyCenterCalMessage(string[] msg)
        {
            string oldExpName = msg[0];
            string newExpName = msg[1];
            if (CopyCenterCal(oldExpName, newExpName, false))
            {
                Messenger.Default.Send<BRX.DBDataSet.A30炉内温度校准试验参数DataTable>(A30Table, "CenterCalTableChanged");
                MessageBox.Show("已复制" + oldExpName + "号试验至" + newExpName + "号试验。", "提示");
            }
            else
            {
                MessageBox.Show("复制试验失败，数据库中可能残存信息！", "错误提示");
            }
        }

        /// <summary>
        ///根据编号删除炉内校准试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void DelCenterCalMessage(string msg)
        {
            if (DelCenterCal(msg))
            {
                Messenger.Default.Send<BRX.DBDataSet.A30炉内温度校准试验参数DataTable>(A30Table, "CenterCalTableChanged");
                MessageBox.Show(msg + "号试验已删除！", "提示");
            }
            else
            {
                MessageBox.Show("删除试验失败！", "错误提示");
            }
        }

        /// <summary>
        ///根据编号载入炉内校准试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void LoadCenterCalMessage(string msg)
        {
              LoadCenterCall(msg);
        }

        /// <summary>
        ///保存炉内校准试验消息回调
        /// </summary>
        /// <param name="msg">试验编号</param>
        private void SaveCenterCalMessage(string msg)
        {
            string str = Convert.ToString(msg);
            if (str == "SaveCenterCal")
            {
                 SaveA30();
                 SaveB30();
                 SaveB31();
                A30TableAdapter.Fill(A30Table);
                Messenger.Default.Send<BRX.DBDataSet.A30炉内温度校准试验参数DataTable>(A30Table, "CenterCalTableChanged");
            }
        }

        /// <summary>
        /// 保存一条炉内校准试验检测原始记录
        /// </summary>
        /// <param name="rec"></param>
        private void SaveCenterCalInitRecMessage(CenterCalInitRec rec)
        {
            SaveCenterCalInitData(rec);
        }

        /// <summary>
        /// 清空当前炉内校准试验的某个点的测试原始数据记录
        /// </summary>
        /// <param name="heightPointNo">高度点编号</param>
        private void CenterCalInitRecClearMessage(int heightNO)
        {
            CenterCalPointRecReset(heightNO);
        }
        #endregion
    }
}

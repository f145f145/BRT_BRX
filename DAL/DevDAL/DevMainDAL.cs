/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL.DevDAL
 * 文件名：  DevDAL
 * 版本号：  V1.0.0.0
 * 唯一标识：3ec51cfd-d026-4a41-85bc-cf63ad16d653
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/2/8 15:10:12
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2022/2/8 15:10:12		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using BRX.Model.Dev;
using BRX.DevDBDataSetTableAdapters;

namespace BRX.DAL.DevDAL
{
    /// <summary>
    /// 装置参数读写操作类
    /// </summary>
    public partial class DevDAL : ObservableObject
    {
        public DevDAL(DevModel dev)
        {
            DAL_Dev = dev;

            //Dataset、DataTable、TableAdapter初始化
            SetTableAdapterInit_Dev();

            //装置数据读写消息
            Messenger.Default.Register<string>(this, "DevDataRWMessage", DevDataRWMessage);
            Messenger.Default.Register<string>(this, "ExpBRXDQChanged", SaveLastExpNOMessage);

            //订阅测试试验已载入消息
            Messenger.Default.Register<string>(this, "ExpBRXLoadedMessage", ExpLoadedMessage);
            Messenger.Default.Register<string>(this, "WallCalLoadedMessage", WallCalLoadedMessage);
            Messenger.Default.Register<string>(this, "CenterCalLoadedMessage", CenterCalLoadedMessage);

            //订阅模块类型修改消息
            Messenger.Default.Register<string>(this, "ModelChangedMessage", ModelChangedMessage);
            //订阅有功率模块修改消息
            Messenger.Default.Register<string>(this, "WithPowerChangedMessage", WithPowerChangedMessage);
        }


        #region DEV属性

        /// <summary>
        /// 幕墙装置
        /// </summary>
        private DevModel _dal_Dev;

        /// <summary>
        /// 幕墙装置
        /// </summary>
        private DevModel DAL_Dev
        {
            get { return _dal_Dev; }
            set
            {
                _dal_Dev = value;
                RaisePropertyChanged(() => DAL_Dev);
            }
        }

        #endregion


        #region C装置参数表DataTable属性

        /// <summary>
        /// C01装置信息DataTable
        /// </summary>
        private DevDBDataSet.C01装置信息DataTable _c01Table;

        /// <summary>
        /// C01装置信息DataTable
        /// </summary>
        private DevDBDataSet.C01装置信息DataTable C01Table
        {
            get { return _c01Table; }
            set
            {
                _c01Table = value;
                RaisePropertyChanged(() => C01Table);
            }
        }

        /// <summary>
        /// C02装置基本参数DataTable
        /// </summary>
        private DevDBDataSet.C02装置基本参数DataTable _c02Table;

        /// <summary>
        /// C02装置基本参数DataTable
        /// </summary>
        private DevDBDataSet.C02装置基本参数DataTable C02Table
        {
            get { return _c02Table; }
            set
            {
                _c02Table = value;
                RaisePropertyChanged(() => C02Table);
            }
        }

        /// <summary>
        /// C03公司信息DataTable
        /// </summary>
        private DevDBDataSet.C03公司信息DataTable _c03Table;

        /// <summary>
        /// C03公司信息DataTable
        /// </summary>
        private DevDBDataSet.C03公司信息DataTable C03Table
        {
            get { return _c03Table; }
            set
            {
                _c03Table = value;
                RaisePropertyChanged(() => C03Table);
            }
        }

        /// <summary>
        /// C04串口参数设置DataTable
        /// </summary>
        private DevDBDataSet.C04串口参数设置DataTable  _c04Table;

        /// <summary>
        /// C04串口参数设置DataTable
        /// </summary>
        private DevDBDataSet.C04串口参数设置DataTable C04Table
        {
            get { return _c04Table; }
            set
            {
                _c04Table = value;
                RaisePropertyChanged(() => C04Table);
            }
        }

        /// <summary>
        /// C05模拟量参数DataTable
        /// </summary>
        private DevDBDataSet.C05模拟量参数DataTable _c05Table;

        /// <summary>
        /// C05模拟量参数DataTable
        /// </summary>
        private DevDBDataSet.C05模拟量参数DataTable C05Table
        {
            get { return _c05Table; }
            set
            {
                _c05Table = value;
                RaisePropertyChanged(() => C05Table);
            }
        }

        /// <summary>
        /// C06模拟量通道参数DataTable
        /// </summary>
        private DevDBDataSet.C06模拟量通道参数DataTable _c06Table;

        /// <summary>
        /// C06模拟量通道参数DataTable
        /// </summary>
        private DevDBDataSet.C06模拟量通道参数DataTable C06Table
        {
            get { return _c06Table; }
            set
            {
                _c06Table = value;
                RaisePropertyChanged(() => C06Table);
            }
        }

        /// <summary>
        /// C07数字量参数DataTable
        /// </summary>
        private DevDBDataSet.C07数字量参数DataTable _c07Table;

        /// <summary>
        ///C07数字量参数DataTable
        /// </summary>
        private DevDBDataSet.C07数字量参数DataTable C07Table
        {
            get { return _c07Table; }
            set
            {
                _c07Table = value;
                RaisePropertyChanged(() => C07Table);
            }
        }

        /// <summary>
        /// C08数字量通道参数DataTable
        /// </summary>
        private DevDBDataSet.C08数字量通道参数DataTable  _c08Table;

        /// <summary>
        /// C08数字量通道参数DataTable
        /// </summary>
        private DevDBDataSet.C08数字量通道参数DataTable C08Table
        {
            get { return _c08Table; }
            set
            {
                _c08Table = value;
                RaisePropertyChanged(() => C08Table);
            }
        }

        /// <summary>
        /// C09PID控制参数DataTable
        /// </summary>
        private DevDBDataSet.C09PID控制参数DataTable  _c09Table;

        /// <summary>
        /// C09PID控制参数DataTable
        /// </summary>
        private DevDBDataSet.C09PID控制参数DataTable C09Table
        {
            get { return _c09Table; }
            set
            {
                _c09Table = value;
                RaisePropertyChanged(() => C09Table);
            }
        }

        #endregion


        #region 装置参数TableAdapter属性

        /// <summary>
        /// C01装置信息TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C01装置信息TableAdapter  _c01TableAdapter;

        /// <summary>
        /// C01装置信息TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C01装置信息TableAdapter C01TableAdapter
        {
            get { return _c01TableAdapter; }
            set
            {
                _c01TableAdapter = value;
                RaisePropertyChanged(() => C01TableAdapter);
            }
        }

        /// <summary>
        /// C02装置基本参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C02装置基本参数TableAdapter  _c02TableAdapter;

        /// <summary>
        /// C02装置基本参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C02装置基本参数TableAdapter C02TableAdapter
        {
            get { return _c02TableAdapter; }
            set
            {
                _c02TableAdapter = value;
                RaisePropertyChanged(() => C02TableAdapter);
            }
        }

        /// <summary>
        /// C03公司信息TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C03公司信息TableAdapter  _c03TableAdapter;

        /// <summary>
        /// C03公司信息TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C03公司信息TableAdapter C03TableAdapter
        {
            get { return _c03TableAdapter; }
            set
            {
                _c03TableAdapter = value;
                RaisePropertyChanged(() => C03TableAdapter);
            }
        }

        /// <summary>
        /// C04串口参数设置TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C04串口参数设置TableAdapter _c04TableAdapter;

        /// <summary>
        /// C04串口参数设置TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C04串口参数设置TableAdapter C04TableAdapter
        {
            get { return _c04TableAdapter; }
            set
            {
                _c04TableAdapter = value;
                RaisePropertyChanged(() => C04TableAdapter);
            }
        }

        /// <summary>
        /// C05模拟量参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C05模拟量参数TableAdapter _c05TableAdapter;

        /// <summary>
        /// C05模拟量参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C05模拟量参数TableAdapter C05TableAdapter
        {
            get { return _c05TableAdapter; }
            set
            {
                _c05TableAdapter = value;
                RaisePropertyChanged(() => C05TableAdapter);
            }
        }

        /// <summary>
        /// C06模拟量通道参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C06模拟量通道参数TableAdapter _c06TableAdapter;

        /// <summary>
        /// C06模拟量通道参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C06模拟量通道参数TableAdapter C06TableAdapter
        {
            get { return _c06TableAdapter; }
            set
            {
                _c06TableAdapter = value;
                RaisePropertyChanged(() => C06TableAdapter);
            }
        }

        /// <summary>
        /// C07数字量参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C07数字量参数TableAdapter  _c07TableAdapter;

        /// <summary>
        /// C07数字量参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C07数字量参数TableAdapter C07TableAdapter
        {
            get { return _c07TableAdapter; }
            set
            {
                _c07TableAdapter = value;
                RaisePropertyChanged(() => C07TableAdapter);
            }
        }

        /// <summary>
        /// C08数字量通道参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C08数字量通道参数TableAdapter _c08TableAdapter;

        /// <summary>
        /// C08数字量通道参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C08数字量通道参数TableAdapter C08TableAdapter
        {
            get { return _c08TableAdapter; }
            set
            {
                _c08TableAdapter = value;
                RaisePropertyChanged(() => C08TableAdapter);
            }
        }

        /// <summary>
        /// C09PID控制参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C09PID控制参数TableAdapter  _c09TableAdapter;

        /// <summary>
        /// C09PID控制参数TableAdapter
        /// </summary>
        private DevDBDataSetTableAdapters.C09PID控制参数TableAdapter C09TableAdapter
        {
            get { return _c09TableAdapter; }
            set
            {
                _c09TableAdapter = value;
                RaisePropertyChanged(() => C09TableAdapter);
            }
        }

        #endregion


        #region DataTable、TableAdapter初始化、更新

        /// <summary>
        /// Dataset、DataTable、TableAdapter初始化、读取数据
        /// </summary>
        private void SetTableAdapterInit_Dev()
        {
            //TableAdapter初始化
            C01TableAdapter = new C01装置信息TableAdapter();
            C02TableAdapter = new C02装置基本参数TableAdapter();
            C03TableAdapter = new C03公司信息TableAdapter();
            C04TableAdapter = new C04串口参数设置TableAdapter();
            C05TableAdapter = new C05模拟量参数TableAdapter();
            C06TableAdapter = new C06模拟量通道参数TableAdapter();
            C07TableAdapter = new C07数字量参数TableAdapter();
            C08TableAdapter = new C08数字量通道参数TableAdapter();
            C09TableAdapter = new C09PID控制参数TableAdapter();

            //DataTable初始化
            C01Table = new DevDBDataSet.C01装置信息DataTable();
            C02Table = new DevDBDataSet.C02装置基本参数DataTable();
            C03Table = new DevDBDataSet.C03公司信息DataTable();
            C04Table = new DevDBDataSet.C04串口参数设置DataTable();
            C05Table = new DevDBDataSet.C05模拟量参数DataTable();
            C06Table = new DevDBDataSet.C06模拟量通道参数DataTable();
            C07Table = new DevDBDataSet.C07数字量参数DataTable();
            C08Table = new DevDBDataSet.C08数字量通道参数DataTable();
            C09Table = new DevDBDataSet.C09PID控制参数DataTable();

            //数据读取
            C01TableAdapter.Fill(C01Table);
            C02TableAdapter.Fill(C02Table);
            C03TableAdapter.Fill(C03Table);
            C04TableAdapter.Fill(C04Table);
            C05TableAdapter.Fill(C05Table);
            C06TableAdapter.Fill(C06Table);
            C07TableAdapter.Fill(C07Table);
            C08TableAdapter.Fill(C08Table);
            C09TableAdapter.Fill(C09Table);
        }

        #endregion


        #region 根据消息保存最新载入的试验编号
        
        /// <summary>
        /// 根据消息保存最新载入的试验编号
        /// </summary>
        private void ExpLoadedMessage(string msg)
        {
            DAL_Dev.ExpNOLast = msg;
            SaveBisicParam();
        }

        /// <summary>
        /// 根据消息保存最新载入的试验编号
        /// </summary>
        private void WallCalLoadedMessage(string msg)
        {
            DAL_Dev.WallCalNOLast = msg;
            SaveBisicParam();
        }

        /// <summary>
        /// 根据消息保存最新载入的试验编号
        /// </summary>
        private void CenterCalLoadedMessage(string msg)
        {
            DAL_Dev.CenterCalNOLast = msg;
            SaveBisicParam();
        }

        #endregion


        #region 根据消息读写装置数据

        /// <summary>
        /// 装置数据读写消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void DevDataRWMessage(string msg)
        {
            //装置忙，无法操作
            if (DAL_Dev.IsBusy)
            {
                MessageBox.Show("装置忙，请关闭正在运行的试验或测试软件", "错误提示");
                return;
            }

            //载入dev设置
            if (msg == "LoadDevSettings")
            {
                LoadDevSettings();
            }

            //载入装置工厂设置
            else if(msg == "LoadFacDevSettings")
            {
                LoadFacDevSettings();
            }

            //载入装置默认设置
            else if (msg == "LoadDefaultDevSettings")
            {
                LoadDefaultDevSettings();
            }


            //将当前装置参数保存为默认设置
            else if (msg == "SaveAsDefaultSettings")
            {
                SaveAsDefaultSettings();
            }


            //保存dev设置
            else if (msg == "SaveDevSettings")
            {
                SaveDevSettings();
            }

            //保存dev设置
            else if (msg == "SaveDevSettings")
            {
                SaveDevSettings();
            }
        }


        /// <summary>
        /// 模块参数修改消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void ModelChangedMessage(string msg)
        {
            //装置忙，无法操作
            if (DAL_Dev.IsBusy)
            {
                MessageBox.Show("装置忙，请关闭正在运行的试验或测试软件", "错误提示");
                return;
            }

            //载入dev设置
            if (msg == "ModelChangedMessage")
            {
                //重新载入串口参数
                LoadComSettings();

                //重新载入模拟量通道参数
                LoadAIOChannelParam();
                //重新绑定模拟量参数和通道
                BindAioAndChennel();
            }
        }


        /// <summary>
        /// 模块参数修改消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void WithPowerChangedMessage(string msg)
        {
            //装置忙，无法操作
            if (DAL_Dev.IsBusy)
            {
                MessageBox.Show("装置忙，请关闭正在运行的试验或测试软件", "错误提示");
                return;
            }

            //载入dev设置
            if (msg == "WithPowerChangedMessage")
            {
                //重新载入串口参数
                LoadPIDSettings();
            }
        }

        #endregion
    }
}

/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL.CalDAL
 * 文件名：  ExpLoadDAL
 * 版本号：  V1.0.0.0
 * 唯一标识：e00a44b3-f135-41b7-8d36-8939c995a537
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-4-5 10:21:38
 * 描述：
 * 试验读写。载入部分。
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/22 23:14:24		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using BRX.DAL.CalDAL.CalDALModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace BRX.DAL.CalDAL
{
    public partial class CalDAL : ObservableObject
    {
        /// <summary>
        /// 载入指定名称的炉壁校准试验（当前试验）
        /// </summary>
        /// <param name="expNo"></param>
        public void LoadWallCall(string expNo)
        {
            string loadExpNo = expNo.Clone().ToString();

            if ((loadExpNo == null) || (loadExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            if (loadExpNo == "FactoryWallCal")
            {
                MessageBox.Show("工厂试验不能载入");
                return;
            }

            LoadA20(expNo);
            LoadB20(expNo);
            LoadWallCalInitRec(expNo);

            RaisePropertyChanged(() => WallCalDQ.QtyComplete);
            RaisePropertyChanged(() => WallCalDQ.QtyLeft);
            Messenger.Default.Send<string>(WallCalDQ.ExpNO, "WallCalLoadedMessage");
        }


        /// <summary>
        /// 载入指定名称的炉内校准试验（当前试验）
        /// </summary>
        /// <param name="expNo"></param>
        public void LoadCenterCall(string expNo)
        {
            string loadExpNo = expNo.Clone().ToString();

            if ((loadExpNo == null) || (loadExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            if (loadExpNo == "FactoryCenterCal")
            {
                MessageBox.Show("工厂试验不能载入");
                return;
            }

            LoadA30(expNo);
            LoadB30(expNo);
            LoadB31(expNo);
            LoadCenterCalInitRec(expNo);

            RaisePropertyChanged(() => CenterCalDQ.QtyComplete);
            RaisePropertyChanged(() => CenterCalDQ.QtyLeft);
            Messenger.Default.Send<string>(CenterCalDQ.ExpNO, "CenterCalLoadedMessage");
        }


        #region 炉壁校准

        /// <summary>
        /// 载入A20试验信息参数
        /// </summary>
        private void LoadA20(string expNo)
        {
            string loadExpNo = expNo.Clone().ToString();

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.A20炉壁温度校准试验参数Row checkExistA20Row = A20Table.FindBy试验编号(loadExpNo);
                if (checkExistA20Row == null)
                {
                    DBDataSet.A20炉壁温度校准试验参数Row defA20Row = A20Table.FindBy试验编号("DefaultWallCal");
                    DBDataSet.A20炉壁温度校准试验参数Row newA20Row = A20Table.NewA20炉壁温度校准试验参数Row();
                    newA20Row.ItemArray = (object[])defA20Row.ItemArray.Clone();
                    newA20Row.试验编号 = loadExpNo;
                    A20Table.AddA20炉壁温度校准试验参数Row(newA20Row);
                    A20TableAdapter.Update(A20Table);
                    A20Table.AcceptChanges();
                    RaisePropertyChanged(() => A20Table);
                    MessageBox.Show("未找到" + loadExpNo + "A20试验参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.A20炉壁温度校准试验参数Row expA20Row = A20Table.FindBy试验编号(loadExpNo);
                if (expA20Row != null)
                {
                    WallCalDQ.ExpNO = expA20Row.试验编号;
                    WallCalDQ.ExpDetail = expA20Row.试验补充说明;
                    WallCalDQ.RepNO = expA20Row.报告编号;
                    WallCalDQ.CreatTime = expA20Row.创建日期时间;
                    WallCalDQ.RepTime = expA20Row.报告日期时间;
                    //WallCalDQ.IsCompleted = expA20Row.已完成标志;
                    WallCalDQ.WallCalPointsList[0].IsReced = expA20Row.a正30mm完成;
                    WallCalDQ.WallCalPointsList[1].IsReced = expA20Row.b0mm完成;
                    WallCalDQ.WallCalPointsList[2].IsReced = expA20Row.c负30mm完成;

                    WallCalDQ.IsReal = expA20Row.原始试验标志;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 载入B20炉壁温度校准数据
        /// </summary>
        private void LoadB20(string expNo)
        {
            string loadExpNo = expNo.Clone().ToString();

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B20炉壁温度校准数据Row checkExistB20Row = B20Table.FindBy试验编号(loadExpNo);
                if (checkExistB20Row == null)
                {
                    DBDataSet.B20炉壁温度校准数据Row defB20Row = B20Table.FindBy试验编号("DefaultWallCal");
                    DBDataSet.B20炉壁温度校准数据Row newB20Row = B20Table.NewB20炉壁温度校准数据Row();
                    newB20Row.ItemArray = (object[])defB20Row.ItemArray.Clone();
                    newB20Row.试验编号 = loadExpNo;
                    B20Table.AddB20炉壁温度校准数据Row(newB20Row);
                    B20TableAdapter.Update(B20Table);
                    B20Table.AcceptChanges();
                    RaisePropertyChanged(() => B20Table);
                    MessageBox.Show("未找到" + loadExpNo + "B20校准数据，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B20炉壁温度校准数据Row expB20Row = B20Table.FindBy试验编号(loadExpNo);
                if (expB20Row != null)
                {
                    WallCalDQ.ExpNO = expB20Row.试验编号;
                    WallCalDQ.WallCalPointsList[1].RecTime = expB20Row.t_b;
                    WallCalDQ.WallCalPointsList[1].IsReced = expB20Row.IsRecorded_b;
                    WallCalDQ.WallCalPointsList[1].T1 = expB20Row.Tw1_b;
                    WallCalDQ.WallCalPointsList[1].T2 = expB20Row.Tw2_b;
                    WallCalDQ.WallCalPointsList[1].T3 = expB20Row.Tw3_b;
                    WallCalDQ.WallCalPointsList[1].Tf1 = expB20Row.Tf1_b;
                    WallCalDQ.WallCalPointsList[1].Tf2 = expB20Row.Tf2_b;

                    WallCalDQ.WallCalPointsList[0].RecTime = expB20Row.t_a;
                    WallCalDQ.WallCalPointsList[0].IsReced = expB20Row.IsRecorded_a;
                    WallCalDQ.WallCalPointsList[0].T1 = expB20Row.Tw1_a;
                    WallCalDQ.WallCalPointsList[0].T2 = expB20Row.Tw2_a;
                    WallCalDQ.WallCalPointsList[0].T3 = expB20Row.Tw3_a;
                    WallCalDQ.WallCalPointsList[0].Tf1 = expB20Row.Tf1_a;
                    WallCalDQ.WallCalPointsList[0].Tf2 = expB20Row.Tf2_a;

                    WallCalDQ.WallCalPointsList[2].RecTime = expB20Row.t_c;
                    WallCalDQ.WallCalPointsList[2].IsReced = expB20Row.IsRecorded_c;
                    WallCalDQ.WallCalPointsList[2].T1 = expB20Row.Tw1_c;
                    WallCalDQ.WallCalPointsList[2].T2 = expB20Row.Tw2_c;
                    WallCalDQ.WallCalPointsList[2].T3 = expB20Row.Tw3_c;
                    WallCalDQ.WallCalPointsList[2].Tf1 = expB20Row.Tf1_c;
                    WallCalDQ.WallCalPointsList[2].Tf2 = expB20Row.Tf2_c;
                    WallCalDQ.Tavg = expB20Row.Tavg;
                    WallCalDQ.Tavg_axis1 = expB20Row.Tavg_axis1;
                    WallCalDQ.Tavg_axis2 = expB20Row.Tavg_axis2;
                    WallCalDQ.Tavg_axis3 = expB20Row.Tavg_axis3;
                    WallCalDQ.Tdev_axis1 = expB20Row.Tdev_axis1;
                    WallCalDQ.Tdev_axis2 = expB20Row.Tdev_axis2;
                    WallCalDQ.Tdev_axis3 = expB20Row.Tdev_axis3;
                    WallCalDQ.Tavg_dev_axis = expB20Row.Tavg_dev_axis;
                    WallCalDQ.Tavg_levelb = expB20Row.Tavg_levelb;
                    WallCalDQ.Tavg_levela = expB20Row.Tavg_levela;
                    WallCalDQ.Tavg_levelc = expB20Row.Tavg_levelc;
                    WallCalDQ.Tdev_levelb = expB20Row.Tdev_levelb;
                    WallCalDQ.Tdev_levela = expB20Row.Tdev_levela;
                    WallCalDQ.Tdev_levelc = expB20Row.Tdev_levelc;
                    WallCalDQ.Tavg_dev_level = expB20Row.Tavg_dev_level;
                    WallCalDQ.Result = expB20Row.Result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 炉壁校准原始记录数据
        /// </summary>
        private void LoadWallCalInitRec(string expNo)
        {
            string loadExpNo = expNo.Clone().ToString();
            //逐个试样载入
            for (int i = 1; i <= 3; i++)
            {
                //List<InitRec> initRec_Sp = InitDB.Queryable<InitRec>().AS(loadExpNo).Where(it => it.SpNO == i).OrderBy(it => it.RecNum).ToList();
                List<WallCalInitRec> initRec_Point = WallCalInitDB.Queryable<WallCalInitRec>().AS(loadExpNo).Where(it => it.HeightNO == i).ToList();
                ObservableCollection<WallCalInitRec> heightPointRec = new ObservableCollection<WallCalInitRec>();
                for (int j = 0; j < initRec_Point.Count; j++)
                {
                    WallCalInitRec newTestRec = new WallCalInitRec();
                    newTestRec.ExpNO = initRec_Point[j].ExpNO;
                    newTestRec.HeightNO = initRec_Point[j].HeightNO;
                    newTestRec.Height = initRec_Point[j].Height;
                    newTestRec.RecNum = initRec_Point[j].RecNum;
                    newTestRec.RecTime = initRec_Point[j].RecTime;
                    newTestRec.Tf1 = initRec_Point[j].Tf1;
                    newTestRec.Tf2 = initRec_Point[j].Tf2;
                    newTestRec.T1 = initRec_Point[j].T1;
                    newTestRec.T2 = initRec_Point[j].T2;
                    newTestRec.T3 = initRec_Point[j].T3;
                    newTestRec.Vo = initRec_Point[j].Vo;
                    newTestRec.Detail = initRec_Point[j].Detail;
                    
                    heightPointRec.Add(newTestRec);
                }

                WallCalDQ.WallCalPointsList[i-1].RecList_WallCal = heightPointRec;
            }
        }
        #endregion



        #region 炉内校准

        /// <summary>
        /// 载入A30炉内温度校准试验参数
        /// </summary>
        private void LoadA30(string expNo)
        {
            string loadExpNo = expNo.Clone().ToString();

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.A30炉内温度校准试验参数Row checkExistA30Row = A30Table.FindBy试验编号(loadExpNo);
                if (checkExistA30Row == null)
                {
                    DBDataSet.A30炉内温度校准试验参数Row defA30Row = A30Table.FindBy试验编号("DefaultCenterCal");
                    DBDataSet.A30炉内温度校准试验参数Row newA30Row = A30Table.NewA30炉内温度校准试验参数Row();
                    newA30Row.ItemArray = (object[])defA30Row.ItemArray.Clone();
                    newA30Row.试验编号 = loadExpNo;
                    A30Table.AddA30炉内温度校准试验参数Row(newA30Row);
                    A30TableAdapter.Update(A30Table);
                    A30Table.AcceptChanges();
                    RaisePropertyChanged(() => A30Table);
                    MessageBox.Show("未找到" + loadExpNo + "A30试验参数，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.A30炉内温度校准试验参数Row expA30Row = A30Table.FindBy试验编号(loadExpNo);
                if (expA30Row != null)
                {
                    CenterCalDQ.ExpNO = expA30Row.试验编号;
                    CenterCalDQ.ExpDetail = expA30Row.试验补充说明;
                    CenterCalDQ.RepNO = expA30Row.报告编号;
                    CenterCalDQ.CreatTime = expA30Row.创建日期时间;
                    CenterCalDQ.RepTime = expA30Row.报告日期时间;
                    CenterCalDQ.IsCompleted = expA30Row.已完成标志;
                    CenterCalDQ.IsReal = expA30Row.原始试验标志;
                    CenterCalDQ.CenterCalPointsList[0].IsReced = expA30Row.完成标志75D;
                    CenterCalDQ.CenterCalPointsList[1].IsReced = expA30Row.完成标志65D;
                    CenterCalDQ.CenterCalPointsList[2].IsReced = expA30Row.完成标志55D;
                    CenterCalDQ.CenterCalPointsList[3].IsReced = expA30Row.完成标志45D;
                    CenterCalDQ.CenterCalPointsList[4].IsReced = expA30Row.完成标志35D;
                    CenterCalDQ.CenterCalPointsList[5].IsReced = expA30Row.完成标志25D;
                    CenterCalDQ.CenterCalPointsList[6].IsReced = expA30Row.完成标志15D;
                    CenterCalDQ.CenterCalPointsList[7].IsReced = expA30Row.完成标志5D;

                    CenterCalDQ.CenterCalPointsList[8].IsReced = expA30Row.完成标志5U;
                    CenterCalDQ.CenterCalPointsList[9].IsReced = expA30Row.完成标志15U;
                    CenterCalDQ.CenterCalPointsList[10].IsReced = expA30Row.完成标志25U;
                    CenterCalDQ.CenterCalPointsList[11].IsReced = expA30Row.完成标志35U;
                    CenterCalDQ.CenterCalPointsList[12].IsReced = expA30Row.完成标志45U;
                    CenterCalDQ.CenterCalPointsList[13].IsReced = expA30Row.完成标志55U;
                    CenterCalDQ.CenterCalPointsList[14].IsReced = expA30Row.完成标志65U;
                    CenterCalDQ.CenterCalPointsList[15].IsReced = expA30Row.完成标志75U;

                    CenterCalDQ.CenterCalPointsList[16].IsReced = expA30Row.完成标志85U;
                    CenterCalDQ.CenterCalPointsList[17].IsReced = expA30Row.完成标志95U;
                    CenterCalDQ.CenterCalPointsList[18].IsReced = expA30Row.完成标志105U;
                    CenterCalDQ.CenterCalPointsList[19].IsReced = expA30Row.完成标志115U;
                    CenterCalDQ.CenterCalPointsList[20].IsReced = expA30Row.完成标志125U;
                    CenterCalDQ.CenterCalPointsList[21].IsReced = expA30Row.完成标志135U;
                    CenterCalDQ.CenterCalPointsList[22].IsReced = expA30Row.完成标志145U;

                    CenterCalDQ.CenterCalPointsList[23].IsReced = expA30Row.完成标志145D;
                    CenterCalDQ.CenterCalPointsList[24].IsReced = expA30Row.完成标志135D;
                    CenterCalDQ.CenterCalPointsList[25].IsReced = expA30Row.完成标志125D;
                    CenterCalDQ.CenterCalPointsList[26].IsReced = expA30Row.完成标志115D;
                    CenterCalDQ.CenterCalPointsList[27].IsReced = expA30Row.完成标志105D;
                    CenterCalDQ.CenterCalPointsList[28].IsReced = expA30Row.完成标志95D;
                    CenterCalDQ.CenterCalPointsList[29].IsReced = expA30Row.完成标志85D;

                    CenterCalDQ.IsFitStdList[0] = expA30Row.IsFit_5;
                    CenterCalDQ.IsFitStdList[1] = expA30Row.IsFit_15;
                    CenterCalDQ.IsFitStdList[2] = expA30Row.IsFit_25;
                    CenterCalDQ.IsFitStdList[3] = expA30Row.IsFit_35;
                    CenterCalDQ.IsFitStdList[4] = expA30Row.IsFit_45;
                    CenterCalDQ.IsFitStdList[5] = expA30Row.IsFit_55;
                    CenterCalDQ.IsFitStdList[6] = expA30Row.IsFit_65;
                    CenterCalDQ.IsFitStdList[7] = expA30Row.IsFit_75;
                    CenterCalDQ.IsFitStdList[8] = expA30Row.IsFit_85;
                    CenterCalDQ.IsFitStdList[9] = expA30Row.IsFit_95;
                    CenterCalDQ.IsFitStdList[10] = expA30Row.IsFit_105;
                    CenterCalDQ.IsFitStdList[11] = expA30Row.IsFit_115;
                    CenterCalDQ.IsFitStdList[12] = expA30Row.IsFit_125;
                    CenterCalDQ.IsFitStdList[13] = expA30Row.IsFit_135;
                    CenterCalDQ.IsFitStdList[14] = expA30Row.IsFit_145;

                    CenterCalDQ.TAvgList[0] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[1] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[2] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[3] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[4] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[5] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[6] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[7] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[8] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[9] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[10] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[11] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[12] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[13] = expA30Row.Tfc_Avg_5;
                    CenterCalDQ.TAvgList[14] = expA30Row.Tfc_Avg_5;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 载入B30炉内温度校准数据
        /// </summary>
        private void LoadB30(string expNo)
        {
            string loadExpNo = expNo.Clone().ToString();

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B30炉内温度校准数据Row checkExistB30Row = B30Table.FindBy试验编号(loadExpNo);
                if (checkExistB30Row == null)
                {
                    DBDataSet.B30炉内温度校准数据Row defB30Row = B30Table.FindBy试验编号("DefaultCenterCal");
                    DBDataSet.B30炉内温度校准数据Row newB30Row = B30Table.NewB30炉内温度校准数据Row();
                    newB30Row.ItemArray = (object[])defB30Row.ItemArray.Clone();
                    newB30Row.试验编号 = loadExpNo;
                    B30Table.AddB30炉内温度校准数据Row(newB30Row);
                    B30TableAdapter.Update(B30Table);
                    B30Table.AcceptChanges();
                    RaisePropertyChanged(() => B30Table);
                    MessageBox.Show("未找到" + loadExpNo + "B30校准数据，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B30炉内温度校准数据Row expB30Row = B30Table.FindBy试验编号(loadExpNo);
                if (expB30Row != null)
                {
                    CenterCalDQ.ExpNO = expB30Row.试验编号;
                    CenterCalDQ.CenterCalPointsList[8].RecTime = expB30Row.t_5U;
                    CenterCalDQ.CenterCalPointsList[8].Tfc = expB30Row.Tfc_5U;
                    CenterCalDQ.CenterCalPointsList[8].Tf1 = expB30Row.Tf1_5U;
                    CenterCalDQ.CenterCalPointsList[8].Tf2 = expB30Row.Tf2_5U;

                    CenterCalDQ.CenterCalPointsList[9].RecTime = expB30Row.t_15U;
                    CenterCalDQ.CenterCalPointsList[9].Tfc = expB30Row.Tfc_15U;
                    CenterCalDQ.CenterCalPointsList[9].Tf1 = expB30Row.Tf1_15U;
                    CenterCalDQ.CenterCalPointsList[9].Tf2 = expB30Row.Tf2_15U;

                    CenterCalDQ.CenterCalPointsList[10].RecTime = expB30Row.t_25U;
                    CenterCalDQ.CenterCalPointsList[10].Tfc = expB30Row.Tfc_25U;
                    CenterCalDQ.CenterCalPointsList[10].Tf1 = expB30Row.Tf1_25U;
                    CenterCalDQ.CenterCalPointsList[10].Tf2 = expB30Row.Tf2_25U;

                    CenterCalDQ.CenterCalPointsList[11].RecTime = expB30Row.t_35U;
                    CenterCalDQ.CenterCalPointsList[11].Tfc = expB30Row.Tfc_35U;
                    CenterCalDQ.CenterCalPointsList[11].Tf1 = expB30Row.Tf1_35U;
                    CenterCalDQ.CenterCalPointsList[11].Tf2 = expB30Row.Tf2_35U;

                    CenterCalDQ.CenterCalPointsList[12].RecTime = expB30Row.t_45U;
                    CenterCalDQ.CenterCalPointsList[12].Tfc = expB30Row.Tfc_45U;
                    CenterCalDQ.CenterCalPointsList[12].Tf1 = expB30Row.Tf1_45U;
                    CenterCalDQ.CenterCalPointsList[12].Tf2 = expB30Row.Tf2_45U;

                    CenterCalDQ.CenterCalPointsList[13].RecTime = expB30Row.t_55U;
                    CenterCalDQ.CenterCalPointsList[13].Tfc = expB30Row.Tfc_55U;
                    CenterCalDQ.CenterCalPointsList[13].Tf1 = expB30Row.Tf1_55U;
                    CenterCalDQ.CenterCalPointsList[13].Tf2 = expB30Row.Tf2_55U;

                    CenterCalDQ.CenterCalPointsList[14].RecTime = expB30Row.t_65U;
                    CenterCalDQ.CenterCalPointsList[14].Tfc = expB30Row.Tfc_65U;
                    CenterCalDQ.CenterCalPointsList[14].Tf1 = expB30Row.Tf1_65U;
                    CenterCalDQ.CenterCalPointsList[14].Tf2 = expB30Row.Tf2_65U;

                    CenterCalDQ.CenterCalPointsList[15].RecTime = expB30Row.t_75U;
                    CenterCalDQ.CenterCalPointsList[15].Tfc = expB30Row.Tfc_75U;
                    CenterCalDQ.CenterCalPointsList[15].Tf1 = expB30Row.Tf1_75U;
                    CenterCalDQ.CenterCalPointsList[15].Tf2 = expB30Row.Tf2_75U;

                    CenterCalDQ.CenterCalPointsList[7].RecTime = expB30Row.t_5D;
                    CenterCalDQ.CenterCalPointsList[7].Tfc = expB30Row.Tfc_5D;
                    CenterCalDQ.CenterCalPointsList[7].Tf1 = expB30Row.Tf1_5D;
                    CenterCalDQ.CenterCalPointsList[7].Tf2 = expB30Row.Tf2_5D;

                    CenterCalDQ.CenterCalPointsList[6].RecTime = expB30Row.t_15D;
                    CenterCalDQ.CenterCalPointsList[6].Tfc = expB30Row.Tfc_15D;
                    CenterCalDQ.CenterCalPointsList[6].Tf1 = expB30Row.Tf1_15D;
                    CenterCalDQ.CenterCalPointsList[6].Tf2 = expB30Row.Tf2_15D;

                    CenterCalDQ.CenterCalPointsList[5].RecTime = expB30Row.t_25D;
                    CenterCalDQ.CenterCalPointsList[5].Tfc = expB30Row.Tfc_25D;
                    CenterCalDQ.CenterCalPointsList[5].Tf1 = expB30Row.Tf1_25D;
                    CenterCalDQ.CenterCalPointsList[5].Tf2 = expB30Row.Tf2_25D;

                    CenterCalDQ.CenterCalPointsList[4].RecTime = expB30Row.t_35D;
                    CenterCalDQ.CenterCalPointsList[4].Tfc = expB30Row.Tfc_35D;
                    CenterCalDQ.CenterCalPointsList[4].Tf1 = expB30Row.Tf1_35D;
                    CenterCalDQ.CenterCalPointsList[4].Tf2 = expB30Row.Tf2_35D;

                    CenterCalDQ.CenterCalPointsList[3].RecTime = expB30Row.t_45D;
                    CenterCalDQ.CenterCalPointsList[3].Tfc = expB30Row.Tfc_45D;
                    CenterCalDQ.CenterCalPointsList[3].Tf1 = expB30Row.Tf1_45D;
                    CenterCalDQ.CenterCalPointsList[3].Tf2 = expB30Row.Tf2_45D;

                    CenterCalDQ.CenterCalPointsList[2].RecTime = expB30Row.t_55D;
                    CenterCalDQ.CenterCalPointsList[2].Tfc = expB30Row.Tfc_55D;
                    CenterCalDQ.CenterCalPointsList[2].Tf1 = expB30Row.Tf1_55D;
                    CenterCalDQ.CenterCalPointsList[2].Tf2 = expB30Row.Tf2_55D;

                    CenterCalDQ.CenterCalPointsList[1].RecTime = expB30Row.t_65D;
                    CenterCalDQ.CenterCalPointsList[1].Tfc = expB30Row.Tfc_65D;
                    CenterCalDQ.CenterCalPointsList[1].Tf1 = expB30Row.Tf1_65D;
                    CenterCalDQ.CenterCalPointsList[1].Tf2 = expB30Row.Tf2_65D;

                    CenterCalDQ.CenterCalPointsList[0].RecTime = expB30Row.t_75D;
                    CenterCalDQ.CenterCalPointsList[0].Tfc = expB30Row.Tfc_75D;
                    CenterCalDQ.CenterCalPointsList[0].Tf1 = expB30Row.Tf1_75D;
                    CenterCalDQ.CenterCalPointsList[0].Tf2 = expB30Row.Tf2_75D;

                    CenterCalDQ.Result = expB30Row.Result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        /// <summary>
        /// 载入B31炉内温度校准数据2
        /// </summary>
        private void LoadB31(string expNo)
        {
            string loadExpNo = expNo.Clone().ToString();

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B31炉内温度校准数据2Row checkExistB31Row = B31Table.FindBy试验编号(loadExpNo);
                if (checkExistB31Row == null)
                {
                    DBDataSet.B31炉内温度校准数据2Row defB31Row = B31Table.FindBy试验编号("DefaultCenterCal");
                    DBDataSet.B31炉内温度校准数据2Row newB31Row = B31Table.NewB31炉内温度校准数据2Row();
                    newB31Row.ItemArray = (object[])defB31Row.ItemArray.Clone();
                    newB31Row.试验编号 = loadExpNo;
                    B31Table.AddB31炉内温度校准数据2Row(newB31Row);
                    B31TableAdapter.Update(B31Table);
                    B31Table.AcceptChanges();
                    RaisePropertyChanged(() => B31Table);
                    MessageBox.Show("未找到" + loadExpNo + "B31校准数据，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B31炉内温度校准数据2Row expB31Row = B31Table.FindBy试验编号(loadExpNo);
                if (expB31Row != null)
                {
                    CenterCalDQ.ExpNO = expB31Row.试验编号;
                   
                    CenterCalDQ.CenterCalPointsList[16].RecTime = expB31Row.t_85U;
                    CenterCalDQ.CenterCalPointsList[16].Tfc = expB31Row.Tfc_85U;
                    CenterCalDQ.CenterCalPointsList[16].Tf1 = expB31Row.Tf1_85U;
                    CenterCalDQ.CenterCalPointsList[16].Tf2 = expB31Row.Tf2_85U;

                    CenterCalDQ.CenterCalPointsList[17].RecTime = expB31Row.t_95U;
                    CenterCalDQ.CenterCalPointsList[17].Tfc = expB31Row.Tfc_95U;
                    CenterCalDQ.CenterCalPointsList[17].Tf1 = expB31Row.Tf1_95U;
                    CenterCalDQ.CenterCalPointsList[17].Tf2 = expB31Row.Tf2_95U;

                    CenterCalDQ.CenterCalPointsList[18].RecTime = expB31Row.t_105U;
                    CenterCalDQ.CenterCalPointsList[18].Tfc = expB31Row.Tfc_105U;
                    CenterCalDQ.CenterCalPointsList[18].Tf1 = expB31Row.Tf1_105U;
                    CenterCalDQ.CenterCalPointsList[18].Tf2 = expB31Row.Tf2_105U;

                    CenterCalDQ.CenterCalPointsList[19].RecTime = expB31Row.t_115U;
                    CenterCalDQ.CenterCalPointsList[19].Tfc = expB31Row.Tfc_115U;
                    CenterCalDQ.CenterCalPointsList[19].Tf1 = expB31Row.Tf1_115U;
                    CenterCalDQ.CenterCalPointsList[19].Tf2 = expB31Row.Tf2_115U;

                    CenterCalDQ.CenterCalPointsList[20].RecTime = expB31Row.t_125U;
                    CenterCalDQ.CenterCalPointsList[20].Tfc = expB31Row.Tfc_125U;
                    CenterCalDQ.CenterCalPointsList[20].Tf1 = expB31Row.Tf1_125U;
                    CenterCalDQ.CenterCalPointsList[20].Tf2 = expB31Row.Tf2_125U;

                    CenterCalDQ.CenterCalPointsList[21].RecTime = expB31Row.t_135U;
                    CenterCalDQ.CenterCalPointsList[21].Tfc = expB31Row.Tfc_135U;
                    CenterCalDQ.CenterCalPointsList[21].Tf1 = expB31Row.Tf1_135U;
                    CenterCalDQ.CenterCalPointsList[21].Tf2 = expB31Row.Tf2_135U;

                    CenterCalDQ.CenterCalPointsList[22].RecTime = expB31Row.t_145U;
                    CenterCalDQ.CenterCalPointsList[22].Tfc = expB31Row.Tfc_145U;
                    CenterCalDQ.CenterCalPointsList[22].Tf1 = expB31Row.Tf1_145U;
                    CenterCalDQ.CenterCalPointsList[22].Tf2 = expB31Row.Tf2_145U;
                    
                    CenterCalDQ.CenterCalPointsList[29].RecTime = expB31Row.t_85D;
                    CenterCalDQ.CenterCalPointsList[29].Tfc = expB31Row.Tfc_85D;
                    CenterCalDQ.CenterCalPointsList[29].Tf1 = expB31Row.Tf1_85D;
                    CenterCalDQ.CenterCalPointsList[29].Tf2 = expB31Row.Tf2_85D;

                    CenterCalDQ.CenterCalPointsList[28].RecTime = expB31Row.t_95D;
                    CenterCalDQ.CenterCalPointsList[28].Tfc = expB31Row.Tfc_95D;
                    CenterCalDQ.CenterCalPointsList[28].Tf1 = expB31Row.Tf1_95D;
                    CenterCalDQ.CenterCalPointsList[28].Tf2 = expB31Row.Tf2_95D;

                    CenterCalDQ.CenterCalPointsList[27].RecTime = expB31Row.t_105D;
                    CenterCalDQ.CenterCalPointsList[27].Tfc = expB31Row.Tfc_105D;
                    CenterCalDQ.CenterCalPointsList[27].Tf1 = expB31Row.Tf1_105D;
                    CenterCalDQ.CenterCalPointsList[27].Tf2 = expB31Row.Tf2_105D;

                    CenterCalDQ.CenterCalPointsList[26].RecTime = expB31Row.t_115D;
                    CenterCalDQ.CenterCalPointsList[26].Tfc = expB31Row.Tfc_115D;
                    CenterCalDQ.CenterCalPointsList[26].Tf1 = expB31Row.Tf1_115D;
                    CenterCalDQ.CenterCalPointsList[26].Tf2 = expB31Row.Tf2_115D;

                    CenterCalDQ.CenterCalPointsList[25].RecTime = expB31Row.t_125D;
                    CenterCalDQ.CenterCalPointsList[25].Tfc = expB31Row.Tfc_125D;
                    CenterCalDQ.CenterCalPointsList[25].Tf1 = expB31Row.Tf1_125D;
                    CenterCalDQ.CenterCalPointsList[25].Tf2 = expB31Row.Tf2_125D;

                    CenterCalDQ.CenterCalPointsList[24].RecTime = expB31Row.t_135D;
                    CenterCalDQ.CenterCalPointsList[24].Tfc = expB31Row.Tfc_135D;
                    CenterCalDQ.CenterCalPointsList[24].Tf1 = expB31Row.Tf1_135D;
                    CenterCalDQ.CenterCalPointsList[24].Tf2 = expB31Row.Tf2_135D;

                    CenterCalDQ.CenterCalPointsList[23].RecTime = expB31Row.t_145D;
                    CenterCalDQ.CenterCalPointsList[23].Tfc = expB31Row.Tfc_145D;
                    CenterCalDQ.CenterCalPointsList[23].Tf1 = expB31Row.Tf1_145D;
                    CenterCalDQ.CenterCalPointsList[23].Tf2 = expB31Row.Tf2_145D;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 炉内校准原始记录数据
        /// </summary>
        private void LoadCenterCalInitRec(string expNo)
        {
            string loadExpNo = expNo.Clone().ToString();
            //逐个试样载入
            for (int i = 0; i <CenterCalDQ.CenterCalPointsList.Count; i++)
            {
                List<CenterCalInitRec> initRec_Point = CenterCalInitDB.Queryable<CenterCalInitRec>().AS(loadExpNo).Where(it => it.HeightNO == i).ToList();
                ObservableCollection<CenterCalInitRec> heightPointRec = new ObservableCollection<CenterCalInitRec>();
                for (int j = 0; j < initRec_Point.Count; j++)
                {
                    CenterCalInitRec newTestRec = new CenterCalInitRec();
                    newTestRec.ExpNO = initRec_Point[j].ExpNO;
                    newTestRec.HeightNO = initRec_Point[j].HeightNO;
                    newTestRec.Height = initRec_Point[j].Height;
                    newTestRec.RecNum = initRec_Point[j].RecNum;
                    newTestRec.RecTime = initRec_Point[j].RecTime;
                    newTestRec.Tf1 = initRec_Point[j].Tf1;
                    newTestRec.Tf2 = initRec_Point[j].Tf2;
                    newTestRec.Tc = initRec_Point[j].Tc;
                    newTestRec.Vo = initRec_Point[j].Vo;
                    newTestRec.Detail = initRec_Point[j].Detail;

                    heightPointRec.Add(newTestRec);
                }

                CenterCalDQ.CenterCalPointsList[i].RecList_CenterCal = heightPointRec;
            }
        }
        #endregion
    }
}
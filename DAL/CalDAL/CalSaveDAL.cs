/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL.CalDAL
 * 文件名：  ExpSaveDAL
 * 版本号：  V1.0.0.0
 * 唯一标识：b801d247-9991-400c-a532-8a8f6d00d4d8
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-4-5 10:22:02
 * 描述：
 * 试验读写。保存部分。
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/22 23:14:24		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows;
using BRX.DAL.CalDAL.CalDALModel;
using BRX.DAL.ExpDAL.ExpDALModel;
using BRX.Model.Enums;
using BRX.Model.Exp;
using GalaSoft.MvvmLight;
using NPOI.Util;

namespace BRX.DAL.CalDAL
{
    public partial class CalDAL : ObservableObject
    {
        /// <summary>
        /// 保存A20试验基本参数
        /// </summary>
        private void SaveA20()
        {
            string saveExpNo = WallCalDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.A20炉壁温度校准试验参数Row checkExistA20Row = A20Table.FindBy试验编号(saveExpNo);
                if (checkExistA20Row == null)
                {
                    DBDataSet.A20炉壁温度校准试验参数Row defA20Row = A20Table.FindBy试验编号("DefaultWallCal");
                    DBDataSet.A20炉壁温度校准试验参数Row newA20Row = A20Table.NewA20炉壁温度校准试验参数Row();
                    newA20Row.ItemArray = (object[])defA20Row.ItemArray.Clone();
                    newA20Row.试验编号 = saveExpNo;
                    A20Table.AddA20炉壁温度校准试验参数Row(newA20Row);
                    A20TableAdapter.Update(A20Table);
                    A20Table.AcceptChanges();
                    RaisePropertyChanged(() => A20Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.A20炉壁温度校准试验参数Row expA20Row = A20Table.FindBy试验编号(saveExpNo);
                if (expA20Row != null)
                {
                    expA20Row.试验编号 = WallCalDQ.ExpNO;
                    expA20Row.试验补充说明 = WallCalDQ.ExpDetail;
                    expA20Row.报告编号 = WallCalDQ.RepNO;
                    expA20Row.创建日期时间 = WallCalDQ.CreatTime;
                    expA20Row.报告日期时间 = WallCalDQ.RepTime;
                    expA20Row.原始试验标志 = WallCalDQ.IsReal;
                    expA20Row.已完成标志 = WallCalDQ.IsCompleted;
                    expA20Row.a正30mm完成 = WallCalDQ.WallCalPointsList[0].IsReced;
                    expA20Row.b0mm完成 = WallCalDQ.WallCalPointsList[1].IsReced;
                    expA20Row.c负30mm完成 = WallCalDQ.WallCalPointsList[2].IsReced;

                    A20TableAdapter.Update(A20Table);
                    A20Table.AcceptChanges();
                    RaisePropertyChanged(() => A20Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 保存A30炉内温度校准试验参数
        /// </summary>
        private void SaveA30()
        {
            string saveExpNo = CenterCalDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.A30炉内温度校准试验参数Row checkExistA30Row = A30Table.FindBy试验编号(saveExpNo);
                if (checkExistA30Row == null)
                {
                    DBDataSet.A30炉内温度校准试验参数Row defA30Row = A30Table.FindBy试验编号("DefaultCenterCal");
                    DBDataSet.A30炉内温度校准试验参数Row newA30Row = A30Table.NewA30炉内温度校准试验参数Row();
                    newA30Row.ItemArray = (object[])defA30Row.ItemArray.Clone();
                    newA30Row.试验编号 = saveExpNo;
                    A30Table.AddA30炉内温度校准试验参数Row(newA30Row);
                    A30TableAdapter.Update(A30Table);
                    A30Table.AcceptChanges();
                    RaisePropertyChanged(() => A30Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.A30炉内温度校准试验参数Row expA30Row = A30Table.FindBy试验编号(saveExpNo);
                if (expA30Row != null)
                {
                    expA30Row.试验编号 = CenterCalDQ.ExpNO;
                    expA30Row.试验补充说明 = CenterCalDQ.ExpDetail;
                    expA30Row.报告编号 = CenterCalDQ.RepNO;
                    expA30Row.创建日期时间 = CenterCalDQ.CreatTime;
                    expA30Row.报告日期时间 = CenterCalDQ.RepTime;
                    expA30Row.原始试验标志 = CenterCalDQ.IsReal;
                    expA30Row.已完成标志 = CenterCalDQ.IsCompleted;

                    expA30Row.完成标志75D = CenterCalDQ.CenterCalPointsList[0].IsReced;
                    expA30Row.完成标志65D = CenterCalDQ.CenterCalPointsList[1].IsReced;
                    expA30Row.完成标志55D = CenterCalDQ.CenterCalPointsList[2].IsReced;
                    expA30Row.完成标志45D = CenterCalDQ.CenterCalPointsList[3].IsReced;
                    expA30Row.完成标志35D = CenterCalDQ.CenterCalPointsList[4].IsReced;
                    expA30Row.完成标志25D = CenterCalDQ.CenterCalPointsList[5].IsReced;
                    expA30Row.完成标志15D = CenterCalDQ.CenterCalPointsList[6].IsReced;
                    expA30Row.完成标志5D = CenterCalDQ.CenterCalPointsList[7].IsReced;
                    expA30Row.完成标志5U = CenterCalDQ.CenterCalPointsList[8].IsReced;
                    expA30Row.完成标志15U = CenterCalDQ.CenterCalPointsList[9].IsReced;
                    expA30Row.完成标志25U = CenterCalDQ.CenterCalPointsList[10].IsReced;
                    expA30Row.完成标志35U = CenterCalDQ.CenterCalPointsList[11].IsReced;
                    expA30Row.完成标志45U = CenterCalDQ.CenterCalPointsList[12].IsReced;
                    expA30Row.完成标志55U = CenterCalDQ.CenterCalPointsList[13].IsReced;
                    expA30Row.完成标志65U = CenterCalDQ.CenterCalPointsList[14].IsReced;
                    expA30Row.完成标志75U = CenterCalDQ.CenterCalPointsList[15].IsReced;
                    expA30Row.完成标志85U = CenterCalDQ.CenterCalPointsList[16].IsReced;
                    expA30Row.完成标志95U = CenterCalDQ.CenterCalPointsList[17].IsReced;
                    expA30Row.完成标志105U = CenterCalDQ.CenterCalPointsList[18].IsReced;
                    expA30Row.完成标志115U = CenterCalDQ.CenterCalPointsList[19].IsReced;
                    expA30Row.完成标志125U = CenterCalDQ.CenterCalPointsList[20].IsReced;
                    expA30Row.完成标志135U = CenterCalDQ.CenterCalPointsList[21].IsReced;
                    expA30Row.完成标志145U = CenterCalDQ.CenterCalPointsList[22].IsReced;
                    expA30Row.完成标志145D = CenterCalDQ.CenterCalPointsList[23].IsReced;
                    expA30Row.完成标志135D = CenterCalDQ.CenterCalPointsList[24].IsReced;
                    expA30Row.完成标志125D = CenterCalDQ.CenterCalPointsList[25].IsReced;
                    expA30Row.完成标志115D = CenterCalDQ.CenterCalPointsList[26].IsReced;
                    expA30Row.完成标志105D = CenterCalDQ.CenterCalPointsList[27].IsReced;
                    expA30Row.完成标志95D = CenterCalDQ.CenterCalPointsList[28].IsReced;
                    expA30Row.完成标志85D = CenterCalDQ.CenterCalPointsList[29].IsReced;

                    expA30Row.IsFit_5 = CenterCalDQ.IsFitStdList[0];
                    expA30Row.IsFit_15 = CenterCalDQ.IsFitStdList[1];
                    expA30Row.IsFit_25 = CenterCalDQ.IsFitStdList[2];
                    expA30Row.IsFit_35 = CenterCalDQ.IsFitStdList[3];
                    expA30Row.IsFit_45 = CenterCalDQ.IsFitStdList[4];
                    expA30Row.IsFit_55 = CenterCalDQ.IsFitStdList[5];
                    expA30Row.IsFit_65 = CenterCalDQ.IsFitStdList[6];
                    expA30Row.IsFit_75 = CenterCalDQ.IsFitStdList[7];
                    expA30Row.IsFit_85 = CenterCalDQ.IsFitStdList[8];
                    expA30Row.IsFit_95 = CenterCalDQ.IsFitStdList[9];
                    expA30Row.IsFit_105 = CenterCalDQ.IsFitStdList[10];
                    expA30Row.IsFit_115 = CenterCalDQ.IsFitStdList[11];
                    expA30Row.IsFit_125 = CenterCalDQ.IsFitStdList[12];
                    expA30Row.IsFit_135 = CenterCalDQ.IsFitStdList[13];
                    expA30Row.IsFit_145 = CenterCalDQ.IsFitStdList[14];

                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[0];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[1];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[2];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[3];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[4];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[5];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[6];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[7];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[8];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[9];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[10];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[11];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[12];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[13];
                    expA30Row.Tfc_Avg_5 = CenterCalDQ.TAvgList[14];

                    A30TableAdapter.Update(A30Table);
                    A30Table.AcceptChanges();
                    RaisePropertyChanged(() => A30Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 保存B20炉壁温度校准数据
        /// </summary>
        private void SaveB20()
        {
            string saveExpNo = WallCalDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B20炉壁温度校准数据Row checkExistB20Row = B20Table.FindBy试验编号(saveExpNo);
                if (checkExistB20Row == null)
                {
                    DBDataSet.B20炉壁温度校准数据Row defB20Row = B20Table.FindBy试验编号("DefaultWallCal");
                    DBDataSet.B20炉壁温度校准数据Row newB20Row = B20Table.NewB20炉壁温度校准数据Row();
                    newB20Row.ItemArray = (object[])defB20Row.ItemArray.Clone();
                    newB20Row.试验编号 = saveExpNo;
                    B20Table.AddB20炉壁温度校准数据Row(newB20Row);
                    B20TableAdapter.Update(B20Table);
                    B20Table.AcceptChanges();
                    RaisePropertyChanged(() => B20Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B20炉壁温度校准数据Row expB20Row = B20Table.FindBy试验编号(saveExpNo);
                if (expB20Row != null)
                {
                    expB20Row.试验编号 = WallCalDQ.ExpNO;
                    expB20Row.t_b = WallCalDQ.WallCalPointsList[1].RecTime;
                    expB20Row.IsRecorded_b = WallCalDQ.WallCalPointsList[1].IsReced;
                    expB20Row.Tw1_b = WallCalDQ.WallCalPointsList[1].T1;
                    expB20Row.Tw2_b = WallCalDQ.WallCalPointsList[1].T2;
                    expB20Row.Tw3_b = WallCalDQ.WallCalPointsList[1].T3;
                    expB20Row.Tf1_b = WallCalDQ.WallCalPointsList[1].Tf1;
                    expB20Row.Tf2_b = WallCalDQ.WallCalPointsList[1].Tf2;
                    expB20Row.t_a = WallCalDQ.WallCalPointsList[0].RecTime;
                    expB20Row.IsRecorded_a = WallCalDQ.WallCalPointsList[0].IsReced;
                    expB20Row.Tw1_a = WallCalDQ.WallCalPointsList[0].T1;
                    expB20Row.Tw2_a = WallCalDQ.WallCalPointsList[0].T2;
                    expB20Row.Tw3_a = WallCalDQ.WallCalPointsList[0].T3;
                    expB20Row.Tf1_a = WallCalDQ.WallCalPointsList[0].Tf1;
                    expB20Row.Tf2_a = WallCalDQ.WallCalPointsList[0].Tf2;
                    expB20Row.t_c = WallCalDQ.WallCalPointsList[2].RecTime;
                    expB20Row.IsRecorded_c = WallCalDQ.WallCalPointsList[2].IsReced;
                    expB20Row.Tw1_c = WallCalDQ.WallCalPointsList[2].T1;
                    expB20Row.Tw2_c = WallCalDQ.WallCalPointsList[2].T2;
                    expB20Row.Tw3_c = WallCalDQ.WallCalPointsList[2].T3;
                    expB20Row.Tf1_c = WallCalDQ.WallCalPointsList[2].Tf1;
                    expB20Row.Tf2_c = WallCalDQ.WallCalPointsList[2].Tf2;
                    expB20Row.Tavg = WallCalDQ.Tavg;
                    expB20Row.Tavg_axis1 = WallCalDQ.Tavg_axis1;
                    expB20Row.Tavg_axis2 = WallCalDQ.Tavg_axis2;
                    expB20Row.Tavg_axis3 = WallCalDQ.Tavg_axis3;
                    expB20Row.Tdev_axis1 = WallCalDQ.Tdev_axis1;
                    expB20Row.Tdev_axis2 = WallCalDQ.Tdev_axis2;
                    expB20Row.Tdev_axis3 = WallCalDQ.Tdev_axis3;
                    expB20Row.Tavg_dev_axis = WallCalDQ.Tavg_dev_axis;
                    expB20Row.Tavg_levelb = WallCalDQ.Tavg_levelb;
                    expB20Row.Tavg_levela = WallCalDQ.Tavg_levela;
                    expB20Row.Tavg_levelc = WallCalDQ.Tavg_levelc;
                    expB20Row.Tdev_levelb = WallCalDQ.Tdev_levelb;
                    expB20Row.Tdev_levela = WallCalDQ.Tdev_levela;
                    expB20Row.Tdev_levelc = WallCalDQ.Tdev_levelc;
                    expB20Row.Tavg_dev_level = WallCalDQ.Tavg_dev_level;
                    expB20Row.Result = WallCalDQ.Result;

                    B20TableAdapter.Update(B20Table);
                    B20Table.AcceptChanges();
                    RaisePropertyChanged(() => B20Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 保存B30试样5检测数据
        /// </summary>
        private void SaveB30()
        {
            string saveExpNo = CenterCalDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B30炉内温度校准数据Row checkExistB30Row = B30Table.FindBy试验编号(saveExpNo);
                if (checkExistB30Row == null)
                {
                    DBDataSet.B30炉内温度校准数据Row defB30Row = B30Table.FindBy试验编号("DefaultCenterCal");
                    DBDataSet.B30炉内温度校准数据Row newB30Row = B30Table.NewB30炉内温度校准数据Row();
                    newB30Row.ItemArray = (object[])defB30Row.ItemArray.Clone();
                    newB30Row.试验编号 = saveExpNo;
                    B30Table.AddB30炉内温度校准数据Row(newB30Row);
                    B30TableAdapter.Update(B30Table);
                    B30Table.AcceptChanges();
                    RaisePropertyChanged(() => B30Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B30炉内温度校准数据Row expB30Row = B30Table.FindBy试验编号(saveExpNo);
                if (expB30Row != null)
                {
                    expB30Row.试验编号 = CenterCalDQ.ExpNO;
                    expB30Row.t_5U = CenterCalDQ.CenterCalPointsList[8].RecTime;
                    expB30Row.Tfc_5U = CenterCalDQ.CenterCalPointsList[8].Tfc;
                    expB30Row.Tf1_5U = CenterCalDQ.CenterCalPointsList[8].Tf1;
                    expB30Row.Tf2_5U = CenterCalDQ.CenterCalPointsList[8].Tf2;

                    expB30Row.t_15U = CenterCalDQ.CenterCalPointsList[9].RecTime;
                    expB30Row.Tfc_15U = CenterCalDQ.CenterCalPointsList[9].Tfc;
                    expB30Row.Tf1_15U = CenterCalDQ.CenterCalPointsList[9].Tf1;
                    expB30Row.Tf2_15U = CenterCalDQ.CenterCalPointsList[9].Tf2;

                    expB30Row.t_25U = CenterCalDQ.CenterCalPointsList[10].RecTime;
                    expB30Row.Tfc_25U = CenterCalDQ.CenterCalPointsList[10].Tfc;
                    expB30Row.Tf1_25U = CenterCalDQ.CenterCalPointsList[10].Tf1;
                    expB30Row.Tf2_25U = CenterCalDQ.CenterCalPointsList[10].Tf2;

                    expB30Row.t_35U = CenterCalDQ.CenterCalPointsList[11].RecTime;
                    expB30Row.Tfc_35U = CenterCalDQ.CenterCalPointsList[11].Tfc;
                    expB30Row.Tf1_35U = CenterCalDQ.CenterCalPointsList[11].Tf1;
                    expB30Row.Tf2_35U = CenterCalDQ.CenterCalPointsList[11].Tf2;

                    expB30Row.t_45U = CenterCalDQ.CenterCalPointsList[12].RecTime;
                    expB30Row.Tfc_45U = CenterCalDQ.CenterCalPointsList[12].Tfc;
                    expB30Row.Tf1_45U = CenterCalDQ.CenterCalPointsList[12].Tf1;
                    expB30Row.Tf2_45U = CenterCalDQ.CenterCalPointsList[12].Tf2;

                    expB30Row.t_55U = CenterCalDQ.CenterCalPointsList[13].RecTime;
                    expB30Row.Tfc_55U = CenterCalDQ.CenterCalPointsList[13].Tfc;
                    expB30Row.Tf1_55U = CenterCalDQ.CenterCalPointsList[13].Tf1;
                    expB30Row.Tf2_55U = CenterCalDQ.CenterCalPointsList[13].Tf2;

                    expB30Row.t_65U = CenterCalDQ.CenterCalPointsList[14].RecTime;
                    expB30Row.Tfc_65U = CenterCalDQ.CenterCalPointsList[14].Tfc;
                    expB30Row.Tf1_65U = CenterCalDQ.CenterCalPointsList[14].Tf1;
                    expB30Row.Tf2_65U = CenterCalDQ.CenterCalPointsList[14].Tf2;

                    expB30Row.t_75U = CenterCalDQ.CenterCalPointsList[15].RecTime;
                    expB30Row.Tfc_75U = CenterCalDQ.CenterCalPointsList[15].Tfc;
                    expB30Row.Tf1_75U = CenterCalDQ.CenterCalPointsList[15].Tf1;
                    expB30Row.Tf2_75U = CenterCalDQ.CenterCalPointsList[15].Tf2;

                    expB30Row.t_5D = CenterCalDQ.CenterCalPointsList[7].RecTime;
                    expB30Row.Tfc_5D = CenterCalDQ.CenterCalPointsList[7].Tfc;
                    expB30Row.Tf1_5D = CenterCalDQ.CenterCalPointsList[7].Tf1;
                    expB30Row.Tf2_5D = CenterCalDQ.CenterCalPointsList[7].Tf2;

                    expB30Row.t_15D = CenterCalDQ.CenterCalPointsList[6].RecTime;
                    expB30Row.Tfc_15D = CenterCalDQ.CenterCalPointsList[6].Tfc;
                    expB30Row.Tf1_15D = CenterCalDQ.CenterCalPointsList[6].Tf1;
                    expB30Row.Tf2_15D = CenterCalDQ.CenterCalPointsList[6].Tf2;

                    expB30Row.t_25D = CenterCalDQ.CenterCalPointsList[5].RecTime;
                    expB30Row.Tfc_25D = CenterCalDQ.CenterCalPointsList[5].Tfc;
                    expB30Row.Tf1_25D = CenterCalDQ.CenterCalPointsList[5].Tf1;
                    expB30Row.Tf2_25D = CenterCalDQ.CenterCalPointsList[5].Tf2;

                    expB30Row.t_35D = CenterCalDQ.CenterCalPointsList[4].RecTime;
                    expB30Row.Tfc_35D = CenterCalDQ.CenterCalPointsList[4].Tfc;
                    expB30Row.Tf1_35D = CenterCalDQ.CenterCalPointsList[4].Tf1;
                    expB30Row.Tf2_35D = CenterCalDQ.CenterCalPointsList[4].Tf2;

                    expB30Row.t_45D = CenterCalDQ.CenterCalPointsList[3].RecTime;
                    expB30Row.Tfc_45D = CenterCalDQ.CenterCalPointsList[3].Tfc;
                    expB30Row.Tf1_45D = CenterCalDQ.CenterCalPointsList[3].Tf1;
                    expB30Row.Tf2_45D = CenterCalDQ.CenterCalPointsList[3].Tf2;

                    expB30Row.t_55D = CenterCalDQ.CenterCalPointsList[2].RecTime;
                    expB30Row.Tfc_55D = CenterCalDQ.CenterCalPointsList[2].Tfc;
                    expB30Row.Tf1_55D = CenterCalDQ.CenterCalPointsList[2].Tf1;
                    expB30Row.Tf2_55D = CenterCalDQ.CenterCalPointsList[2].Tf2;

                    expB30Row.t_65D = CenterCalDQ.CenterCalPointsList[1].RecTime;
                    expB30Row.Tfc_65D = CenterCalDQ.CenterCalPointsList[1].Tfc;
                    expB30Row.Tf1_65D = CenterCalDQ.CenterCalPointsList[1].Tf1;
                    expB30Row.Tf2_65D = CenterCalDQ.CenterCalPointsList[1].Tf2;

                    expB30Row.t_75D = CenterCalDQ.CenterCalPointsList[0].RecTime;
                    expB30Row.Tfc_75D = CenterCalDQ.CenterCalPointsList[0].Tfc;
                    expB30Row.Tf1_75D = CenterCalDQ.CenterCalPointsList[0].Tf1;
                    expB30Row.Tf2_75D = CenterCalDQ.CenterCalPointsList[0].Tf2;
                    
                    expB30Row.Result = CenterCalDQ.Result;


                    B30TableAdapter.Update(B30Table);
                    B30Table.AcceptChanges();
                    RaisePropertyChanged(() => B30Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 保存B31试样5检测数据
        /// </summary>
        private void SaveB31()
        {
            string saveExpNo = CenterCalDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B31炉内温度校准数据2Row checkExistB31Row = B31Table.FindBy试验编号(saveExpNo);
                if (checkExistB31Row == null)
                {
                    DBDataSet.B31炉内温度校准数据2Row defB31Row = B31Table.FindBy试验编号("DefaultCenterCal");
                    DBDataSet.B31炉内温度校准数据2Row newB31Row = B31Table.NewB31炉内温度校准数据2Row();
                    newB31Row.ItemArray = (object[])defB31Row.ItemArray.Clone();
                    newB31Row.试验编号 = saveExpNo;
                    B31Table.AddB31炉内温度校准数据2Row(newB31Row);
                    B31TableAdapter.Update(B31Table);
                    B31Table.AcceptChanges();
                    RaisePropertyChanged(() => B31Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B31炉内温度校准数据2Row expB31Row = B31Table.FindBy试验编号(saveExpNo);
                if (expB31Row != null)
                {
                    expB31Row.试验编号 = CenterCalDQ.ExpNO;
                   
                    expB31Row.t_85U = CenterCalDQ.CenterCalPointsList[16].RecTime;
                    expB31Row.Tfc_85U = CenterCalDQ.CenterCalPointsList[16].Tfc;
                    expB31Row.Tf1_85U = CenterCalDQ.CenterCalPointsList[16].Tf1;
                    expB31Row.Tf2_85U = CenterCalDQ.CenterCalPointsList[16].Tf2;

                    expB31Row.t_95U = CenterCalDQ.CenterCalPointsList[17].RecTime;
                    expB31Row.Tfc_95U = CenterCalDQ.CenterCalPointsList[17].Tfc;
                    expB31Row.Tf1_95U = CenterCalDQ.CenterCalPointsList[17].Tf1;
                    expB31Row.Tf2_95U = CenterCalDQ.CenterCalPointsList[17].Tf2;

                    expB31Row.t_105U = CenterCalDQ.CenterCalPointsList[18].RecTime;
                    expB31Row.Tfc_105U = CenterCalDQ.CenterCalPointsList[18].Tfc;
                    expB31Row.Tf1_105U = CenterCalDQ.CenterCalPointsList[18].Tf1;
                    expB31Row.Tf2_105U = CenterCalDQ.CenterCalPointsList[18].Tf2;

                    expB31Row.t_115U = CenterCalDQ.CenterCalPointsList[19].RecTime;
                    expB31Row.Tfc_115U = CenterCalDQ.CenterCalPointsList[19].Tfc;
                    expB31Row.Tf1_115U = CenterCalDQ.CenterCalPointsList[19].Tf1;
                    expB31Row.Tf2_115U = CenterCalDQ.CenterCalPointsList[19].Tf2;

                    expB31Row.t_125U = CenterCalDQ.CenterCalPointsList[20].RecTime;
                    expB31Row.Tfc_125U = CenterCalDQ.CenterCalPointsList[20].Tfc;
                    expB31Row.Tf1_125U = CenterCalDQ.CenterCalPointsList[20].Tf1;
                    expB31Row.Tf2_125U = CenterCalDQ.CenterCalPointsList[20].Tf2;

                    expB31Row.t_135U = CenterCalDQ.CenterCalPointsList[21].RecTime;
                    expB31Row.Tfc_135U = CenterCalDQ.CenterCalPointsList[21].Tfc;
                    expB31Row.Tf1_135U = CenterCalDQ.CenterCalPointsList[21].Tf1;
                    expB31Row.Tf2_135U = CenterCalDQ.CenterCalPointsList[21].Tf2;

                    expB31Row.t_145U = CenterCalDQ.CenterCalPointsList[22].RecTime;
                    expB31Row.Tfc_145U = CenterCalDQ.CenterCalPointsList[22].Tfc;
                    expB31Row.Tf1_145U = CenterCalDQ.CenterCalPointsList[22].Tf1;
                    expB31Row.Tf2_145U = CenterCalDQ.CenterCalPointsList[22].Tf2;
                    
                    expB31Row.t_85D = CenterCalDQ.CenterCalPointsList[29].RecTime;
                    expB31Row.Tfc_85D = CenterCalDQ.CenterCalPointsList[29].Tfc;
                    expB31Row.Tf1_85D = CenterCalDQ.CenterCalPointsList[29].Tf1;
                    expB31Row.Tf2_85D = CenterCalDQ.CenterCalPointsList[29].Tf2;

                    expB31Row.t_95D = CenterCalDQ.CenterCalPointsList[28].RecTime;
                    expB31Row.Tfc_95D = CenterCalDQ.CenterCalPointsList[28].Tfc;
                    expB31Row.Tf1_95D = CenterCalDQ.CenterCalPointsList[28].Tf1;
                    expB31Row.Tf2_95D = CenterCalDQ.CenterCalPointsList[28].Tf2;

                    expB31Row.t_105D = CenterCalDQ.CenterCalPointsList[27].RecTime;
                    expB31Row.Tfc_105D = CenterCalDQ.CenterCalPointsList[27].Tfc;
                    expB31Row.Tf1_105D = CenterCalDQ.CenterCalPointsList[27].Tf1;
                    expB31Row.Tf2_105D = CenterCalDQ.CenterCalPointsList[27].Tf2;

                    expB31Row.t_115D = CenterCalDQ.CenterCalPointsList[26].RecTime;
                    expB31Row.Tfc_115D = CenterCalDQ.CenterCalPointsList[26].Tfc;
                    expB31Row.Tf1_115D = CenterCalDQ.CenterCalPointsList[26].Tf1;
                    expB31Row.Tf2_115D = CenterCalDQ.CenterCalPointsList[26].Tf2;

                    expB31Row.t_125D = CenterCalDQ.CenterCalPointsList[25].RecTime;
                    expB31Row.Tfc_125D = CenterCalDQ.CenterCalPointsList[25].Tfc;
                    expB31Row.Tf1_125D = CenterCalDQ.CenterCalPointsList[25].Tf1;
                    expB31Row.Tf2_125D = CenterCalDQ.CenterCalPointsList[25].Tf2;

                    expB31Row.t_135D = CenterCalDQ.CenterCalPointsList[24].RecTime;
                    expB31Row.Tfc_135D = CenterCalDQ.CenterCalPointsList[24].Tfc;
                    expB31Row.Tf1_135D = CenterCalDQ.CenterCalPointsList[24].Tf1;
                    expB31Row.Tf2_135D = CenterCalDQ.CenterCalPointsList[24].Tf2;

                    expB31Row.t_145D = CenterCalDQ.CenterCalPointsList[23].RecTime;
                    expB31Row.Tfc_145D = CenterCalDQ.CenterCalPointsList[23].Tfc;
                    expB31Row.Tf1_145D = CenterCalDQ.CenterCalPointsList[23].Tf1;
                    expB31Row.Tf2_145D = CenterCalDQ.CenterCalPointsList[23].Tf2;


                    B31TableAdapter.Update(B31Table);
                    B31Table.AcceptChanges();
                    RaisePropertyChanged(() => B31Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 保存炉壁校准原始数据记录WallCalInitDB
        /// </summary>
        private void SaveWallCalInitData(WallCalInitRec rec)
        {
            try
            {
                WallCalInitRec recWillSave = new WallCalInitRec
                {
                    ExpNO = rec.ExpNO,
                    HeightNO = rec.HeightNO,
                    Height = rec.Height,
                    RecNum = rec.RecNum,
                    RecTime = rec.RecTime,
                    Tf1 = rec.Tf1,
                    Tf2 = rec.Tf2,
                    T1 = rec.T1,
                    T2 = rec.T2,
                    T3 = rec.T3,
                    Vo = rec.Vo
                };

                List<WallCalInitRec> tempRecList = WallCalInitDB.Queryable<WallCalInitRec>().AS(recWillSave.ExpNO).Where(it => it.HeightNO == recWillSave.HeightNO && it.RecNum == recWillSave.RecNum).ToList();
                if(tempRecList.Count > 0)
                    WallCalInitDB.Deleteable<WallCalInitRec>().AS(recWillSave.ExpNO).Where(it => it.HeightNO == recWillSave.HeightNO && it.RecNum == recWillSave.RecNum).ExecuteCommand();
                WallCalInitDB.Insertable<WallCalInitRec>(recWillSave).AS(recWillSave.ExpNO).ExecuteCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        /// <summary>
        /// 保存炉内校准原始数据记录CenterCalInitDB
        /// </summary>
        private void SaveCenterCalInitData(CenterCalInitRec rec)
        {
            try
            {
                CenterCalInitRec recWillSave = new CenterCalInitRec
                {
                    ExpNO = rec.ExpNO,
                    HeightNO = rec.HeightNO,
                    Height = rec.Height,
                    RecNum = rec.RecNum,
                    RecTime = rec.RecTime,
                    Tf1 = rec.Tf1,
                    Tf2 = rec.Tf2,
                    Tc = rec.Tc,
                    Vo = rec.Vo
                };

                List<CenterCalInitRec> tempRecList = CenterCalInitDB.Queryable<CenterCalInitRec>().AS(recWillSave.ExpNO).Where(it => it.HeightNO == recWillSave.HeightNO && it.RecNum == recWillSave.RecNum).ToList();
                if (tempRecList.Count > 0)
                    CenterCalInitDB.Deleteable<CenterCalInitRec>().AS(recWillSave.ExpNO).Where(it => it.HeightNO == recWillSave.HeightNO && it.RecNum == recWillSave.RecNum).ExecuteCommand();
                CenterCalInitDB.Insertable<CenterCalInitRec>(recWillSave).AS(recWillSave.ExpNO).ExecuteCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

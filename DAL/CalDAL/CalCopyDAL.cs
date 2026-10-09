/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL.CalDAL
 * 文件名：  ExpCopyDAL
 * 版本号：  V1.0.0.0
 * 唯一标识：15353074-3dde-4a80-87f1-584b35f07bb7
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-4-5 10:18:00
 * 描述：
 * 试验读写。新建部分。
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/22 23:14:24		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using BRX.DAL.CalDAL.CalDALModel;
using BRX.DAL.ExpDAL.ExpDALModel;
using GalaSoft.MvvmLight;

namespace BRX.DAL.CalDAL
{
    public partial class CalDAL : ObservableObject
    {
       /// <summary>
        /// 根据名称复制炉壁校准试验
        /// </summary>
        private bool CopyWallCal(string OldNo, string expNo, bool isNew)
        {
            //检查编号字符长度
            if (OldNo.Length <= 0)
            {
                MessageBox.Show("旧编号长度有误", "错误提示");
                return false;
            }
            if ((expNo.Length <= 0) || (expNo.Length > 50))
            {
                MessageBox.Show("新试验编号长度有误", "错误提示");
                return false;
            }

            string oldExpNo = OldNo.Clone().ToString();
            string newExpNo = expNo.Clone().ToString();

            #region 检查旧表数据

            //A20主表
            try
            {
                BRX.DBDataSet.A20炉壁温度校准试验参数Row checkA20Row = A20Table.NewA20炉壁温度校准试验参数Row();
                checkA20Row = A20Table.FindBy试验编号(oldExpNo);
                if (checkA20Row == null)
                {
                    MessageBox.Show("编号" + oldExpNo + "A20不存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            #endregion

            #region 检查新编号是否重复

            //A20表
            try
            {
                BRX.DBDataSet.A20炉壁温度校准试验参数Row checkA20Row = A20Table.NewA20炉壁温度校准试验参数Row();
                checkA20Row = A20Table.FindBy试验编号(newExpNo);
                if (checkA20Row != null)
                {
                    MessageBox.Show("此编号在A20表中已存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B20表
            try
            {
                BRX.DBDataSet.B20炉壁温度校准数据Row checkB20Row = B20Table.NewB20炉壁温度校准数据Row();
                checkB20Row = B20Table.FindBy试验编号(newExpNo);
                if (checkB20Row != null)
                {
                    MessageBox.Show("此编号在B20表中已存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            
            #endregion

            MessageBoxResult msgBoxResult = MessageBox.Show("确认新建" + newExpNo + "号试验？", "检查提示", MessageBoxButton.YesNo);
            if (msgBoxResult == MessageBoxResult.No)
            {
                return false;
            }

            #region 复制表

            //A20主表
            try
            {
                BRX.DBDataSet.A20炉壁温度校准试验参数Row oldExpA20Row = A20Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.A20炉壁温度校准试验参数Row newExpA20Row = A20Table.NewA20炉壁温度校准试验参数Row();
                newExpA20Row.ItemArray = (object[])oldExpA20Row.ItemArray.Clone();
                newExpA20Row.试验编号 = newExpNo;
                newExpA20Row.报告编号 = newExpNo + "RPT";
                newExpA20Row.试验补充说明 = "//";
                newExpA20Row.原始试验标志 = true;
                newExpA20Row.创建日期时间 = DateTime.Now;
                if (isNew)
                {
                    newExpA20Row.已完成标志 = false;
                    newExpA20Row.a正30mm完成 = false;
                    newExpA20Row.b0mm完成 = false;
                    newExpA20Row.c负30mm完成 = false;
                }

                A20Table.AddA20炉壁温度校准试验参数Row(newExpA20Row);
                A20TableAdapter.Update(A20Table);
                A20Table.AcceptChanges();
                RaisePropertyChanged(() => A20Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B20表
            try
            {
                BRX.DBDataSet.B20炉壁温度校准数据Row oldExpB20Row = B20Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.B20炉壁温度校准数据Row newExpB20Row = B20Table.NewB20炉壁温度校准数据Row();
                newExpB20Row.ItemArray = (object[])oldExpB20Row.ItemArray.Clone();
                newExpB20Row.试验编号 = newExpNo;
                if (isNew)
                {
                    newExpB20Row.t_b = DateTime.MinValue;
                    newExpB20Row.IsRecorded_b = false;
                    newExpB20Row.Tw1_b = 0;
                    newExpB20Row.Tw2_b = 0;
                    newExpB20Row.Tw3_b = 0;
                    newExpB20Row.Tf1_b = 0;
                    newExpB20Row.Tf2_b = 0; 
                    
                    newExpB20Row.t_a = DateTime.MinValue;
                    newExpB20Row.IsRecorded_a = false;
                    newExpB20Row.Tw1_a = 0;
                    newExpB20Row.Tw2_a = 0;
                    newExpB20Row.Tw3_a = 0;
                    newExpB20Row.Tf1_a = 0;
                    newExpB20Row.Tf2_a = 0;

                    newExpB20Row.t_c = DateTime.MinValue;
                    newExpB20Row.IsRecorded_c = false;
                    newExpB20Row.Tw1_c = 0;
                    newExpB20Row.Tw2_c = 0;
                    newExpB20Row.Tw3_c = 0;
                    newExpB20Row.Tf1_c = 0;
                    newExpB20Row.Tf2_c = 0;
                }

                B20Table.AddB20炉壁温度校准数据Row(newExpB20Row);
                B20TableAdapter.Update(B20Table);
                B20Table.AcceptChanges();
                RaisePropertyChanged(() => B20Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }


            //炉壁校准原始数据表
            try
            {
                //创建新表
                bool isExists = WallCalInitDB.DbMaintenance.IsAnyTable(expNo, false);
                if (isExists)
                    WallCalInitDB.DbMaintenance.DropTable(expNo);
                WallCalInitDB.CodeFirst.As<WallCalInitRec>(expNo).InitTables<WallCalInitRec>();
                //复制内容
                if (!isNew)
                {
                    List<WallCalInitRec> tempList = WallCalInitDB.Queryable<WallCalInitRec>().AS(oldExpNo).ToList();
                    for (int i = 0; i < tempList.Count; i++)
                        tempList[i].ExpNO = expNo;
                    WallCalInitDB.Insertable<WallCalInitRec>(tempList).AS(expNo).ExecuteCommand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            #endregion

            return true;
        }


        /// <summary>
        /// 根据名称复制炉内校准试验
        /// </summary>
        private bool CopyCenterCal(string OldNo, string expNo, bool isNew)
        {
            //检查编号字符长度
            if (OldNo.Length <= 0)
            {
                MessageBox.Show("旧编号长度有误", "错误提示");
                return false;
            }
            if ((expNo.Length <= 0) || (expNo.Length > 50))
            {
                MessageBox.Show("新试验编号长度有误", "错误提示");
                return false;
            }

            string oldExpNo = OldNo.Clone().ToString();
            string newExpNo = expNo.Clone().ToString();

            #region 检查旧表数据

            //A30主表
            try
            {
                BRX.DBDataSet.A30炉内温度校准试验参数Row checkA30Row = A30Table.NewA30炉内温度校准试验参数Row();
                checkA30Row = A30Table.FindBy试验编号(oldExpNo);
                if (checkA30Row == null)
                {
                    MessageBox.Show("编号" + oldExpNo + "A30不存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            #endregion

            #region 检查新编号是否重复

            //A30表
            try
            {
                BRX.DBDataSet.A30炉内温度校准试验参数Row checkA30Row = A30Table.NewA30炉内温度校准试验参数Row();
                checkA30Row = A30Table.FindBy试验编号(newExpNo);
                if (checkA30Row != null)
                {
                    MessageBox.Show("此编号在A30表中已存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B30表
            try
            {
                BRX.DBDataSet.B30炉内温度校准数据Row checkB30Row = B30Table.NewB30炉内温度校准数据Row();
                checkB30Row = B30Table.FindBy试验编号(newExpNo);
                if (checkB30Row != null)
                {
                    MessageBox.Show("此编号在B30表中已存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B31表
            try
            {
                BRX.DBDataSet.B31炉内温度校准数据2Row checkB31Row = B31Table.NewB31炉内温度校准数据2Row();
                checkB31Row = B31Table.FindBy试验编号(newExpNo);
                if (checkB31Row != null)
                {
                    MessageBox.Show("此编号在B31表中已存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            #endregion

            MessageBoxResult msgBoxResult = MessageBox.Show("确认新建" + newExpNo + "号试验？", "检查提示", MessageBoxButton.YesNo);
            if (msgBoxResult == MessageBoxResult.No)
            {
                return false;
            }

            #region 复制表

            //A30主表
            try
            {
                BRX.DBDataSet.A30炉内温度校准试验参数Row oldExpA30Row = A30Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.A30炉内温度校准试验参数Row newExpA30Row = A30Table.NewA30炉内温度校准试验参数Row();
                newExpA30Row.ItemArray = (object[])oldExpA30Row.ItemArray.Clone();
                newExpA30Row.试验编号 = newExpNo;
                newExpA30Row.报告编号 = newExpNo + "RPT";
                newExpA30Row.试验补充说明 = "//";
                newExpA30Row.原始试验标志 = true;
                newExpA30Row.创建日期时间 = DateTime.Now;
                if (isNew)
                {
                    newExpA30Row.完成标志75D = false;
                    newExpA30Row.完成标志65D = false;
                    newExpA30Row.完成标志55D = false;
                    newExpA30Row.完成标志45D = false;
                    newExpA30Row.完成标志35D = false;
                    newExpA30Row.完成标志25D = false;
                    newExpA30Row.完成标志15D = false;
                    newExpA30Row.完成标志5D = false;
                    newExpA30Row.完成标志5U = false;
                    newExpA30Row.完成标志15U = false;
                    newExpA30Row.完成标志25U = false;
                    newExpA30Row.完成标志35U = false;
                    newExpA30Row.完成标志45U = false;
                    newExpA30Row.完成标志55U = false;
                    newExpA30Row.完成标志65U = false;
                    newExpA30Row.完成标志75U = false;
                    newExpA30Row.完成标志85U = false;
                    newExpA30Row.完成标志95U = false;
                    newExpA30Row.完成标志105U = false;
                    newExpA30Row.完成标志115U = false;
                    newExpA30Row.完成标志125U = false;
                    newExpA30Row.完成标志135U = false;
                    newExpA30Row.完成标志145U = false;
                    newExpA30Row.完成标志145D = false;
                    newExpA30Row.完成标志135D = false;
                    newExpA30Row.完成标志125D = false;
                    newExpA30Row.完成标志115D = false;
                    newExpA30Row.完成标志105D = false;
                    newExpA30Row.完成标志95D = false;
                    newExpA30Row.完成标志85D = false;

                    newExpA30Row.IsFit_5 = false;
                    newExpA30Row.IsFit_15 = false;
                    newExpA30Row.IsFit_25 = false;
                    newExpA30Row.IsFit_35 = false;
                    newExpA30Row.IsFit_45 = false;
                    newExpA30Row.IsFit_55 = false;
                    newExpA30Row.IsFit_65 = false;
                    newExpA30Row.IsFit_75 = false;
                    newExpA30Row.IsFit_85 = false;
                    newExpA30Row.IsFit_95 = false;
                    newExpA30Row.IsFit_105 = false;
                    newExpA30Row.IsFit_115 = false;
                    newExpA30Row.IsFit_125 = false;
                    newExpA30Row.IsFit_135 = false;
                    newExpA30Row.IsFit_145 = false;

                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                    newExpA30Row.Tfc_Avg_5 = 0;
                }

                A30Table.AddA30炉内温度校准试验参数Row(newExpA30Row);
                A30TableAdapter.Update(A30Table);
                A30Table.AcceptChanges();
                RaisePropertyChanged(() => A30Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B30表
            try
            {
                BRX.DBDataSet.B30炉内温度校准数据Row oldExpB30Row = B30Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.B30炉内温度校准数据Row newExpB30Row = B30Table.NewB30炉内温度校准数据Row();
                newExpB30Row.ItemArray = (object[])oldExpB30Row.ItemArray.Clone();
                newExpB30Row.试验编号 = newExpNo;
                if (isNew)
                {
                    newExpB30Row.t_5U = DateTime.MinValue;
                    newExpB30Row.Tfc_5U = 0;
                    newExpB30Row.Tf1_5U = 0;
                    newExpB30Row.Tf2_5U = 0;

                    newExpB30Row.t_15U = DateTime.MinValue;
                    newExpB30Row.Tfc_15U = 0;
                    newExpB30Row.Tf1_15U = 0;
                    newExpB30Row.Tf2_15U = 0;

                    newExpB30Row.t_25U = DateTime.MinValue;
                    newExpB30Row.Tfc_25U = 0;
                    newExpB30Row.Tf1_25U = 0;
                    newExpB30Row.Tf2_25U = 0;

                    newExpB30Row.t_35U = DateTime.MinValue;
                    newExpB30Row.Tfc_35U = 0;
                    newExpB30Row.Tf1_35U = 0;
                    newExpB30Row.Tf2_35U = 0;

                    newExpB30Row.t_45U = DateTime.MinValue;
                    newExpB30Row.Tfc_45U = 0;
                    newExpB30Row.Tf1_45U = 0;
                    newExpB30Row.Tf2_45U = 0;

                    newExpB30Row.t_55U = DateTime.MinValue;
                    newExpB30Row.Tfc_55U = 0;
                    newExpB30Row.Tf1_55U = 0;
                    newExpB30Row.Tf2_55U = 0;

                    newExpB30Row.t_65U = DateTime.MinValue;
                    newExpB30Row.Tfc_65U = 0;
                    newExpB30Row.Tf1_65U = 0;
                    newExpB30Row.Tf2_65U = 0;

                    newExpB30Row.t_75U = DateTime.MinValue;
                    newExpB30Row.Tfc_75U = 0;
                    newExpB30Row.Tf1_75U = 0;
                    newExpB30Row.Tf2_75U = 0;
 
                    newExpB30Row.t_5D = DateTime.MinValue;
                    newExpB30Row.Tfc_5D = 0;
                    newExpB30Row.Tf1_5D = 0;
                    newExpB30Row.Tf2_5D = 0;

                    newExpB30Row.t_15D = DateTime.MinValue;
                    newExpB30Row.Tfc_15D = 0;
                    newExpB30Row.Tf1_15D = 0;
                    newExpB30Row.Tf2_15D = 0;

                    newExpB30Row.t_25D = DateTime.MinValue;
                    newExpB30Row.Tfc_25D = 0;
                    newExpB30Row.Tf1_25D = 0;
                    newExpB30Row.Tf2_25D = 0;

                    newExpB30Row.t_35D = DateTime.MinValue;
                    newExpB30Row.Tfc_35D = 0;
                    newExpB30Row.Tf1_35D = 0;
                    newExpB30Row.Tf2_35D = 0;

                    newExpB30Row.t_45D = DateTime.MinValue;
                    newExpB30Row.Tfc_45D = 0;
                    newExpB30Row.Tf1_45D = 0;
                    newExpB30Row.Tf2_45D = 0;

                    newExpB30Row.t_55D = DateTime.MinValue;
                    newExpB30Row.Tfc_55D = 0;
                    newExpB30Row.Tf1_55D = 0;
                    newExpB30Row.Tf2_55D = 0;

                    newExpB30Row.t_65D = DateTime.MinValue;
                    newExpB30Row.Tfc_65D = 0;
                    newExpB30Row.Tf1_65D = 0;
                    newExpB30Row.Tf2_65D = 0;

                    newExpB30Row.t_75D = DateTime.MinValue;
                    newExpB30Row.Tfc_75D = 0;
                    newExpB30Row.Tf1_75D = 0;
                    newExpB30Row.Tf2_75D = 0;

                    newExpB30Row.Result = "不合格";

                }

                B30Table.AddB30炉内温度校准数据Row(newExpB30Row);
                B30TableAdapter.Update(B30Table);
                B30Table.AcceptChanges();
                RaisePropertyChanged(() => B30Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B31表
            try
            {
                BRX.DBDataSet.B31炉内温度校准数据2Row oldExpB31Row = B31Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.B31炉内温度校准数据2Row newExpB31Row = B31Table.NewB31炉内温度校准数据2Row();
                newExpB31Row.ItemArray = (object[])oldExpB31Row.ItemArray.Clone();
                newExpB31Row.试验编号 = newExpNo;
                if (isNew)
                {
                   newExpB31Row.t_85U = DateTime.MinValue;
                    newExpB31Row.Tfc_85U = 0;
                    newExpB31Row.Tf1_85U = 0;
                    newExpB31Row.Tf2_85U = 0;

                    newExpB31Row.t_95U = DateTime.MinValue;
                    newExpB31Row.Tfc_95U = 0;
                    newExpB31Row.Tf1_95U = 0;
                    newExpB31Row.Tf2_95U = 0;

                    newExpB31Row.t_105U = DateTime.MinValue;
                    newExpB31Row.Tfc_105U = 0;
                    newExpB31Row.Tf1_105U = 0;
                    newExpB31Row.Tf2_105U = 0;

                    newExpB31Row.t_115U = DateTime.MinValue;
                    newExpB31Row.Tfc_115U = 0;
                    newExpB31Row.Tf1_115U = 0;
                    newExpB31Row.Tf2_115U = 0;

                    newExpB31Row.t_125U = DateTime.MinValue;
                    newExpB31Row.Tfc_125U = 0;
                    newExpB31Row.Tf1_125U = 0;
                    newExpB31Row.Tf2_125U = 0;

                    newExpB31Row.t_135U = DateTime.MinValue;
                    newExpB31Row.Tfc_135U = 0;
                    newExpB31Row.Tf1_135U = 0;
                    newExpB31Row.Tf2_135U = 0;

                    newExpB31Row.t_145U = DateTime.MinValue;
                    newExpB31Row.Tfc_145U = 0;
                    newExpB31Row.Tf1_145U = 0;
                    newExpB31Row.Tf2_145U = 0;
                    
                    newExpB31Row.t_85D = DateTime.MinValue;
                    newExpB31Row.Tfc_85D = 0;
                    newExpB31Row.Tf1_85D = 0;
                    newExpB31Row.Tf2_85D = 0;

                    newExpB31Row.t_95D = DateTime.MinValue;
                    newExpB31Row.Tfc_95D = 0;
                    newExpB31Row.Tf1_95D = 0;
                    newExpB31Row.Tf2_95D = 0;

                    newExpB31Row.t_105D = DateTime.MinValue;
                    newExpB31Row.Tfc_105D = 0;
                    newExpB31Row.Tf1_105D = 0;
                    newExpB31Row.Tf2_105D = 0;

                    newExpB31Row.t_115D = DateTime.MinValue;
                    newExpB31Row.Tfc_115D = 0;
                    newExpB31Row.Tf1_115D = 0;
                    newExpB31Row.Tf2_115D = 0;

                    newExpB31Row.t_125D = DateTime.MinValue;
                    newExpB31Row.Tfc_125D = 0;
                    newExpB31Row.Tf1_125D = 0;
                    newExpB31Row.Tf2_125D = 0;

                    newExpB31Row.t_135D = DateTime.MinValue;
                    newExpB31Row.Tfc_135D = 0;
                    newExpB31Row.Tf1_135D = 0;
                    newExpB31Row.Tf2_135D = 0;

                    newExpB31Row.t_145D = DateTime.MinValue;
                    newExpB31Row.Tfc_145D = 0;
                    newExpB31Row.Tf1_145D = 0;
                    newExpB31Row.Tf2_145D = 0;
                }

                B31Table.AddB31炉内温度校准数据2Row(newExpB31Row);
                B31TableAdapter.Update(B31Table);
                B31Table.AcceptChanges();
                RaisePropertyChanged(() => B31Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //炉内校准原始数据表
            try
            {
                //创建新表
                bool isExists = CenterCalInitDB.DbMaintenance.IsAnyTable(expNo, false);
                if (isExists)
                    CenterCalInitDB.DbMaintenance.DropTable(expNo);
                CenterCalInitDB.CodeFirst.As<CenterCalInitRec>(expNo).InitTables<CenterCalInitRec>();
                //复制内容
                if (!isNew)
                {
                    List<CenterCalInitRec> tempList = CenterCalInitDB.Queryable<CenterCalInitRec>().AS(oldExpNo).ToList();
                    for (int i = 0; i < tempList.Count; i++)
                        tempList[i].ExpNO = expNo;
                    CenterCalInitDB.Insertable<CenterCalInitRec>(tempList).AS(expNo).ExecuteCommand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            #endregion

            return true;
        }
    }
}

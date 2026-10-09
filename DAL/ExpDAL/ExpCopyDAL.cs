/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL.ExpDAL
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
using BRX.DAL.ExpDAL.ExpDALModel;
using GalaSoft.MvvmLight;

namespace BRX.DAL.ExpDAL
{
    public partial class ExpDAL : ObservableObject
    {
        /// <summary>
        /// 根据名称复制单点测试试验
        /// </summary>
        private bool CopyBRX(string OldNo, string expNo, bool isNew)
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

            #region 检查旧编号试验是否存在

            //A10主表
            try
            {
                BRX.DBDataSet.A10检测试验参数Row checkA10Row = A10Table.NewA10检测试验参数Row();
                checkA10Row = A10Table.FindBy试验编号(oldExpNo);
                if (checkA10Row == null)
                {
                    MessageBox.Show("编号" + oldExpNo + "A10主表不存在！", "错误提示");
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

            //A10表
            try
            {
                BRX.DBDataSet.A10检测试验参数Row checkA10Row = A10Table.NewA10检测试验参数Row();
                checkA10Row = A10Table.FindBy试验编号(newExpNo);
                if (checkA10Row != null)
                {
                    MessageBox.Show("此编号在A10表中已存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B11表
            try
            {
                BRX.DBDataSet.B11试样1检测数据Row checkB01Row = B11Table.NewB11试样1检测数据Row();
                checkB01Row = B11Table.FindBy试验编号(newExpNo);
                if (checkB01Row != null)
                {
                    MessageBox.Show("此编号在B11表中已存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B12表
            try
            {
                BRX.DBDataSet.B12试样2检测数据Row checkB01Row = B12Table.NewB12试样2检测数据Row();
                checkB01Row = B12Table.FindBy试验编号(newExpNo);
                if (checkB01Row != null)
                {
                    MessageBox.Show("此编号在B12表中已存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B13表
            try
            {
                BRX.DBDataSet.B13试样3检测数据Row checkB01Row = B13Table.NewB13试样3检测数据Row();
                checkB01Row = B13Table.FindBy试验编号(newExpNo);
                if (checkB01Row != null)
                {
                    MessageBox.Show("此编号在B13表中已存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B14表
            try
            {
                BRX.DBDataSet.B14试样4检测数据Row checkB01Row = B14Table.NewB14试样4检测数据Row();
                checkB01Row = B14Table.FindBy试验编号(newExpNo);
                if (checkB01Row != null)
                {
                    MessageBox.Show("此编号在B14表中已存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B15表
            try
            {
                BRX.DBDataSet.B15试样5检测数据Row checkB01Row = B15Table.NewB15试样5检测数据Row();
                checkB01Row = B15Table.FindBy试验编号(newExpNo);
                if (checkB01Row != null)
                {
                    MessageBox.Show("此编号在B15表中已存在！", "错误提示");
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

            //A10主表
            try
            {
                BRX.DBDataSet.A10检测试验参数Row oldExpA10Row = A10Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.A10检测试验参数Row newExpA10Row = A10Table.NewA10检测试验参数Row();
                newExpA10Row.ItemArray = (object[])oldExpA10Row.ItemArray.Clone();
                newExpA10Row.试验编号 = newExpNo;
                newExpA10Row.报告编号 = newExpNo + "RPT";
                newExpA10Row.试验补充说明 = "//";
                newExpA10Row.原始试验标志 = true;
                newExpA10Row.创建日期时间 = DateTime.Now;
                if (isNew)
                {
                    newExpA10Row.报告日期时间 = DateTime.Now;
                    newExpA10Row.委托时间 = DateTime.Now;
                    newExpA10Row.到样日期 = DateTime.Now;

                    newExpA10Row.IsCompleted = false;
                    newExpA10Row.试样1完成 = false;
                    newExpA10Row.试样2完成 = false;
                    newExpA10Row.试样3完成 = false;
                    newExpA10Row.试样4完成 = false;
                    newExpA10Row.试样5完成 = false;
                }

                A10Table.AddA10检测试验参数Row(newExpA10Row);
                A10TableAdapter.Update(A10Table);
                A10Table.AcceptChanges();
                RaisePropertyChanged(() => A10Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B11表
            try
            {
                BRX.DBDataSet.B11试样1检测数据Row oldExpB11Row = B11Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.B11试样1检测数据Row newExpB11Row = B11Table.NewB11试样1检测数据Row();
                newExpB11Row.ItemArray = (object[])oldExpB11Row.ItemArray.Clone();
                newExpB11Row.试验编号 = newExpNo;
                if (isNew)
                {
                    newExpB11Row.SpNO = 1;
                    newExpB11Row.WeightBefore = 0;
                    newExpB11Row.WeightFinal = 0;
                    newExpB11Row.LostRatio = 0;
                    newExpB11Row.TStart1 = 0;
                    newExpB11Row.TStart2 = 0;
                    newExpB11Row.TStartAvg = 0;
                    newExpB11Row.TFinal1 = 0;
                    newExpB11Row.TFinal2 = 0;
                    newExpB11Row.TFinalAvg = 0;
                    newExpB11Row.TUp1 = 0;
                    newExpB11Row.TUp2 = 0;
                    newExpB11Row.TUpAvg = 0;
                    newExpB11Row.TMax1 = 0;
                    newExpB11Row.TMax2 = 0;
                    newExpB11Row.TscFinal = 0;
                    newExpB11Row.TssFinal = 0;
                    newExpB11Row.TscMax = 0;
                    newExpB11Row.TssMax = 0;
                    newExpB11Row.TscUp = 0;
                    newExpB11Row.TssUp = 0;
                    newExpB11Row.FireTimeSum = 0;
                    newExpB11Row.TimeTest = 0;
                }

                B11Table.AddB11试样1检测数据Row(newExpB11Row);
                B11TableAdapter.Update(B11Table);
                B11Table.AcceptChanges();
                RaisePropertyChanged(() => B11Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B12表
            try
            {
                BRX.DBDataSet.B12试样2检测数据Row oldExpB12Row = B12Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.B12试样2检测数据Row newExpB12Row = B12Table.NewB12试样2检测数据Row();
                newExpB12Row.ItemArray = (object[])oldExpB12Row.ItemArray.Clone();
                newExpB12Row.试验编号 = newExpNo;
                if (isNew)
                {
                    newExpB12Row.SpNO = 2;

                    newExpB12Row.WeightBefore = 0;
                    newExpB12Row.WeightFinal = 0;
                    newExpB12Row.LostRatio = 0;
                    newExpB12Row.TStart1 = 0;
                    newExpB12Row.TStart2 = 0;
                    newExpB12Row.TStartAvg = 0;
                    newExpB12Row.TFinal1 = 0;
                    newExpB12Row.TFinal2 = 0;
                    newExpB12Row.TFinalAvg = 0;
                    newExpB12Row.TUp1 = 0;
                    newExpB12Row.TUp2 = 0;
                    newExpB12Row.TUpAvg = 0;
                    newExpB12Row.TMax1 = 0;
                    newExpB12Row.TMax2 = 0;
                    newExpB12Row.TscFinal = 0;
                    newExpB12Row.TssFinal = 0;
                    newExpB12Row.TscMax = 0;
                    newExpB12Row.TssMax = 0;
                    newExpB12Row.TscUp = 0;
                    newExpB12Row.TssUp = 0;
                    newExpB12Row.FireTimeSum = 0;
                    newExpB12Row.TimeTest = 0;
                }

                B12Table.AddB12试样2检测数据Row(newExpB12Row);
                B12TableAdapter.Update(B12Table);
                B12Table.AcceptChanges();
                RaisePropertyChanged(() => B12Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B13表
            try
            {
                BRX.DBDataSet.B13试样3检测数据Row oldExpB13Row = B13Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.B13试样3检测数据Row newExpB13Row = B13Table.NewB13试样3检测数据Row();
                newExpB13Row.ItemArray = (object[])oldExpB13Row.ItemArray.Clone();
                newExpB13Row.试验编号 = newExpNo;
                if (isNew)
                {
                    newExpB13Row.SpNO = 3;

                    newExpB13Row.WeightBefore = 0;
                    newExpB13Row.WeightFinal = 0;
                    newExpB13Row.LostRatio = 0;
                    newExpB13Row.TStart1 = 0;
                    newExpB13Row.TStart2 = 0;
                    newExpB13Row.TStartAvg = 0;
                    newExpB13Row.TFinal1 = 0;
                    newExpB13Row.TFinal2 = 0;
                    newExpB13Row.TFinalAvg = 0;
                    newExpB13Row.TUp1 = 0;
                    newExpB13Row.TUp2 = 0;
                    newExpB13Row.TUpAvg = 0;
                    newExpB13Row.TMax1 = 0;
                    newExpB13Row.TMax2 = 0;
                    newExpB13Row.TscFinal = 0;
                    newExpB13Row.TssFinal = 0;
                    newExpB13Row.TscMax = 0;
                    newExpB13Row.TssMax = 0;
                    newExpB13Row.TscUp = 0;
                    newExpB13Row.TssUp = 0;
                    newExpB13Row.FireTimeSum = 0;
                    newExpB13Row.TimeTest = 0;
                }

                B13Table.AddB13试样3检测数据Row(newExpB13Row);
                B13TableAdapter.Update(B13Table);
                B13Table.AcceptChanges();
                RaisePropertyChanged(() => B13Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B14表
            try
            {
                BRX.DBDataSet.B14试样4检测数据Row oldExpB14Row = B14Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.B14试样4检测数据Row newExpB14Row = B14Table.NewB14试样4检测数据Row();
                newExpB14Row.ItemArray = (object[])oldExpB14Row.ItemArray.Clone();
                newExpB14Row.试验编号 = newExpNo;
                if (isNew)
                {
                    newExpB14Row.SpNO = 4;

                    newExpB14Row.WeightBefore = 0;
                    newExpB14Row.WeightFinal = 0;
                    newExpB14Row.LostRatio = 0;
                    newExpB14Row.TStart1 = 0;
                    newExpB14Row.TStart2 = 0;
                    newExpB14Row.TStartAvg = 0;
                    newExpB14Row.TFinal1 = 0;
                    newExpB14Row.TFinal2 = 0;
                    newExpB14Row.TFinalAvg = 0;
                    newExpB14Row.TUp1 = 0;
                    newExpB14Row.TUp2 = 0;
                    newExpB14Row.TUpAvg = 0;
                    newExpB14Row.TMax1 = 0;
                    newExpB14Row.TMax2 = 0;
                    newExpB14Row.TscFinal = 0;
                    newExpB14Row.TssFinal = 0;
                    newExpB14Row.TscMax = 0;
                    newExpB14Row.TssMax = 0;
                    newExpB14Row.TscUp = 0;
                    newExpB14Row.TssUp = 0;
                    newExpB14Row.FireTimeSum = 0;
                    newExpB14Row.TimeTest = 0;
                }

                B14Table.AddB14试样4检测数据Row(newExpB14Row);
                B14TableAdapter.Update(B14Table);
                B14Table.AcceptChanges();
                RaisePropertyChanged(() => B14Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //B15表
            try
            {
                BRX.DBDataSet.B15试样5检测数据Row oldExpB15Row = B15Table.FindBy试验编号(oldExpNo);
                BRX.DBDataSet.B15试样5检测数据Row newExpB15Row = B15Table.NewB15试样5检测数据Row();
                newExpB15Row.ItemArray = (object[])oldExpB15Row.ItemArray.Clone();
                newExpB15Row.试验编号 = newExpNo;
                if (isNew)
                {
                    newExpB15Row.SpNO = 5;

                    newExpB15Row.WeightBefore = 0;
                    newExpB15Row.WeightFinal = 0;
                    newExpB15Row.LostRatio = 0;
                    newExpB15Row.TStart1 = 0;
                    newExpB15Row.TStart2 = 0;
                    newExpB15Row.TStartAvg = 0;
                    newExpB15Row.TFinal1 = 0;
                    newExpB15Row.TFinal2 = 0;
                    newExpB15Row.TFinalAvg = 0;
                    newExpB15Row.TUp1 = 0;
                    newExpB15Row.TUp2 = 0;
                    newExpB15Row.TUpAvg = 0;
                    newExpB15Row.TMax1 = 0;
                    newExpB15Row.TMax2 = 0;
                    newExpB15Row.TscFinal = 0;
                    newExpB15Row.TssFinal = 0;
                    newExpB15Row.TscMax = 0;
                    newExpB15Row.TssMax = 0;
                    newExpB15Row.TscUp = 0;
                    newExpB15Row.TssUp = 0;
                    newExpB15Row.FireTimeSum = 0;
                    newExpB15Row.TimeTest = 0;
                }

                B15Table.AddB15试样5检测数据Row(newExpB15Row);
                B15TableAdapter.Update(B15Table);
                B15Table.AcceptChanges();
                RaisePropertyChanged(() => B15Table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //原始数据表
            try
            {
                //创建新表
                bool isExists = InitDB.DbMaintenance.IsAnyTable(expNo, false);
                if (isExists)
                    InitDB.DbMaintenance.DropTable(expNo);
                InitDB.CodeFirst.As<InitRec>(expNo).InitTables<InitRec>();
                //复制内容
                if (!isNew)
                {
                    List<InitRec> tempList = InitDB.Queryable<InitRec>().AS(oldExpNo).ToList();
                    for (int i = 0; i < tempList.Count; i++)
                        tempList[i].ExpNO = expNo;
                    InitDB.Insertable<InitRec>(tempList).AS(expNo).ExecuteCommand();
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

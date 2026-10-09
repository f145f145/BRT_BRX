/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL.CalDAL
 * 文件名：  ExpDelDAL
 * 版本号：  V1.0.0.0
 * 唯一标识：fc36a47c-f9ec-4290-bd91-f8ae584a13f1
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-4-5 10:21:08
 * 描述：
 * 试验读写。删除部分。
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
using GalaSoft.MvvmLight;

namespace BRX.DAL.CalDAL
{
    public partial class CalDAL : ObservableObject
    {
        /// <summary>
        /// 删除炉壁校准试验
        /// </summary>
        private bool DelWallCal(string expNo)
        {
            string delExpNo;
            if (expNo == null)
            {
                MessageBox.Show("编号为空！", "错误提示");
                return false;
            }

            delExpNo = expNo.Clone().ToString();

            if ((delExpNo == null) || (delExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return false;
            }

            #region 检查编号在各表中是否存在

            //A20主表
            try
            {
                BRX.DBDataSet.A20炉壁温度校准试验参数Row checkA20Row = A20Table.NewA20炉壁温度校准试验参数Row();
                checkA20Row = A20Table.FindBy试验编号(expNo);
                if (checkA20Row == null)
                {
                    MessageBox.Show("编号" + expNo + "A20主表不存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            #endregion

            //当前试验、工厂试验、默认试验不允许删除
            if (delExpNo == WallCalDQ.ExpNO)
            {
                MessageBox.Show("当前试验无法删除，如需删除请先关闭实验！", "错误提示");
                return false;
            }
            if (delExpNo == "DefaultWallCal")
            {
                MessageBox.Show("系统默认试验，不允许删除！", "错误提示");
                return false;
            }
            if (delExpNo == "FactoryWallCal")
            {
                MessageBox.Show("工厂设定试验，不允许删除", "错误提示");
                return false;
            }

            //删除前确认
            MessageBoxResult msgBoxResult = MessageBox.Show("确认删除" + delExpNo + "号实验？试验信息和实验数据将被全部删除，且无法恢复！",
                "检查提示", MessageBoxButton.YesNo);
            if (msgBoxResult == MessageBoxResult.No)
            {
                return false;
            }

            //删除A20
            try
            {
                BRX.DBDataSet.A20炉壁温度校准试验参数Row willDelA20Row = A20Table.FindBy试验编号(delExpNo);
                if (willDelA20Row != null)
                {
                    willDelA20Row.Delete();
                    A20TableAdapter.Update(A20Table);
                    A20Table.AcceptChanges();
                    RaisePropertyChanged(() => A20Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //删除B20
            try
            {
                BRX.DBDataSet.B20炉壁温度校准数据Row willDelB20Row = B20Table.FindBy试验编号(delExpNo);
                if (willDelB20Row != null)
                {
                    willDelB20Row.Delete();
                    B20TableAdapter.Update(B20Table);
                    B20Table.AcceptChanges();
                    RaisePropertyChanged(() => B20Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //删除原始数据
            try
            {
                bool isExists = WallCalInitDB.DbMaintenance.IsAnyTable(delExpNo, false);
                if (isExists)
                    WallCalInitDB.DbMaintenance.DropTable(delExpNo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除炉内校准试验
        /// </summary>
        private bool DelCenterCal(string expNo)
        {
            string delExpNo;
            if (expNo == null)
            {
                MessageBox.Show("编号为空！", "错误提示");
                return false;
            }

            delExpNo = expNo.Clone().ToString();

            if ((delExpNo == null) || (delExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return false;
            }

            #region 检查编号在各表中是否存在

            //A30主表
            try
            {
                BRX.DBDataSet.A30炉内温度校准试验参数Row checkA30Row = A30Table.NewA30炉内温度校准试验参数Row();
                checkA30Row = A30Table.FindBy试验编号(expNo);
                if (checkA30Row == null)
                {
                    MessageBox.Show("编号" + expNo + "A30主表不存在！", "错误提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            #endregion

            //当前试验、工厂试验、默认试验不允许删除
            if (delExpNo == CenterCalDQ.ExpNO)
            {
                MessageBox.Show("当前试验无法删除，如需删除请先关闭实验！", "错误提示");
                return false;
            }
            if (delExpNo == "DefaultCenterCal")
            {
                MessageBox.Show("系统默认试验，不允许删除！", "错误提示");
                return false;
            }
            if (delExpNo == "FactoryCenterCal")
            {
                MessageBox.Show("工厂设定试验，不允许删除", "错误提示");
                return false;
            }

            //删除前确认
            MessageBoxResult msgBoxResult = MessageBox.Show("确认删除" + delExpNo + "号实验？试验信息和实验数据将被全部删除，且无法恢复！",
                "检查提示", MessageBoxButton.YesNo);
            if (msgBoxResult == MessageBoxResult.No)
            {
                return false;
            }

            //删除A30
            try
            {
                BRX.DBDataSet.A30炉内温度校准试验参数Row willDelA30Row = A30Table.FindBy试验编号(delExpNo);
                if (willDelA30Row != null)
                {
                    willDelA30Row.Delete();
                    A30TableAdapter.Update(A30Table);
                    A30Table.AcceptChanges();
                    RaisePropertyChanged(() => A30Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //删除B30
            try
            {
                BRX.DBDataSet.B30炉内温度校准数据Row willDelB30Row = B30Table.FindBy试验编号(delExpNo);
                if (willDelB30Row != null)
                {
                    willDelB30Row.Delete();
                    B30TableAdapter.Update(B30Table);
                    B30Table.AcceptChanges();
                    RaisePropertyChanged(() => B30Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //删除B31
            try
            {
                BRX.DBDataSet.B31炉内温度校准数据2Row willDelB31Row = B31Table.FindBy试验编号(delExpNo);
                if (willDelB31Row != null)
                {
                    willDelB31Row.Delete();
                    B31TableAdapter.Update(B31Table);
                    B31Table.AcceptChanges();
                    RaisePropertyChanged(() => B31Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //删除原始数据
            try
            {
                bool isExists = CenterCalInitDB.DbMaintenance.IsAnyTable(delExpNo, false);
                if (isExists)
                    CenterCalInitDB.DbMaintenance.DropTable(delExpNo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }


        /// <summary>
        /// 清空指定炉壁校准试验的某个高度点的原始数据记录
        /// </summary>
        private void WallCalPointRecReset(int heightNO)
        {
            try
            {
                List<WallCalInitRec> tempRecList = WallCalInitDB.Queryable<WallCalInitRec>().Where(it => it.HeightNO == heightNO).ToList();
                if (tempRecList.Count > 0)
                {
                    WallCalInitDB.Deleteable<WallCalInitRec>().AS(WallCalDQ.ExpNO).Where(it => it.HeightNO == heightNO).ExecuteCommand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 清空指定炉内校准试验的某个高度点的原始数据记录
        /// </summary>
        private void CenterCalPointRecReset(int heightNO)
        {
            try
            {
                List<CenterCalInitRec> tempRecList = CenterCalInitDB.Queryable<CenterCalInitRec>().AS(CenterCalDQ.ExpNO).Where(it => it.HeightNO == heightNO).ToList();
                if  (tempRecList.Count > 0)
                    CenterCalInitDB.Deleteable<CenterCalInitRec>().AS(CenterCalDQ.ExpNO).Where(it => it.HeightNO == heightNO).ExecuteCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
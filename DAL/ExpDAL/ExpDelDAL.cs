/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL.ExpDAL
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
using BRX.DAL.ExpDAL.ExpDALModel;
using GalaSoft.MvvmLight;

namespace BRX.DAL.ExpDAL
{
    public partial class ExpDAL : ObservableObject
    {
        /// <summary>
        /// 删除试验
        /// </summary>
        private bool DelBRX(string expNo)
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

            //A10主表
            try
            {
                BRX.DBDataSet.A10检测试验参数Row checkA10Row = A10Table.NewA10检测试验参数Row();
                checkA10Row = A10Table.FindBy试验编号(expNo);
                if (checkA10Row == null)
                {
                    MessageBox.Show("编号" + expNo + "A10主表不存在！", "错误提示");
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
            if (delExpNo == ExpBRXDQ.ExpNO)
            {
                MessageBox.Show("当前试验无法删除，如需删除请先关闭实验！", "错误提示");
                return false;
            }
            if (delExpNo == "DefaultExp")
            {
                MessageBox.Show("系统默认试验，不允许删除！", "错误提示");
                return false;
            }
            if (delExpNo == "FactoryExp")
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

            //删除A10
            try
            {
                BRX.DBDataSet.A10检测试验参数Row willDelA10Row = A10Table.FindBy试验编号(delExpNo);
                if (willDelA10Row != null)
                {
                    willDelA10Row.Delete();
                    A10TableAdapter.Update(A10Table);
                    A10Table.AcceptChanges();
                    RaisePropertyChanged(() => A10Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //删除B11
            try
            {
                BRX.DBDataSet.B11试样1检测数据Row willDelB11Row = B11Table.FindBy试验编号(delExpNo);
                if (willDelB11Row != null)
                {
                    willDelB11Row.Delete();
                    B11TableAdapter.Update(B11Table);
                    B11Table.AcceptChanges();
                    RaisePropertyChanged(() => B11Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //删除B12
            try
            {
                BRX.DBDataSet.B12试样2检测数据Row willDelB12Row = B12Table.FindBy试验编号(delExpNo);
                if (willDelB12Row != null)
                {
                    willDelB12Row.Delete();
                    B12TableAdapter.Update(B12Table);
                    B12Table.AcceptChanges();
                    RaisePropertyChanged(() => B12Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //删除B13
            try
            {
                BRX.DBDataSet.B13试样3检测数据Row willDelB13Row = B13Table.FindBy试验编号(delExpNo);
                if (willDelB13Row != null)
                {
                    willDelB13Row.Delete();
                    B13TableAdapter.Update(B13Table);
                    B13Table.AcceptChanges();
                    RaisePropertyChanged(() => B13Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //删除B14
            try
            {
                BRX.DBDataSet.B14试样4检测数据Row willDelB14Row = B14Table.FindBy试验编号(delExpNo);
                if (willDelB14Row != null)
                {
                    willDelB14Row.Delete();
                    B14TableAdapter.Update(B14Table);
                    B14Table.AcceptChanges();
                    RaisePropertyChanged(() => B14Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            //删除B15
            try
            {
                BRX.DBDataSet.B15试样5检测数据Row willDelB15Row = B15Table.FindBy试验编号(delExpNo);
                if (willDelB15Row != null)
                {
                    willDelB15Row.Delete();
                    B15TableAdapter.Update(B15Table);
                    B15Table.AcceptChanges();
                    RaisePropertyChanged(() => B15Table);
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
                bool isExists = InitDB.DbMaintenance.IsAnyTable(delExpNo, false);
                if (isExists)
                    InitDB.DbMaintenance.DropTable(delExpNo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }


        /// <summary>
        /// 清空当前试验的某个试样的测试原始数据记录
        /// </summary>
        private void SpRecReset(int spNo)
        {
            try
            {
                List<InitRec> tempRecList = InitDB.Queryable<InitRec>().AS(ExpBRXDQ.ExpNO).Where(it => it.SpNO == spNo).ToList();
                if (tempRecList.Count > 0)
                    InitDB.Deleteable<InitRec>().AS(ExpBRXDQ.ExpNO).Where(it => it.SpNO == spNo ).ExecuteCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
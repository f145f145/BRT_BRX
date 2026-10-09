/************************************************************************************
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
using BRX.DAL.ExpDAL.ExpDALModel;
using BRX.Model.Enums;
using BRX.Model.Exp;
using GalaSoft.MvvmLight;
using NPOI.Util;

namespace BRX.DAL.ExpDAL
{
    public partial class ExpDAL : ObservableObject
    {
        /// <summary>
        /// 保存A10试验基本参数
        /// </summary>
        private void SaveA10()
        {
            string saveExpNo = ExpBRXDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.A10检测试验参数Row checkExistA10Row = A10Table.FindBy试验编号(saveExpNo);
                if (checkExistA10Row == null)
                {
                    DBDataSet.A10检测试验参数Row defA10Row = A10Table.FindBy试验编号("DefaultExp");
                    DBDataSet.A10检测试验参数Row newA10Row = A10Table.NewA10检测试验参数Row();
                    newA10Row.ItemArray = (object[])defA10Row.ItemArray.Clone();
                    newA10Row.试验编号 = saveExpNo;
                    A10Table.AddA10检测试验参数Row(newA10Row);
                    A10TableAdapter.Update(A10Table);
                    A10Table.AcceptChanges();
                    RaisePropertyChanged(() => A10Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.A10检测试验参数Row expA10Row = A10Table.FindBy试验编号(saveExpNo);
                if (expA10Row != null)
                {
                    expA10Row.试验编号 = ExpBRXDQ.ExpNO;
                    expA10Row.试验补充说明 = ExpBRXDQ.ExpDetail;
                    expA10Row.报告编号 = ExpBRXDQ.RepNO;
                    expA10Row.创建日期时间 = ExpBRXDQ.CreatTime;
                    expA10Row.报告日期时间 = ExpBRXDQ.RepTime;
                    expA10Row.委托单位 = ExpBRXDQ.WTDW;
                    expA10Row.委托单位地址 = ExpBRXDQ.WTDWDZ;
                    expA10Row.委托时间 = ExpBRXDQ.WTTime;
                    expA10Row.项目名称 = ExpBRXDQ.XMMC;
                    expA10Row.项目地址 = ExpBRXDQ.XMDZ;
                    expA10Row.生产厂家 = ExpBRXDQ.SCCJ;
                    expA10Row.厂家厂址 = ExpBRXDQ.CJCZ;
                    expA10Row.原始试验标志 = ExpBRXDQ.IsReal;

                    expA10Row.样品名称 = ExpBRXDQ.YPMC;
                    expA10Row.样品编号 = ExpBRXDQ.YPNO;
                    expA10Row.样品数量 = ExpBRXDQ.YPSL;
                    expA10Row.到样日期 = ExpBRXDQ.DYRQ;
                    expA10Row.制品标识 = ExpBRXDQ.ZPBS;
                    expA10Row.抽样程序说明 = ExpBRXDQ.CYCX;
                    expA10Row.状态调节说明 = ExpBRXDQ.ZTTJ;
                    expA10Row.密度 = ExpBRXDQ.MD;
                    expA10Row.面密度 = ExpBRXDQ.MMD;
                    expA10Row.厚度 = ExpBRXDQ.HD;
                    expA10Row.IsCompleted = ExpBRXDQ.IsCompleted;

                    expA10Row.试样1完成 = ExpBRXDQ.SpList[0].IsCompleted;
                    expA10Row.试样2完成 = ExpBRXDQ.SpList[1].IsCompleted;
                    expA10Row.试样3完成 = ExpBRXDQ.SpList[2].IsCompleted;
                    expA10Row.试样4完成 = ExpBRXDQ.SpList[3].IsCompleted;
                    expA10Row.试样5完成 = ExpBRXDQ.SpList[4].IsCompleted;

                    A10TableAdapter.Update(A10Table);
                    A10Table.AcceptChanges();
                    RaisePropertyChanged(() => A10Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 保存B11试样1检测数据
        /// </summary>
        private void SaveB11()
        {
            string saveExpNo = ExpBRXDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B11试样1检测数据Row checkExistB11Row = B11Table.FindBy试验编号(saveExpNo);
                if (checkExistB11Row == null)
                {
                    DBDataSet.B11试样1检测数据Row defB11Row = B11Table.FindBy试验编号("DefaultExp");
                    DBDataSet.B11试样1检测数据Row newB11Row = B11Table.NewB11试样1检测数据Row();
                    newB11Row.ItemArray = (object[])defB11Row.ItemArray.Clone();
                    newB11Row.试验编号 = saveExpNo;
                    B11Table.AddB11试样1检测数据Row(newB11Row);
                    B11TableAdapter.Update(B11Table);
                    B11Table.AcceptChanges();
                    RaisePropertyChanged(() => B11Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B11试样1检测数据Row expB11Row = B11Table.FindBy试验编号(saveExpNo);
                if (expB11Row != null)
                {
                    expB11Row.试验编号 = ExpBRXDQ.ExpNO;
                    expB11Row.SpNO = ExpBRXDQ.SpList[0].SpNO;
                    expB11Row.WeightBefore = ExpBRXDQ.SpList[0].WeightBefore;
                    expB11Row.WeightFinal = ExpBRXDQ.SpList[0].WeightFinal;
                    expB11Row.LostRatio = ExpBRXDQ.SpList[0].LostRatio;
                    expB11Row.TStart1 = ExpBRXDQ.SpList[0].TStart1;
                    expB11Row.TStart2 = ExpBRXDQ.SpList[0].TStart2;
                    expB11Row.TStartAvg = ExpBRXDQ.SpList[0].TStartAvg;
                    expB11Row.TFinal1 = ExpBRXDQ.SpList[0].TFinal1;
                    expB11Row.TFinal2 = ExpBRXDQ.SpList[0].TFinal2;
                    expB11Row.TFinalAvg = ExpBRXDQ.SpList[0].TFinalAvg;
                    expB11Row.TUp1 = ExpBRXDQ.SpList[0].TUp1;
                    expB11Row.TUp2 = ExpBRXDQ.SpList[0].TUp2;
                    expB11Row.TUpAvg = ExpBRXDQ.SpList[0].TUpAvg;
                    expB11Row.TMax1 = ExpBRXDQ.SpList[0].TMax1;
                    expB11Row.TMax2 = ExpBRXDQ.SpList[0].TMax2;
                    expB11Row.TscFinal = ExpBRXDQ.SpList[0].TscFinal;
                    expB11Row.TssFinal = ExpBRXDQ.SpList[0].TssFinal;
                    expB11Row.TscMax = ExpBRXDQ.SpList[0].TscMax;
                    expB11Row.TssMax = ExpBRXDQ.SpList[0].TssMax;
                    expB11Row.TscUp = ExpBRXDQ.SpList[0].TscUp;
                    expB11Row.TssUp = ExpBRXDQ.SpList[0].TssUp;
                    expB11Row.FireTimeSum = ExpBRXDQ.SpList[0].FireTimeSum;
                    expB11Row.TimeTest = ExpBRXDQ.SpList[0].TimeTest;
                    expB11Row.TestPower = ExpBRXDQ.SpList[0].TestPower;

                    B11TableAdapter.Update(B11Table);
                    B11Table.AcceptChanges();
                    RaisePropertyChanged(() => B11Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <summary>
        /// 保存B12试样2检测数据
        /// </summary>
        private void SaveB12()
        {
            string saveExpNo = ExpBRXDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B12试样2检测数据Row checkExistB12Row = B12Table.FindBy试验编号(saveExpNo);
                if (checkExistB12Row == null)
                {
                    DBDataSet.B12试样2检测数据Row defB12Row = B12Table.FindBy试验编号("DefaultExp");
                    DBDataSet.B12试样2检测数据Row newB12Row = B12Table.NewB12试样2检测数据Row();
                    newB12Row.ItemArray = (object[])defB12Row.ItemArray.Clone();
                    newB12Row.试验编号 = saveExpNo;
                    B12Table.AddB12试样2检测数据Row(newB12Row);
                    B12TableAdapter.Update(B12Table);
                    B12Table.AcceptChanges();
                    RaisePropertyChanged(() => B12Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B12试样2检测数据Row expB12Row = B12Table.FindBy试验编号(saveExpNo);
                if (expB12Row != null)
                {
                    expB12Row.试验编号 = ExpBRXDQ.ExpNO;
                    expB12Row.SpNO = ExpBRXDQ.SpList[1].SpNO;
                    expB12Row.WeightBefore = ExpBRXDQ.SpList[1].WeightBefore;
                    expB12Row.WeightFinal = ExpBRXDQ.SpList[1].WeightFinal;
                    expB12Row.LostRatio = ExpBRXDQ.SpList[1].LostRatio;
                    expB12Row.TStart1 = ExpBRXDQ.SpList[1].TStart1;
                    expB12Row.TStart2 = ExpBRXDQ.SpList[1].TStart2;
                    expB12Row.TStartAvg = ExpBRXDQ.SpList[1].TStartAvg;
                    expB12Row.TFinal1 = ExpBRXDQ.SpList[1].TFinal1;
                    expB12Row.TFinal2 = ExpBRXDQ.SpList[1].TFinal2;
                    expB12Row.TFinalAvg = ExpBRXDQ.SpList[1].TFinalAvg;
                    expB12Row.TUp1 = ExpBRXDQ.SpList[1].TUp1;
                    expB12Row.TUp2 = ExpBRXDQ.SpList[1].TUp2;
                    expB12Row.TUpAvg = ExpBRXDQ.SpList[1].TUpAvg;
                    expB12Row.TMax1 = ExpBRXDQ.SpList[1].TMax1;
                    expB12Row.TMax2 = ExpBRXDQ.SpList[1].TMax2;
                    expB12Row.TscFinal = ExpBRXDQ.SpList[1].TscFinal;
                    expB12Row.TssFinal = ExpBRXDQ.SpList[1].TssFinal;
                    expB12Row.TscMax = ExpBRXDQ.SpList[1].TscMax;
                    expB12Row.TssMax = ExpBRXDQ.SpList[1].TssMax;
                    expB12Row.TscUp = ExpBRXDQ.SpList[1].TscUp;
                    expB12Row.TssUp = ExpBRXDQ.SpList[1].TssUp;
                    expB12Row.FireTimeSum = ExpBRXDQ.SpList[1].FireTimeSum;
                    expB12Row.TimeTest = ExpBRXDQ.SpList[1].TimeTest;
                    expB12Row.TestPower = ExpBRXDQ.SpList[1].TestPower;

                    B12TableAdapter.Update(B12Table);
                    B12Table.AcceptChanges();
                    RaisePropertyChanged(() => B12Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存B13试样3检测数据
        /// </summary>
        private void SaveB13()
        {
            string saveExpNo = ExpBRXDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B13试样3检测数据Row checkExistB13Row = B13Table.FindBy试验编号(saveExpNo);
                if (checkExistB13Row == null)
                {
                    DBDataSet.B13试样3检测数据Row defB13Row = B13Table.FindBy试验编号("DefaultExp");
                    DBDataSet.B13试样3检测数据Row newB13Row = B13Table.NewB13试样3检测数据Row();
                    newB13Row.ItemArray = (object[])defB13Row.ItemArray.Clone();
                    newB13Row.试验编号 = saveExpNo;
                    B13Table.AddB13试样3检测数据Row(newB13Row);
                    B13TableAdapter.Update(B13Table);
                    B13Table.AcceptChanges();
                    RaisePropertyChanged(() => B13Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B13试样3检测数据Row expB13Row = B13Table.FindBy试验编号(saveExpNo);
                if (expB13Row != null)
                {
                    expB13Row.试验编号 = ExpBRXDQ.ExpNO;
                    expB13Row.SpNO = ExpBRXDQ.SpList[2].SpNO;
                    expB13Row.WeightBefore = ExpBRXDQ.SpList[2].WeightBefore;
                    expB13Row.WeightFinal = ExpBRXDQ.SpList[2].WeightFinal;
                    expB13Row.LostRatio = ExpBRXDQ.SpList[2].LostRatio;
                    expB13Row.TStart1 = ExpBRXDQ.SpList[2].TStart1;
                    expB13Row.TStart2 = ExpBRXDQ.SpList[2].TStart2;
                    expB13Row.TStartAvg = ExpBRXDQ.SpList[2].TStartAvg;
                    expB13Row.TFinal1 = ExpBRXDQ.SpList[2].TFinal1;
                    expB13Row.TFinal2 = ExpBRXDQ.SpList[2].TFinal2;
                    expB13Row.TFinalAvg = ExpBRXDQ.SpList[2].TFinalAvg;
                    expB13Row.TUp1 = ExpBRXDQ.SpList[2].TUp1;
                    expB13Row.TUp2 = ExpBRXDQ.SpList[2].TUp2;
                    expB13Row.TUpAvg = ExpBRXDQ.SpList[2].TUpAvg;
                    expB13Row.TMax1 = ExpBRXDQ.SpList[2].TMax1;
                    expB13Row.TMax2 = ExpBRXDQ.SpList[2].TMax2;
                    expB13Row.TscFinal = ExpBRXDQ.SpList[2].TscFinal;
                    expB13Row.TssFinal = ExpBRXDQ.SpList[2].TssFinal;
                    expB13Row.TscMax = ExpBRXDQ.SpList[2].TscMax;
                    expB13Row.TssMax = ExpBRXDQ.SpList[2].TssMax;
                    expB13Row.TscUp = ExpBRXDQ.SpList[2].TscUp;
                    expB13Row.TssUp = ExpBRXDQ.SpList[2].TssUp;
                    expB13Row.FireTimeSum = ExpBRXDQ.SpList[2].FireTimeSum;
                    expB13Row.TimeTest = ExpBRXDQ.SpList[2].TimeTest;
                    expB13Row.TestPower = ExpBRXDQ.SpList[2].TestPower;

                    B13TableAdapter.Update(B13Table);
                    B13Table.AcceptChanges();
                    RaisePropertyChanged(() => B13Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存B14试样4检测数据
        /// </summary>
        private void SaveB14()
        {
            string saveExpNo = ExpBRXDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B14试样4检测数据Row checkExistB14Row = B14Table.FindBy试验编号(saveExpNo);
                if (checkExistB14Row == null)
                {
                    DBDataSet.B14试样4检测数据Row defB14Row = B14Table.FindBy试验编号("DefaultExp");
                    DBDataSet.B14试样4检测数据Row newB14Row = B14Table.NewB14试样4检测数据Row();
                    newB14Row.ItemArray = (object[])defB14Row.ItemArray.Clone();
                    newB14Row.试验编号 = saveExpNo;
                    B14Table.AddB14试样4检测数据Row(newB14Row);
                    B14TableAdapter.Update(B14Table);
                    B14Table.AcceptChanges();
                    RaisePropertyChanged(() => B14Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B14试样4检测数据Row expB14Row = B14Table.FindBy试验编号(saveExpNo);
                if (expB14Row != null)
                {
                    expB14Row.试验编号 = ExpBRXDQ.ExpNO;
                    expB14Row.SpNO = ExpBRXDQ.SpList[3].SpNO;
                    expB14Row.WeightBefore = ExpBRXDQ.SpList[3].WeightBefore;
                    expB14Row.WeightFinal = ExpBRXDQ.SpList[3].WeightFinal;
                    expB14Row.LostRatio = ExpBRXDQ.SpList[3].LostRatio;
                    expB14Row.TStart1 = ExpBRXDQ.SpList[3].TStart1;
                    expB14Row.TStart2 = ExpBRXDQ.SpList[3].TStart2;
                    expB14Row.TStartAvg = ExpBRXDQ.SpList[3].TStartAvg;
                    expB14Row.TFinal1 = ExpBRXDQ.SpList[3].TFinal1;
                    expB14Row.TFinal2 = ExpBRXDQ.SpList[3].TFinal2;
                    expB14Row.TFinalAvg = ExpBRXDQ.SpList[3].TFinalAvg;
                    expB14Row.TUp1 = ExpBRXDQ.SpList[3].TUp1;
                    expB14Row.TUp2 = ExpBRXDQ.SpList[3].TUp2;
                    expB14Row.TUpAvg = ExpBRXDQ.SpList[3].TUpAvg;
                    expB14Row.TMax1 = ExpBRXDQ.SpList[3].TMax1;
                    expB14Row.TMax2 = ExpBRXDQ.SpList[3].TMax2;
                    expB14Row.TscFinal = ExpBRXDQ.SpList[3].TscFinal;
                    expB14Row.TssFinal = ExpBRXDQ.SpList[3].TssFinal;
                    expB14Row.TscMax = ExpBRXDQ.SpList[3].TscMax;
                    expB14Row.TssMax = ExpBRXDQ.SpList[3].TssMax;
                    expB14Row.TscUp = ExpBRXDQ.SpList[3].TscUp;
                    expB14Row.TssUp = ExpBRXDQ.SpList[3].TssUp;
                    expB14Row.FireTimeSum = ExpBRXDQ.SpList[3].FireTimeSum;
                    expB14Row.TimeTest = ExpBRXDQ.SpList[3].TimeTest;
                    expB14Row.TestPower = ExpBRXDQ.SpList[3].TestPower;

                    B14TableAdapter.Update(B14Table);
                    B14Table.AcceptChanges();
                    RaisePropertyChanged(() => B14Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存B15试样5检测数据
        /// </summary>
        private void SaveB15()
        {
            string saveExpNo = ExpBRXDQ.ExpNO.Clone().ToString();

            if ((saveExpNo == null) || (saveExpNo == string.Empty))
            {
                MessageBox.Show("编号为空！", "错误提示");
                return;
            }

            //若编号在表中不存在，则新建（拷贝DefaultExp）
            try
            {
                DBDataSet.B15试样5检测数据Row checkExistB15Row = B15Table.FindBy试验编号(saveExpNo);
                if (checkExistB15Row == null)
                {
                    DBDataSet.B15试样5检测数据Row defB15Row = B15Table.FindBy试验编号("DefaultExp");
                    DBDataSet.B15试样5检测数据Row newB15Row = B15Table.NewB15试样5检测数据Row();
                    newB15Row.ItemArray = (object[])defB15Row.ItemArray.Clone();
                    newB15Row.试验编号 = saveExpNo;
                    B15Table.AddB15试样5检测数据Row(newB15Row);
                    B15TableAdapter.Update(B15Table);
                    B15Table.AcceptChanges();
                    RaisePropertyChanged(() => B15Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DBDataSet.B15试样5检测数据Row expB15Row = B15Table.FindBy试验编号(saveExpNo);
                if (expB15Row != null)
                {
                    expB15Row.试验编号 = ExpBRXDQ.ExpNO;
                    expB15Row.SpNO = ExpBRXDQ.SpList[4].SpNO;
                    expB15Row.WeightBefore = ExpBRXDQ.SpList[4].WeightBefore;
                    expB15Row.WeightFinal = ExpBRXDQ.SpList[4].WeightFinal;
                    expB15Row.LostRatio = ExpBRXDQ.SpList[4].LostRatio;
                    expB15Row.TStart1 = ExpBRXDQ.SpList[4].TStart1;
                    expB15Row.TStart2 = ExpBRXDQ.SpList[4].TStart2;
                    expB15Row.TStartAvg = ExpBRXDQ.SpList[4].TStartAvg;
                    expB15Row.TFinal1 = ExpBRXDQ.SpList[4].TFinal1;
                    expB15Row.TFinal2 = ExpBRXDQ.SpList[4].TFinal2;
                    expB15Row.TFinalAvg = ExpBRXDQ.SpList[4].TFinalAvg;
                    expB15Row.TUp1 = ExpBRXDQ.SpList[4].TUp1;
                    expB15Row.TUp2 = ExpBRXDQ.SpList[4].TUp2;
                    expB15Row.TUpAvg = ExpBRXDQ.SpList[4].TUpAvg;
                    expB15Row.TMax1 = ExpBRXDQ.SpList[4].TMax1;
                    expB15Row.TMax2 = ExpBRXDQ.SpList[4].TMax2;
                    expB15Row.TscFinal = ExpBRXDQ.SpList[4].TscFinal;
                    expB15Row.TssFinal = ExpBRXDQ.SpList[4].TssFinal;
                    expB15Row.TscMax = ExpBRXDQ.SpList[4].TscMax;
                    expB15Row.TssMax = ExpBRXDQ.SpList[4].TssMax;
                    expB15Row.TscUp = ExpBRXDQ.SpList[4].TscUp;
                    expB15Row.TssUp = ExpBRXDQ.SpList[4].TssUp;
                    expB15Row.FireTimeSum = ExpBRXDQ.SpList[4].FireTimeSum;
                    expB15Row.TimeTest = ExpBRXDQ.SpList[4].TimeTest;
                    expB15Row.TestPower = ExpBRXDQ.SpList[4].TestPower;

                    B15TableAdapter.Update(B15Table);
                    B15Table.AcceptChanges();
                    RaisePropertyChanged(() => B15Table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存测试原始数据记录InitDB
        /// </summary>
        private void SaveInitData(TestRec rec)
        {
            try
            {
                InitRec recWillSave = new InitRec
                {
                    ExpNO = ExpBRXDQ.ExpNO,
                    SpNO = ExpBRXDQ.SpDQ.SpNO,
                    RecNum = rec.No,
                    RecTime = rec.RecTime,
                    ExpStage = ExpBRXDQ.Converter<Enums.TestStage>(rec.Stage),
                    T1 = rec.T1,
                    T2 = rec.T2,
                    Tsc = rec.Tsc,
                    Tss = rec.Tss,
                    IsFired = rec.IsFired,
                    Vo = rec.Vo,
                    Detail = "//"
                };

                List<InitRec> tempRecList = InitDB.CopyNew().Queryable<InitRec>().AS(recWillSave.ExpNO).Where(it => it.SpNO == recWillSave.SpNO && it.RecNum == recWillSave.RecNum).ToList();
                if (tempRecList.Count > 0)
                    InitDB.CopyNew().Deleteable<InitRec>().AS(recWillSave.ExpNO).Where(it=>it.SpNO== recWillSave.SpNO && it.RecNum== recWillSave.RecNum).ExecuteCommand();
                InitDB.CopyNew().Insertable<InitRec>(recWillSave).AS(recWillSave.ExpNO).ExecuteCommand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
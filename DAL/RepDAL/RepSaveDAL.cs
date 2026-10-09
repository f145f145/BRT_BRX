/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL.RepDAL
 * 文件名：  RepSaveDAL
 * 版本号：  V1.0.0.0
 * 唯一标识：e5242da2-b62d-4b6c-8829-c014767ac13b
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-4-21 14:12:47
 * 描述：
 * 报告导出。保存测试试验报告部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-4-21 13:28:13		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using GalaSoft.MvvmLight;
using System.Windows;
using System.IO;
using BRX.Model.Enums;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace BRX.DAL.RepDAL
{
    public partial class RepDAL : ObservableObject
    {
        /// <summary>
        /// 保存测试试验报告（2010）
        /// </summary>
        private void SaveBRXRPT_2010()
        {
            IWorkbook iWorkbookRPT;

            #region 用NPOI复制模板法

            ////打开模板文件
            //try
            //{
            //    FileStream sFormfile = new FileStream(RptFormFolder+ RptFormFile, FileMode.Open, FileAccess.Read, FileShare.None);
            //    iWorkbookRPT = WorkbookFactory.Create(sFormfile);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message+"\r\n导出报告失败！");
            //    return;
            //}
            ////复制模板文件内容
            //try
            //{
            //    int tempRPTSheets = iWorkbookRPT.NumberOfSheets;
            //    for (int i = 0; i < tempRPTSheets; ++i)
            //    {
            //        ISheet ish = iWorkbookRPT.GetSheetAt(i);
            //        ish.DisplayGridlines = true;
            //        ish.DisplayRowColHeadings = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message + "\r\n导出报告失败！");
            //}

            #endregion


            //打开文件
            try
            {
                iWorkbookRPT = new HSSFWorkbook(new FileStream(Config.FormDir + Config.ExpRep10FormName, FileMode.Open, FileAccess.Read, FileShare.None));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n报告文件读取失败！");
                return;
            }

            #region 测试报告第1页
            try
            {
                int sheetIndex = iWorkbookRPT.GetSheetIndex("试验报告");
                ISheet sht0 = iWorkbookRPT.GetSheetAt(sheetIndex);
                string str = "";

                //单位名头
                HSSFRow row = (HSSFRow)sht0.GetRow(0);
                HSSFCell cell = (HSSFCell)row.GetCell(0);
                cell.SetCellValue(Dev.CoName);
                //报告编号
                row = (HSSFRow)sht0.GetRow(2);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.RepNO);
                //报告日期
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(ExpBRXDQ.RepTime.ToLongDateString());

                //试验编号
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.ExpNO);
                //检测日期
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(ExpBRXDQ.CreatTime.ToLongDateString());
                //检测依据
                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(3);
                if (Dev.IsStd2010)
                    str = "《GBT 5464-2010 建筑材料不燃性试验方法》";
                cell.SetCellValue(str);
                //检测单位
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(Dev.CoName);
                //实验室地址
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(Dev.LabAddr);
               //委托单位
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.WTDW);
                //委托单位地址
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.WTDWDZ);
                //生产厂家
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.SCCJ);
                //厂家厂址
                row = (HSSFRow)sht0.GetRow(10);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.CJCZ);

                //试样信息
                //样品名称
                row = (HSSFRow)sht0.GetRow(11);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.YPMC);
                //委托日期
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(ExpBRXDQ.WTTime.ToLongDateString());
                //样品标识
                row = (HSSFRow)sht0.GetRow(12);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.ZPBS);
                //样品编号
                cell = (HSSFCell)row.GetCell(17);
                cell.SetCellValue(ExpBRXDQ.YPNO);
                //密度
                row = (HSSFRow)sht0.GetRow(13);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.MD);
                //面密度
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(ExpBRXDQ.MMD);
                //厚度
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(ExpBRXDQ.HD);
                //结构
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.JGXX);
                //状态调节信息
                row = (HSSFRow)sht0.GetRow(15);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.ZTTJ);
                //抽样程序
                row = (HSSFRow)sht0.GetRow(16);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.CYCX);

                //汇总测试数据
                for (int i = 0; i < 5; i++)
                {
                    row = (HSSFRow)sht0.GetRow(18+i);
                    //炉内温升
                    cell = (HSSFCell)row.GetCell(3);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TUp1);
                    //试样中心温升
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TscUp);
                    //试样表面温升
                    cell = (HSSFCell)row.GetCell(9);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TssUp);
                    //试样初始质量
                    cell = (HSSFCell)row.GetCell(12);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].WeightBefore);
                    //试验后质量
                    cell = (HSSFCell)row.GetCell(15);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].WeightFinal);
                    //质量损失率
                    cell = (HSSFCell)row.GetCell(18);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].LostRatio);
                    //持续火焰总时长
                    cell = (HSSFCell)row.GetCell(21);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].FireTimeSum);
                }

                if(Dev.EncryptRPT)
                    sht0.ProtectSheet("12345678");//设置密码保护
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n导出报告第1页失败！");
                throw;
            }

            #endregion


            #region 测试报告第2页
            try
            {
                int sheetIndex = iWorkbookRPT.GetSheetIndex("试验数据");
                ISheet sht0 = iWorkbookRPT.GetSheetAt(sheetIndex);
                string str;

                HSSFRow row = (HSSFRow)sht0.GetRow(0);
                HSSFCell cell = (HSSFCell)row.GetCell(0);
              
                //各试样检测数据计算结果
                for (int i = 0; i < 5; i++)
                {
                    row = (HSSFRow)sht0.GetRow(3 + i);
                    //炉内，初始温度
                    cell = (HSSFCell)row.GetCell(2);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TStart1);
                    //炉内，最终温度
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TFinal1);
                    //炉内，温升
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TUp1);
                    //炉内，最高温度
                    cell = (HSSFCell)row.GetCell(8);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TMax1);
                    //火焰持续时间总计
                    cell = (HSSFCell)row.GetCell(10);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].FireTimeSum);

                    //初始质量
                    cell = (HSSFCell)row.GetCell(12);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].WeightBefore);
                    //试验后质量
                    cell = (HSSFCell)row.GetCell(14);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].WeightFinal);
                    //质量损失率
                    cell = (HSSFCell)row.GetCell(16);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].LostRatio);

                    //附加热电偶
                    cell = (HSSFCell)row.GetCell(18);
                    str = ExpBRXDQ.UseAddT ? "√" : "×";
                    cell.SetCellValue(str);
                    //试样中心，温升
                    cell = (HSSFCell)row.GetCell(20);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TscUp);
                    //试样中心，最终温度
                    cell = (HSSFCell)row.GetCell(22);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TscFinal);
                    //试样中心，最高温度
                    cell = (HSSFCell)row.GetCell(24);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TscMax);
                    //试样表面，温升
                    cell = (HSSFCell)row.GetCell(26);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TssUp);
                    //试样表面，最终温度
                    cell = (HSSFCell)row.GetCell(28);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TssFinal);
                    //试样表面，最高温度
                    cell = (HSSFCell)row.GetCell(30);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TssMax);
                }

                if(Dev.EncryptRPT)
                    sht0.ProtectSheet("12345678");//设置密码保护
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n导出报告第1页失败！");
                throw;
            }

            #endregion


            #region 测试报告第3-7页

            int indexStyle = iWorkbookRPT.GetSheetIndex("试样1");
            ISheet shtStyle = iWorkbookRPT.GetSheetAt(indexStyle);
            HSSFRow row0;
            HSSFCell cell0;
            
            ICellStyle style0 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style1 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style2 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style3 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style4 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style5 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style6 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style7 = iWorkbookRPT.CreateCellStyle();

            IDataFormat dataformat = iWorkbookRPT.CreateDataFormat();

            row0 = (HSSFRow)shtStyle.GetRow(2);

            cell0 = (HSSFCell)row0.GetCell(0);
            style0.CloneStyleFrom(cell0.CellStyle);

            cell0 = (HSSFCell)row0.GetCell(1);
            style1.CloneStyleFrom(cell0.CellStyle);
            style2.DataFormat = dataformat.GetFormat("yyyy-MM-DD hh:mm:ss");

            cell0 = (HSSFCell)row0.GetCell(2);
            style2.CloneStyleFrom(cell0.CellStyle);

            cell0 = (HSSFCell)row0.GetCell(3);
            style3.CloneStyleFrom(cell0.CellStyle);
            style3.DataFormat = dataformat.GetFormat("0.00");

            cell0 = (HSSFCell)row0.GetCell(4);
            style4.CloneStyleFrom(cell0.CellStyle);
            style4.DataFormat = dataformat.GetFormat("0.00");

            cell0 = (HSSFCell)row0.GetCell(5);
            style5.CloneStyleFrom(cell0.CellStyle);
            style5.DataFormat = dataformat.GetFormat("0.00");

            cell0 = (HSSFCell)row0.GetCell(6);
            style6.CloneStyleFrom(cell0.CellStyle);
 
            cell0 = (HSSFCell)row0.GetCell(7);
            style7.CloneStyleFrom(cell0.CellStyle);

            for (int spNum = 1; spNum <= 5; spNum++)
            {
                try
                {
                    string sheetName = "试样" + spNum.ToString();
                    int sheetIndex = iWorkbookRPT.GetSheetIndex(sheetName);

                    ISheet sht = iWorkbookRPT.GetSheetAt(sheetIndex);
                    string str;

                    HSSFRow row = (HSSFRow)sht.GetRow(0);
                    HSSFCell cell = (HSSFCell)row.GetCell(0);

                    if ((ExpBRXDQ.SpList[spNum - 1].RecList_All != null) && (ExpBRXDQ.SpList[spNum - 1].RecList_All.Count > 0))
                    {
                        //导出所有记录
                        int rowNum = 0;
                        for (int i = 0; i < ExpBRXDQ.SpList[spNum - 1].RecList_All.Count; i++)
                        {
                            rowNum = i + 2;
                            row = (HSSFRow)sht.GetRow(rowNum);
                            //序号
                            cell = (HSSFCell)row.GetCell(0);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_All[i].No);
                            cell.CellStyle = style0;
                            //采样时间
                            cell = (HSSFCell)row.GetCell(1);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_All[i].RecTime);
                            cell.CellStyle = style1;
                            //试验阶段
                            cell = (HSSFCell)row.GetCell(2);
                            switch (ExpBRXDQ.SpList[spNum - 1].RecList_All[i].Stage)
                            {
                                case Enums.TestStage.TCtl_Stage:
                                    str = "控温阶段";
                                    break;
                                case Enums.TestStage.Test_Stage:
                                    str = "测试阶段";
                                    break;
                                case Enums.TestStage.TestEnd_Stage:
                                    str = "测试完成";
                                    break;
                                default:
                                    str = "等待阶段";
                                    break;
                            }
                            cell.SetCellValue(str);
                            cell.CellStyle = style2;
                            //炉内温度
                            cell = (HSSFCell)row.GetCell(3);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_All[i].T1);
                            cell.CellStyle = style3;
                            //试样中心温度
                            cell = (HSSFCell)row.GetCell(4);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_All[i].Tsc);
                            cell.CellStyle = style4;
                            //试样表面温度
                            cell = (HSSFCell)row.GetCell(5);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_All[i].Tss);
                            cell.CellStyle = style5;
                            //有火焰
                            cell = (HSSFCell)row.GetCell(6); 
                            str = ExpBRXDQ.SpList[spNum - 1].RecList_All[i].IsFired ? "√" : "×";
                            cell.SetCellValue(str);
                            cell.CellStyle = style6;
                            //备注
                            cell = (HSSFCell)row.GetCell(7);
                            str = "";
                            cell.SetCellValue(str);
                            cell.CellStyle = style7;
                        }

                        ////单独导出30min试验记录
                        //int rowNumStart = rowNum + 1;
                        //for (int i = 0; i < ExpBRXDQ.SpList[spNum - 1].RecList_Test.Count; i++)
                        //{
                        //    rowNum = i + rowNumStart;
                        //    row = (HSSFRow)sht.GetRow(rowNum);
                        //    //序号
                        //    cell = (HSSFCell)row.GetCell(0);
                        //    cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].No);
                        //    cell.CellStyle = style0;
                        //    //采样时间
                        //    cell = (HSSFCell)row.GetCell(1);
                        //    cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].RecTime);
                        //    cell.CellStyle = style1;
                        //    //试验阶段
                        //    cell = (HSSFCell)row.GetCell(2);
                        //    switch (ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].Stage)
                        //    {
                        //        case Enums.TestStage.TCtl_Stage:
                        //            str = "控温阶段";
                        //            break;
                        //        case Enums.TestStage.Test_Stage:
                        //            str = "测试阶段";
                        //            break;
                        //        case Enums.TestStage.TestEnd_Stage:
                        //            str = "测试完成";
                        //            break;
                        //        default:
                        //            str = "等待阶段";
                        //            break;
                        //    }
                        //    cell.SetCellValue(str);
                        //    cell.CellStyle = style2;
                        //    //炉内温度
                        //    cell = (HSSFCell)row.GetCell(3);
                        //    cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].T1);
                        //    cell.CellStyle = style3;
                        //    //试样中心温度
                        //    cell = (HSSFCell)row.GetCell(4);
                        //    cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].Tsc);
                        //    cell.CellStyle = style4;
                        //    //试样表面温度
                        //    cell = (HSSFCell)row.GetCell(5);
                        //    cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].Tss);
                        //    cell.CellStyle = style5;
                        //    //有火焰
                        //    cell = (HSSFCell)row.GetCell(6);
                        //    str = ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].IsFired ? "√" : "×";
                        //    cell.SetCellValue(str);
                        //    cell.CellStyle = style6;
                        //    //备注
                        //    cell = (HSSFCell)row.GetCell(7);
                        //    str = "";
                        //    cell.SetCellValue(str);
                        //    cell.CellStyle = style7;
                        //}
                    }

                    if (Dev.EncryptRPT)
                        sht.ProtectSheet("12345678");//设置密码保护
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "\r\n试样" + spNum + "数据失败！");
                    throw;
                }

            }
            #endregion

            //数据最终导出
            try
            {
                iWorkbookRPT.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();        //强制更新公式
                FileStream streamRPT = File.OpenWrite(Config.RepDir + RptAimFile);
                iWorkbookRPT.Write(streamRPT);
                streamRPT.Flush();
                streamRPT.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n导出报告失败！");
            }

            //打开报告所在文件夹
            System.Diagnostics.Process.Start("explorer.exe", Config.RepDir);
        }

        /// <summary>
        /// 保存测试试验报告（2022）
        /// </summary>
        private void SaveBRXRPT_2022()
        {
            IWorkbook iWorkbookRPT;

            #region 用NPOI复制模板法

            ////打开模板文件
            //try
            //{
            //    FileStream sFormfile = new FileStream(RptFormFolder+ RptFormFile, FileMode.Open, FileAccess.Read, FileShare.None);
            //    iWorkbookRPT = WorkbookFactory.Create(sFormfile);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message+"\r\n导出报告失败！");
            //    return;
            //}
            ////复制模板文件内容
            //try
            //{
            //    int tempRPTSheets = iWorkbookRPT.NumberOfSheets;
            //    for (int i = 0; i < tempRPTSheets; ++i)
            //    {
            //        ISheet ish = iWorkbookRPT.GetSheetAt(i);
            //        ish.DisplayGridlines = true;
            //        ish.DisplayRowColHeadings = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message + "\r\n导出报告失败！");
            //}

            #endregion


            //打开文件
            try
            {
                iWorkbookRPT = new HSSFWorkbook(new FileStream(Config.FormDir + Config.ExpRep22FormName, FileMode.Open, FileAccess.Read, FileShare.None));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n报告文件读取失败！");
                return;
            }

            #region 测试报告第1页
            try
            {
                int sheetIndex = iWorkbookRPT.GetSheetIndex("试验报告");
                ISheet sht0 = iWorkbookRPT.GetSheetAt(sheetIndex);
                string str="";

                //单位名头
                HSSFRow row = (HSSFRow)sht0.GetRow(0);
                HSSFCell cell = (HSSFCell)row.GetCell(0);
                cell.SetCellValue(Dev.CoName);
                //报告编号
                row = (HSSFRow)sht0.GetRow(2);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.RepNO);

                //试验编号
                row = (HSSFRow)sht0.GetRow(3);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.ExpNO);
                //检测日期
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(ExpBRXDQ.CreatTime.ToLongDateString());
                //检测依据
                row = (HSSFRow)sht0.GetRow(4);
                cell = (HSSFCell)row.GetCell(3);
                if (Dev.IsStd2023)
                    str = "《GBT 5464-2023 建筑材料不燃性试验方法》";
                cell.SetCellValue(str);
                //检测单位
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(Dev.CoName);
                //实验室地址
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(Dev.LabAddr);
                //委托单位
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.WTDW);
                //委托日期
                cell = (HSSFCell)row.GetCell(15);
                cell.SetCellValue(ExpBRXDQ.WTTime.ToLongDateString());
                //委托单位地址
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.WTDWDZ);
                //生产厂家
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.SCCJ);
                //厂家厂址
                row = (HSSFRow)sht0.GetRow(10);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(ExpBRXDQ.CJCZ);

                //试样信息
                //样品标识
                row = (HSSFRow)sht0.GetRow(11);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.ZPBS);
                //密度
                row = (HSSFRow)sht0.GetRow(12);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.MD);
                //面密度
                cell = (HSSFCell)row.GetCell(13);
                cell.SetCellValue(ExpBRXDQ.MMD);
                //厚度
                cell = (HSSFCell)row.GetCell(20);
                cell.SetCellValue(ExpBRXDQ.HD);
                //结构
                row = (HSSFRow)sht0.GetRow(13);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.JGXX);
                //状态调节信息
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.ZTTJ);
                //抽样程序
                row = (HSSFRow)sht0.GetRow(15);
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(ExpBRXDQ.CYCX);

                //汇总测试数据
                for (int i = 0; i < 5; i++)
                {
                    row = (HSSFRow)sht0.GetRow(17 + i);
                    //炉内平均
                    cell = (HSSFCell)row.GetCell(3);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TUpAvg);
                    //试样中心温升
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TscUp);
                    //试样表面温升
                    cell = (HSSFCell)row.GetCell(9);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TssUp);
                    //试样初始质量
                    cell = (HSSFCell)row.GetCell(12);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].WeightBefore);
                    //试验后质量
                    cell = (HSSFCell)row.GetCell(15);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].WeightFinal);
                    //质量损失率
                    cell = (HSSFCell)row.GetCell(18);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].LostRatio);
                    //持续火焰总时长
                    cell = (HSSFCell)row.GetCell(21);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].FireTimeSum);
                }

                if (Dev.EncryptRPT)
                    sht0.ProtectSheet("12345678");//设置密码保护
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n导出报告第1页失败！");
                throw;
            }

            #endregion


            #region 测试报告第2页
            try
            {
                int sheetIndex = iWorkbookRPT.GetSheetIndex("试验数据");
                ISheet sht0 = iWorkbookRPT.GetSheetAt(sheetIndex);
                string str = "";

                HSSFRow row = (HSSFRow)sht0.GetRow(0);
                HSSFCell cell = (HSSFCell)row.GetCell(0);

                //各试样检测数据计算结果
                for (int i = 0; i < 5; i++)
                {
                    row = (HSSFRow)sht0.GetRow(3 + i);
                    //炉内，初始温度1
                    cell = (HSSFCell)row.GetCell(1);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TStart1);
                    //炉内，初始温度2
                    cell = (HSSFCell)row.GetCell(2);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TStart2);
                    //炉内，初始平均
                    cell = (HSSFCell)row.GetCell(3);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TStartAvg);
                    //炉内，最终温度1
                    cell = (HSSFCell)row.GetCell(4);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TFinal1);
                    //炉内，最终温度2
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TFinal2);
                    //炉内，温升1
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TUp1);
                    //炉内，温升2
                    cell = (HSSFCell)row.GetCell(7);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TUp2);
                    //炉内，温升平均
                    cell = (HSSFCell)row.GetCell(8);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TUpAvg);
                    //炉内，最高温度1
                    cell = (HSSFCell)row.GetCell(9);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TMax1);
                    //炉内，最高温度2
                    cell = (HSSFCell)row.GetCell(10);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TMax2);
                    //火焰持续时间总计
                    cell = (HSSFCell)row.GetCell(11);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].FireTimeSum);


                    row = (HSSFRow)sht0.GetRow(10 + i);
                    //初始质量
                    cell = (HSSFCell)row.GetCell(1);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].WeightBefore);
                    //试验后质量
                    cell = (HSSFCell)row.GetCell(2);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].WeightFinal);
                    //质量损失率
                    cell = (HSSFCell)row.GetCell(3);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].LostRatio);

                    //附加热电偶
                    cell = (HSSFCell)row.GetCell(4);
                    str = ExpBRXDQ.UseAddT ? "√" : "×";
                    cell.SetCellValue(str);
                    //试样中心，温升
                    cell = (HSSFCell)row.GetCell(5);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TscUp);
                    //试样中心，最终温度
                    cell = (HSSFCell)row.GetCell(6);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TscFinal);
                    //试样中心，最高温度
                    cell = (HSSFCell)row.GetCell(7);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TscMax);
                    //试样表面，温升
                    cell = (HSSFCell)row.GetCell(8);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TssUp);
                    //试样表面，最终温度
                    cell = (HSSFCell)row.GetCell(9);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TssFinal);
                    //试样表面，最高温度
                    cell = (HSSFCell)row.GetCell(10);
                    cell.SetCellValue(ExpBRXDQ.SpList[i].TssMax);
                }

                if (Dev.EncryptRPT)
                    sht0.ProtectSheet("12345678");//设置密码保护
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n导出报告第1页失败！");
                throw;
            }

            #endregion


            #region 测试报告第3-7页

            int indexStyle = iWorkbookRPT.GetSheetIndex("试样1");
            ISheet shtStyle = iWorkbookRPT.GetSheetAt(indexStyle);
            HSSFRow row0;
            HSSFCell cell0;

            ICellStyle style0 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style1 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style2 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style3 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style4 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style5 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style6 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style7 = iWorkbookRPT.CreateCellStyle();
            ICellStyle style8 = iWorkbookRPT.CreateCellStyle();

            IDataFormat dataformat = iWorkbookRPT.CreateDataFormat();

            row0 = (HSSFRow)shtStyle.GetRow(2);

            cell0 = (HSSFCell)row0.GetCell(0);
            style0.CloneStyleFrom(cell0.CellStyle);

            cell0 = (HSSFCell)row0.GetCell(1);
            style1.CloneStyleFrom(cell0.CellStyle);
            style2.DataFormat = dataformat.GetFormat("yyyy-MM-DD hh:mm:ss");

            cell0 = (HSSFCell)row0.GetCell(2);
            style2.CloneStyleFrom(cell0.CellStyle);

            cell0 = (HSSFCell)row0.GetCell(3);
            style3.CloneStyleFrom(cell0.CellStyle);
            style3.DataFormat = dataformat.GetFormat("0.00");

            cell0 = (HSSFCell)row0.GetCell(4);
            style4.CloneStyleFrom(cell0.CellStyle);
            style4.DataFormat = dataformat.GetFormat("0.00");

            cell0 = (HSSFCell)row0.GetCell(5);
            style5.CloneStyleFrom(cell0.CellStyle);
            style5.DataFormat = dataformat.GetFormat("0.00");

            cell0 = (HSSFCell)row0.GetCell(6);
            style6.CloneStyleFrom(cell0.CellStyle);
            style5.DataFormat = dataformat.GetFormat("0.00");

            cell0 = (HSSFCell)row0.GetCell(7);
            style7.CloneStyleFrom(cell0.CellStyle);

            cell0 = (HSSFCell)row0.GetCell(8);
            style8.CloneStyleFrom(cell0.CellStyle);

            for (int spNum = 1; spNum <= 5; spNum++)
            {
                try
                {
                    string sheetName = "试样" + spNum.ToString();
                    int sheetIndex = iWorkbookRPT.GetSheetIndex(sheetName);

                    ISheet sht = iWorkbookRPT.GetSheetAt(sheetIndex);
                    string str;

                    HSSFRow row = (HSSFRow)sht.GetRow(0);
                    HSSFCell cell = (HSSFCell)row.GetCell(0);

                    if ((ExpBRXDQ.SpList[spNum - 1].RecList_Stb != null) && (ExpBRXDQ.SpList[spNum - 1].RecList_Stb.Count > 0))
                    {
                        int rowNum = 0;
                        for (int i = 0; i < ExpBRXDQ.SpList[spNum - 1].RecList_Stb.Count; i++)
                        {
                            rowNum = i + 2;
                            row = (HSSFRow)sht.GetRow(rowNum);
                            //序号
                            cell = (HSSFCell)row.GetCell(0);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Stb[i].No);
                            cell.CellStyle = style0;
                            //采样时间
                            cell = (HSSFCell)row.GetCell(1);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Stb[i].RecTime);
                            cell.CellStyle = style1;
                            //试验阶段
                            cell = (HSSFCell)row.GetCell(2);
                            switch (ExpBRXDQ.SpList[spNum - 1].RecList_Stb[i].Stage)
                            {
                                case Enums.TestStage.TCtl_Stage:
                                    str = "控温阶段";
                                    break;
                                case Enums.TestStage.Test_Stage:
                                    str = "测试阶段";
                                    break;
                                case Enums.TestStage.TestEnd_Stage:
                                    str = "测试完成";
                                    break;
                                default:
                                    str = "等待阶段";
                                    break;
                            }
                            cell.SetCellValue(str);
                            cell.CellStyle = style2;
                            //炉内温度1
                            cell = (HSSFCell)row.GetCell(3);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Stb[i].T1);
                            cell.CellStyle = style3;
                            //炉内温度2
                            cell = (HSSFCell)row.GetCell(4);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Stb[i].T2);
                            cell.CellStyle = style4;
                            //试样中心温度
                            cell = (HSSFCell)row.GetCell(5);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Stb[i].Tsc);
                            cell.CellStyle = style5;
                            //试样表面温度
                            cell = (HSSFCell)row.GetCell(6);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Stb[i].Tss);
                            cell.CellStyle = style6;
                            //有火焰
                            cell = (HSSFCell)row.GetCell(7);
                            str = ExpBRXDQ.SpList[spNum - 1].RecList_Stb[i].IsFired ? "√" : "×";
                            cell.SetCellValue(str);
                            cell.CellStyle = style7;
                            //备注
                            cell = (HSSFCell)row.GetCell(8);
                            str = "";
                            cell.SetCellValue(str);
                            cell.CellStyle = style8;
                        }

                        int rowNumStart = rowNum+1;
                        for (int i = 0; i < ExpBRXDQ.SpList[spNum - 1].RecList_Test.Count; i++)
                        {
                            rowNum = i + rowNumStart;
                            row = (HSSFRow)sht.GetRow(rowNum);
                            //序号
                            cell = (HSSFCell)row.GetCell(0);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].No);
                            cell.CellStyle = style0;
                            //采样时间
                            cell = (HSSFCell)row.GetCell(1);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].RecTime);
                            cell.CellStyle = style1;
                            //试验阶段
                            cell = (HSSFCell)row.GetCell(2);
                            switch (ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].Stage)
                            {
                                case Enums.TestStage.TCtl_Stage:
                                    str = "控温阶段";
                                    break;
                                case Enums.TestStage.Test_Stage:
                                    str = "测试阶段";
                                    break;
                                case Enums.TestStage.TestEnd_Stage:
                                    str = "测试完成";
                                    break;
                                default:
                                    str = "等待阶段";
                                    break;
                            }
                            cell.SetCellValue(str);
                            cell.CellStyle = style2;
                            //炉内温度1
                            cell = (HSSFCell)row.GetCell(3);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].T1);
                            cell.CellStyle = style3;
                            //炉内温度2
                            cell = (HSSFCell)row.GetCell(4);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].T2);
                            cell.CellStyle = style4;
                            //试样中心温度
                            cell = (HSSFCell)row.GetCell(5);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].Tsc);
                            cell.CellStyle = style5;
                            //试样表面温度
                            cell = (HSSFCell)row.GetCell(6);
                            cell.SetCellValue(ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].Tss);
                            cell.CellStyle = style6;
                            //有火焰
                            cell = (HSSFCell)row.GetCell(7);
                            str = ExpBRXDQ.SpList[spNum - 1].RecList_Test[i].IsFired ? "√" : "×";
                            cell.SetCellValue(str);
                            cell.CellStyle = style7;
                            //备注
                            cell = (HSSFCell)row.GetCell(8);
                            str = "";
                            cell.SetCellValue(str);
                            cell.CellStyle = style8;
                        }
                    }

                    if (Dev.EncryptRPT)
                        sht.ProtectSheet("12345678");//设置密码保护
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "\r\n试样" + spNum + "数据失败！");
                    throw;
                }

            }
            #endregion

            //数据最终导出
            try
            {
                //   iWorkbookRPT.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();        //强制更新公式
                FileStream streamRPT = File.OpenWrite(Config.RepDir + RptAimFile);
                iWorkbookRPT.Write(streamRPT);
                streamRPT.Flush();
                streamRPT.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n导出报告失败！");
            }

            //打开报告所在文件夹
            System.Diagnostics.Process.Start("explorer.exe", Config.RepDir);
        }
    }
}

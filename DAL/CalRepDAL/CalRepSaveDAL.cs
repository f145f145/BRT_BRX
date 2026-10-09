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
using System.Threading;
using BRX.Model.Enums;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

namespace BRX.DAL.CalRepDAL
{
    public partial class CalRepDAL : ObservableObject
    {
        /// <summary>
        /// 保存炉壁校准报告
        /// </summary>
        private void SaveWallCalRPT()
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
                iWorkbookRPT = new HSSFWorkbook(new FileStream(Config.FormDir + Config.WallCalRepFormName, FileMode.Open, FileAccess.Read, FileShare.None));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n报告文件读取失败！");
                return;
            }

            #region 测试报告第1页
            try
            {
                int sheetIndex = iWorkbookRPT.GetSheetIndex("校准报告");
                ISheet sht0 = iWorkbookRPT.GetSheetAt(sheetIndex);

                //单位名头
                HSSFRow row = (HSSFRow)sht0.GetRow(0);
                HSSFCell cell = (HSSFCell)row.GetCell(0);
                cell.SetCellValue(Dev.CoName);
                //校准编号
                row = (HSSFRow)sht0.GetRow(2);
                cell = (HSSFCell)row.GetCell(3);
                cell.SetCellValue(WallCalDQ.RepNO);
                //校准日期
                cell = (HSSFCell)row.GetCell(27);
                cell.SetCellValue(WallCalDQ.CreatTime.ToLongDateString());


                row = (HSSFRow)sht0.GetRow(6);
                //T1a
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[0].T1);
                //T1b
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[1].T1);
                //T1c
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[2].T1);

                row = (HSSFRow)sht0.GetRow(7);
                //T2a
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[0].T2);
                //T2b
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[1].T2);
                //T2c
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[2].T2);

                row = (HSSFRow)sht0.GetRow(8);
                //T3a
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[0].T3);
                //T3b
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[1].T3);
                //T3c
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[2].T3);

                row = (HSSFRow)sht0.GetRow(12);
                //炉内1a
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[0].Tf1);
                //炉内1b
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[1].Tf1);
                //炉内1c
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[2].Tf1);

                row = (HSSFRow)sht0.GetRow(13);
                //炉内1a
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[0].Tf2);
                //炉内1b
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[1].Tf2);
                //炉内1c
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(WallCalDQ.WallCalPointsList[2].Tf2);

                //Tavg_axis1
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(WallCalDQ.Tavg_axis1);
                //Tavg_axis2
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(WallCalDQ.Tavg_axis2);
                //Tavg_axis3
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(24);
                cell.SetCellValue(WallCalDQ.Tavg_axis3);
                //Tdev_axis1
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(30);
                cell.SetCellValue(WallCalDQ.Tdev_axis1);
                //Tdev_axis2
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(30);
                cell.SetCellValue(WallCalDQ.Tdev_axis2);
                //Tdev_axis3
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(30);
                cell.SetCellValue(WallCalDQ.Tdev_axis3);
                //Tavg_dev_axis
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(36);
                cell.SetCellValue(WallCalDQ.Tavg_dev_axis);

                row = (HSSFRow)sht0.GetRow(9);
                //Tavg_levela
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(WallCalDQ.Tavg_levela);
                //Tavg_levelb
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(WallCalDQ.Tavg_levelb);
                //Tavg_levelc
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(WallCalDQ.Tavg_levelc);
                row = (HSSFRow)sht0.GetRow(10);
                //Tdev_levela
                cell = (HSSFCell)row.GetCell(6);
                cell.SetCellValue(WallCalDQ.Tdev_levela);
                //Tdev_levelb
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(WallCalDQ.Tdev_levelb);
                //Tdev_levelc
                cell = (HSSFCell)row.GetCell(18);
                cell.SetCellValue(WallCalDQ.Tdev_levelc);
                //Tavg_dev_level
                row = (HSSFRow)sht0.GetRow(11);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(WallCalDQ.Tavg_dev_level);

                //Tavg
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(12);
                cell.SetCellValue(WallCalDQ.Tavg);

                //评定结果
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(30);

                cell.SetCellValue(WallCalDQ.Result);


                sht0.ProtectSheet("12345678");//设置密码保护
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n导出报告第1页失败！");
                throw;
            }

            #endregion

            //数据最终导出
            try
            {
             //   iWorkbookRPT.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();        //强制更新公式
                FileStream streamRPT = File.OpenWrite(Config.WallCalRepDir + WallCalRptAimFile);
                iWorkbookRPT.Write(streamRPT);
                streamRPT.Flush();
                streamRPT.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n导出报告失败！");
            }

            //打开报告所在文件夹
            System.Diagnostics.Process.Start("explorer.exe", Config.WallCalRepDir);
        }

        /// <summary>
        /// 保存炉内校准报告
        /// </summary>
        private void SaveCenterCalRPT()
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
                iWorkbookRPT = new HSSFWorkbook(new FileStream(Config.FormDir + Config.CenterCalRepFormName, FileMode.Open, FileAccess.Read, FileShare.None));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n报告文件读取失败！");
                return;
            }

            #region 测试报告第1页
            try
            {
                int sheetIndex = iWorkbookRPT.GetSheetIndex("校准报告");
                ISheet sht0 = iWorkbookRPT.GetSheetAt(sheetIndex);

                //单位名头
                HSSFRow row = (HSSFRow)sht0.GetRow(0);
                HSSFCell cell = (HSSFCell)row.GetCell(0);
                cell.SetCellValue(Dev.CoName);
                //校准编号
                row = (HSSFRow)sht0.GetRow(2);
                cell = (HSSFCell)row.GetCell(2);
                cell.SetCellValue(CenterCalDQ.RepNO);
                //校准日期
                cell = (HSSFCell)row.GetCell(14);
                cell.SetCellValue(CenterCalDQ.CreatTime.ToLongDateString());
              
                //校准数据清单
                //平均值
                for (int i = CenterCalDQ.TAvgList.Count-1; i >=0; i--)
                {
                    row = (HSSFRow)sht0.GetRow(19-i);
                    cell = (HSSFCell)row.GetCell(14);
                    cell.SetCellValue(CenterCalDQ.TAvgList[i]);
                }
                //145mm下
                row = (HSSFRow)sht0.GetRow(5);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[23].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[23].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[23].Tfc);
                //145mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[22].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[22].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[22].Tfc);

                //135mm下
                row = (HSSFRow)sht0.GetRow(6);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[24].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[24].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[24].Tfc);
                //135mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[21].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[21].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[21].Tfc);

                //125mm下
                row = (HSSFRow)sht0.GetRow(7);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[25].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[25].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[25].Tfc);
                //125mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[20].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[20].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[20].Tfc);

                //115mm下
                row = (HSSFRow)sht0.GetRow(8);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[26].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[26].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[26].Tfc);
                //115mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[19].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[19].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[19].Tfc);

                //105mm下
                row = (HSSFRow)sht0.GetRow(9);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[27].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[27].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[27].Tfc);
                //105mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[18].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[18].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[18].Tfc);

                //95mm下
                row = (HSSFRow)sht0.GetRow(10);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[28].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[28].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[28].Tfc);
                //95mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[17].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[17].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[17].Tfc);

                //85mm下
                row = (HSSFRow)sht0.GetRow(11);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[29].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[29].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[29].Tfc);
                //85mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[16].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[16].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[16].Tfc);

                //75mm下
                row = (HSSFRow)sht0.GetRow(12);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[0].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[0].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[0].Tfc);
                //75mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[15].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[15].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[15].Tfc);

                //65mm下
                row = (HSSFRow)sht0.GetRow(13);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[1].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[1].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[1].Tfc);
                //65mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[14].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[14].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[14].Tfc);

                //55mm下
                row = (HSSFRow)sht0.GetRow(14);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[2].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[2].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[2].Tfc);
                //55mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[13].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[13].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[13].Tfc);

                //45mm下
                row = (HSSFRow)sht0.GetRow(15);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[3].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[3].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[3].Tfc);
                //45mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[12].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[12].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[12].Tfc);

                //35mm下
                row = (HSSFRow)sht0.GetRow(16);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[4].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[4].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[4].Tfc);
                //35mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[11].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[11].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[11].Tfc);

                //25mm下
                row = (HSSFRow)sht0.GetRow(17);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[5].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[5].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[5].Tfc);
                //25mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[10].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[10].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[10].Tfc);

                //15mm下
                row = (HSSFRow)sht0.GetRow(18);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[6].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[6].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[6].Tfc);
                //15mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[9].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[9].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[9].Tfc);

                //5mm下
                row = (HSSFRow)sht0.GetRow(19);
                cell = (HSSFCell)row.GetCell(2);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[7].Tf1);
                cell = (HSSFCell)row.GetCell(6);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[7].Tf2);
                cell = (HSSFCell)row.GetCell(10);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[7].Tfc);
                //5mm上
                cell = (HSSFCell)row.GetCell(4);    //T1
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[8].Tf1);
                cell = (HSSFCell)row.GetCell(8);    //T2
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[8].Tf2);
                cell = (HSSFCell)row.GetCell(12);    //Tfc
                cell.SetCellValue(CenterCalDQ.CenterCalPointsList[8].Tfc);

                sht0.ProtectSheet("12345678");//设置密码保护
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n导出报告第1页失败！");
                throw;
            }

            #endregion


            //数据最终导出
            try
            {
                //   iWorkbookRPT.GetCreationHelper().CreateFormulaEvaluator().EvaluateAll();        //强制更新公式
                FileStream streamRPT = File.OpenWrite(Config.CenterCalRepDir + CenterCalRptAimFile);
                iWorkbookRPT.Write(streamRPT);
                streamRPT.Flush();
                streamRPT.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n导出报告失败！");
            }

            //打开报告所在文件夹
            System.Diagnostics.Process.Start("explorer.exe", Config.CenterCalRepDir);
        }
    }
}

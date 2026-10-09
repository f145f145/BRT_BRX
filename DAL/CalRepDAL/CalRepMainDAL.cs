/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL.RepDAL
 * 文件名：  RepMainDAL
 * 版本号：  V1.0.0.0
 * 唯一标识：5582ba35-1f9f-4e7d-a072-2b19b43cb99c
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-4-21 13:28:13
 * 描述：
 * 报告导出。主体部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022-4-21 13:28:13		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using BRX.Model.Dev;
using BRX.Model.Exp;
using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace BRX.DAL.CalRepDAL
{
    public partial class CalRepDAL : ObservableObject 
    {
        public CalRepDAL(DevModel dev,ExpModel_CalWall wallCall, ExpModel_CalCenter centerCal)
        {
            Dev = dev;
            CenterCalDQ = centerCal;
            WallCalDQ = wallCall;

            //报告导出指令消息
            Messenger.Default.Register<string>(this, "ExportRPTMessage", ExportRPTMessage);
        }


        #region dev、EXP属性

        /// <summary>
        /// 装置
        /// </summary>
        private DevModel _dev;
        /// <summary>
        /// 装置
        /// </summary>
        public DevModel Dev
        {
            get { return _dev; }
            set
            {
                _dev = value;
                RaisePropertyChanged(() => Dev);
            }
        }

        /// <summary>
        /// 炉壁校准测试
        /// </summary>
        private ExpModel_CalWall _wallCalDQ;
        /// <summary>
        /// 炉壁校准测试
        /// </summary>
        public ExpModel_CalWall WallCalDQ
        {
            get { return _wallCalDQ; }
            set
            {
                _wallCalDQ = value;
                RaisePropertyChanged(() => WallCalDQ);
            }
        }

        /// <summary>
        /// 炉内校准测试
        /// </summary>
        private ExpModel_CalCenter _centerCalDQ;
        /// <summary>
        /// 炉内校准测试
        /// </summary>
        public ExpModel_CalCenter CenterCalDQ
        {
            get { return _centerCalDQ; }
            set
            {
                _centerCalDQ = value;
                RaisePropertyChanged(() => CenterCalDQ);
            }
        }

        #endregion


        #region 相关字符串

        /// <summary>
        /// 当前时间字符串
        /// </summary>
        private string NowTimeStr
        {
            get
            {
                return DateTime.Now.ToString("—MMddHHmm");
            }
        }

        /// <summary>
        /// 炉壁校准报告文件名
        /// </summary>
        private string WallCalRptAimFile
        {
            get { return "炉壁校准报告" + WallCalDQ.ExpNO + NowTimeStr + ".xls"; }
        }

        /// <summary>
        /// 炉内校准报告文件名
        /// </summary>
        private string CenterCalRptAimFile
        {
            get { return "炉内校准报告" + CenterCalDQ.ExpNO + NowTimeStr + ".xls"; }
        }

        #endregion


        #region 导出报告消息回调

        /// <summary>
        ///根据指令导出报告消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void ExportRPTMessage(string msg)
        {
            //导出报告
            if (msg == "WallCalDQ")
            {
                //文件复制、重命名
                if (CopyFile(Config.FormDir, Config.WallCalRepFormName, Config.WallCalRepDir, WallCalRptAimFile) != 1)
                    return;
                //修改文件
                SaveWallCalRPT();
            }
            if (msg == "CenterCalDQ")
            {
                //文件复制、重命名
                if (CopyFile(Config.FormDir, Config.CenterCalRepFormName, Config.CenterCalRepDir, CenterCalRptAimFile) != 1)
                    return;
                //修改文件
                SaveCenterCalRPT();
            }
        }

        #endregion
        
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourceFile">原文件路径</param>
        /// <param name="destFile">目标文件路径</param>
        /// <returns></returns>
        public int CopyFile(string sourceFolder, string sourceFile, string destFolder, string destFile)
        {
            try
            {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }

                string sourceF = sourceFolder + sourceFile;
                System.IO.File.Copy(sourceF, destFolder + sourceFile,true);//复制文件，强行覆盖

                string sourceF2 = destFolder + sourceFile;
                string delF = destFolder + destFile;
                System.IO.File.Delete(delF);
               Microsoft.VisualBasic.FileIO.FileSystem.RenameFile(sourceF2, destFile);
                
                return 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n复制失败！");
                return 0;
            }

        }
    }
}

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

namespace BRX.DAL.RepDAL
{
    public partial class RepDAL:ObservableObject 
    {
        public RepDAL(DevModel dev, ExpModel_Test expBRX)
        {
            ExpBRXDQ = expBRX;
            Dev = dev;

            //报告导出指令消息
            Messenger.Default.Register<string>(this, "ExportRPTMessage", ExportRPTMessage);

            //数据备份指令消息
            Messenger.Default.Register<string>(this, "DataBackUpMessage", DataBackUpMessage);
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
        /// 当前测试
        /// </summary>
        private ExpModel_Test _expBRXDQ;
        /// <summary>
        /// 当前测试
        /// </summary>
        public ExpModel_Test ExpBRXDQ
        {
            get { return _expBRXDQ; }
            set
            {
                _expBRXDQ = value;
                RaisePropertyChanged(() => ExpBRXDQ);
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
                return DateTime.Now.ToString("MMddHHmm");
            }
        }

        /// <summary>
        /// 试验主数据库备份文件名
        /// </summary>
        private string BackUpFileMainDB
        {
            get { return "DBBackup" + NowTimeStr + "_Main"+".mdb"; }
        }

        /// <summary>
        /// 装置参数数据库备份文件
        /// </summary>
        private string BackUpFileDevDB
        {
            get { return "DBBackup" + NowTimeStr + "_Dev" + ".mdb"; }
        }

        /// <summary>
        /// 试验原始记录数据库备份文件名
        /// </summary>
        private string BackUpFileInitDB
        {
            get { return "DBBackup" + NowTimeStr + "_Init" + ".db"; }
        }

        /// <summary>
        /// 日志数据库备份文件名
        /// </summary>
        private string BackUpFileLogDB
        {
            get { return "DBBackup" + NowTimeStr + "_Log" + ".db"; }
        }

        /// <summary>
        /// 辨识数据库备份文件名
        /// </summary>
        private string BackUpFileIDDB
        {
            get { return "DBBackup" + NowTimeStr + "_ID" + ".db"; }
        }

        /// <summary>
        /// 炉内校准数据库备份文件名
        /// </summary>
        private string BackUpFileCenterCalDB
        {
            get { return "DBBackup" + NowTimeStr+ "_CenterCal" + ".db"; }
        }

        /// <summary>
        /// 炉壁校准数据库备份文件名
        /// </summary>
        private string BackUpFileWallCalDB
        {
            get { return "DBBackup" + NowTimeStr+ "_WallCal" + ".db"; }
        }

        /// <summary>
        /// 检测试验报告文件名
        /// </summary>
        private string RptAimFile
        {
            get { return "试验报告" + ExpBRXDQ.ExpNO + NowTimeStr + ".xls"; }
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
            if (msg == "BRXDQ")
            {
                if (Dev.IsStd2010)
                {
                    //文件复制、重命名
                    if (CopyFile(Config.FormDir, Config.ExpRep10FormName, Config.RepDir, RptAimFile) != 1)
                        return;
                    //修改文件
                    SaveBRXRPT_2010();
                }
                else
                {
                    //文件复制、重命名
                    if (CopyFile(Config.FormDir, Config.ExpRep22FormName, Config.RepDir, RptAimFile) != 1)
                        return;
                    //修改文件
                    SaveBRXRPT_2022();
                }
            }
        }

        #endregion


        #region 备份数据库

        /// <summary>
        /// 数据备份指令消息回调
        /// </summary>
        /// <param name="msg"></param>
        private void DataBackUpMessage(string msg)
        {
            //所有数据库
            if (msg == "All")
            {
                //文件复制、重命名
                if (CopyFile(Config.DataDir, Config.MainDBName, Config.BackUpDir, BackUpFileMainDB) != 1)   //主数据
                    return;
                if (CopyFile(Config.DataDir, Config.DevDBName, Config.BackUpDir, BackUpFileDevDB) != 1)     //装置数据
                    return;
                if (CopyFile(Config.DataDir, Config.InitDBName, Config.BackUpDir, BackUpFileInitDB) != 1)   //原始数据
                    return;
                if (CopyFile(Config.DataDir, Config.LogDBName, Config.BackUpDir, BackUpFileLogDB) != 1)     //日志数据
                    return;
                if (CopyFile(Config.DataDir, Config.IDDBName, Config.BackUpDir, BackUpFileIDDB) != 1)     //辨识数据
                    return;
                if (CopyFile(Config.DataDir, Config.CenterCalInitDBName, Config.BackUpDir, BackUpFileCenterCalDB) != 1)     //中心校准
                    return;
                if (CopyFile(Config.DataDir, Config.WallCalInitDBName, Config.BackUpDir, BackUpFileWallCalDB) != 1)     //炉壁校准
                    return;

                MessageBox.Show("数据已备份至" + Config.BackUpDir);
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

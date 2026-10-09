/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL
 * 文件名：  Config
 * 版本号：  V1.0.0.0
 * 唯一标识：579a7638-2663-4ce7-8f57-2d146c0ae62d
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/9/1 8:00:00
 * 描述：
 * 基础字符串、路径定义
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/9/3 8:00:00		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;

namespace BRX.DAL
{
    /// <summary>
    /// 数据库操作基础类
    /// </summary>
    public static class Config 
    {
        //文件夹
        public static string AppDir = AppDomain.CurrentDomain.BaseDirectory;                            //exe文件路径
        public static string MainDir = System.IO.Directory.GetParent(AppDir).Parent.Parent.FullName;    //程序根目录
        public static string DataDir = MainDir + "\\Data\\DataBase\\";                                  //数据库文件夹
        public static string FormDir = MainDir + "\\Data\\模板\\";                                      //报告模板文件夹
        public static string RepDir = MainDir + "\\Data\\试验报告\\";                                   //检测报告文件夹
        public static string WallCalRepDir = MainDir + "\\Data\\炉壁校准报告\\";                        //炉壁较准报告文件夹
        public static string CenterCalRepDir = MainDir + "\\Data\\炉内校准报告\\";                      //炉内校准报告文件夹
        public static string BackUpDir = MainDir + "\\Data\\Backup\\";                                  //数据库备份文件夹
        public static string VoiceDir = MainDir + "\\Data\\Voice\\";                                   //音频文件夹


        //数据库文件
        public static string MainDBName = "DB.mdb";                                                     //主数据库
        public static string DevDBName = "DevDB.mdb";                                                   //硬件参数配置数据库
        public static string InitDBName = "InitDB.db";                                                  //原始记录数据库
        public static string WallCalInitDBName = "WallCalInitDB.db";                                    //炉壁校准原始记录数据库
        public static string CenterCalInitDBName = "CenterCalInitDB.db";                                  //炉内校准原始记录数据库
        public static string LogDBName = "LogDB.db";                                                    //日志数据库
        public static string IDDBName = "IDDB.db";                                                      //系统辨识数据库
        //报告模板
        public static string ExpRep10FormName = "报告模板-2010.xls";                                    //老标准报告模板文件名
        public static string ExpRep22FormName = "报告模板-2022.xls";                                    //新标准报告模板文件名
        public static string WallCalRepFormName = "炉壁校准报告模板.xls";                               //炉壁校准报告模板文件名
        public static string CenterCalRepFormName = "炉内校准报告模板.xls";                             //炉内校准报告模板文件名

        //数据库文件完整路径
        public static string MainDBFile = MainDir + "\\Data\\DataBase\\DB.mdb";                         //主数据库
        public static string DevDBFile = MainDir + "\\Data\\DataBase\\DevDB.mdb";                       //硬件参数配置数据库
        public static string InitDBFile = MainDir + "\\Data\\DataBase\\InitDB.db";                      //原始记录数据库
        public static string WallCalInitDBFile = MainDir + "\\Data\\DataBase\\WallCalInitDB.db";        //炉壁校准原始记录数据库
        public static string CenterCalInitDBFile = MainDir + "\\Data\\DataBase\\CenterCalInitDB.db";    //炉内校准原始记录数据库
        public static string LogDBFile = MainDir + "\\Data\\DataBase\\LogDB.db";                        //日志数据库
        public static string IDDBFile = MainDir + "\\Data\\DataBase\\IDDB.db";                          //系统辨识数据库
        //数据库连接字符串
        public static string ConnString_Init = @"DataSource=" + InitDBFile;                             //原始记录数据库连接字符串（sqlite）
        public static string ConnString_Log = @"DataSource=" + InitDBFile;                              //日志数据库连接字符串（sqlite）
        public static string ConnString_ID = @"DataSource=" + IDDBFile;                                 //系统辨识数据库连接字符串（sqlite）
        public static string ConnString_WallCalInit = @"DataSource=" + WallCalInitDBFile;               //炉壁校准原始记录数据库连接字符串（sqlite）
        public static string ConnString_CenterCalInit = @"DataSource=" +  CenterCalInitDBFile;          //炉内校准原始记录数据库连接字符串（sqlite）



    }
}

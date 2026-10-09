/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.DAL.ExpDAL.ExpDALModel
 * 文件名：  InitRec
 * 版本号：  V1.0.0.0
 * 唯一标识：15353074-3dde-4a80-87f1-584b35f07bb7
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022-9-3 10:18:00
 * 描述：
 * 检测原始数据记录表Model（sqlite—sqlsugar）
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/9/3 23:14:24		郝正强			V1.0.0.0
 *
************************************************************************************/


using SqlSugar;
using System;

namespace BRX.DAL.ExpDAL.ExpDALModel
{
    /// <summary>
    /// 检测原始数据记录表Model（sqlite—sqlsugar）
    ///</summary>
    [SugarTable("InitRec")]
    public class InitRec
    {
        /// <summary>
        ///  记录编号，自增
        ///</summary>
        [SugarColumn(ColumnName = "ID", IsPrimaryKey = true, IsIdentity = true, IsNullable = false)]
        public int Id { get; set; }

        /// <summary>
        ///  试验编号
        ///</summary>
        [SugarColumn(ColumnName = "试验编号", IsNullable = true)]
        public string ExpNO { get; set; }

        /// <summary>
        ///  样品编号
        ///</summary>
        [SugarColumn(ColumnName = "样品编号", IsNullable = true)]
        public int SpNO { get; set; }

        /// <summary>
        ///  本样品的记录序号，从1开始
        ///</summary>
        [SugarColumn(ColumnName = "记录序号", IsNullable = true)]
        public int RecNum { get; set; }

        /// <summary>
        ///  记录时间
        ///</summary>
        [SugarColumn(ColumnName = "记录时间", IsNullable = true)]
        public DateTime RecTime { get; set; }

        /// <summary>
        ///  试验阶段
        ///</summary>
        [SugarColumn(ColumnName = "试验阶段", IsNullable = true)]
        public string ExpStage { get; set; }

        /// <summary>
        ///  炉内温度1
        ///</summary>
        [SugarColumn(ColumnName = "炉内温度1", IsNullable = true)]
        public double T1 { get; set; }

        /// <summary>
        ///  炉内温度2
        ///</summary>
        [SugarColumn(ColumnName = "炉内温度2", IsNullable = true)]
        public double T2 { get; set; }

        /// <summary>
        ///  试样中心温度
        ///</summary>
        [SugarColumn(ColumnName = "试样中心温度", IsNullable = true)]
        public double Tsc { get; set; }

        /// <summary>
        ///  试样表面温度
        ///</summary>
        [SugarColumn(ColumnName = "试样表面温度", IsNullable = true)]
        public double Tss { get; set; }

        /// <summary>
        ///  输出电压百分比（%）
        ///</summary>
        [SugarColumn(ColumnName = "输出电压", IsNullable = true)]
        public double Vo { get; set; }

        /// <summary>
        ///  有火焰
        ///</summary>
        [SugarColumn(ColumnName = "有火焰", IsNullable = true)]
        public bool IsFired { get; set; }

        /// <summary>
        ///  说明
        ///</summary>
        [SugarColumn(ColumnName = "说明", IsNullable = true)]
        public string Detail { get; set; }

    }
}
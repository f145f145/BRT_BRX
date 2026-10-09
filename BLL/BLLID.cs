/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.BLL
 * 文件名：  BLLExp
 * 版本号：  V1.0.0.0
 * 唯一标识：2951152b-32bb-4f70-861a-78ea2f2d2012
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/23 18:51:29
 * 描述：
  * 。BLL，数据辨识部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/18 16:22:39		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using BRX.Model.Exp;
using static BRX.Model.Enums.Enums;
using BRX.DAL.ExpDAL.ExpDALModel;

namespace BRX.BLL
{
    public partial class Bll : ObservableObject
    {
        /// <summary>
        /// 系统辨识事务
        /// </summary>
        private void IDBLLAsync()
        {
            if (Dev.RunMode != RunMode.ID_Mode)
                return;

            //AO输出
            //软启动
            double aoOutMax = SoftBoot();
            double aoOutShould = Dev.VOut_ID;
            if (aoOutShould >= aoOutMax)
                aoOutShould = aoOutMax;
            Dev.AOList[0].ValueFinal = aoOutShould;
            Dev.AOList[0].CalcAODatas();

            //记录数据
            RecID();
           IdNum++;

        }

        #region 辅助属性、方法

        /// <summary>
        /// 当前编号
        /// </summary>
        private int _idNum = 0;
        /// <summary>
        /// 当前编号
        /// </summary>
        private int IdNum
        {
            get { return _idNum; }
            set
            {
                _idNum = value;
                RaisePropertyChanged(() => IdNum);
            }
        }

        /// <summary>
        /// 记录当前辨识记录
        /// </summary>
        private void RecID()
        {
            IDRec newRec = new IDRec();

            newRec.RecNum = IdNum;
            newRec.RecTime = DateTime.Now;
            newRec.T1 = Dev.AIList[0].ValueFinal;
            newRec.T2 = Dev.AIList[1].ValueFinal;
            newRec.Vo = Dev.AOList[0].ValueFinal;
            newRec.Vo_V = Dev.VO_110VBase;
            newRec.UseOut = true; 
            newRec.Detail = " ";


            //保存试验数据记录
            Messenger.Default.Send<IDRec>(newRec, "SaveIDRecMessage");
        }



        #endregion

        #region 消息处理
        
        /// <summary>
        /// 系统辨识消息处理
        /// </summary>
        /// <param name="msg"></param>
        private void StartIDMessage(int msg)
        {
            //开始辨识
            if (msg == 1)
            {
                IdNum = 0;
                StopCMD = false;
                Dev.RunMode = RunMode.ID_Mode;
                Dev.IsBusy = true;
            }
        }
        
        #endregion
    }
}
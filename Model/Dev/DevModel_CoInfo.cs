/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.Model.Dev
 * 文件名：  DevModel_CoInfo
 * 版本号：  V1.0.0.0
 * 唯一标识：f719d0d1-6e8b-4a17-a173-ca2ceb82543f
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/4 7:48:42
 * 描述：
 * 装置Model，公司信息信息部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/3 22:45:36		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRX.Model.Dev
{
    public partial class DevModel : ObservableObject
    {
        
        #region 公司信息

        /// <summary>
        /// 公司简称
        /// </summary>
        private string _coShortName = "//";
        /// <summary>
        /// 公司简称
        /// </summary>
        public string CoShortName
        {
            get { return _coShortName; }
            set
            {
                _coShortName = value;
                RaisePropertyChanged(() => CoShortName);
            }
        }

        /// <summary>
        /// 公司全称
        /// </summary>
        private string _coName = "//";
        /// <summary>
        /// 公司全称
        /// </summary>
        public string CoName
        {
            get { return _coName; }
            set
            {
                _coName = value;
                RaisePropertyChanged(() => CoName);
            }
        }

        /// <summary>
        /// 公司地址
        /// </summary>
        private string _coAddr = "//";
        /// <summary>
        /// 公司地址
        /// </summary>
        public string CoAddr
        {
            get { return _coAddr; }
            set
            {
                _coAddr = value;
                RaisePropertyChanged(() => CoAddr);
            }
        }

        /// <summary>
        /// 公司邮政编码
        /// </summary>
        private string _comPostNO = "//";
        /// <summary>
        /// 公司邮政编码
        /// </summary>
        public string CoPostNO
        {
            get { return _comPostNO; }
            set
            {
                _comPostNO = value;
                RaisePropertyChanged(() => CoPostNO);
            }
        }

        /// <summary>
        /// 公司电话
        /// </summary>
        private string _coTel = "//";
        /// <summary>
        /// 公司电话
        /// </summary>
        public string CoTel
        {
            get { return _coTel; }
            set
            {
                _coTel = value;
                RaisePropertyChanged(() => CoTel);
            }
        }

        /// <summary>
        /// 实验室地址
        /// </summary>
        private string _labAddr = "//";
        /// <summary>
        /// 实验室地址
        /// </summary>
        public string LabAddr
        {
            get { return _labAddr; }
            set
            {
                _labAddr = value;
                RaisePropertyChanged(() => LabAddr);
            }
        }
        
        /// <summary>
        /// 实验室邮政编码
        /// </summary>
        private string _labPostNO = "//";
        /// <summary>
        /// 实验室邮政编码
        /// </summary>
        public string LabPostNO
        {
            get { return _labPostNO; }
            set
            {
                _labPostNO = value;
                RaisePropertyChanged(() => LabPostNO);
            }
        }
        
        /// <summary>
        /// 实验室电话
        /// </summary>
        private string _labTel = "//";
        /// <summary>
        /// 实验室电话
        /// </summary>
        public string LabTel
        {
            get { return _labTel; }
            set
            {
                _labTel = value;
                RaisePropertyChanged(() => LabTel);
            }
        }

        #endregion

    }
}

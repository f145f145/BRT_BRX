/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/4 5:20:02
 * 描述：
 * 装置Model，装置基本信息部分
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

        /// <summary>
        /// 装置编号
        /// </summary>
        private string _deviceID = "BRX01";
        /// <summary>
        /// 装置ID
        /// </summary>
        public string DeviceID
        {
            get { return _deviceID; }
            set
            {
                _deviceID = value;
                RaisePropertyChanged(() => DeviceID);
            }
        }

        /// <summary>
        /// 装置名称
        /// </summary>
        private string _deviceName = "建筑材料不燃性测试仪";
        /// <summary>
        /// 装置名称
        /// </summary>
        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                _deviceName = value;
                RaisePropertyChanged(() => DeviceName);
            }
        }


        /// <summary>
        /// 装置出厂编号
        /// </summary>
        private string _deviceSerialNO = "BRX2201";
        /// <summary>
        /// 装置出厂编号
        /// </summary>
        public string DeviceSerialNO
        {
            get { return _deviceSerialNO; }
            set
            {
                _deviceSerialNO = value;
                RaisePropertyChanged(() => DeviceSerialNO);
            }
        }

        /// <summary>
        /// 装置设备型号
        /// </summary>
        private string _deviceType = "BRX";
        /// <summary>
        /// 装置设备型号
        /// </summary>
        public string DeviceType
        {
            get { return _deviceType; }
            set
            {
                _deviceType = value;
                RaisePropertyChanged(() => DeviceType);
            }
        }

        /// <summary>
        /// 参考标准
        /// </summary>
        private string _referenceStd = "《》";
        /// <summary>
        /// 参考标准
        /// </summary>
        public string ReferenceStd
        {
            get { return _referenceStd; }
            set
            {
                _referenceStd = value;
                RaisePropertyChanged(() => ReferenceStd);
            }
        }

        /// <summary>
        /// 设计单位
        /// </summary>
        private string _sjDW = "";
        /// <summary>
        /// 设计单位
        /// </summary>
        public string SJDW
        {
            get { return _sjDW; }
            set
            {
                _sjDW = value;
                RaisePropertyChanged(() => SJDW);
            }
        }
    }
}

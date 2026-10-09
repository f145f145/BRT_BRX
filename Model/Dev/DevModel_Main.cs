/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 * 装置Model，主体部分
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/3 22:45:36		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Linq;

namespace BRX.Model.Dev
{
    public partial class DevModel : ObservableObject
    {
        public DevModel()
        {
            //获取串口列表
            GetComList();


            //错误信息复位定时器初始化
            ErrInfoRstTimer.Tick += new EventHandler(ErrInfoRstTimer_Tick);
            ErrInfoRstTimer.Interval = TimeSpan.FromSeconds(5);
            ErrInfoRstTimer.Start();

            //订阅传输值更新消息
            Messenger.Default.Register<ushort[]>(this, "TDatasUpdated", TDatasUpdatedMessage);
            Messenger.Default.Register<ushort[]>(this, "T3134DatasUpdated", T3134DatasUpdatedMessage);
            Messenger.Default.Register<ushort[]>(this, "AODatasUpdated", AODatasUpdatedMessage);
            Messenger.Default.Register<ushort[]>(this, "PowerDatasUpdated", PowerDatasUpdatedMessage);

            //订阅一键调零消息
            Messenger.Default.Register<int>(this, "AutoZeroMessage", AutoZeroMessage);

            //订阅错误信息提示消息
            Messenger.Default.Register<string>(this, "ComErrorT", ErrorMessage);
            Messenger.Default.Register<string>(this, "ComErrorAO", ErrorMessage);
            Messenger.Default.Register<string>(this, "ComErrorPower", ErrorMessage);
        }


        #region 根据通讯消息更新AI、DI变量

        /// <summary>
        /// 根据3138热电偶模块通讯采集数据更新变量值
        /// </summary>
        private void TDatasUpdatedMessage(ushort[] msg)
        {
            ushort[] inArry;
            inArry = (ushort[])msg.Clone();
            if (inArry.Length >= 1)
            {
                //数据解析，更新模块AI通道传输值
                for (int i = 0; i < Mod_AI_T.Channels.Count; i++)
                {
                    uint uintValue = inArry[i];
                    Mod_AI_T.Channels[i].DataRealTime = uintValue;
                }

                for (int i = 0; i < 8; i++)     //0-7
                    AIList[i].CalcAIValues();
            }
        }

        /// <summary>
        /// 根据3134热电偶模块通讯采集数据更新变量值
        /// </summary>
        private void T3134DatasUpdatedMessage(ushort[] msg)
        {
            ushort[] inArry;
            inArry = (ushort[])msg.Clone();
            if (inArry.Length >= 1)
            {
                //数据解析，更新模块AI通道传输值
                for (int i = 0; i < 4; i++)
                {
                    uint uintValue = inArry[i];
                    Mod_AI_T.Channels[i].DataRealTime = uintValue;
                }

                for (int i = 0; i < 4; i++)     //0-7
                    AIList[i].CalcAIValues();
            }
        }

        /// <summary>
        /// 根据功率模块通讯采集数据更新变量值
        /// </summary>
        private void PowerDatasUpdatedMessage(ushort[] msg)
        {
            ushort[] inArry;
            inArry = (ushort[])msg.Clone();

            if (inArry.Length >= 1)
            {
                //数据解析，更新模块AI通道传输值
                for (int i = 0; i < Mod_AI_Power.Channels.Count; i++)
                {
                    uint uintValue;
                    switch (i)
                    {
                        case 2:
                        case 3:
                        case 5:
                            Mod_AI_Power.Channels[i].DataRealTime = ValueCvt.GetI16FromUshortArry(inArry, i);
                            Mod_AI_Power.Channels[i].DataRealTime = Math.Abs(Mod_AI_Power.Channels[i].DataRealTime);
                            // uintValue = inArry[i];
                            // Mod_AI_Power.Channels[i].DataRealTime = uintValue;
                            break;

                        default:
                            uintValue = inArry[i];
                            Mod_AI_Power.Channels[i].DataRealTime = uintValue;
                            break;
                    }
                }

                for (int i = 8; i < AIList.Count; i++)      //8-15
                    AIList[i].CalcAIValues();
            }
        }

        /// <summary>
        /// 根据AO通讯采集数据更新变量值
        /// </summary>
        private void AODatasUpdatedMessage(ushort[] msg)
        {
            RaisePropertyChanged(() => VO_110VBase);
            RaisePropertyChanged(() => Power_110VBase);
        }

        #endregion
    }

    /// <summary>
    /// 错误信息类
    /// </summary>
    public class ErrInfo : ObservableObject
    {
        /// <summary>
        /// 错误编号
        /// </summary>
        private string _no = "";
        /// <summary>
        /// 错误编号
        /// </summary>
        public string NO
        {
            get { return _no; }
            set
            {
                _no = value;
                RaisePropertyChanged(() => NO);
            }
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        private string _code = "";
        /// <summary>
        /// 错误代码
        /// </summary>
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                RaisePropertyChanged(() => Code);
            }
        }

        /// <summary>
        /// 错误类型
        /// </summary>
        private int _type = 0;
        /// <summary>
        /// 错误类型
        /// </summary>
        public int Type
        {
            get { return _type; }
            set
            {
                _type = value;
                RaisePropertyChanged(() => Type);
            }
        }

        /// <summary>
        /// 时间
        /// </summary>
        private DateTime _time = DateTime.Now;
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        /// <summary>
        /// 补充说明
        /// </summary>
        private string _ps = "";
        /// <summary>
        /// 补充说明
        /// </summary>
        public string PS
        {
            get { return _ps; }
            set
            {
                _ps = value;
                RaisePropertyChanged(() => PS);
            }
        }
    }
}

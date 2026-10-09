/************************************************************************************
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			        修改人			版本号			描述
 * 2022/2/8 15:10:12		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using BRX.Model.Dev;
using GalaSoft.MvvmLight.Messaging;

namespace BRX.DAL.DevDAL
{
    /// <summary>
    /// 装置参数读写操作类
    /// </summary>
    public partial  class DevDAL : ObservableObject
    {
        #region 装置数据保存

        /// <summary>
        /// 保存装置设置参数
        /// </summary>
        private void SaveDevSettings()
        {
            //装置忙，无法保存
            if (DAL_Dev.IsBusy)
            {
                MessageBox.Show("装置忙，请关闭正在运行的试验或测试软件", "错误提示");
                return;
            }
            // 保存装置基本信息C01
            SaveBisicInfo();
            //基本参数
            SaveBisicParam();
            //保存公司信息C03
            SaveCoInfo();
            //保存串口参数
            SaveComSettings();
            //模拟量参数
            SaveAIOParam();
            //模拟量通道
            SaveAIOChannels();
            //PID参数
            SavePIDSettings();

            Messenger.Default.Send<DevModel>(DAL_Dev, "DevSavedMessage");
        }

        /// <summary>
        /// 保存装置基本信息C01
        /// </summary>
        private void SaveBisicInfo()
        {
            string tempSettingsNO = "Using";
            
            //保存C01装置基本参数
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                DevDBDataSet.C01装置信息Row checkExistC01Row = C01Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC01Row == null)
                {
                    DevDBDataSet.C01装置信息Row defDevC01Row = C01Table.FindBy配置编号("DefaultSetting");
                    DevDBDataSet.C01装置信息Row newDevC01Row = C01Table.NewC01装置信息Row();
                    newDevC01Row.ItemArray = (object[])defDevC01Row.ItemArray.Clone();
                    newDevC01Row.配置编号 = tempSettingsNO;
                    C01Table.AddC01装置信息Row(newDevC01Row);
                    C01TableAdapter.Update(C01Table);
                    C01Table.AcceptChanges();
                    RaisePropertyChanged(() => C01Table);
                    MessageBox.Show("未找到在用装置基本信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DevDBDataSet.C01装置信息Row devC01Row = C01Table.FindBy配置编号(tempSettingsNO);
                if (devC01Row != null)
                {
                    devC01Row.装置ID = DAL_Dev.DeviceID;
                    devC01Row.装置名称 = DAL_Dev.DeviceName;
                    devC01Row.出厂编号 = DAL_Dev.DeviceSerialNO;
                    devC01Row.设备型号 = DAL_Dev.DeviceType;
                    devC01Row.试验方法标准 = DAL_Dev.ReferenceStd;
                    devC01Row.设计单位 = DAL_Dev.SJDW;

                    C01TableAdapter.Update(C01Table);
                    C01Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存C02装置基本参数
        /// </summary>
        private void SaveBisicParam()
        {
            string tempSettingsNO = "Using";
            
            //保存C02装置基本参数
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                DevDBDataSet.C02装置基本参数Row checkExistC02Row = C02Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC02Row == null)
                {
                    DevDBDataSet.C02装置基本参数Row defDevC02Row = C02Table.FindBy配置编号("DefaultSetting");
                    DevDBDataSet.C02装置基本参数Row newDevC02Row = C02Table.NewC02装置基本参数Row();
                    newDevC02Row.ItemArray = (object[])defDevC02Row.ItemArray.Clone();
                    newDevC02Row.配置编号 = tempSettingsNO;
                    C02Table.AddC02装置基本参数Row(newDevC02Row);
                    C02TableAdapter.Update(C02Table);
                    C02Table.AcceptChanges();
                    RaisePropertyChanged(() => C02Table);
                    MessageBox.Show("未找到基本参数C02，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DevDBDataSet.C02装置基本参数Row devC02Row = C02Table.FindBy配置编号(tempSettingsNO);
                if (devC02Row != null)
                {
                    devC02Row.绘图更新周期 = DAL_Dev.PlotPeriod;
                    devC02Row.单曲线总点数 = DAL_Dev.PointsPerLine;

                    devC02Row.Period_BLL = DAL_Dev.Period_BLL;
                    devC02Row.Period_Comm = DAL_Dev.Period_Comm;

                    devC02Row.Time_Stabilize = DAL_Dev.Time_Stabilize;
                    devC02Row.Period_Stabilize = DAL_Dev.Period_Stabilize;
                    devC02Row.Time_FinalEqu = DAL_Dev.Time_FinalEqu;
                    devC02Row.Period_FinalEqu = DAL_Dev.Period_FinalEqu;
                    devC02Row.Time_FinalEquEvalFist = DAL_Dev.Time_FinalEquEvalFist;
                    devC02Row.Time_FinalEquEvalMax = DAL_Dev.Time_FinalEquEvalMax;
                    devC02Row.TCtlAimTest = DAL_Dev.TCtlAimTest;
                    devC02Row.TCtlErrPermit = DAL_Dev.TCtlErrPermit;
                    devC02Row.TCtlDriftPermit = DAL_Dev.TCtlDriftPermit;
                    devC02Row.TCtlDeviationPermit = DAL_Dev.TCtlDeviationPermit;
                    devC02Row.TFinalEquDriftPermit = DAL_Dev.TFinalEquDriftPermit;
                    devC02Row.TUpSpeed = DAL_Dev.TUpSpeed;

                    devC02Row.IsLoadLastExpPowerOn = DAL_Dev.IsLoadLastExpPowerOn;
                    devC02Row.IsAutoContinue = DAL_Dev.IsAutoContinue;
                    devC02Row.WithPowerSenser = DAL_Dev.WithPowerSenser;
                    devC02Row.ExpNOLast = DAL_Dev.ExpNOLast;
                    devC02Row.WallCalNOLast = DAL_Dev.WallCalNOLast;
                    devC02Row.CenterCalNOLast = DAL_Dev.CenterCalNOLast;

                    devC02Row.StdSelected = DAL_Dev.StdSelected;
                    devC02Row.R = DAL_Dev.R;
                    devC02Row.Vin = DAL_Dev.Vin;

                    devC02Row.TModelType = DAL_Dev.TModelType;
                    devC02Row.AOModelType = DAL_Dev.AOModelType;
                    devC02Row.VOut_ID = DAL_Dev.VOut_ID;

                    devC02Row.SoftBootTime = DAL_Dev.SoftBootTime;
                    devC02Row.RatioAoOutMax = DAL_Dev.RatioAoOutMax;

                    devC02Row.OutReduceP = DAL_Dev.OutReduceP;
                    devC02Row.OutReduceU = DAL_Dev.OutReduceU;

                    devC02Row.AutoPower = DAL_Dev.AutoPower;
                    devC02Row.TimeCalcPower = DAL_Dev.TimeCalcPower;
                    devC02Row.FixedPower_P = DAL_Dev.FixedPower_P;
                    devC02Row.FixedPower_U = DAL_Dev.FixedPower_U;

                    devC02Row.TStartPID = DAL_Dev.TStartPID;

                    devC02Row.InitSumUI_P = DAL_Dev.InitSumUI_P;
                    devC02Row.InitSumUI_U = DAL_Dev.InitSumUI_U;

                    devC02Row.DelayTime = DAL_Dev.DelayTime;
                    devC02Row.TDelayU = DAL_Dev.TDelayU;
                    devC02Row.TDelayP = DAL_Dev.TDelayP;

                    devC02Row.备用bool1 = DAL_Dev.EncryptRPT;
                    devC02Row.备用bool2 = DAL_Dev.HideSY2345;

                    C02TableAdapter.Update(C02Table);
                    C02Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存公司信息C03
        /// </summary>
        private void SaveCoInfo()
        {
            string tempSettingsNO = "Using";
            
            //保存C03公司信息
            //若编号在表中不存在，则新建（拷贝DefaultSetting）
            try
            {
                DevDBDataSet.C03公司信息Row checkExistC03Row = C03Table.FindBy配置编号(tempSettingsNO);
                if (checkExistC03Row == null)
                {
                    DevDBDataSet.C03公司信息Row defDevC03Row = C03Table.FindBy配置编号("DefaultSetting");
                    DevDBDataSet.C03公司信息Row newDevC03Row = C03Table.NewC03公司信息Row();
                    newDevC03Row.ItemArray = (object[])defDevC03Row.ItemArray.Clone();
                    newDevC03Row.配置编号 = tempSettingsNO;
                    C03Table.AddC03公司信息Row(newDevC03Row);
                    C03TableAdapter.Update(C03Table);
                    C03Table.AcceptChanges();
                    RaisePropertyChanged(() => C03Table);
                    MessageBox.Show("未找到公司信息，已重新建立！", "错误提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            try
            {
                DevDBDataSet.C03公司信息Row devC03Row = C03Table.FindBy配置编号(tempSettingsNO);
                if (devC03Row != null)
                {
                    devC03Row.公司简称 = DAL_Dev.CoShortName;
                    devC03Row.公司全称 = DAL_Dev.CoName;
                    devC03Row.公司地址 = DAL_Dev.CoAddr;
                    devC03Row.实验室地址 = DAL_Dev.LabAddr;
                    devC03Row.公司邮政编码 = DAL_Dev.CoPostNO;
                    devC03Row.实验室邮政编码 = DAL_Dev.LabPostNO;
                    devC03Row.公司电话 = DAL_Dev.CoTel;
                    devC03Row.实验室电话 = DAL_Dev.LabTel;
                    C03TableAdapter.Update(C03Table);
                    C03Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        /// <summary>
        /// 保存C04串口设置参数
        /// </summary>
        private void SaveComSettings()
        {
            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC04RowName = new string[3]
            {
                "TCOM","AOCom","PowerCom"
            };

            if (DAL_Dev.TModelType == "DAM3134")
                tempC04RowName[0] = tempC04RowName[0] + "3134";

            if (DAL_Dev.AOModelType == "UT5564A")
                tempC04RowName[1] = tempC04RowName[1] + "5564A";
            else if (DAL_Dev.AOModelType == "Modbus-4AI4AO")
                tempC04RowName[1] = tempC04RowName[1] + "4AI4AO";
            for (int i = 0; i < tempC04RowName.Length ; i++)
            {
                try
                {
                    DevDBDataSet.C04串口参数设置Row checkExistC04Row = C04Table.FindBy配置编号(tempC04RowName[i]);
                    if (checkExistC04Row == null)
                    {
                        DevDBDataSet.C04串口参数设置Row defDevC04Row = C04Table.FindBy配置编号("DefaultSetting");
                        DevDBDataSet.C04串口参数设置Row newDevC04ow = C04Table.NewC04串口参数设置Row();
                        newDevC04ow.ItemArray = (object[])defDevC04Row.ItemArray.Clone();
                        newDevC04ow.配置编号 = tempC04RowName[i];
                        C04Table.AddC04串口参数设置Row(newDevC04ow);
                        C04TableAdapter.Update(C04Table);
                        C04Table.AcceptChanges();
                        RaisePropertyChanged(() => C04Table);
                        MessageBox.Show("未找到串口配置参数" + tempC04RowName[i] + "，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //TCOM
            try
            {
                DevDBDataSet.C04串口参数设置Row devC04thpRow = C04Table.FindBy配置编号(tempC04RowName[0]);
                if (devC04thpRow != null)
                {
                    devC04thpRow.PhyPortNO = DAL_Dev.TCom.PhyPortNO;
                    devC04thpRow.BaoudRate = DAL_Dev.TCom.BoundRate;
                    devC04thpRow.StartBits = DAL_Dev.TCom.StartBits;
                    devC04thpRow.DataBits = DAL_Dev.TCom.DataBits;
                    devC04thpRow.PlCAddr = DAL_Dev.TCom.Addr;
                    devC04thpRow.CommRW_Period = DAL_Dev.TCom.PeriodRW;
                    devC04thpRow.WatchDogReset_Period = DAL_Dev.TCom.WatchDogPeriod;
                    devC04thpRow.Timeout = DAL_Dev.TCom.Timeout;
                    devC04thpRow.Time_BusyDealy = DAL_Dev.TCom.Time_BusyDealy;
                    devC04thpRow.CMDRepeat = DAL_Dev.TCom.CMDRepeat;

                    if (DAL_Dev.TCom.StopBits == StopBits.None)
                        devC04thpRow.StopBits = 0;
                    else if (DAL_Dev.TCom.StopBits == StopBits.One)
                        devC04thpRow.StopBits = 1;
                    else if (DAL_Dev.TCom.StopBits == StopBits.Two)
                        devC04thpRow.StopBits = 2;
                    else if (DAL_Dev.TCom.StopBits == StopBits.Two)
                        devC04thpRow.StopBits = 2;
                    else if (DAL_Dev.TCom.StopBits == StopBits.OnePointFive)
                        devC04thpRow.StopBits = 15;

                    if (DAL_Dev.TCom.Parity == Parity.None)
                        devC04thpRow.Parity = 0;
                    else if (DAL_Dev.TCom.Parity == Parity.Odd)
                        devC04thpRow.Parity = 1;
                    else if (DAL_Dev.TCom.Parity == Parity.Even)
                        devC04thpRow.Parity = 2;
                    else if (DAL_Dev.TCom.Parity == Parity.Mark)
                        devC04thpRow.Parity = 10;
                    else if (DAL_Dev.TCom.Parity == Parity.Space)
                        devC04thpRow.Parity = 20;
                    C04TableAdapter.Update(C04Table);
                    C04Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //AOCom
            try
            {
                DevDBDataSet.C04串口参数设置Row devC04thpRow = C04Table.FindBy配置编号(tempC04RowName[1]);
                if (devC04thpRow != null)
                {
                    devC04thpRow.PhyPortNO = DAL_Dev.AOCom.PhyPortNO;
                    devC04thpRow.BaoudRate = DAL_Dev.AOCom.BoundRate;
                    devC04thpRow.StartBits = DAL_Dev.AOCom.StartBits;
                    devC04thpRow.DataBits = DAL_Dev.AOCom.DataBits;
                    devC04thpRow.PlCAddr = DAL_Dev.AOCom.Addr;
                    devC04thpRow.CommRW_Period = DAL_Dev.AOCom.PeriodRW;
                    devC04thpRow.WatchDogReset_Period = DAL_Dev.AOCom.WatchDogPeriod;
                    devC04thpRow.Timeout = DAL_Dev.AOCom.Timeout;
                    devC04thpRow.Time_BusyDealy = DAL_Dev.AOCom.Time_BusyDealy;
                    devC04thpRow.CMDRepeat = DAL_Dev.AOCom.CMDRepeat;

                    if (DAL_Dev.AOCom.StopBits == StopBits.None)
                        devC04thpRow.StopBits = 0;
                    else if (DAL_Dev.AOCom.StopBits == StopBits.One)
                        devC04thpRow.StopBits = 1;
                    else if (DAL_Dev.AOCom.StopBits == StopBits.Two)
                        devC04thpRow.StopBits = 2;
                    else if (DAL_Dev.AOCom.StopBits == StopBits.Two)
                        devC04thpRow.StopBits = 2;
                    else if (DAL_Dev.AOCom.StopBits == StopBits.OnePointFive)
                        devC04thpRow.StopBits = 15;

                    if (DAL_Dev.AOCom.Parity == Parity.None)
                        devC04thpRow.Parity = 0;
                    else if (DAL_Dev.AOCom.Parity == Parity.Odd)
                        devC04thpRow.Parity = 1;
                    else if (DAL_Dev.AOCom.Parity == Parity.Even)
                        devC04thpRow.Parity = 2;
                    else if (DAL_Dev.AOCom.Parity == Parity.Mark)
                        devC04thpRow.Parity = 10;
                    else if (DAL_Dev.AOCom.Parity == Parity.Space)
                        devC04thpRow.Parity = 20;
                    C04TableAdapter.Update(C04Table);
                    C04Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PowerCom
            try
            {
                DevDBDataSet.C04串口参数设置Row devC04thpRow = C04Table.FindBy配置编号(tempC04RowName[2]);
                if (devC04thpRow != null)
                {
                    devC04thpRow.PhyPortNO = DAL_Dev.PowerCom.PhyPortNO;
                    devC04thpRow.BaoudRate = DAL_Dev.PowerCom.BoundRate;
                    devC04thpRow.StartBits = DAL_Dev.PowerCom.StartBits;
                    devC04thpRow.DataBits = DAL_Dev.PowerCom.DataBits;
                    devC04thpRow.PlCAddr = DAL_Dev.PowerCom.Addr;
                    devC04thpRow.CommRW_Period = DAL_Dev.PowerCom.PeriodRW;
                    devC04thpRow.WatchDogReset_Period = DAL_Dev.PowerCom.WatchDogPeriod;
                    devC04thpRow.Timeout = DAL_Dev.PowerCom.Timeout;
                    devC04thpRow.Time_BusyDealy = DAL_Dev.PowerCom.Time_BusyDealy;
                    devC04thpRow.CMDRepeat = DAL_Dev.PowerCom.CMDRepeat;

                    if (DAL_Dev.PowerCom.StopBits == StopBits.None)
                        devC04thpRow.StopBits = 0;
                    else if (DAL_Dev.PowerCom.StopBits == StopBits.One)
                        devC04thpRow.StopBits = 1;
                    else if (DAL_Dev.PowerCom.StopBits == StopBits.Two)
                        devC04thpRow.StopBits = 2;
                    else if (DAL_Dev.PowerCom.StopBits == StopBits.Two)
                        devC04thpRow.StopBits = 2;
                    else if (DAL_Dev.PowerCom.StopBits == StopBits.OnePointFive)
                        devC04thpRow.StopBits = 15;

                    if (DAL_Dev.PowerCom.Parity == Parity.None)
                        devC04thpRow.Parity = 0;
                    else if (DAL_Dev.PowerCom.Parity == Parity.Odd)
                        devC04thpRow.Parity = 1;
                    else if (DAL_Dev.PowerCom.Parity == Parity.Even)
                        devC04thpRow.Parity = 2;
                    else if (DAL_Dev.PowerCom.Parity == Parity.Mark)
                        devC04thpRow.Parity = 10;
                    else if (DAL_Dev.PowerCom.Parity == Parity.Space)
                        devC04thpRow.Parity = 20;
                    C04TableAdapter.Update(C04Table);
                    C04Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 保存C05模拟量参数
        /// </summary>
        private void SaveAIOParam()
        {
            #region 模拟量输入

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC05AIINRowName = new string[15]
            {
                "T1", "T2", "Tsc", "Tss", "Tfc", "Tw1", "Tw2", "Tw3","V","I","P","Q","S","PF","F"
            };
            for (int i = 0; i < tempC05AIINRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C05模拟量参数Row checkExistC05Row = C05Table.FindBySingalNO(tempC05AIINRowName[i]);
                    if (checkExistC05Row == null)
                    {
                        DevDBDataSet.C05模拟量参数Row defDevC05Row =
                            C05Table.FindBySingalNO(tempC05AIINRowName[i].ToString() + "Def");
                        DevDBDataSet.C05模拟量参数Row newDevC05ow = C05Table.NewC05模拟量参数Row();
                        newDevC05ow.ItemArray = (object[])defDevC05Row.ItemArray.Clone();
                        newDevC05ow.SingalNO = tempC05AIINRowName[i];
                        C05Table.AddC05模拟量参数Row(newDevC05ow);
                        C05TableAdapter.Update(C05Table);
                        C05Table.AcceptChanges();
                        RaisePropertyChanged(() => C05Table);
                        MessageBox.Show("未找到" + tempC05AIINRowName[i] + "配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            for (int i = 0; i < tempC05AIINRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C05模拟量参数Row devC05aiRow = C05Table.FindBySingalNO(tempC05AIINRowName[i]);
                    if (devC05aiRow != null)
                    {
                        //基本参数
                        devC05aiRow.SingalUnit = DAL_Dev.AIList[i].SingalUnit;
                        devC05aiRow.IsOutType = DAL_Dev.AIList[i].IsOutType;
                        devC05aiRow.InfType = DAL_Dev.AIList[i].InfType;
                        devC05aiRow.ElecSig_Unit = DAL_Dev.AIList[i].ElecSig_Unit;
                        devC05aiRow.ElecSig_LowerRange = DAL_Dev.AIList[i].ElecSigLowerRange;
                        devC05aiRow.ElecSig_UpperRange = DAL_Dev.AIList[i].ElecSigUpperRange;
                        devC05aiRow.SingalLowerRange = DAL_Dev.AIList[i].SingalLowerRange;
                        devC05aiRow.SingalUpperRange = DAL_Dev.AIList[i].SingalUpperRange;
                        devC05aiRow.ZeroCalValue = DAL_Dev.AIList[i].ZeroCalValue;
                        devC05aiRow.KCalValue = DAL_Dev.AIList[i].KCalValue;
                        devC05aiRow.ModulNO = DAL_Dev.AIList[i].ModulNO;
                        if (DAL_Dev.AIList[i].ChannelSerialNO < 1)
                            DAL_Dev.AIList[i].ChannelSerialNO = 1;
                        devC05aiRow.ChannelSerialNO = DAL_Dev.AIList[i].ChannelSerialNO;
                        devC05aiRow.ConvRatio = DAL_Dev.AIList[i].ConvRatio;

                        DAL_Dev.AIList[i].CalPointsTemp = new ObservableCollection<CalPoint>()
                        {
                            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),
                            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint()
                        };
                        //标定点参数复制
                        for (int j = 0; j < 11; j++)
                        {
                            DAL_Dev.AIList[i].CalPointsTemp[j].IsUse = DAL_Dev.AIList[i].CalPoints[j].IsUse;
                            DAL_Dev.AIList[i].CalPointsTemp[j].StdValue = DAL_Dev.AIList[i].CalPoints[j].StdValue;
                            DAL_Dev.AIList[i].CalPointsTemp[j].ViewValue = DAL_Dev.AIList[i].CalPoints[j].ViewValue;
                        }

                        int usefulPoints = 0;
                        //未启用标定点的数值清零
                        for (int j = 0; j < 11; j++)
                        {
                            if (!DAL_Dev.AIList[i].CalPointsTemp[j].IsUse)
                            {
                                DAL_Dev.AIList[i].CalPointsTemp[j].StdValue = 0;
                                DAL_Dev.AIList[i].CalPointsTemp[j].ViewValue = 0;
                            }
                            else
                                usefulPoints++;
                        }
                        //对标定点列表先按启用情况排序，后按viewValu排序
                        DAL_Dev.AIList[i].CalPoints = new ObservableCollection<CalPoint>(DAL_Dev.AIList[i].CalPointsTemp.OrderByDescending(v => v.IsUse).ThenBy(v => v.ViewValue));
                        //仅1个标定点时，设置为无标定点
                        if (usefulPoints == 1)
                        {
                            DAL_Dev.AIList[i].CalPointsTemp[0].IsUse = false;
                            DAL_Dev.AIList[i].CalPointsTemp[0].StdValue = 0;
                            DAL_Dev.AIList[i].CalPointsTemp[0].ViewValue = 0;
                        }
                        //标定点启用
                        devC05aiRow.Use_Cal1 = DAL_Dev.AIList[i].CalPoints[0].IsUse;
                        devC05aiRow.Use_Cal2 = DAL_Dev.AIList[i].CalPoints[1].IsUse;
                        devC05aiRow.Use_Cal3 = DAL_Dev.AIList[i].CalPoints[2].IsUse;
                        devC05aiRow.Use_Cal4 = DAL_Dev.AIList[i].CalPoints[3].IsUse;
                        devC05aiRow.Use_Cal5 = DAL_Dev.AIList[i].CalPoints[4].IsUse;
                        devC05aiRow.Use_Cal6 = DAL_Dev.AIList[i].CalPoints[5].IsUse;
                        devC05aiRow.Use_Cal7 = DAL_Dev.AIList[i].CalPoints[6].IsUse;
                        devC05aiRow.Use_Cal8 = DAL_Dev.AIList[i].CalPoints[7].IsUse;
                        devC05aiRow.Use_Cal9 = DAL_Dev.AIList[i].CalPoints[8].IsUse;
                        devC05aiRow.Use_Cal10 = DAL_Dev.AIList[i].CalPoints[9].IsUse;
                        devC05aiRow.Use_Cal11 = DAL_Dev.AIList[i].CalPoints[10].IsUse;
                        //标定点标准值
                        devC05aiRow.Sensor_Cal1_StdValue = DAL_Dev.AIList[i].CalPoints[0].StdValue;
                        devC05aiRow.Sensor_Cal2_StdValue = DAL_Dev.AIList[i].CalPoints[1].StdValue;
                        devC05aiRow.Sensor_Cal3_StdValue = DAL_Dev.AIList[i].CalPoints[2].StdValue;
                        devC05aiRow.Sensor_Cal4_StdValue = DAL_Dev.AIList[i].CalPoints[3].StdValue;
                        devC05aiRow.Sensor_Cal5_StdValue = DAL_Dev.AIList[i].CalPoints[4].StdValue;
                        devC05aiRow.Sensor_Cal6_StdValue = DAL_Dev.AIList[i].CalPoints[5].StdValue;
                        devC05aiRow.Sensor_Cal7_StdValue = DAL_Dev.AIList[i].CalPoints[6].StdValue;
                        devC05aiRow.Sensor_Cal8_StdValue = DAL_Dev.AIList[i].CalPoints[7].StdValue;
                        devC05aiRow.Sensor_Cal9_StdValue = DAL_Dev.AIList[i].CalPoints[8].StdValue;
                        devC05aiRow.Sensor_Cal10_StdValue = DAL_Dev.AIList[i].CalPoints[9].StdValue;
                        devC05aiRow.Sensor_Cal11_StdValue = DAL_Dev.AIList[i].CalPoints[10].StdValue;
                        //标定点显示值
                        devC05aiRow.Sensor_Cal1_ViewValue = DAL_Dev.AIList[i].CalPoints[0].ViewValue;
                        devC05aiRow.Sensor_Cal2_ViewValue = DAL_Dev.AIList[i].CalPoints[1].ViewValue;
                        devC05aiRow.Sensor_Cal3_ViewValue = DAL_Dev.AIList[i].CalPoints[2].ViewValue;
                        devC05aiRow.Sensor_Cal4_ViewValue = DAL_Dev.AIList[i].CalPoints[3].ViewValue;
                        devC05aiRow.Sensor_Cal5_ViewValue = DAL_Dev.AIList[i].CalPoints[4].ViewValue;
                        devC05aiRow.Sensor_Cal6_ViewValue = DAL_Dev.AIList[i].CalPoints[5].ViewValue;
                        devC05aiRow.Sensor_Cal7_ViewValue = DAL_Dev.AIList[i].CalPoints[6].ViewValue;
                        devC05aiRow.Sensor_Cal8_ViewValue = DAL_Dev.AIList[i].CalPoints[7].ViewValue;
                        devC05aiRow.Sensor_Cal9_ViewValue = DAL_Dev.AIList[i].CalPoints[8].ViewValue;
                        devC05aiRow.Sensor_Cal10_ViewValue = DAL_Dev.AIList[i].CalPoints[9].ViewValue;
                        devC05aiRow.Sensor_Cal11_ViewValue = DAL_Dev.AIList[i].CalPoints[10].ViewValue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            try
            {
                C05TableAdapter.Update(C05Table);
                C05Table.AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion


            #region 模拟量输出

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC05AOOutRowName = new string[1]
            {
                "Vo"
            };
            for (int i = 0; i < tempC05AOOutRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C05模拟量参数Row checkExistC05Row = C05Table.FindBySingalNO(tempC05AOOutRowName[i]);
                    if (checkExistC05Row == null)
                    {
                        DevDBDataSet.C05模拟量参数Row defDevC05Row =
                            C05Table.FindBySingalNO(tempC05AOOutRowName[i].ToString() + "Def");
                        DevDBDataSet.C05模拟量参数Row newDevC05ow = C05Table.NewC05模拟量参数Row();
                        newDevC05ow.ItemArray = (object[])defDevC05Row.ItemArray.Clone();
                        newDevC05ow.SingalNO = tempC05AOOutRowName[i];
                        C05Table.AddC05模拟量参数Row(newDevC05ow);
                        C05TableAdapter.Update(C05Table);
                        C05Table.AcceptChanges();
                        RaisePropertyChanged(() => C05Table);
                        MessageBox.Show("未找到" + tempC05AOOutRowName[i] + "配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            for (int i = 0; i < tempC05AOOutRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C05模拟量参数Row devC05aiRow = C05Table.FindBySingalNO(tempC05AOOutRowName[i]);
                    if (devC05aiRow != null)
                    {
                        //基本参数
                        devC05aiRow.SingalUnit = DAL_Dev.AOList[i].SingalUnit;
                        devC05aiRow.IsOutType = DAL_Dev.AOList[i].IsOutType;
                        devC05aiRow.InfType = DAL_Dev.AOList[i].InfType;
                        devC05aiRow.ElecSig_Unit = DAL_Dev.AOList[i].ElecSig_Unit;
                        devC05aiRow.ElecSig_LowerRange = DAL_Dev.AOList[i].ElecSigLowerRange;
                        devC05aiRow.ElecSig_UpperRange = DAL_Dev.AOList[i].ElecSigUpperRange;
                        devC05aiRow.SingalLowerRange = DAL_Dev.AOList[i].SingalLowerRange;
                        devC05aiRow.SingalUpperRange = DAL_Dev.AOList[i].SingalUpperRange;
                        devC05aiRow.ZeroCalValue = DAL_Dev.AOList[i].ZeroCalValue;
                        devC05aiRow.KCalValue = DAL_Dev.AOList[i].KCalValue;
                        devC05aiRow.ModulNO = DAL_Dev.AOList[i].ModulNO;
                        if (DAL_Dev.AOList[i].ChannelSerialNO < 1)
                            DAL_Dev.AOList[i].ChannelSerialNO = 1;
                        devC05aiRow.ChannelSerialNO = DAL_Dev.AOList[i].ChannelSerialNO;
                        devC05aiRow.ConvRatio = DAL_Dev.AOList[i].ConvRatio;

                        DAL_Dev.AOList[i].CalPointsTemp = new ObservableCollection<CalPoint>()
                        {
                            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),
                            new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint(),new CalPoint()
                        };
                        //标定点参数复制
                        for (int j = 0; j < 11; j++)
                        {
                            DAL_Dev.AOList[i].CalPointsTemp[j].IsUse = DAL_Dev.AOList[i].CalPoints[j].IsUse;
                            DAL_Dev.AOList[i].CalPointsTemp[j].StdValue = DAL_Dev.AOList[i].CalPoints[j].StdValue;
                            DAL_Dev.AOList[i].CalPointsTemp[j].ViewValue = DAL_Dev.AOList[i].CalPoints[j].ViewValue;
                        }

                        int usefulPoints = 0;
                        //未启用标定点的数值清零
                        for (int j = 0; j < 11; j++)
                        {
                            if (!DAL_Dev.AOList[i].CalPointsTemp[j].IsUse)
                            {
                                DAL_Dev.AOList[i].CalPointsTemp[j].StdValue = 0;
                                DAL_Dev.AOList[i].CalPointsTemp[j].ViewValue = 0;
                            }
                            else
                                usefulPoints++;
                        }
                        //对标定点列表先按启用情况排序，后按viewValu排序
                        DAL_Dev.AOList[i].CalPoints = new ObservableCollection<CalPoint>(DAL_Dev.AOList[i].CalPointsTemp.OrderByDescending(v => v.IsUse).ThenBy(v => v.ViewValue));
                        //仅1个标定点时，设置为无标定点
                        if (usefulPoints == 1)
                        {
                            DAL_Dev.AOList[i].CalPointsTemp[0].IsUse = false;
                            DAL_Dev.AOList[i].CalPointsTemp[0].StdValue = 0;
                            DAL_Dev.AOList[i].CalPointsTemp[0].ViewValue = 0;
                        }
                        //标定点启用
                        devC05aiRow.Use_Cal1 = DAL_Dev.AOList[i].CalPoints[0].IsUse;
                        devC05aiRow.Use_Cal2 = DAL_Dev.AOList[i].CalPoints[1].IsUse;
                        devC05aiRow.Use_Cal3 = DAL_Dev.AOList[i].CalPoints[2].IsUse;
                        devC05aiRow.Use_Cal4 = DAL_Dev.AOList[i].CalPoints[3].IsUse;
                        devC05aiRow.Use_Cal5 = DAL_Dev.AOList[i].CalPoints[4].IsUse;
                        devC05aiRow.Use_Cal6 = DAL_Dev.AOList[i].CalPoints[5].IsUse;
                        devC05aiRow.Use_Cal7 = DAL_Dev.AOList[i].CalPoints[6].IsUse;
                        devC05aiRow.Use_Cal8 = DAL_Dev.AOList[i].CalPoints[7].IsUse;
                        devC05aiRow.Use_Cal9 = DAL_Dev.AOList[i].CalPoints[8].IsUse;
                        devC05aiRow.Use_Cal10 = DAL_Dev.AOList[i].CalPoints[9].IsUse;
                        devC05aiRow.Use_Cal11 = DAL_Dev.AOList[i].CalPoints[10].IsUse;
                        //标定点标准值
                        devC05aiRow.Sensor_Cal1_StdValue = DAL_Dev.AOList[i].CalPoints[0].StdValue;
                        devC05aiRow.Sensor_Cal2_StdValue = DAL_Dev.AOList[i].CalPoints[1].StdValue;
                        devC05aiRow.Sensor_Cal3_StdValue = DAL_Dev.AOList[i].CalPoints[2].StdValue;
                        devC05aiRow.Sensor_Cal4_StdValue = DAL_Dev.AOList[i].CalPoints[3].StdValue;
                        devC05aiRow.Sensor_Cal5_StdValue = DAL_Dev.AOList[i].CalPoints[4].StdValue;
                        devC05aiRow.Sensor_Cal6_StdValue = DAL_Dev.AOList[i].CalPoints[5].StdValue;
                        devC05aiRow.Sensor_Cal7_StdValue = DAL_Dev.AOList[i].CalPoints[6].StdValue;
                        devC05aiRow.Sensor_Cal8_StdValue = DAL_Dev.AOList[i].CalPoints[7].StdValue;
                        devC05aiRow.Sensor_Cal9_StdValue = DAL_Dev.AOList[i].CalPoints[8].StdValue;
                        devC05aiRow.Sensor_Cal10_StdValue = DAL_Dev.AOList[i].CalPoints[9].StdValue;
                        devC05aiRow.Sensor_Cal11_StdValue = DAL_Dev.AOList[i].CalPoints[10].StdValue;
                        //标定点显示值
                        devC05aiRow.Sensor_Cal1_ViewValue = DAL_Dev.AOList[i].CalPoints[0].ViewValue;
                        devC05aiRow.Sensor_Cal2_ViewValue = DAL_Dev.AOList[i].CalPoints[1].ViewValue;
                        devC05aiRow.Sensor_Cal3_ViewValue = DAL_Dev.AOList[i].CalPoints[2].ViewValue;
                        devC05aiRow.Sensor_Cal4_ViewValue = DAL_Dev.AOList[i].CalPoints[3].ViewValue;
                        devC05aiRow.Sensor_Cal5_ViewValue = DAL_Dev.AOList[i].CalPoints[4].ViewValue;
                        devC05aiRow.Sensor_Cal6_ViewValue = DAL_Dev.AOList[i].CalPoints[5].ViewValue;
                        devC05aiRow.Sensor_Cal7_ViewValue = DAL_Dev.AOList[i].CalPoints[6].ViewValue;
                        devC05aiRow.Sensor_Cal8_ViewValue = DAL_Dev.AOList[i].CalPoints[7].ViewValue;
                        devC05aiRow.Sensor_Cal9_ViewValue = DAL_Dev.AOList[i].CalPoints[8].ViewValue;
                        devC05aiRow.Sensor_Cal10_ViewValue = DAL_Dev.AOList[i].CalPoints[9].ViewValue;
                        devC05aiRow.Sensor_Cal11_ViewValue = DAL_Dev.AOList[i].CalPoints[10].ViewValue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            try
            {
                C05TableAdapter.Update(C05Table);
                C05Table.AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion
        }

        /// <summary>
        /// 保存C06模拟量通道参数
        /// </summary>
        private void SaveAIOChannels()
        {
            #region AI模块通道

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC06AIchlRowName = new string[15]
            {
                "T-01","T-02","T-03","T-04","T-05","T-06","T-07","T-08",
                "Power-01","Power-02","Power-03","Power-04","Power-05","Power-06","Power-07"
            };
            if (DAL_Dev.TModelType == "DAM3134")
            {
                tempC06AIchlRowName[0] = "T-013134";
                tempC06AIchlRowName[1] = "T-023134";
                tempC06AIchlRowName[2] = "T-033134";
                tempC06AIchlRowName[3] = "T-043134";
            }
            for (int i = 0; i < tempC06AIchlRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C06模拟量通道参数Row checkExistC06Row = C06Table.FindByChannelNO(tempC06AIchlRowName[i]);
                    if (checkExistC06Row == null)
                    {
                        DevDBDataSet.C06模拟量通道参数Row defDevC06Row = C06Table.FindByChannelNO(tempC06AIchlRowName[i].ToString() + "Def");
                        DevDBDataSet.C06模拟量通道参数Row newDevC06ow = C06Table.NewC06模拟量通道参数Row();
                        newDevC06ow.ItemArray = (object[])defDevC06Row.ItemArray.Clone();
                        newDevC06ow.ChannelNO = tempC06AIchlRowName[i];
                        C06Table.AddC06模拟量通道参数Row(newDevC06ow);
                        C06TableAdapter.Update(C06Table);
                        C06Table.AcceptChanges();
                        RaisePropertyChanged(() => C06Table);
                        MessageBox.Show("未找到" + tempC06AIchlRowName[i] + "通道配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //热电偶采集模块通道配置参数
            for (int i = 0; i < 8; i++)
            {
                try
                {
                    DevDBDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06AIchlRowName[i]);
                    if (devC06chlRow != null)
                    {
                        devC06chlRow.ChannelNO = DAL_Dev.Mod_AI_T.Channels[i].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_AI_T.Channels[i].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_AI_T.Channels[i].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_AI_T.Channels[i].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_AI_T.Channels[i].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_AI_T.Channels[i].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_AI_T.Channels[i].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_AI_T.Channels[i].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_AI_T.Channels[i].IsOutType;
                        devC06chlRow.FitRitio = DAL_Dev.Mod_AI_T.Channels[i].FitRatio;
                        devC06chlRow.FilterType = DAL_Dev.Mod_AI_T.Channels[i].FilterType;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //功率采集模块通道配置参数
            for (int i = 8; i < tempC06AIchlRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06AIchlRowName[i]);
                    if (devC06chlRow != null)
                    {
                        devC06chlRow.ChannelNO = DAL_Dev.Mod_AI_Power.Channels[i - 8].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_AI_Power.Channels[i - 8].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_AI_Power.Channels[i - 8].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_AI_Power.Channels[i - 8].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_AI_Power.Channels[i - 8].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_AI_Power.Channels[i - 8].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_AI_Power.Channels[i - 8].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_AI_Power.Channels[i - 8].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_AI_Power.Channels[i - 8].IsOutType;
                        devC06chlRow.FitRitio = DAL_Dev.Mod_AI_Power.Channels[i - 8].FitRatio;
                        devC06chlRow.FilterType = DAL_Dev.Mod_AI_Power.Channels[i - 8].FilterType;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            try
            {
                C06TableAdapter.Update(C06Table);
                C06Table.AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion


            #region AO模块通道

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC06AOchlRowName = new string[4]
            {
                "AO-01","AO-02","AO-03","AO-04"
            };
            if (DAL_Dev.AOModelType == "DAM3060C")
            {
                tempC06AOchlRowName[0] = "AO-01";
                tempC06AOchlRowName[1] = "AO-02";
                tempC06AOchlRowName[2] = "AO-03";
                tempC06AOchlRowName[3] = "AO-04";
            }
            else if (DAL_Dev.AOModelType == "UT5564A")
            {
                tempC06AOchlRowName[0] = "AO-015564A";
                tempC06AOchlRowName[1] = "AO-025564A";
                tempC06AOchlRowName[2] = "AO-035564A";
                tempC06AOchlRowName[3] = "AO-045564A";
            }
            else if (DAL_Dev.AOModelType == "Modbus-4AI4AO")
            {
                tempC06AOchlRowName[0] = "AO-014AI4AO";
                tempC06AOchlRowName[1] = "AO-024AI4AO";
                tempC06AOchlRowName[2] = "AO-034AI4AO";
                tempC06AOchlRowName[3] = "AO-044AI4AO";
            }
            for (int i = 0; i < tempC06AOchlRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C06模拟量通道参数Row checkExistC06Row = C06Table.FindByChannelNO(tempC06AOchlRowName[i]);
                    if (checkExistC06Row == null)
                    {
                        DevDBDataSet.C06模拟量通道参数Row defDevC06Row = C06Table.FindByChannelNO(tempC06AOchlRowName[i].ToString() + "Def");
                        DevDBDataSet.C06模拟量通道参数Row newDevC06ow = C06Table.NewC06模拟量通道参数Row();
                        newDevC06ow.ItemArray = (object[])defDevC06Row.ItemArray.Clone();
                        newDevC06ow.ChannelNO = tempC06AOchlRowName[i];
                        C06Table.AddC06模拟量通道参数Row(newDevC06ow);
                        C06TableAdapter.Update(C06Table);
                        C06Table.AcceptChanges();
                        RaisePropertyChanged(() => C06Table);
                        MessageBox.Show("未找到" + tempC06AOchlRowName[i] + "通道配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //保存AO模块通道配置参数
            for (int i = 0; i < tempC06AOchlRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06AOchlRowName[i]);
                    if (devC06chlRow != null)
                    {
                      //  devC06chlRow.ChannelNO = DAL_Dev.Mod_AO.Channels[i].ChannelNO;
                        devC06chlRow.ColType = DAL_Dev.Mod_AO.Channels[i].InfType;
                        devC06chlRow.ElecSig_Unit = DAL_Dev.Mod_AO.Channels[i].ElecSigUnit;
                        devC06chlRow.ElecSig_LowerRange = DAL_Dev.Mod_AO.Channels[i].ElecSigLowerRange;
                        devC06chlRow.ElecSig_UpperRange = DAL_Dev.Mod_AO.Channels[i].ElecSigUpperRange;
                        devC06chlRow.Data_LowerRange = DAL_Dev.Mod_AO.Channels[i].DataLowerRange;
                        devC06chlRow.Data_UpperRange = DAL_Dev.Mod_AO.Channels[i].DataUpperRange;
                        devC06chlRow.IsUsed = DAL_Dev.Mod_AO.Channels[i].IsUsed;
                        devC06chlRow.IsOutType = DAL_Dev.Mod_AO.Channels[i].IsOutType;
                        devC06chlRow.FitRitio = DAL_Dev.Mod_AO.Channels[i].FitRatio;
                        devC06chlRow.FilterType = DAL_Dev.Mod_AO.Channels[i].FilterType;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            try
            {
                C06TableAdapter.Update(C06Table);
                C06Table.AcceptChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }

        /// <summary>
        /// 保存C09PID控制参数
        /// </summary>
        private void SavePIDSettings()
        {
            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC09RowName = new string[7]
            {
                "PID_T_Err50Up","PID_T_Err15_50","PID_T_Err0_15","PID_Power","PID_T_Err50UpPower","PID_T_Err15_50Power","PID_T_Err0_15Power"
            };
            for (int i = 0; i < tempC09RowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C09PID控制参数Row checkExistC09Row = C09Table.FindBy配置编号(tempC09RowName[i]);
                    if (checkExistC09Row == null)
                    {
                        DevDBDataSet.C09PID控制参数Row defDevC09Row =
                            C09Table.FindBy配置编号(tempC09RowName[i].ToString() + "Def");
                        DevDBDataSet.C09PID控制参数Row newDevC09ow = C09Table.NewC09PID控制参数Row();
                        newDevC09ow.ItemArray = (object[])defDevC09Row.ItemArray.Clone();
                        newDevC09ow.配置编号 = tempC09RowName[i];
                        C09Table.AddC09PID控制参数Row(newDevC09ow);
                        C09TableAdapter.Update(C09Table);
                        C09Table.AcceptChanges();
                        RaisePropertyChanged(() => C09Table);
                        MessageBox.Show("未找到" + tempC09RowName[i] + "PID控制参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //PID_T_Err50Up
            try
            {
                DevDBDataSet.C09PID控制参数Row devC0911Row;
                if (DAL_Dev.WithPowerSenser)
                    devC0911Row = C09Table.FindBy配置编号(tempC09RowName[4]);
                else
                    devC0911Row = C09Table.FindBy配置编号(tempC09RowName[0]);
                if (devC0911Row != null)
                {
                    devC0911Row.PIDName = DAL_Dev.PID_T_Err50Up.PID_Param.ControllerName;
                    devC0911Row.Kp = DAL_Dev.PID_T_Err50Up.PID_Param.Kp;
                    devC0911Row.Ki = DAL_Dev.PID_T_Err50Up.PID_Param.Ki;
                    devC0911Row.Kd = DAL_Dev.PID_T_Err50Up.PID_Param.Kd;
                    devC0911Row.U_UpperBound = DAL_Dev.PID_T_Err50Up.PID_Param.U_UpperBound;
                    devC0911Row.U_LowerBound = DAL_Dev.PID_T_Err50Up.PID_Param.U_LowerBound;
                    devC0911Row.T = DAL_Dev.PID_T_Err50Up.PID_Param.T;
                    devC0911Row.PIDType = (int)DAL_Dev.PID_T_Err50Up.PID_Param.ControllerType;
                    devC0911Row.U_IMax_Limit = DAL_Dev.PID_T_Err50Up.PID_Param.U_IMax_Limit;
                    devC0911Row.ErrBound_IntegralSeparate = DAL_Dev.PID_T_Err50Up.PID_Param.ErrBound_IntegralSeparate;
                    devC0911Row.Is_Kp_Used = DAL_Dev.PID_T_Err50Up.PID_Param.Is_Kp_Used;
                    devC0911Row.Is_Ki_Used = DAL_Dev.PID_T_Err50Up.PID_Param.Is_Ki_Used;
                    devC0911Row.Is_Kd_Used = DAL_Dev.PID_T_Err50Up.PID_Param.Is_Kd_Used;
                    devC0911Row.Is_U_Limit = DAL_Dev.PID_T_Err50Up.PID_Param.Is_U_Limit;
                    devC0911Row.Is_UILimit_Used = DAL_Dev.PID_T_Err50Up.PID_Param.Is_UILimit_Used;
                    devC0911Row.Is_ISeparate_Used = DAL_Dev.PID_T_Err50Up.PID_Param.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID_T_Err15_50
            try
            {
                DevDBDataSet.C09PID控制参数Row devC0911Row;
                if (DAL_Dev.WithPowerSenser)
                    devC0911Row = C09Table.FindBy配置编号(tempC09RowName[5]);
                else
                    devC0911Row = C09Table.FindBy配置编号(tempC09RowName[1]);              
                if (devC0911Row != null)
                {
                    devC0911Row.PIDName = DAL_Dev.PID_T_Err15_50.PID_Param.ControllerName;
                    devC0911Row.Kp = DAL_Dev.PID_T_Err15_50.PID_Param.Kp;
                    devC0911Row.Ki = DAL_Dev.PID_T_Err15_50.PID_Param.Ki;
                    devC0911Row.Kd = DAL_Dev.PID_T_Err15_50.PID_Param.Kd;
                    devC0911Row.U_UpperBound = DAL_Dev.PID_T_Err15_50.PID_Param.U_UpperBound;
                    devC0911Row.U_LowerBound = DAL_Dev.PID_T_Err15_50.PID_Param.U_LowerBound;
                    devC0911Row.T = DAL_Dev.PID_T_Err15_50.PID_Param.T;
                    devC0911Row.PIDType = (int)DAL_Dev.PID_T_Err15_50.PID_Param.ControllerType;
                    devC0911Row.U_IMax_Limit = DAL_Dev.PID_T_Err15_50.PID_Param.U_IMax_Limit;
                    devC0911Row.ErrBound_IntegralSeparate = DAL_Dev.PID_T_Err15_50.PID_Param.ErrBound_IntegralSeparate;
                    devC0911Row.Is_Kp_Used = DAL_Dev.PID_T_Err15_50.PID_Param.Is_Kp_Used;
                    devC0911Row.Is_Ki_Used = DAL_Dev.PID_T_Err15_50.PID_Param.Is_Ki_Used;
                    devC0911Row.Is_Kd_Used = DAL_Dev.PID_T_Err15_50.PID_Param.Is_Kd_Used;
                    devC0911Row.Is_U_Limit = DAL_Dev.PID_T_Err15_50.PID_Param.Is_U_Limit;
                    devC0911Row.Is_UILimit_Used = DAL_Dev.PID_T_Err15_50.PID_Param.Is_UILimit_Used;
                    devC0911Row.Is_ISeparate_Used = DAL_Dev.PID_T_Err15_50.PID_Param.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            //PID_T_Err0_15
            try
            {
                DevDBDataSet.C09PID控制参数Row devC0911Row;
                if (DAL_Dev.WithPowerSenser)
                    devC0911Row = C09Table.FindBy配置编号(tempC09RowName[6]);
                else
                    devC0911Row = C09Table.FindBy配置编号(tempC09RowName[2]);
                if (devC0911Row != null)
                {
                    devC0911Row.PIDName = DAL_Dev.PID_T_Err0_15.PID_Param.ControllerName;
                    devC0911Row.Kp = DAL_Dev.PID_T_Err0_15.PID_Param.Kp;
                    devC0911Row.Ki = DAL_Dev.PID_T_Err0_15.PID_Param.Ki;
                    devC0911Row.Kd = DAL_Dev.PID_T_Err0_15.PID_Param.Kd;
                    devC0911Row.U_UpperBound = DAL_Dev.PID_T_Err0_15.PID_Param.U_UpperBound;
                    devC0911Row.U_LowerBound = DAL_Dev.PID_T_Err0_15.PID_Param.U_LowerBound;
                    devC0911Row.T = DAL_Dev.PID_T_Err0_15.PID_Param.T;
                    devC0911Row.PIDType = (int)DAL_Dev.PID_T_Err0_15.PID_Param.ControllerType;
                    devC0911Row.U_IMax_Limit = DAL_Dev.PID_T_Err0_15.PID_Param.U_IMax_Limit;
                    devC0911Row.ErrBound_IntegralSeparate = DAL_Dev.PID_T_Err0_15.PID_Param.ErrBound_IntegralSeparate;
                    devC0911Row.Is_Kp_Used = DAL_Dev.PID_T_Err0_15.PID_Param.Is_Kp_Used;
                    devC0911Row.Is_Ki_Used = DAL_Dev.PID_T_Err0_15.PID_Param.Is_Ki_Used;
                    devC0911Row.Is_Kd_Used = DAL_Dev.PID_T_Err0_15.PID_Param.Is_Kd_Used;
                    devC0911Row.Is_U_Limit = DAL_Dev.PID_T_Err0_15.PID_Param.Is_U_Limit;
                    devC0911Row.Is_UILimit_Used = DAL_Dev.PID_T_Err0_15.PID_Param.Is_UILimit_Used;
                    devC0911Row.Is_ISeparate_Used = DAL_Dev.PID_T_Err0_15.PID_Param.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID_Power
            try
            {
                DevDBDataSet.C09PID控制参数Row devC0911Row = C09Table.FindBy配置编号(tempC09RowName[3]);
                if (devC0911Row != null)
                {
                    devC0911Row.PIDName = DAL_Dev.PID_Power.PID_Param.ControllerName;
                    devC0911Row.Kp = DAL_Dev.PID_Power.PID_Param.Kp;
                    devC0911Row.Ki = DAL_Dev.PID_Power.PID_Param.Ki;
                    devC0911Row.Kd = DAL_Dev.PID_Power.PID_Param.Kd;
                    devC0911Row.U_UpperBound = DAL_Dev.PID_Power.PID_Param.U_UpperBound;
                    devC0911Row.U_LowerBound = DAL_Dev.PID_Power.PID_Param.U_LowerBound;
                    devC0911Row.T = DAL_Dev.PID_Power.PID_Param.T;
                    devC0911Row.PIDType = (int)DAL_Dev.PID_Power.PID_Param.ControllerType;
                    devC0911Row.U_IMax_Limit = DAL_Dev.PID_Power.PID_Param.U_IMax_Limit;
                    devC0911Row.ErrBound_IntegralSeparate = DAL_Dev.PID_Power.PID_Param.ErrBound_IntegralSeparate;
                    devC0911Row.Is_Kp_Used = DAL_Dev.PID_Power.PID_Param.Is_Kp_Used;
                    devC0911Row.Is_Ki_Used = DAL_Dev.PID_Power.PID_Param.Is_Ki_Used;
                    devC0911Row.Is_Kd_Used = DAL_Dev.PID_Power.PID_Param.Is_Kd_Used;
                    devC0911Row.Is_U_Limit = DAL_Dev.PID_Power.PID_Param.Is_U_Limit;
                    devC0911Row.Is_UILimit_Used = DAL_Dev.PID_Power.PID_Param.Is_UILimit_Used;
                    devC0911Row.Is_ISeparate_Used = DAL_Dev.PID_Power.PID_Param.Is_ISeparate_Used;
                    C09TableAdapter.Update(C09Table);
                    C09Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion


        #region 根据消息读写装置数据

        /// <summary>
        /// 保存最后打开的试验编号
        /// </summary>
        /// <param name="msg"></param>
        private void SaveLastExpNOMessage(string msg)
        {
            //更新上次试验编号
            DAL_Dev.ExpNOLast = msg;
            SaveBisicParam();
        }
        
        #endregion
    }
}
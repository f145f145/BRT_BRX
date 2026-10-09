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
using System.Diagnostics.Eventing.Reader;
using System.IO.Ports;
using System.Windows;
using GalaSoft.MvvmLight;


namespace BRX.DAL.DevDAL
{
    /// <summary>
    /// 装置参数读写操作类
    /// </summary>
    public partial class DevDAL : ObservableObject
    {
        /// <summary>
        /// 载入工厂设定
        /// </summary>
        private void LoadFacDevSettings()
        {

        }

        /// <summary>
        /// 恢复默认设置
        /// </summary>
        private void LoadDefaultDevSettings()
        {

        }

        /// <summary>
        /// 保存为默认设置
        /// </summary>
        private void SaveAsDefaultSettings()
        {

        }

        /// <summary>
        /// 载入装置数据（总）
        /// </summary>
        private void LoadDevSettings()
        {
            //基本信息
            LoadBisicInfo();
            //基本参数
            LoadBisicParam();
            //公司信息
            LoadCoSettings();
            //串口参数
            LoadComSettings();
            //模拟量参数
            LoadAIOParam();
            //模拟量通道参数
            LoadAIOChannelParam();
            //绑定模拟量参数和通道
            BindAioAndChennel();
            //PID参数
            LoadPIDSettings();
        }

        /// <summary>
        /// 载入装置基本信息C01
        /// </summary>
        private void LoadBisicInfo()
        {
            string tempSettingsNO = "Using";

            //载入C01装置基本参数
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
                    MessageBox.Show("未找到在用装置C01配置参数，已重新建立！", "错误提示");
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
                    DAL_Dev.DeviceID = devC01Row.装置ID;
                    DAL_Dev.DeviceName = devC01Row.装置名称;
                    DAL_Dev.DeviceSerialNO = devC01Row.出厂编号;
                    DAL_Dev.DeviceType = devC01Row.设备型号;
                    DAL_Dev.ReferenceStd = devC01Row.试验方法标准;
                    DAL_Dev.SJDW = devC01Row.设计单位;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 载入C02装置基本参数
        /// </summary>
        private void LoadBisicParam()
        {
            string tempSettingsNO = "Using";

            //载入C02装置基本参数
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
                    MessageBox.Show("未找到C02装置基本参数，已重新建立！", "错误提示");
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
                    DAL_Dev.PlotPeriod = devC02Row.绘图更新周期;
                    DAL_Dev.PointsPerLine = devC02Row.单曲线总点数;

                    DAL_Dev.Period_BLL = devC02Row.Period_BLL;
                    DAL_Dev.Period_Comm = devC02Row.Period_Comm;

                    DAL_Dev.Time_Stabilize = devC02Row.Time_Stabilize;
                    DAL_Dev.Period_Stabilize = devC02Row.Period_Stabilize;
                    DAL_Dev.Time_FinalEqu = devC02Row.Time_FinalEqu;
                    DAL_Dev.Period_FinalEqu = devC02Row.Period_FinalEqu;
                    DAL_Dev.Time_FinalEquEvalFist = devC02Row.Time_FinalEquEvalFist;
                    DAL_Dev.Time_FinalEquEvalMax = devC02Row.Time_FinalEquEvalMax;
                    DAL_Dev.TCtlAimTest = devC02Row.TCtlAimTest;
                    DAL_Dev.TCtlErrPermit = devC02Row.TCtlErrPermit;
                    DAL_Dev.TCtlDriftPermit = devC02Row.TCtlDriftPermit;
                    DAL_Dev.TCtlDeviationPermit = devC02Row.TCtlDeviationPermit;
                    DAL_Dev.TFinalEquDriftPermit = devC02Row.TFinalEquDriftPermit;
                    DAL_Dev.TUpSpeed = devC02Row.TUpSpeed;

                    DAL_Dev.IsLoadLastExpPowerOn = devC02Row.IsLoadLastExpPowerOn;
                    DAL_Dev.IsAutoContinue = devC02Row.IsAutoContinue;
                    DAL_Dev.WithPowerSenser = devC02Row.WithPowerSenser;
                    DAL_Dev.ExpNOLast = devC02Row.ExpNOLast;
                    DAL_Dev.WallCalNOLast = devC02Row.WallCalNOLast;
                    DAL_Dev.CenterCalNOLast = devC02Row.CenterCalNOLast;

                    DAL_Dev.StdSelected = devC02Row.StdSelected;
                    DAL_Dev.R = devC02Row.R;
                    DAL_Dev.Vin = devC02Row.Vin;

                    DAL_Dev.TModelType = devC02Row.TModelType;
                    DAL_Dev.AOModelType = devC02Row.AOModelType;
                    DAL_Dev.VOut_ID = devC02Row.VOut_ID;

                    DAL_Dev.RatioAoOutMax = devC02Row.RatioAoOutMax;
                    DAL_Dev.SoftBootTime = devC02Row.SoftBootTime;

                    DAL_Dev.OutReduceU = devC02Row.OutReduceU;
                    DAL_Dev.OutReduceP = devC02Row.OutReduceP;

                    DAL_Dev.AutoPower = devC02Row.AutoPower;
                    DAL_Dev.TimeCalcPower = devC02Row.TimeCalcPower;
                    DAL_Dev.FixedPower_P = devC02Row.FixedPower_P;
                    DAL_Dev.FixedPower_U = devC02Row.FixedPower_U;

                    DAL_Dev.TStartPID = devC02Row.TStartPID;

                    DAL_Dev.InitSumUI_P = devC02Row.InitSumUI_P;
                    DAL_Dev.InitSumUI_U = devC02Row.InitSumUI_U;

                    DAL_Dev.DelayTime = devC02Row.DelayTime;
                    DAL_Dev.TDelayU = devC02Row.TDelayU;
                    DAL_Dev.TDelayP = devC02Row.TDelayP;

                    DAL_Dev.EncryptRPT = devC02Row.备用bool1;
                    DAL_Dev.HideSY2345 = devC02Row.备用bool2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// 载入C03公司信息
        /// </summary>
        private void LoadCoSettings()
        {
            string tempSettingsNO = "Using";

            //载入C03公司信息
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
                    MessageBox.Show("未找到在用装置C03公司信息，已重新建立！", "错误提示");
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
                    DAL_Dev.CoShortName = devC03Row.公司简称;
                    DAL_Dev.CoName = devC03Row.公司全称;
                    DAL_Dev.CoAddr = devC03Row.公司地址;
                    DAL_Dev.CoPostNO = devC03Row.公司邮政编码;
                    DAL_Dev.CoTel = devC03Row.公司电话;
                    DAL_Dev.LabAddr = devC03Row.实验室地址;
                    DAL_Dev.LabPostNO = devC03Row.实验室邮政编码;
                    DAL_Dev.LabTel = devC03Row.实验室电话;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 载入C04串口设置参数
        /// </summary>
        private void LoadComSettings()
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

            for (int i = 0; i < tempC04RowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C04串口参数设置Row checkExistC04Row = C04Table.FindBy配置编号(tempC04RowName[i]);
                    if (checkExistC04Row == null)
                    {
                        DevDBDataSet.C04串口参数设置Row defDevC04Row =
                            C04Table.FindBy配置编号(tempC04RowName[i].ToString() + "Def");
                        DevDBDataSet.C04串口参数设置Row newDevC04ow = C04Table.NewC04串口参数设置Row();
                        newDevC04ow.ItemArray = (object[])defDevC04Row.ItemArray.Clone();
                        newDevC04ow.配置编号 = tempC04RowName[i];
                        C04Table.AddC04串口参数设置Row(newDevC04ow);
                        C04TableAdapter.Update(C04Table);
                        C04Table.AcceptChanges();
                        RaisePropertyChanged(() => C04Table);
                        MessageBox.Show("未找到" + tempC04RowName[i] + "串口配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //TCom
            try
            {
                DevDBDataSet.C04串口参数设置Row devC04thpRow = C04Table.FindBy配置编号(tempC04RowName[0]);
                if (devC04thpRow != null)
                {
                    DAL_Dev.TCom.ComName = devC04thpRow.配置编号;
                    DAL_Dev.TCom.PhyPortNO = devC04thpRow.PhyPortNO;
                    DAL_Dev.TCom.BoundRate = devC04thpRow.BaoudRate;
                    DAL_Dev.TCom.StartBits = devC04thpRow.StartBits;
                    DAL_Dev.TCom.DataBits = devC04thpRow.DataBits;
                    DAL_Dev.TCom.Addr = devC04thpRow.PlCAddr;
                    DAL_Dev.TCom.PeriodRW = devC04thpRow.CommRW_Period;
                    DAL_Dev.TCom.WatchDogPeriod = devC04thpRow.WatchDogReset_Period;
                    DAL_Dev.TCom.Timeout = devC04thpRow.Timeout;
                    DAL_Dev.TCom.Time_BusyDealy = devC04thpRow.Time_BusyDealy;
                    DAL_Dev.TCom.CMDRepeat = devC04thpRow.CMDRepeat;
                    switch (devC04thpRow.StopBits)
                    {
                        case 0:
                            DAL_Dev.TCom.StopBits = StopBits.None;
                            break;
                        case 1:
                            DAL_Dev.TCom.StopBits = StopBits.One;
                            break;
                        case 2:
                            DAL_Dev.TCom.StopBits = StopBits.Two;
                            break;
                        case 15:
                            DAL_Dev.TCom.StopBits = StopBits.OnePointFive;
                            break;
                    }

                    switch (devC04thpRow.Parity)
                    {
                        case 0:
                            DAL_Dev.TCom.Parity = Parity.None;
                            break;
                        case 1:
                            DAL_Dev.TCom.Parity = Parity.Odd;
                            break;
                        case 2:
                            DAL_Dev.TCom.Parity = Parity.Even;
                            break;
                        case 10:
                            DAL_Dev.TCom.Parity = Parity.Mark;
                            break;
                        case 20:
                            DAL_Dev.TCom.Parity = Parity.Space;
                            break;
                    }
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
                    DAL_Dev.AOCom.ComName = devC04thpRow.配置编号;
                    DAL_Dev.AOCom.PhyPortNO = devC04thpRow.PhyPortNO;
                    DAL_Dev.AOCom.BoundRate = devC04thpRow.BaoudRate;
                    DAL_Dev.AOCom.StartBits = devC04thpRow.StartBits;
                    DAL_Dev.AOCom.DataBits = devC04thpRow.DataBits;
                    DAL_Dev.AOCom.Addr = devC04thpRow.PlCAddr;
                    DAL_Dev.AOCom.PeriodRW = devC04thpRow.CommRW_Period;
                    DAL_Dev.AOCom.WatchDogPeriod = devC04thpRow.WatchDogReset_Period;
                    DAL_Dev.AOCom.Timeout = devC04thpRow.Timeout;
                    DAL_Dev.AOCom.Time_BusyDealy = devC04thpRow.Time_BusyDealy;
                    DAL_Dev.AOCom.CMDRepeat = devC04thpRow.CMDRepeat;
                    switch (devC04thpRow.StopBits)
                    {
                        case 0:
                            DAL_Dev.AOCom.StopBits = StopBits.None;
                            break;
                        case 1:
                            DAL_Dev.AOCom.StopBits = StopBits.One;
                            break;
                        case 2:
                            DAL_Dev.AOCom.StopBits = StopBits.Two;
                            break;
                        case 15:
                            DAL_Dev.AOCom.StopBits = StopBits.OnePointFive;
                            break;
                    }

                    switch (devC04thpRow.Parity)
                    {
                        case 0:
                            DAL_Dev.AOCom.Parity = Parity.None;
                            break;
                        case 1:
                            DAL_Dev.AOCom.Parity = Parity.Odd;
                            break;
                        case 2:
                            DAL_Dev.AOCom.Parity = Parity.Even;
                            break;
                        case 10:
                            DAL_Dev.AOCom.Parity = Parity.Mark;
                            break;
                        case 20:
                            DAL_Dev.AOCom.Parity = Parity.Space;
                            break;
                    }
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
                    DAL_Dev.PowerCom.PhyPortNO = devC04thpRow.PhyPortNO;
                    DAL_Dev.PowerCom.BoundRate = devC04thpRow.BaoudRate;
                    DAL_Dev.PowerCom.StartBits = devC04thpRow.StartBits;
                    DAL_Dev.PowerCom.DataBits = devC04thpRow.DataBits;
                    DAL_Dev.PowerCom.Addr = devC04thpRow.PlCAddr;
                    DAL_Dev.PowerCom.PeriodRW = devC04thpRow.CommRW_Period;
                    DAL_Dev.PowerCom.WatchDogPeriod = devC04thpRow.WatchDogReset_Period;
                    DAL_Dev.PowerCom.Timeout = devC04thpRow.Timeout;
                    DAL_Dev.PowerCom.Time_BusyDealy = devC04thpRow.Time_BusyDealy;
                    DAL_Dev.PowerCom.CMDRepeat = devC04thpRow.CMDRepeat;
                    switch (devC04thpRow.StopBits)
                    {
                        case 0:
                            DAL_Dev.PowerCom.StopBits = StopBits.None;
                            break;
                        case 1:
                            DAL_Dev.PowerCom.StopBits = StopBits.One;
                            break;
                        case 2:
                            DAL_Dev.PowerCom.StopBits = StopBits.Two;
                            break;
                        case 15:
                            DAL_Dev.PowerCom.StopBits = StopBits.OnePointFive;
                            break;
                    }

                    switch (devC04thpRow.Parity)
                    {
                        case 0:
                            DAL_Dev.PowerCom.Parity = Parity.None;
                            break;
                        case 1:
                            DAL_Dev.PowerCom.Parity = Parity.Odd;
                            break;
                        case 2:
                            DAL_Dev.PowerCom.Parity = Parity.Even;
                            break;
                        case 10:
                            DAL_Dev.PowerCom.Parity = Parity.Mark;
                            break;
                        case 20:
                            DAL_Dev.PowerCom.Parity = Parity.Space;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 载入C05模拟量参数
        /// </summary>
        private void LoadAIOParam()
        {
            #region 模拟量输入部分

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
                        DevDBDataSet.C05模拟量参数Row newDevC05Row = C05Table.NewC05模拟量参数Row();
                        newDevC05Row.ItemArray = (object[])defDevC05Row.ItemArray.Clone();
                        newDevC05Row.SingalNO = tempC05AIINRowName[i];
                        C05Table.AddC05模拟量参数Row(newDevC05Row);
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

            //载入模拟量配置参数
            for (int i = 0; i < tempC05AIINRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C05模拟量参数Row devC05aiRow = C05Table.FindBySingalNO(tempC05AIINRowName[i]);
                    if (devC05aiRow != null)
                    {
                        //基本参数
                        DAL_Dev.AIList[i].SingalNO = devC05aiRow.SingalNO;
                        DAL_Dev.AIList[i].SingalName = devC05aiRow.SingalName;
                        DAL_Dev.AIList[i].SingalUnit = devC05aiRow.SingalUnit;
                        DAL_Dev.AIList[i].IsOutType = devC05aiRow.IsOutType;
                        DAL_Dev.AIList[i].InfType = devC05aiRow.InfType;
                        DAL_Dev.AIList[i].ElecSig_Unit = devC05aiRow.ElecSig_Unit;
                        DAL_Dev.AIList[i].ElecSigLowerRange = devC05aiRow.ElecSig_LowerRange;
                        DAL_Dev.AIList[i].ElecSigUpperRange = devC05aiRow.ElecSig_UpperRange;
                        DAL_Dev.AIList[i].SingalLowerRange = devC05aiRow.SingalLowerRange;
                        DAL_Dev.AIList[i].SingalUpperRange = devC05aiRow.SingalUpperRange;
                        DAL_Dev.AIList[i].ZeroCalValue = devC05aiRow.ZeroCalValue;
                        DAL_Dev.AIList[i].KCalValue = devC05aiRow.KCalValue;
                        DAL_Dev.AIList[i].ModulNO = devC05aiRow.ModulNO;
                        DAL_Dev.AIList[i].ChannelSerialNO = devC05aiRow.ChannelSerialNO;
                        DAL_Dev.AIList[i].ConvRatio = devC05aiRow.ConvRatio;
                        DAL_Dev.AIList[i].IsOutType = false;
                        //标定点启用
                        DAL_Dev.AIList[i].CalPoints[0].IsUse = devC05aiRow.Use_Cal1;
                        DAL_Dev.AIList[i].CalPoints[1].IsUse = devC05aiRow.Use_Cal2;
                        DAL_Dev.AIList[i].CalPoints[2].IsUse = devC05aiRow.Use_Cal3;
                        DAL_Dev.AIList[i].CalPoints[3].IsUse = devC05aiRow.Use_Cal4;
                        DAL_Dev.AIList[i].CalPoints[4].IsUse = devC05aiRow.Use_Cal5;
                        DAL_Dev.AIList[i].CalPoints[5].IsUse = devC05aiRow.Use_Cal6;
                        DAL_Dev.AIList[i].CalPoints[6].IsUse = devC05aiRow.Use_Cal7;
                        DAL_Dev.AIList[i].CalPoints[7].IsUse = devC05aiRow.Use_Cal8;
                        DAL_Dev.AIList[i].CalPoints[8].IsUse = devC05aiRow.Use_Cal9;
                        DAL_Dev.AIList[i].CalPoints[9].IsUse = devC05aiRow.Use_Cal10;
                        DAL_Dev.AIList[i].CalPoints[10].IsUse = devC05aiRow.Use_Cal11;
                        //标定点标准值
                        DAL_Dev.AIList[i].CalPoints[0].StdValue = devC05aiRow.Sensor_Cal1_StdValue;
                        DAL_Dev.AIList[i].CalPoints[1].StdValue = devC05aiRow.Sensor_Cal2_StdValue;
                        DAL_Dev.AIList[i].CalPoints[2].StdValue = devC05aiRow.Sensor_Cal3_StdValue;
                        DAL_Dev.AIList[i].CalPoints[3].StdValue = devC05aiRow.Sensor_Cal4_StdValue;
                        DAL_Dev.AIList[i].CalPoints[4].StdValue = devC05aiRow.Sensor_Cal5_StdValue;
                        DAL_Dev.AIList[i].CalPoints[5].StdValue = devC05aiRow.Sensor_Cal6_StdValue;
                        DAL_Dev.AIList[i].CalPoints[6].StdValue = devC05aiRow.Sensor_Cal7_StdValue;
                        DAL_Dev.AIList[i].CalPoints[7].StdValue = devC05aiRow.Sensor_Cal8_StdValue;
                        DAL_Dev.AIList[i].CalPoints[8].StdValue = devC05aiRow.Sensor_Cal9_StdValue;
                        DAL_Dev.AIList[i].CalPoints[9].StdValue = devC05aiRow.Sensor_Cal10_StdValue;
                        DAL_Dev.AIList[i].CalPoints[10].StdValue = devC05aiRow.Sensor_Cal11_StdValue;
                        //标定点显示值
                        DAL_Dev.AIList[i].CalPoints[0].ViewValue = devC05aiRow.Sensor_Cal1_ViewValue;
                        DAL_Dev.AIList[i].CalPoints[1].ViewValue = devC05aiRow.Sensor_Cal2_ViewValue;
                        DAL_Dev.AIList[i].CalPoints[2].ViewValue = devC05aiRow.Sensor_Cal3_ViewValue;
                        DAL_Dev.AIList[i].CalPoints[3].ViewValue = devC05aiRow.Sensor_Cal4_ViewValue;
                        DAL_Dev.AIList[i].CalPoints[4].ViewValue = devC05aiRow.Sensor_Cal5_ViewValue;
                        DAL_Dev.AIList[i].CalPoints[5].ViewValue = devC05aiRow.Sensor_Cal6_ViewValue;
                        DAL_Dev.AIList[i].CalPoints[6].ViewValue = devC05aiRow.Sensor_Cal7_ViewValue;
                        DAL_Dev.AIList[i].CalPoints[7].ViewValue = devC05aiRow.Sensor_Cal8_ViewValue;
                        DAL_Dev.AIList[i].CalPoints[8].ViewValue = devC05aiRow.Sensor_Cal9_ViewValue;
                        DAL_Dev.AIList[i].CalPoints[9].ViewValue = devC05aiRow.Sensor_Cal10_ViewValue;
                        DAL_Dev.AIList[i].CalPoints[10].ViewValue = devC05aiRow.Sensor_Cal11_ViewValue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            #endregion


            #region 模拟量输出部分

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
                        DevDBDataSet.C05模拟量参数Row newDevC05Row = C05Table.NewC05模拟量参数Row();
                        newDevC05Row.ItemArray = (object[])defDevC05Row.ItemArray.Clone();
                        newDevC05Row.SingalNO = tempC05AOOutRowName[i];
                        C05Table.AddC05模拟量参数Row(newDevC05Row);
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

            //载入模拟量配置参数
            for (int i = 0; i < tempC05AOOutRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C05模拟量参数Row devC05aoRow = C05Table.FindBySingalNO(tempC05AOOutRowName[i]);
                    if (devC05aoRow != null)
                    {
                        //基本参数
                        DAL_Dev.AOList[i].SingalNO = devC05aoRow.SingalNO;
                        DAL_Dev.AOList[i].SingalName = devC05aoRow.SingalName;
                        DAL_Dev.AOList[i].SingalUnit = devC05aoRow.SingalUnit;
                        DAL_Dev.AOList[i].IsOutType = devC05aoRow.IsOutType;
                        DAL_Dev.AOList[i].InfType = devC05aoRow.InfType;
                        DAL_Dev.AOList[i].ElecSig_Unit = devC05aoRow.ElecSig_Unit;
                        DAL_Dev.AOList[i].ElecSigLowerRange = devC05aoRow.ElecSig_LowerRange;
                        DAL_Dev.AOList[i].ElecSigUpperRange = devC05aoRow.ElecSig_UpperRange;
                        DAL_Dev.AOList[i].SingalLowerRange = devC05aoRow.SingalLowerRange;
                        DAL_Dev.AOList[i].SingalUpperRange = devC05aoRow.SingalUpperRange;
                        DAL_Dev.AOList[i].ZeroCalValue = devC05aoRow.ZeroCalValue;
                        DAL_Dev.AOList[i].KCalValue = devC05aoRow.KCalValue;
                        DAL_Dev.AOList[i].ModulNO = devC05aoRow.ModulNO;
                        DAL_Dev.AOList[i].ChannelSerialNO = devC05aoRow.ChannelSerialNO;
                        DAL_Dev.AOList[i].ConvRatio = devC05aoRow.ConvRatio;
                        //标定点启用
                        DAL_Dev.AOList[i].CalPoints[0].IsUse = devC05aoRow.Use_Cal1;
                        DAL_Dev.AOList[i].CalPoints[1].IsUse = devC05aoRow.Use_Cal2;
                        DAL_Dev.AOList[i].CalPoints[2].IsUse = devC05aoRow.Use_Cal3;
                        DAL_Dev.AOList[i].CalPoints[3].IsUse = devC05aoRow.Use_Cal4;
                        DAL_Dev.AOList[i].CalPoints[4].IsUse = devC05aoRow.Use_Cal5;
                        DAL_Dev.AOList[i].CalPoints[5].IsUse = devC05aoRow.Use_Cal6;
                        DAL_Dev.AOList[i].CalPoints[6].IsUse = devC05aoRow.Use_Cal7;
                        DAL_Dev.AOList[i].CalPoints[7].IsUse = devC05aoRow.Use_Cal8;
                        DAL_Dev.AOList[i].CalPoints[8].IsUse = devC05aoRow.Use_Cal9;
                        DAL_Dev.AOList[i].CalPoints[9].IsUse = devC05aoRow.Use_Cal10;
                        DAL_Dev.AOList[i].CalPoints[10].IsUse = devC05aoRow.Use_Cal11;
                        //标定点标准值
                        DAL_Dev.AOList[i].CalPoints[0].StdValue = devC05aoRow.Sensor_Cal1_StdValue;
                        DAL_Dev.AOList[i].CalPoints[1].StdValue = devC05aoRow.Sensor_Cal2_StdValue;
                        DAL_Dev.AOList[i].CalPoints[2].StdValue = devC05aoRow.Sensor_Cal3_StdValue;
                        DAL_Dev.AOList[i].CalPoints[3].StdValue = devC05aoRow.Sensor_Cal4_StdValue;
                        DAL_Dev.AOList[i].CalPoints[4].StdValue = devC05aoRow.Sensor_Cal5_StdValue;
                        DAL_Dev.AOList[i].CalPoints[5].StdValue = devC05aoRow.Sensor_Cal6_StdValue;
                        DAL_Dev.AOList[i].CalPoints[6].StdValue = devC05aoRow.Sensor_Cal7_StdValue;
                        DAL_Dev.AOList[i].CalPoints[7].StdValue = devC05aoRow.Sensor_Cal8_StdValue;
                        DAL_Dev.AOList[i].CalPoints[8].StdValue = devC05aoRow.Sensor_Cal9_StdValue;
                        DAL_Dev.AOList[i].CalPoints[9].StdValue = devC05aoRow.Sensor_Cal10_StdValue;
                        DAL_Dev.AOList[i].CalPoints[10].StdValue = devC05aoRow.Sensor_Cal11_StdValue;
                        //标定点显示值
                        DAL_Dev.AOList[i].CalPoints[0].ViewValue = devC05aoRow.Sensor_Cal1_ViewValue;
                        DAL_Dev.AOList[i].CalPoints[1].ViewValue = devC05aoRow.Sensor_Cal2_ViewValue;
                        DAL_Dev.AOList[i].CalPoints[2].ViewValue = devC05aoRow.Sensor_Cal3_ViewValue;
                        DAL_Dev.AOList[i].CalPoints[3].ViewValue = devC05aoRow.Sensor_Cal4_ViewValue;
                        DAL_Dev.AOList[i].CalPoints[4].ViewValue = devC05aoRow.Sensor_Cal5_ViewValue;
                        DAL_Dev.AOList[i].CalPoints[5].ViewValue = devC05aoRow.Sensor_Cal6_ViewValue;
                        DAL_Dev.AOList[i].CalPoints[6].ViewValue = devC05aoRow.Sensor_Cal7_ViewValue;
                        DAL_Dev.AOList[i].CalPoints[7].ViewValue = devC05aoRow.Sensor_Cal8_ViewValue;
                        DAL_Dev.AOList[i].CalPoints[8].ViewValue = devC05aoRow.Sensor_Cal9_ViewValue;
                        DAL_Dev.AOList[i].CalPoints[9].ViewValue = devC05aoRow.Sensor_Cal10_ViewValue;
                        DAL_Dev.AOList[i].CalPoints[10].ViewValue = devC05aoRow.Sensor_Cal11_ViewValue;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            #endregion
        }

        /// <summary>
        /// 载入C06模拟量通道参数
        /// </summary>
        private void LoadAIOChannelParam()
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
                        //基本参数
                        DAL_Dev.Mod_AI_T.Channels[i].ChannelNO = devC06chlRow.ChannelNO;
                        DAL_Dev.Mod_AI_T.Channels[i].InfType = devC06chlRow.ColType;
                        DAL_Dev.Mod_AI_T.Channels[i].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        DAL_Dev.Mod_AI_T.Channels[i].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        DAL_Dev.Mod_AI_T.Channels[i].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        DAL_Dev.Mod_AI_T.Channels[i].DataLowerRange = devC06chlRow.Data_LowerRange;
                        DAL_Dev.Mod_AI_T.Channels[i].DataUpperRange = devC06chlRow.Data_UpperRange;
                        DAL_Dev.Mod_AI_T.Channels[i].IsUsed = devC06chlRow.IsUsed;
                        DAL_Dev.Mod_AI_T.Channels[i].IsOutType = devC06chlRow.IsOutType;
                        DAL_Dev.Mod_AI_T.Channels[i].FitRatio = devC06chlRow.FitRitio;
                        DAL_Dev.Mod_AI_T.Channels[i].FilterType = devC06chlRow.FilterType;
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
                        //基本参数
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].ChannelNO = devC06chlRow.ChannelNO;
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].InfType = devC06chlRow.ColType;
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].DataLowerRange = devC06chlRow.Data_LowerRange;
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].DataUpperRange = devC06chlRow.Data_UpperRange;
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].IsUsed = devC06chlRow.IsUsed;
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].IsOutType = devC06chlRow.IsOutType;
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].FitRatio = devC06chlRow.FitRitio;
                        DAL_Dev.Mod_AI_Power.Channels[i - 8].FilterType = devC06chlRow.FilterType;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
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

            //载入AO模块通道配置参数
            for (int i = 0; i < tempC06AOchlRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C06模拟量通道参数Row devC06chlRow = C06Table.FindByChannelNO(tempC06AOchlRowName[i]);
                    if (devC06chlRow != null)
                    {
                        //基本参数
                        DAL_Dev.Mod_AO.Channels[i].ChannelNO = devC06chlRow.ChannelNO;
                        DAL_Dev.Mod_AO.Channels[i].InfType = devC06chlRow.ColType;
                        DAL_Dev.Mod_AO.Channels[i].ElecSigUnit = devC06chlRow.ElecSig_Unit;
                        DAL_Dev.Mod_AO.Channels[i].ElecSigLowerRange = devC06chlRow.ElecSig_LowerRange;
                        DAL_Dev.Mod_AO.Channels[i].ElecSigUpperRange = devC06chlRow.ElecSig_UpperRange;
                        DAL_Dev.Mod_AO.Channels[i].DataLowerRange = devC06chlRow.Data_LowerRange;
                        DAL_Dev.Mod_AO.Channels[i].DataUpperRange = devC06chlRow.Data_UpperRange;
                        DAL_Dev.Mod_AO.Channels[i].IsUsed = devC06chlRow.IsUsed;
                        DAL_Dev.Mod_AO.Channels[i].IsOutType = devC06chlRow.IsOutType;
                        DAL_Dev.Mod_AO.Channels[i].FitRatio = devC06chlRow.FitRitio;
                        DAL_Dev.Mod_AO.Channels[i].FilterType = devC06chlRow.FilterType;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            #endregion

        }

        /// <summary>
        /// 绑定模拟量参数和通道
        /// </summary>
        private void BindAioAndChennel()
        {
            #region 模拟量输入部分
            try
            {
                //热电偶采集
                for (int i = 0; i < 8; i++)
                {
                    if (DAL_Dev.AIList[i].ChannelSerialNO > DAL_Dev.Mod_AI_T.Channels.Count)
                        DAL_Dev.AIList[i].ChannelSerialNO = DAL_Dev.Mod_AI_T.Channels.Count;
                    DAL_Dev.AIList[i].AnalogChannel = DAL_Dev.Mod_AI_T.Channels[DAL_Dev.AIList[i].ChannelSerialNO - 1];
                    DAL_Dev.AIList[i].CalcData_SigBounds();
                }

                //热电偶采集
                for (int i = 8; i < DAL_Dev.AIList.Count; i++)
                {
                    if (DAL_Dev.AIList[i].ChannelSerialNO > DAL_Dev.Mod_AI_Power.Channels.Count)
                        DAL_Dev.AIList[i].ChannelSerialNO = DAL_Dev.Mod_AI_Power.Channels.Count;
                    DAL_Dev.AIList[i].AnalogChannel = DAL_Dev.Mod_AI_Power.Channels[DAL_Dev.AIList[i].ChannelSerialNO - 1];
                    DAL_Dev.AIList[i].CalcData_SigBounds();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion


            #region 模拟量输出部分

            try
            {
                //
                for (int i = 0; i < DAL_Dev.AOList.Count; i++)
                {
                    if (DAL_Dev.AOList[i].ChannelSerialNO > DAL_Dev.Mod_AO.Channels.Count)
                        DAL_Dev.AOList[i].ChannelSerialNO = DAL_Dev.Mod_AO.Channels.Count;
                    DAL_Dev.AOList[i].AnalogChannel = DAL_Dev.Mod_AO.Channels[DAL_Dev.AOList[i].ChannelSerialNO - 1];
                    DAL_Dev.AOList[i].CalcData_SigBounds();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            #endregion

        }

        /// <summary>
        /// 载入C07数字量参数
        /// </summary>
        private void LoadDIOParam()
        {

            #region 数字量输入部分

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC07DIRowName = new string[1]
            {
                "ZD_Status"
            };
            for (int i = 0; i < tempC07DIRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C07数字量参数Row checkExistC07Row = C07Table.FindBySingalNO(tempC07DIRowName[i]);
                    if (checkExistC07Row == null)
                    {
                        DevDBDataSet.C07数字量参数Row defDevC07Row =
                            C07Table.FindBySingalNO(tempC07DIRowName[i].ToString() + "Def");
                        DevDBDataSet.C07数字量参数Row newDevC07Row = C07Table.NewC07数字量参数Row();
                        newDevC07Row.ItemArray = (object[])defDevC07Row.ItemArray.Clone();
                        newDevC07Row.SingalNO = tempC07DIRowName[i];
                        C07Table.AddC07数字量参数Row(newDevC07Row);
                        C07TableAdapter.Update(C07Table);
                        C07Table.AcceptChanges();
                        RaisePropertyChanged(() => C07Table);
                        MessageBox.Show("未找到" + tempC07DIRowName[i] + "配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            //载入配置参数
            for (int i = 0; i < tempC07DIRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C07数字量参数Row devC07diRow = C07Table.FindBySingalNO(tempC07DIRowName[i]);
                    if (devC07diRow != null)
                    {
                        //基本参数
                        DAL_Dev.DIList[i].SingalNO = devC07diRow.SingalNO;
                        DAL_Dev.DIList[i].SingalName = devC07diRow.SingalName;
                        DAL_Dev.DIList[i].IsOutType = devC07diRow.IsOutType;
                        DAL_Dev.DIList[i].ModulNO = devC07diRow.ModulNO;
                        DAL_Dev.DIList[i].ChannelSerialNO = devC07diRow.ChannelSerialNO;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            #endregion

            #region 数字量输出部分

            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC07DORowName = new string[1]
            {
                "PL_Ctl"
            };
            for (int i = 0; i < tempC07DORowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C07数字量参数Row checkExistC07Row = C07Table.FindBySingalNO(tempC07DORowName[i]);
                    if (checkExistC07Row == null)
                    {
                        DevDBDataSet.C07数字量参数Row defDevC07Row =
                            C07Table.FindBySingalNO(tempC07DORowName[i].ToString() + "Def");
                        DevDBDataSet.C07数字量参数Row newDevC07Row = C07Table.NewC07数字量参数Row();
                        newDevC07Row.ItemArray = (object[])defDevC07Row.ItemArray.Clone();
                        newDevC07Row.SingalNO = tempC07DORowName[i];
                        C07Table.AddC07数字量参数Row(newDevC07Row);
                        C07TableAdapter.Update(C07Table);
                        C07Table.AcceptChanges();
                        RaisePropertyChanged(() => C07Table);
                        MessageBox.Show("未找到" + tempC07DORowName[i] + "配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            //载入配置参数
            for (int i = 0; i < tempC07DORowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C07数字量参数Row devC07doRow = C07Table.FindBySingalNO(tempC07DORowName[i]);
                    if (devC07doRow != null)
                    {
                        DAL_Dev.DOList[i].SingalNO = devC07doRow.SingalNO;
                        DAL_Dev.DOList[i].SingalName = devC07doRow.SingalName;
                        DAL_Dev.DOList[i].IsOutType = devC07doRow.IsOutType;
                        DAL_Dev.DOList[i].ModulNO = devC07doRow.ModulNO;
                        DAL_Dev.DOList[i].ChannelSerialNO = devC07doRow.ChannelSerialNO;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

                #endregion
            }

        }

        /// <summary>
        /// 载入C08数字量通道参数
        /// </summary>
        private void LoadDIOChannelParam()
        {

            #region DI通道
            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC08dichlRowName = new string[8]
            {
                "DI-01",  "DI-02", "DI-03", "DI-04", "DI-05", "DI-06", "DI-07", "DI-08"
            };
            for (int i = 0; i < tempC08dichlRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C08数字量通道参数Row checkExistC08Row = C08Table.FindByChannelNO(tempC08dichlRowName[i]);
                    if (checkExistC08Row == null)
                    {
                        DevDBDataSet.C08数字量通道参数Row defDevC08Row = C08Table.FindByChannelNO(tempC08dichlRowName[i].ToString() + "Def");
                        DevDBDataSet.C08数字量通道参数Row newDevC08Row = C08Table.NewC08数字量通道参数Row();
                        newDevC08Row.ItemArray = (object[])defDevC08Row.ItemArray.Clone();
                        newDevC08Row.ChannelNO = tempC08dichlRowName[i];
                        C08Table.AddC08数字量通道参数Row(newDevC08Row);
                        C08TableAdapter.Update(C08Table);
                        C08Table.AcceptChanges();
                        RaisePropertyChanged(() => C08Table);
                        MessageBox.Show("未找到" + tempC08dichlRowName[i] + "通道配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //载入数字量通道配置参数
            for (int i = 0; i < tempC08dichlRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C08数字量通道参数Row devC08chlRow = C08Table.FindByChannelNO(tempC08dichlRowName[i]);
                    if (devC08chlRow != null)
                    {
                        DAL_Dev.Mod_DI.Channels[i].ChannelNO = devC08chlRow.ChannelNO;
                        DAL_Dev.Mod_DI.Channels[i].IsUsed = devC08chlRow.IsUsed;
                        DAL_Dev.Mod_DI.Channels[i].IsOutType = devC08chlRow.IsOutType;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            #endregion

            #region DO通道
            //若编号在表中不存在，则新建（拷贝Def数据）
            string[] tempC08dochlRowName = new string[8]
            {
                "DO-01","DO-02","DO-03","DO-04","DO-05","DO-06","DO-07","DO-08"
            };
            for (int i = 0; i < tempC08dochlRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C08数字量通道参数Row checkExistC08Row = C08Table.FindByChannelNO(tempC08dochlRowName[i]);
                    if (checkExistC08Row == null)
                    {
                        DevDBDataSet.C08数字量通道参数Row defDevC08Row = C08Table.FindByChannelNO(tempC08dochlRowName[i].ToString() + "Def");
                        DevDBDataSet.C08数字量通道参数Row newDevC08Row = C08Table.NewC08数字量通道参数Row();
                        newDevC08Row.ItemArray = (object[])defDevC08Row.ItemArray.Clone();
                        newDevC08Row.ChannelNO = tempC08dochlRowName[i];
                        C08Table.AddC08数字量通道参数Row(newDevC08Row);
                        C08TableAdapter.Update(C08Table);
                        C08Table.AcceptChanges();
                        RaisePropertyChanged(() => C08Table);
                        MessageBox.Show("未找到" + tempC08dochlRowName[i] + "通道配置参数，已重新建立！", "错误提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            //载入数字量通道配置参数
            for (int i = 0; i < tempC08dochlRowName.Length; i++)
            {
                try
                {
                    DevDBDataSet.C08数字量通道参数Row devC08chlRow = C08Table.FindByChannelNO(tempC08dochlRowName[i]);
                    if (devC08chlRow != null)
                    {
                        DAL_Dev.Mod_DO.Channels[i].ChannelNO = devC08chlRow.ChannelNO;
                        DAL_Dev.Mod_DO.Channels[i].IsUsed = devC08chlRow.IsUsed;
                        DAL_Dev.Mod_DO.Channels[i].IsOutType = devC08chlRow.IsOutType;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            #endregion

        }

        /// <summary>
        /// 绑定数字量参数和通道
        /// </summary>
        private void BindDioAndChennel()
        {

            #region 数字量输入部分
            try
            {
                for (int i = 0; i < 1; i++)
                {
                    if (DAL_Dev.DIList[i].ChannelSerialNO > DAL_Dev.Mod_DI.Channels.Count)
                        DAL_Dev.DIList[i].ChannelSerialNO = DAL_Dev.Mod_DI.Channels.Count;
                    DAL_Dev.DIList[i].DigitalChannel = DAL_Dev.Mod_DI.Channels[DAL_Dev.DIList[i].ChannelSerialNO - 1];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion

            #region 数字量输出部分
            try
            {
                for (int i = 0; i < 1; i++)
                {
                    if (DAL_Dev.DOList[i].ChannelSerialNO > DAL_Dev.Mod_DO.Channels.Count)
                        DAL_Dev.DOList[i].ChannelSerialNO = DAL_Dev.Mod_DO.Channels.Count;
                    DAL_Dev.DOList[i].DigitalChannel = DAL_Dev.Mod_DO.Channels[DAL_Dev.DOList[i].ChannelSerialNO - 1];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            #endregion
        }

        /// <summary>
        /// 载入C09PID控制参数
        /// </summary>
        private void LoadPIDSettings()
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
                DevDBDataSet.C09PID控制参数Row devC09mRow;
                if (DAL_Dev.WithPowerSenser)
                    devC09mRow = C09Table.FindBy配置编号(tempC09RowName[4]);
                else
                    devC09mRow = C09Table.FindBy配置编号(tempC09RowName[0]);
                if (devC09mRow != null)
                {
                    DAL_Dev.PID_T_Err50Up.PID_Param.ControllerName = devC09mRow.PIDName;
                    DAL_Dev.PID_T_Err50Up.PID_Param.Kp = devC09mRow.Kp;
                    DAL_Dev.PID_T_Err50Up.PID_Param.Ki = devC09mRow.Ki;
                    DAL_Dev.PID_T_Err50Up.PID_Param.Kd = devC09mRow.Kd;
                    DAL_Dev.PID_T_Err50Up.PID_Param.U_UpperBound = devC09mRow.U_UpperBound;
                    DAL_Dev.PID_T_Err50Up.PID_Param.U_LowerBound = devC09mRow.U_LowerBound;
                    DAL_Dev.PID_T_Err50Up.PID_Param.T = devC09mRow.T;
                    DAL_Dev.PID_T_Err50Up.PID_Param.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC09mRow.PIDType;
                    DAL_Dev.PID_T_Err50Up.PID_Param.U_IMax_Limit = devC09mRow.U_IMax_Limit;
                    DAL_Dev.PID_T_Err50Up.PID_Param.ErrBound_IntegralSeparate = devC09mRow.ErrBound_IntegralSeparate;
                    DAL_Dev.PID_T_Err50Up.PID_Param.Is_Kp_Used = devC09mRow.Is_Kp_Used;
                    DAL_Dev.PID_T_Err50Up.PID_Param.Is_Ki_Used = devC09mRow.Is_Ki_Used;
                    DAL_Dev.PID_T_Err50Up.PID_Param.Is_Kd_Used = devC09mRow.Is_Kd_Used;
                    DAL_Dev.PID_T_Err50Up.PID_Param.Is_U_Limit = devC09mRow.Is_U_Limit;
                    DAL_Dev.PID_T_Err50Up.PID_Param.Is_UILimit_Used = devC09mRow.Is_UILimit_Used;
                    DAL_Dev.PID_T_Err50Up.PID_Param.Is_ISeparate_Used = devC09mRow.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID_T_Err15_50
            try
            {
                DevDBDataSet.C09PID控制参数Row devC09mRow;
                if (DAL_Dev.WithPowerSenser)
                    devC09mRow = C09Table.FindBy配置编号(tempC09RowName[5]);
                else
                    devC09mRow = C09Table.FindBy配置编号(tempC09RowName[1]);
                if (devC09mRow != null)
                {
                    DAL_Dev.PID_T_Err15_50.PID_Param.ControllerName = devC09mRow.PIDName;
                    DAL_Dev.PID_T_Err15_50.PID_Param.Kp = devC09mRow.Kp;
                    DAL_Dev.PID_T_Err15_50.PID_Param.Ki = devC09mRow.Ki;
                    DAL_Dev.PID_T_Err15_50.PID_Param.Kd = devC09mRow.Kd;
                    DAL_Dev.PID_T_Err15_50.PID_Param.U_UpperBound = devC09mRow.U_UpperBound;
                    DAL_Dev.PID_T_Err15_50.PID_Param.U_LowerBound = devC09mRow.U_LowerBound;
                    DAL_Dev.PID_T_Err15_50.PID_Param.T = devC09mRow.T;
                    DAL_Dev.PID_T_Err15_50.PID_Param.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC09mRow.PIDType;
                    DAL_Dev.PID_T_Err15_50.PID_Param.U_IMax_Limit = devC09mRow.U_IMax_Limit;
                    DAL_Dev.PID_T_Err15_50.PID_Param.ErrBound_IntegralSeparate = devC09mRow.ErrBound_IntegralSeparate;
                    DAL_Dev.PID_T_Err15_50.PID_Param.Is_Kp_Used = devC09mRow.Is_Kp_Used;
                    DAL_Dev.PID_T_Err15_50.PID_Param.Is_Ki_Used = devC09mRow.Is_Ki_Used;
                    DAL_Dev.PID_T_Err15_50.PID_Param.Is_Kd_Used = devC09mRow.Is_Kd_Used;
                    DAL_Dev.PID_T_Err15_50.PID_Param.Is_U_Limit = devC09mRow.Is_U_Limit;
                    DAL_Dev.PID_T_Err15_50.PID_Param.Is_UILimit_Used = devC09mRow.Is_UILimit_Used;
                    DAL_Dev.PID_T_Err15_50.PID_Param.Is_ISeparate_Used = devC09mRow.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            //PID_T_Err15_50
            try
            {
                DevDBDataSet.C09PID控制参数Row devC09mRow ;
                if (DAL_Dev.WithPowerSenser)
                    devC09mRow = C09Table.FindBy配置编号(tempC09RowName[6]);
                else
                    devC09mRow = C09Table.FindBy配置编号(tempC09RowName[2]);
                if (devC09mRow != null)
                {
                    DAL_Dev.PID_T_Err0_15.PID_Param.ControllerName = devC09mRow.PIDName;
                    DAL_Dev.PID_T_Err0_15.PID_Param.Kp = devC09mRow.Kp;
                    DAL_Dev.PID_T_Err0_15.PID_Param.Ki = devC09mRow.Ki;
                    DAL_Dev.PID_T_Err0_15.PID_Param.Kd = devC09mRow.Kd;
                    DAL_Dev.PID_T_Err0_15.PID_Param.U_UpperBound = devC09mRow.U_UpperBound;
                    DAL_Dev.PID_T_Err0_15.PID_Param.U_LowerBound = devC09mRow.U_LowerBound;
                    DAL_Dev.PID_T_Err0_15.PID_Param.T = devC09mRow.T;
                    DAL_Dev.PID_T_Err0_15.PID_Param.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC09mRow.PIDType;
                    DAL_Dev.PID_T_Err0_15.PID_Param.U_IMax_Limit = devC09mRow.U_IMax_Limit;
                    DAL_Dev.PID_T_Err0_15.PID_Param.ErrBound_IntegralSeparate = devC09mRow.ErrBound_IntegralSeparate;
                    DAL_Dev.PID_T_Err0_15.PID_Param.Is_Kp_Used = devC09mRow.Is_Kp_Used;
                    DAL_Dev.PID_T_Err0_15.PID_Param.Is_Ki_Used = devC09mRow.Is_Ki_Used;
                    DAL_Dev.PID_T_Err0_15.PID_Param.Is_Kd_Used = devC09mRow.Is_Kd_Used;
                    DAL_Dev.PID_T_Err0_15.PID_Param.Is_U_Limit = devC09mRow.Is_U_Limit;
                    DAL_Dev.PID_T_Err0_15.PID_Param.Is_UILimit_Used = devC09mRow.Is_UILimit_Used;
                    DAL_Dev.PID_T_Err0_15.PID_Param.Is_ISeparate_Used = devC09mRow.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            //PID_Power
            try
            {
                DevDBDataSet.C09PID控制参数Row devC09mRow = C09Table.FindBy配置编号(tempC09RowName[3]);
                if (devC09mRow != null)
                {
                    DAL_Dev.PID_Power.PID_Param.ControllerName = devC09mRow.PIDName;
                    DAL_Dev.PID_Power.PID_Param.Kp = devC09mRow.Kp;
                    DAL_Dev.PID_Power.PID_Param.Ki = devC09mRow.Ki;
                    DAL_Dev.PID_Power.PID_Param.Kd = devC09mRow.Kd;
                    DAL_Dev.PID_Power.PID_Param.U_UpperBound = devC09mRow.U_UpperBound;
                    DAL_Dev.PID_Power.PID_Param.U_LowerBound = devC09mRow.U_LowerBound;
                    DAL_Dev.PID_Power.PID_Param.T = devC09mRow.T;
                    DAL_Dev.PID_Power.PID_Param.ControllerType = (CtrlMethod.PID_Enums.CtlType)devC09mRow.PIDType;
                    DAL_Dev.PID_Power.PID_Param.U_IMax_Limit = devC09mRow.U_IMax_Limit;
                    DAL_Dev.PID_Power.PID_Param.ErrBound_IntegralSeparate = devC09mRow.ErrBound_IntegralSeparate;
                    DAL_Dev.PID_Power.PID_Param.Is_Kp_Used = devC09mRow.Is_Kp_Used;
                    DAL_Dev.PID_Power.PID_Param.Is_Ki_Used = devC09mRow.Is_Ki_Used;
                    DAL_Dev.PID_Power.PID_Param.Is_Kd_Used = devC09mRow.Is_Kd_Used;
                    DAL_Dev.PID_Power.PID_Param.Is_U_Limit = devC09mRow.Is_U_Limit;
                    DAL_Dev.PID_Power.PID_Param.Is_UILimit_Used = devC09mRow.Is_UILimit_Used;
                    DAL_Dev.PID_Power.PID_Param.Is_ISeparate_Used = devC09mRow.Is_ISeparate_Used;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
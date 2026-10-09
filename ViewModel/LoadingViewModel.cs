/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.ViewModel
 * 文件名：  LoadingViewModel
 * 版本号：  V1.0.0.0
 * 唯一标识：1c75acf8-89c4-428f-a9be-a6bc88f16916
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/2/4 11:28:27
 * 描述：
 *
 * ==================================================================================
 * 修改标记
 * 修改时间			修改人			版本号			描述
 * 2022/2/4 11:28:27		郝正强			V1.0.0.0
 *
 *
 ************************************************************************************/

using System;
using System.Windows;
using Authorize;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace BRX.ViewModel
{
    /// <summary>
    /// 申请码提取及授权码验证！
    /// </summary>
    public  class LoadingViewModel:ViewModelBase 
    {
        public LoadingViewModel()
        {
            AuthorizationCheckSN.AppType = 22;  //新不燃性，22
            AuthorizationCheckSN.AuthDAL = new AuthorizeDAL(AuthorizationCheckSN.AppType);
            ReqStr = AuthorizationCheckSN.GetRequestStr();
        }

        /// <summary>
        /// 注册授权公共类
        /// </summary>
        private AuthorizeBLL _authorizationCheckSN = new AuthorizeBLL();
        /// <summary>
        /// 注册授权公共类
        /// </summary>
        public AuthorizeBLL AuthorizationCheckSN
        {
            get { return _authorizationCheckSN; }
            set
            {
                _authorizationCheckSN = value;
                RaisePropertyChanged(() => AuthorizationCheckSN);
            }
        }

        /// <summary>
        /// 软件名称
        /// </summary>
        public string AppName
        {
            get { return AuthorizeCommon.AppTypes[AuthorizationCheckSN.AppType].Name; }
        }

        #region 设置用属性

        /// <summary>
        ///授权码字段1
        /// </summary>
        private string _snStr1 = "";
        /// <summary>
        /// 授权码字段1
        /// </summary>
        public string SNStr1
        {
            get { return _snStr1; }
            set
            {
                if (value.Length == 5)
                    _snStr1 = value;
                else
                    _snStr1 = "";
                RaisePropertyChanged(() => SNStr1);
            }
        }

        /// <summary>
        ///授权码字段2
        /// </summary>
        private string _snStr2 = "";
        /// <summary>
        /// 授权码字段2
        /// </summary>
        public string SNStr2
        {
            get { return _snStr2; }
            set
            {
                if (value.Length == 5)
                    _snStr2 = value;
                else
                    _snStr2 = "";
                RaisePropertyChanged(() => SNStr2);
            }
        }

        /// <summary>
        ///授权码字段3
        /// </summary>
        private string _snStr3 = "";
        /// <summary>
        /// 授权码字段3
        /// </summary>
        public string SNStr3
        {
            get { return _snStr3; }
            set
            {
                if (value.Length == 5)
                    _snStr3 = value;
                else
                    _snStr3 = "";
                RaisePropertyChanged(() => SNStr3);
            }
        }

        /// <summary>
        ///授权码字段4
        /// </summary>
        private string _snStr4 = "";
        /// <summary>
        /// 授权码字段4
        /// </summary>
        public string SNStr4
        {
            get { return _snStr4; }
            set
            {
                if (value.Length == 5)
                    _snStr4 = value;
                else
                    _snStr4 = "";
                RaisePropertyChanged(() => SNStr4);
            }
        }

        /// <summary>
        /// 申请码
        /// </summary>
        private string _reqStr = "";
        /// <summary>
        /// 申请码
        /// </summary>
        public string ReqStr
        {
            get { return _reqStr; }
            set
            {
                _reqStr = value;
                RaisePropertyChanged(() => ReqStr);
            }
        }

        #endregion
        

        /// <summary>
        /// 授权码验证指令
        /// </summary>
        private RelayCommand<String> _regCommand;
        /// <summary>
        /// 授权码验证指令
        /// </summary>
        public RelayCommand<String> RegCommand
        {
            get
            {
                if (_regCommand == null)
                    _regCommand = new RelayCommand<String>((p) => ExecuteRegCMD(p));
                return _regCommand;

            }
            set { _regCommand = value; }
        }

        /// <summary>
        /// 授权码验证指令回调
        /// </summary>
        private void ExecuteRegCMD(String num)
        {
            int cmd = Convert.ToInt16(num);
            if (cmd == 2)
            {
                Application.Current .Shutdown();
            }

            if (cmd == 1)
            {
                string tempSN = "";

                if ((SNStr1.Length != 5) || (SNStr2.Length != 5) || (SNStr3.Length != 5) || (SNStr4.Length != 5))
                {
                    MessageBox.Show("授权码不正确1!");
                    return;
                }

                bool isSNFormatWell1 = true;
                bool isSNFormatWell2 = true;
                bool isSNFormatWell3 = true;
                bool isSNFormatWell4 = true;
                //分析授权码字符格式
                for (int i = 0; i < SNStr1.Length; i++)
                {
                    char chr = SNStr1[i];
                    if (!AuthorizeCommon.CharIsNoOrUpLetter(chr))
                        isSNFormatWell1 = false;
                }
                for (int i = 0; i < SNStr2.Length; i++)
                {
                    char chr = SNStr2[i];
                    if (!AuthorizeCommon.CharIsNoOrUpLetter(chr))
                        isSNFormatWell2 = false;
                }
                for (int i = 0; i < SNStr3.Length; i++)
                {
                    char chr = SNStr3[i];
                    if (!AuthorizeCommon.CharIsNoOrUpLetter(chr))
                        isSNFormatWell3 = false;
                }
                for (int i = 0; i < SNStr4.Length; i++)
                {
                    char chr = SNStr4[i];
                    if (!AuthorizeCommon.CharIsNoOrUpLetter(chr))
                        isSNFormatWell4 = false;
                }
                if ((!isSNFormatWell1) || (!isSNFormatWell2) || (!isSNFormatWell3) || (!isSNFormatWell4))
                {
                    MessageBox.Show("授权码不正确2!");
                    return;
                }
                tempSN = SNStr1.Substring(0, SNStr1.Length) +"-"+ SNStr2.Substring(0, SNStr2.Length) + "-" +
                          SNStr3.Substring(0, SNStr3.Length) + "-" + SNStr4.Substring(0, SNStr4.Length);

                int tempRet=AuthorizationCheckSN.CheckSN(tempSN, AuthorizationCheckSN.AppType);

                if (tempRet == 1)
                {
                    MessageBox.Show("未测试到授权信息！");
                    return;
                }
                if (tempRet == 2)
                {
                    MessageBox.Show("授权无效1！");
                    return ;
                }
                if (tempRet == 3)
                {
                    MessageBox.Show("授权无效2！");
                    return ;
                }
                if (tempRet == 4)
                {
                    MessageBox.Show("授权无效3！");
                    return ;
                }
                if (tempRet == 5)
                {
                    MessageBox.Show("授权无效4！");
                    return;
                }
                AuthorizationCheckSN.AuthDAL.SaveSN(tempSN);
                AuthorizationCheckSN.AuthDAL.SaveLastTime();
                MessageBox.Show("请重新打开软件！");
                Application.Current.Shutdown();
            }
        }





    }
}

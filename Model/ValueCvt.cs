/************************************************************************************
 * Copyright (c) 2022  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 命名空间：BRX.Model
 * 文件名：  ValueCvt
 * 版本号：  V1.0.0.0
 * 唯一标识：928282bc-8175-431a-8c9a-911404599625
 * 创建人：  郝正强
 * 电子邮箱：88129312@qq.com
 * 创建时间：2022/3/16 10:18:40
 * 描述：
 * 数据类型转换
 * ==================================================================================
 * 修改标记
 * 修改时间				    修改人			版本号			描述
 * 2022/3/16 10:18:40		郝正强			V1.0.0.0
 *
 ************************************************************************************/

using System;

namespace BRX.Model
{
    /// <summary>
    /// DIO用ushort数据生成及解析类。modbus通讯用。
    /// </summary>
    public static class ValueCvt
    {
        #region 生成ushort

        /// <summary>
        /// 解析Real类型数据放在ushort数组指定位置
        /// </summary>
        /// <param name="tgt">目标ushort数组</param>
        /// <param name="start">目标位置位置</param>
        /// <param name="value">待转换数组</param>
        public static void SetRealToUshort(ushort[] tgt, int start, float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            ushort[] dest = Bytes2Ushorts(bytes);
            dest.CopyTo(tgt, start);
        }

        /// <summary>
        /// Short类型数据放置进ushort数组
        /// </summary>
        /// <param name="tgt">目标数组</param>
        /// <param name="start">目标位置</param>
        /// <param name="value">待放置数据</param>
        private static void SetShortArryToUshort(ushort[] tgt, int start, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            ushort[] dest = Bytes2Ushorts(bytes);
            dest.CopyTo(tgt, start);
        }

        private static byte Bools2Byte(bool[] array)
        {
            if (array != null && array.Length > 0)
            {
                byte b = 0;
                for (int i = 0; i < 8; i++)
                {
                    if (array[i])
                    {
                        byte nn = (byte)(1 << i);//左移一位，相当于×2
                        b += nn;
                    }
                }
                return b;
            }
            return 0;
        }

        private static ushort[] Bytes2Ushorts(byte[] src, bool reverse = false)
        {
            int len = src.Length;

            byte[] srcPlus = new byte[len + 1];
            src.CopyTo(srcPlus, 0);
            int count = len >> 1;

            if (len % 2 != 0)
            {
                count += 1;
            }

            ushort[] dest = new ushort[count];
            if (reverse)
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i] = (ushort)(srcPlus[i * 2] << 8 | srcPlus[2 * i + 1] & 0xff);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i] = (ushort)(srcPlus[i * 2] & 0xff | srcPlus[2 * i + 1] << 8);
                }
            }

            return dest;
        }

        #endregion


        #region ushort解析

        /// <summary>
        /// 获取float类型数据
        /// </summary>
        /// <param name="src">待转换ushort数组</param>
        /// <param name="start">起始位置</param>
        /// <returns>转换结果</returns>
        public static float GetRealFromUshort(ushort[] src, int start)
        {
            if ((src.Length - start) > 1)
            {
                ushort[] temp = new ushort[2];
                for (int i = 0; i < 2; i++)
                {
                    temp[i] = src[i + start];
                }
                byte[] bytesTemp = Ushorts2Bytes(temp);
                float res = BitConverter.ToSingle(bytesTemp, 0);
                return res;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// 获取float类型数据（大端模式）
        /// </summary>
        /// <param name="src">待转换ushort数组</param>
        /// <param name="start">起始位置</param>
        /// <returns>转换结果</returns>
        public static float GetRealFromUshortBigEnd(ushort[] src, int start)
        {
            if ((src.Length - start) > 1)
            {
                ushort[] temp1 = new ushort[2];
                temp1[1] = src[start];
                temp1[0] = src[start + 1];
                float[] floatData = new float[temp1.Length / 2];
                Buffer.BlockCopy(temp1, 0, floatData, 0, temp1.Length * 2);
                return floatData[0];
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// 从ushort获取short类型数据
        /// </summary>
        /// <param name="src">待转换数组</param>
        /// <param name="start">数据位置</param>
        /// <returns>转换结果</returns>
        public static short GetShortFromUshort(ushort[] src, int start)
        {
            ushort[] temp = new ushort[1];
            temp[0] = src[start];
            byte[] bytesTemp = Ushorts2Bytes(temp);
            short res = BitConverter.ToInt16(bytesTemp, 0);
            return res;
        }

        /// <summary>
        /// 从ushort数组解析各位的bool值
        /// </summary>
        /// <param name="src">待转换ushort数组</param>
        /// <param name="start">起始位置</param>
        /// <param name="num">转换长度</param>
        /// <returns></returns>
        public static bool[] GetBoolsFromUshort(ushort[] src, int start, int num)
        {
            ushort[] temp = new ushort[num];
            for (int i = 0; i < num; i++)
            {
                temp[i] = src[i + start];
            }
            byte[] bytes = Ushorts2Bytes(temp);

            bool[] res = Bytes2Bools(bytes);

            return res;
        }

        private static bool[] Bytes2Bools(byte[] b)
        {
            bool[] array = new bool[8 * b.Length];

            for (int i = 0; i < b.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    array[i * 8 + j] = (b[i] & 1) == 1;//判定byte的最后一位是否为1，若为1，则是true；否则是false
                    b[i] = (byte)(b[i] >> 1);//将byte右移一位
                }
            }
            return array;
        }

        private static byte[] Ushorts2Bytes(ushort[] src, bool reverse = false)
        {
            int count = src.Length;
            byte[] dest = new byte[count << 1];
            if (reverse)
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i * 2] = (byte)(src[i] >> 8);
                    dest[i * 2 + 1] = (byte)(src[i] >> 0);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    dest[i * 2] = (byte)(src[i] >> 0);
                    dest[i * 2 + 1] = (byte)(src[i] >> 8);
                }
            }
            return dest;
        }
        

        /// <summary>
        /// 从ushort数组指定位置获取单个i16类型数据
        /// </summary>
        /// <param name="src">待转换ushort数组</param>
        /// <param name="start">起始位置</param>
        /// <returns>转换结果</returns>
        public static Int16 GetI16FromUshortArry(ushort[] src, int start)
        {
            if ((src.Length - start) > 1)
            {
                ushort[] temp = new ushort[1];
                for (int i = 0; i < 1; i++)
                {
                    temp[i] = src[i + start];
                }
                byte[] bytesTemp = Ushorts2Bytes(temp);
                Int16 res = BitConverter.ToInt16(bytesTemp, 0);
                return res;
            }
            return (Int16)0;
        }


        #endregion
    }
}
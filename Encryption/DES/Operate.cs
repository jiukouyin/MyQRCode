﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Encryption.DES
{
    public class Operate
    {
        private static readonly byte[] iv = { 59, 76, 55, 186, 78, 4, 217, 32 };

        /// <summary>
        /// DES加密方法
        /// </summary>
        /// <param name="Source">明文</param>
        /// <param name="Key">密钥</param>
        /// <returns></returns>
        public static string Encrypt(string Source, string Key)
        {
            Source = Source.Trim();
            byte[] key = Encoding.UTF8.GetBytes(Key);
            if (String.IsNullOrEmpty(Source))
            {
                throw new Exception("没有输入明文！");
            }

            if (key.Length != 8)
            {
                throw new Exception("密钥个数必须为8个字符或4个汉字！");
            }

            byte[] bytIn = Encoding.Default.GetBytes(Source);
            DESCryptoServiceProvider mobjCryptoService = new DESCryptoServiceProvider();
            mobjCryptoService.Key = key;
            mobjCryptoService.IV = iv;
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// DES解密方法
        /// </summary>
        /// <param name="Source">密文</param>
        /// <param name="Key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string Source, string Key)
        {
            if (String.IsNullOrEmpty(Source))
            {
                throw new Exception("没有输入密文！");
            }

            byte[] key = Encoding.UTF8.GetBytes(Key);
            if (key.Length != 8)
            {
                throw new Exception("密钥个数必须为8个字符或4个汉字！");
            }

            byte[] bytIn = Convert.FromBase64String(Source);
            DESCryptoServiceProvider mobjCryptoService = new DESCryptoServiceProvider();
            mobjCryptoService.Key = key;
            mobjCryptoService.IV = iv;
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader strd = new StreamReader(cs, Encoding.Default);
            return strd.ReadToEnd();
        }
    }
}

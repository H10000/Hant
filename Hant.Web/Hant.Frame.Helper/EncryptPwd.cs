using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hant.Frame.Helper
{
    public static class EncryptPwd
    {
        #region ---生成加密盐
        public static string CreateSalt(int size = 10)
        {
            //使用加密服务提供程序 (CSP) 提供的实现来实现加密随机数生成器 (RNG).
            //using System.Security.Cryptography;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            //返回Base64字符串表示形式的随机数 
            return Convert.ToBase64String(buff);

        }
        #endregion

        #region ---SHA512+Salt进行加密
        public static string CreatePwdHashSHA512(string pwd, string salt)
        {
            SHA512CryptoServiceProvider SHA512 = new SHA512CryptoServiceProvider();
            string saltAndPwd = string.Concat(pwd, salt);
            byte[] buffer = Encoding.UTF8.GetBytes(saltAndPwd);
            byte[] h5 = SHA512.ComputeHash(buffer);
            string hashedPwd = BitConverter.ToString(h5).Replace("-", string.Empty);
            return hashedPwd;
        }
        #endregion
    }
}

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace dynamicPropertyAccessor
{
    internal static class TypeExtensions
    {

        #region string

        public static bool NotNullOrWhiteSpace(this string str)
           => !string.IsNullOrWhiteSpace(str);

        public static bool IsNullOrWhiteSpace(this string str)
            => string.IsNullOrWhiteSpace(str);

        #endregion string

        public static string GetMD5(this string str)
        {
            string result = null;
            if (NotNullOrWhiteSpace(str))
            {
                var data = calculateMD5(str);
                var sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                result = sb.ToString();
            }
            return result;
        }

        public static string GetShortMD5(this string str)
        {
            string result = null;
            if (NotNullOrWhiteSpace(str))
            {
                var data = calculateMD5(str);
                var sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    if (i < 8)
                        sb.Append(data[i].ToString("x2"));
                    else
                        break;
                }
                result = sb.ToString();
            }
            return result;
        }

        private static byte[] calculateMD5(this string str)
        {
            byte[] result = null;
            if (NotNullOrWhiteSpace(str))
            {
                using var md5 = MD5.Create();
                result = md5.ComputeHash(Encoding.Default.GetBytes(str));
            }
            return result;
        }
    }
}
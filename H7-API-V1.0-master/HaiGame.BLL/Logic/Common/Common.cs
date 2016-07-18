using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaiGame7.BLL.Enum;
using System.Security.Cryptography;
using HaiGame7.Model.EFModel;
using HaiGame7.Model.MyModel;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Net.Mime.MediaTypeNames;

namespace HaiGame7.BLL.Logic.Common
{
    public class Common
    {
        #region 发送短信验证码
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="randomNum"></param>
        /// <returns></returns>
        public static Dictionary<string, object> SendSMS(string phoneNumber, string randomNum)
        {
            Dictionary<string, object> retData = new Dictionary<string, object>();
            string[] data = { randomNum, SMS.TIME_OUT };
            CCPRestSDK.CCPRestSDK api = new CCPRestSDK.CCPRestSDK();
            bool isInit = api.init(SMS.ADDRESS, SMS.PORT);
            api.setAccount(SMS.ACCOUNT_SID, SMS.ACCOUNT_TOKEN);
            api.setAppId(SMS.APP_ID);
            if (isInit)
            {
                retData = api.SendTemplateSMS(phoneNumber, SMS.TEMPLATE_ID, data);
            }
            return retData;
        }
        #endregion

        #region 验证accesstoken是否合法
        /// <summary>
        /// 验证accesstoken是否合法
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsValid(string token)
        {
            if (token == "ABC12abc")
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 产生随机验证码
        /// <summary>
        /// 随机验证码
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string MathRandom(int? length)
        {
            string a = "0123456789";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(a[new Random(Guid.NewGuid().GetHashCode()).Next(0, a.Length - 1)]);
            }
            return sb.ToString();
        }
        #endregion

        #region MD5 加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        #endregion

        #region 返回时间差
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            TimeSpan ts = DateTime2 - DateTime1;
            if (ts.Days >=1)
            {
                dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
            }
            else
            {
                if (ts.Hours > 1)
                {
                    dateDiff = ts.Hours.ToString() + "小时前";
                }
                else
                {
                    dateDiff = ts.Minutes.ToString() + "分钟前";
                }
            }
            return dateDiff;
        }
        #endregion

        #region macSha1算法加密 UTF BASE64加密
        /// <summary>
        /// macSha1算法加密 UTF BASE64加密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HmacSha1(string text, string key)
        {
            Encoding encode = Encoding.GetEncoding("UTF-8");
            byte[] byteData = encode.GetBytes(text);
            byte[] byteKey = encode.GetBytes(key);
            HMACSHA1 hmac = new HMACSHA1(byteKey);
            CryptoStream cs = new CryptoStream(Stream.Null, hmac, CryptoStreamMode.Write);
            cs.Write(byteData, 0, byteData.Length);
            cs.Close();
            return Convert.ToBase64String(hmac.Hash);
        }
        #endregion

        #region 将Base64字符串转换为图片
        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string Base64ToImage(string base64,string userID)
        {
            byte[] arr = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap bmp = new Bitmap(ms);

            string txtFileName = @"D:\HiGameImages\avatar\"+ userID+"_"+DateTime.Now.ToString("yyyyMMddHHmmss")  + ".png";
            bmp.Save(txtFileName, ImageFormat.Png);
            ms.Close();
            ms.Dispose();
            bmp.Dispose();
            return txtFileName.Replace(@"D:\HiGameImages\avatar\",@"http://images.haigame7.com/avatar/");
        }
        #endregion

        #region 将Base64字符串转换为战队图片
        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string Base64ToTeamImage(string base64, string userID)
        {
            byte[] arr = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap bmp = new Bitmap(ms);

            string txtFileName = @"D:\HiGameImages\logo\" + userID + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            bmp.Save(txtFileName, ImageFormat.Png);
            ms.Close();
            ms.Dispose();
            bmp.Dispose();
            return txtFileName.Replace(@"D:\HiGameImages\logo\", @"http://images.haigame7.com/logo/");
        }
        #endregion

        #region 将Base64字符串转换赛果截图
        /// <summary>
        /// 将Base64字符串转换赛果截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string Base64ToFightImage(string base64, string dateID)
        {
            byte[] arr = Convert.FromBase64String(base64);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap bmp = new Bitmap(ms);

            string txtFileName = @"D:\HiGameImages\fight\" + dateID + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            bmp.Save(txtFileName, ImageFormat.Png);
            ms.Close();
            ms.Dispose();
            bmp.Dispose();
            return txtFileName.Replace(@"D:\HiGameImages\fight\", @"http://images.haigame7.com/fight/");
        }
        #endregion
    }
}

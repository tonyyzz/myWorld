using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace QRDemo
{
    public class XYEncryptionTool
    {
        //#region MD5 加密

        /// <summary>
        /// MD5加密
        /// </summary>
        public static string Md5(string strValue)
        {
            string result = "";
            byte[] strBy = Encoding.UTF8.GetBytes(strValue);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(strBy);
            result = BitConverter.ToString(output).Replace("-", "");
            return result.ToLower();
        }

        /// <summary>
        /// AES加密
        /// </summary>
        public static byte[] AESEncrypt(string encryptStr, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(encryptStr);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="toEncryptArray">密文</param>
        /// <param name="keyArray">密钥</param>
        /// <returns></returns>
        public static string AESDecode(byte[] toEncryptArray, byte[] keyArray)
        {
            //byte[] keyArray = Encoding.UTF8.GetBytes(key);
            //byte[] toEncryptArray = Convert.FromBase64String(decryptStr);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptStr">密文</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string AESDecodeStr(string decryptStr, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(decryptStr);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        ///<summary>
        /// BASE64加密 传入bute[]
        /// </summary>
        public static string base64(byte[] byteArray)
        {
            try
            {
                return Convert.ToBase64String(byteArray);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string UrlEncode(string url)
        {
            string result = "";
            result = System.Web.HttpUtility.UrlEncode(Encoding.UTF8.GetBytes(url));
            return result;
        }

        public static string GetIP()
        {
            //重复获取n次
            int index = 1;
            string ipStr = "";
            while (index <= 10)
            {
                try
                {
                    Uri uri = new Uri("http://city.ip138.com/ip2city.asp");
                    System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
                    req.Method = "get";
                    using (Stream s = req.GetResponse().GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(s))
                        {
                            char[] ch = { '[', ']' };
                            string str = reader.ReadToEnd();
                            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(str, @"\[(?<IP>[0-9\.]*)\]");
                            //return m.Value.Trim(ch);
                            ipStr = m.Value.Trim(ch);
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    index += 1;
                }
            }
            return ipStr;

        }

        public static Dictionary<string, object> JsonToDictionary(string jsonData)
        {
            //实例化JavaScriptSerializer类的新实例
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return jss.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>  
        /// 将Dictionary序列化为json数据  
        /// </summary>  
        /// <param name="jsonData">json数据</param>  
        /// <returns></returns>  
        public static string DictionaryToJson(Dictionary<string, object> dic)
        {
            //实例化JavaScriptSerializer类的新实例  
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象  
                return jss.Serialize(dic);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>  
        /// RSA签名验证  
        /// </summary>  
        /// <param name="strKeyPublic">公钥</param>  
        /// <param name="content">Hash描述</param>  
        /// <param name="signCheckStr">签名后的结果</param>  
        /// <returns></returns>  
        public static bool SignatureDeformatter(string strKeyPublic, string content, string signCheckStr)
        {

            byte[] Data = Encoding.UTF8.GetBytes(content);
            byte[] data = Convert.FromBase64String(signCheckStr);

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(strKeyPublic);

            SHA1 sh = new SHA1CryptoServiceProvider();
            return RSA.VerifyData(Data, sh, data);

        }

        public static string getRootFilePath()
        {
            string path = "";
            string upPath = Environment.CurrentDirectory;
            string firstStr = upPath.Substring(upPath.Length - 6, 1);
            if (string.Compare(firstStr, "\\") == 0)
            {
                path = upPath.Replace("bin\\Debug", "Properties\\cer\\");
            }
            else if (string.Compare(firstStr, "/") == 0)
            {
                path = upPath.Replace("bin/Debug", "Properties/cer/");
            }
            return path;
        }
    }
}

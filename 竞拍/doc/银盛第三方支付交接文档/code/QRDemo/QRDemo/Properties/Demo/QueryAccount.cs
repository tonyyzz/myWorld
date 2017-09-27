using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace QRDemo
{
	public class QueryAccount
	{
		
		public static void sendPost()
		{
			/*
			 * 按照开发文档 填写对应参数
			*/
			string url = StrUtil.req_url;
			string merchant = "1000022215230183";
			string sn = StrUtil.getSn();
			string busiType = "150001";
			string merKey = "UVZMMGDM";

			string content = sn + merchant;
			string _sign = null;
			try
			{
				_sign = sign(content, merKey);
			}
			catch (Exception e)
			{
				throw e;
			}

			string keyStr = StrUtil.getRndStr(16);
			string encryptData = ArgsSign.getEncryptData(keyStr, _sign + busiType);
			string encryptKey = ArgsSign.getEncryptKey(keyStr);

			Dictionary<string, string> dataDic = new Dictionary<string, string>();
			dataDic.Add("sn", sn);
			dataDic.Add("merchant", merchant);
			dataDic.Add("sign", ArgsSign.getSignEncryptValue(_sign));
			dataDic.Add("encryptData", encryptData);
			dataDic.Add("encryptKey", encryptKey);
			dataDic.Add("busiType", busiType);

			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			req.Method = "POST";
			req.ContentType = "application/x-www-form-urlencoded";
			StringBuilder builder = new StringBuilder();
			int i = 0;
			foreach (var item in dataDic)
			{
				if (i > 0)
					builder.Append("&");
				builder.AppendFormat("{0}={1}", item.Key, XYEncryptionTool.UrlEncode(item.Value));
				i++;
			}
			string urlEncodeStr = builder.ToString();
			byte[] data = Encoding.UTF8.GetBytes(urlEncodeStr);
			req.ContentLength = data.Length;

			using (Stream reqStream = req.GetRequestStream())
			{
				reqStream.Write(data, 0, data.Length);
				reqStream.Close();
			}

			string result = "";
			HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
			Stream stream = resp.GetResponseStream();
			using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
			{
				result = reader.ReadToEnd();
				postSuccess(result);
			}

		}

		public static void postSuccess(string responseStr)
		{
			Dictionary<string, object> dic = XYEncryptionTool.JsonToDictionary(responseStr);
			Dictionary<string, object> codeRspDic = (Dictionary<string, object>)dic["codeRsp"];

			string code = (string)codeRspDic["code"];
			Dictionary<string, object> encryptDic = (Dictionary<string, object>)dic["encrypt"];
			string encryptData = (string)encryptDic["encryptData"];
			string encryptKey = (string)encryptDic["encryptKey"];

			encryptKey = System.Web.HttpUtility.UrlDecode(encryptKey);
			encryptData = System.Web.HttpUtility.UrlDecode(encryptData);

			byte[] encryptKeyByte = System.Text.Encoding.UTF8.GetBytes(encryptKey);
			string encryptKeyStr = System.Text.Encoding.UTF8.GetString(encryptKeyByte);
			byte[] keyBase64Byte = Convert.FromBase64String(encryptKeyStr);
			byte[] decryptKeyByte = ArgsSign.getDecryptRsaValue(keyBase64Byte);

			byte[] encryptDataByte = System.Text.Encoding.UTF8.GetBytes(encryptData);
			string encryptDataStr = System.Text.Encoding.UTF8.GetString(encryptDataByte);
			byte[] dataBase64Byte = Convert.FromBase64String(encryptDataStr);
			string resultStr = XYEncryptionTool.AESDecode(dataBase64Byte, decryptKeyByte);
			string jsonStr = XYEncryptionTool.DictionaryToJson(codeRspDic);


			if(resultStr == null || string.Compare(resultStr, jsonStr) != 0){
				Console.WriteLine("报文失败");
				return;
			}
			Console.WriteLine("报文成功");
			Console.WriteLine(responseStr);
		}


		public static string sign(string content, string key)
		{
			string signData = content + key;
			return XYEncryptionTool.Md5(signData);
		}
	}
}

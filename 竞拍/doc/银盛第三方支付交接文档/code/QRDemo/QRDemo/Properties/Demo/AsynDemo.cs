using System;
using System.Text;
using System.Collections.Generic;
using System.Net;

namespace QRDemo
{
	public class AsynDemo
	{
		public static Dictionary<string, string> dataDic = new Dictionary<string, string>{  
			{"encryptData","IaXYzl%2FV%2BBAIlliaU4Z6WcloCnX4D8Bn3X3F1w7v17%2BPBI8p0n2UgTh24KMJ7ivApoJJlrkFjnMp6FdV3W56ew%3D%3D"},
			{"sign","WvYStEto%2FC4Dx2UexpTBKsXgf62vNc1qWg7PFIcxTB%2BqkXipfGKG7C1xpZGJ%2FfzaZm5zBfkynt%2FF%2B2OjPrdG7YlmP5oz4YDoFTfiWQoZjNr3mZoBadBoCA5zAx6Dkm1%2FCM0aHwLuD0fLG1lzGYNzItZbAw6s9VZnmRWY%2FaZTki3fp47QhqEEQDVluHzf87B9PrN%2F7hP3CkzqiNIRak5aJU92m0YIJ1Mr1TleZSzILnhFgXBH%2Fal2t7Rbyxu%2FTEgZj4h7FIW0kl90ZoL1qSMQqO3NdnkYstFNDoLi63SL2U3MgaboSVNLHk2%2Fhv9LhGtL46mJoRFMU6z2t80l0l3LbA%3D%3D"},
			{"responeSN","c05751d30ced4be895ea8b7989029d14"},
			{"message","{\"msg\":\"交易成功\",\"code\":\"0000\",\"payAmount\":\"0.01\",\"srcReqSN\":\"20170602094848203387001867\",\"payTime\":\"2017-06-02 09:50:30.0\",\"merchant\":\"1000022215230183\",\"responeSN\":\"2017060209485588189432664789\",\"remark\":\"DEFAULT\",\"deviceInfo\":\"device_pn\"}"},
			{"encryptKey","iZdAflu9WiNAD1FpwPp6phWMX1Wdf%2BZaSrxWNWl9GpRoCVDIHMhQrdWJD550THKArkKPrBOSqOYsbHbAcjR7khIriis52VlM%2BLDln0A6bHbIx6KVVPPRqKuZxG7gKpmEiMIZgtcZMeM8oTM%2FTLpI45vOdDnQSFNUp9IMi4tz3cmlRAkq3EHxxOqJ62oH2g45JPV4JAIqWd4q3On8UJgofi3eGc9MP0%2BhqySm%2B3gbqIu4u40e4VOlvxHo2f9Hx5ilreQnxzwgeWlEzadP0mWRJ1huBetGesd8%2FQqv%2BBdqGaa12NtwGn9XawRWA9dd2Xhy81W7049EEpNB43T%2BZTmA0Q%3D%3D"}};

		public static void resolveData()
		{
			string responeSn = dataDic["responeSN"];
			string merchant = "1000022215230183";
			string message = dataDic["message"];
			string key = "UVZMMGDM";
			string data = responeSn + merchant + message + key;
			Console.WriteLine(data);

			string sign_MD5 = XYEncryptionTool.Md5(data);
			Console.WriteLine(sign_MD5);
			Console.WriteLine(System.Web.HttpUtility.UrlDecode(dataDic["sign"]));
			Console.WriteLine(XYEncryptionTool.SignatureDeformatter(ArgsSign.getPublicKey(), sign_MD5, System.Web.HttpUtility.UrlDecode(dataDic["sign"])));

			string encryptData = dataDic["encryptData"];
			string encryptKey = dataDic["encryptKey"];

			encryptKey = System.Web.HttpUtility.UrlDecode(encryptKey);
			encryptData = System.Web.HttpUtility.UrlDecode(encryptData);

			byte[] encryptBase64Byte = Convert.FromBase64String(encryptKey);
			byte[] decryptKeyByte = ArgsSign.getDecryptRsaValue(encryptBase64Byte);
			string deKeyStr16 = System.Text.Encoding.UTF8.GetString(decryptKeyByte);

			string deData = XYEncryptionTool.AESDecodeStr(encryptData, deKeyStr16);

			Console.WriteLine(deKeyStr16);
			Console.WriteLine(deData);
		}
	}
}

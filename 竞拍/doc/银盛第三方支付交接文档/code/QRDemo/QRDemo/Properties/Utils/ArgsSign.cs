using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
namespace QRDemo
{

	public class ArgsSign
	{
		/// <summary>
		/// AES base64加密
		/// </summary>
		/// <returns>The encrypt data.</returns>
		/// <param name="keyStr">Key string.</param>
		/// <param name="data">Data.</param>
		public static string getEncryptData(string keyStr, string data) 
		{
			try{//"07effaba06ccb18d5a300e383f36a9a0120001"  "sXoNRVaFwUn62GZu"
				return XYEncryptionTool.base64(XYEncryptionTool.AESEncrypt(data, keyStr));
			} catch (Exception e) {
				throw e;
			};
		}


		/// <summary>
		/// 对sine 进行私钥RSA签名
		/// </summary>
		/// <returns>The sine value.</returns>g
		/// <param name="content">Content.</param>
		public static string getSignEncryptValue(string content)
		{
			string result = "";
			string path = XYEncryptionTool.getRootFilePath();
			string certPath = path + "person_rsa_private_key.p12";  //证书已上传到对应目录
			string password = "12345678";  //证书密码

			X509Certificate2 cert2 = new X509Certificate2(certPath, password);
			RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert2.PrivateKey;

			result = sign(content, rsa);
		
			return result;
		}

		public static string sign(string content, RSACryptoServiceProvider rsa)
		{
			RSACryptoServiceProvider crsa = rsa;
			              
			byte[] Data = Encoding.UTF8.GetBytes(content);
			string test = System.Text.Encoding.UTF8.GetString(Data);
			byte[] signData = crsa.SignData(Data, "sha1");

			return Convert.ToBase64String(signData);

		}


		public static byte[] getDecryptRsaValue(byte[] contentByte)
		{
			string path = XYEncryptionTool.getRootFilePath();
			string certPath = path + "person_rsa_private_key.p12";  //证书已上传到对应目录
			string password = "12345678";  //证书密码

			X509Certificate2 cert2 = new X509Certificate2(certPath, password);
			RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert2.PrivateKey;

			byte[] resultData = rsa.Decrypt(contentByte, false);

			return resultData;
		}


		public static string getPublicKey()
		{
			string path = XYEncryptionTool.getRootFilePath();
			X509Certificate2 c2 = GetCertFromCerFile(path + "system_rsa_public_key.der");
			string keyPublic2 = c2.PublicKey.Key.ToXmlString(false);
			return keyPublic2;
		}

		/// <summary>
		/// 对16位随机数 加密
		/// </summary>
		/// <returns>The encrypt key.</returns>
		/// <param name="key">Key.</param>

		public static string getEncryptKey(string key)
		{
			string path = XYEncryptionTool.getRootFilePath();
			X509Certificate2 c2 = GetCertFromCerFile(path + "system_rsa_public_key.der");
			string keyPublic2 = c2.PublicKey.Key.ToXmlString(false);
			string cypher2 = RSAEncrypt(keyPublic2, key);  // 加密  
			return cypher2;
		}


		/// <summary>     
		/// RSA加密     
		/// </summary>     
		/// <param name="xmlPublicKey"></param>     
		/// <param name="m_strEncryptString"></param>     
		/// <returns></returns>     
		static string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
		{
			RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
			provider.FromXmlString(xmlPublicKey);
			//byte[] bytes = new UnicodeEncoding().GetBytes(m_strEncryptString);
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(m_strEncryptString);
			byte[] encryptData = provider.Encrypt(bytes, false);
			//Convert.
			return Convert.ToBase64String(provider.Encrypt(bytes, false));
		}


		public string getDecryptPublicKey(string content)
		{
			string path = XYEncryptionTool.getRootFilePath();
			X509Certificate2 c2 = GetCertFromCerFile(path + "system_rsa_public_key.der");
			string keyPublic2 = c2.PublicKey.Key.ToXmlString(false);
			string cypher2 = RSADecrypt(keyPublic2, content);  // 加密  
			return cypher2;
		}
		static string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
		{
			RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
			provider.FromXmlString(xmlPrivateKey);
			byte[] rgb = Convert.FromBase64String(m_strDecryptString);
			byte[] bytes = provider.Decrypt(rgb, false);
			return new UTF8Encoding().GetString(bytes);
		}


		/// <summary>     
		/// 根据公钥证书，返回证书实体     
		/// </summary>     
		/// <param name="cerPath"></param>     
		public static X509Certificate2 GetCertFromCerFile(string cerPath)
		{
			try
			{
				return new X509Certificate2(cerPath);
			}
			catch (Exception e)
			{
				throw e;
			}
		}


	}

}

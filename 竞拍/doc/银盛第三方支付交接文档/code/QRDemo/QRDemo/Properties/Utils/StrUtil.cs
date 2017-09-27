using System;
namespace QRDemo
{
	public class StrUtil
	{
		private const string random_src = "0123456789abcdefghijklmnopqrstuvwrxyz";
		private const string rnd_num_src = "0123456789";
		public const string req_url = "http://119.23.69.24:8170/BusiM/AL001/Check";



		//测试环境地址： http://119.23.69.24:8170/BusiM/AL001/Check
		//正式环境地址： http://39.108.10.122:8170/BusiM/AL001/Check

		public static string getRndStr(int length)
		{
			string result = "";
			int size = random_src.Length;
			Random rd = new Random();
			for (int i = 0; i < length; i++)
			{
				int randomInt = rd.Next(0, size);
				result += random_src.Substring(randomInt, 1);
			}

			return result;
		}

		public static string getRndNumStr(int length)
		{
			string result = "";
			int size = rnd_num_src.Length;
			Random rd = new Random();
			for (int i = 0; i < length; i++)
			{
				int randomInt = rd.Next(0, size);
				result += rnd_num_src.Substring(randomInt, 1);
			}
			return result;
		}

		public static string getSn()
		{
			return getRndStr(28);
		}

	}
}

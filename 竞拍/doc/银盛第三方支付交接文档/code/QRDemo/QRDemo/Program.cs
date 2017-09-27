using System;

namespace QRDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            AliH5Pay.sendPost();      //H5Demo（返回8001 请求成功）

            //QRPay.sendPost();	  	  //扫码Demo（返回500） （未开通）

            //AsynDemo.resolveData();  //异步回调DEMO（返回0000 请求成功）

            //QueryAccount.sendPost();    //查询余额demo（返回0000 请求成功）

            //Daifu.sendPost();       //代付demo（返回0000 请求成功）

            Console.ReadKey();
        }

    }
}

using EntityManagementService.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ResourceManagementService.entityResource
{
    /// <summary>
    /// 使用httpClient 来处理order资源
    /// </summary>
    public class OrderUtil
    {
        /// <summary>
        /// 已付款
        /// </summary>
        private static string API_1 = "admin/api/index.php/order/getOrderLists?uid=2988&ucode=88616770341";
        /// <summary>
        /// 已送达
        /// </summary>
        private static string API_2 = "admin/api/index.php/order/getArrivedOrderLists?uid=2988&ucode=88616770341";
        /// <summary>
        /// 修改订单
        /// </summary>
        private static string API_3 = "admin/api/index.php/order/updateOrder?uid=2988&ucode=88616770341";
        /// <summary>
        /// 更改订单状态
        /// </summary>
        private static string API_4 = "admin/api/index.php/order/updatestatus/2?uid=2988&ucode=88616770341";
        /// <summary>
        /// 发送短信
        /// </summary>
        private static string API_5 = "admin/api/index.php/order/updatestatus/3?uid=2988&ucode=88616770341";

        /// <summary>
        /// 获取发送短信失败的叮当
        /// </summary>
        private static string API_6 = "admin/api/index.php/order/getSMSfailedLists?uid=2988&ucode=88616770341";

        private static string BASE_ADDRESS = "http://www.wuyoutao.net/";

        /// <summary>
        /// 获取短信发送失败的订单
        /// </summary>
        /// <param name="callBack"></param>
        public static void getTextFailedOrder(Action<List<Order>> callBack)
        {
            getOrder(API_6, callBack);
        }


        /// <summary>
        /// 异步获取已付款订单
        /// </summary>
        /// <param name="callBack"></param>
        public static void getPayOrder( Action<List<Order>> callBack)
        {
            getOrder(API_1, callBack);
        }
        /// <summary>
        /// 异步获取已送达订单
        /// </summary>
        public static void getSendOrder(Action<List<Order>> callBack)
        {
            getOrder(API_2, callBack);
        }

        private static async void getOrder(string api,Action<List<Order>> callBack)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(api);
                response.Content.Headers.Remove("Content-Type");
                response.Content.Headers.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                if (response.IsSuccessStatusCode)
                {
                    JsonResultOrders result = await response.Content.ReadAsAsync<JsonResultOrders>();
                    if (callBack != null) {
                        callBack(result.data);//程序返回是调用回掉函数
                    }
                  
                }
            }
        }
        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="api"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        public static async void updateOrder(Order order, Action callBack) {
            //根据订单修改内容
            if (order == null || order.goods == null || order.goods.Count == 0) return;
            List<Good> goods = order.goods;
            string param = "&ocode=" + order.ocode; 
            for (int i = 0; i < goods.Count; i++)
			{
                param += string.Format("&gweight{0}={1}", i, goods.ElementAt(i).gpreweight);
			}
           
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(API_3+param);
                response.Content.Headers.Remove("Content-Type");
                response.Content.Headers.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                if (response.IsSuccessStatusCode)
                {
                    if (callBack != null)
                    {
                        callBack();//程序返回是调用回掉函数
                    }

                }
            }
        }
        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="order"></param>
        /// <param name="callBack"></param>
        public static void changeOrderStatus(Order order, Action callBack) {
            changeOrderState(API_4,order,callBack);
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="order"></param>
        /// <param name="callBack"></param>
        public static void sendOrderMsgText(Order order, Action callBack)
        {
            changeOrderState(API_5, order, callBack);
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="order"></param>
        /// <param name="callBack"></param>
        /// <returns></returns>
        private  static async void changeOrderState(string api,Order order, Action callBack)
        {
            //根据订单修改内容
            if (order == null ) return;
           
            string param = "&ocode=" + order.ocode;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(api + param);
                response.Content.Headers.Remove("Content-Type");
                response.Content.Headers.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                if (response.IsSuccessStatusCode)
                {
                    if (callBack != null)
                    {
                        callBack();//程序返回是调用回掉函数
                    }

                }
            }
        }

    }

    

}

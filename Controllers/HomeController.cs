using Ding.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Web.Security;
using System.Net;
using System.Runtime.Serialization;
//using System.Web.Http;
//using System.Web.Http.Cors;

namespace Ding
{
   // [RoutePrefix("api/dt")]
    public class HomeController : Controller
    {
        

        public ActionResult Index()
        {
           
            string url = Request.Url.ToString();
            var timeStamp = Helper.timeStamp();
            var nonceStr = Helper.randNonce();
            //var jsapi_ticke = GetJsapiTicket().ToString();

            string accessToken = EnterpriseBusiness.GetToken("ding6b3d79b9451858fd35c2f4657eb6378f", "Bs-htVAVqwf3Iw0-gsmV5HzdZ5MdjfXa_iJSmBXGdtgES77b1C6CS5F5BYpnDqPr").AccessToken;
            string jsapi_ticket = EnterpriseBusiness.GetTickets(accessToken);

            string string1 = "jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}";
            string1 = string.Format(string1, jsapi_ticket, nonceStr, timeStamp, url);
            Helper.WriteLog("signature not sha1:" + string1);

            string signature = FormsAuthentication.HashPasswordForStoringInConfigFile(string1, "SHA1").ToLower();
            Helper.WriteLog("signature sha1:" + signature);

            ViewData["timeStamp"] = timeStamp;
            ViewData["nonceStr"] = nonceStr;
            ViewData["signature"] = signature;

            return View();
        }

        DingTalkManager dtManager;
        public HomeController()
        {
            dtManager = new DingTalkManager();
        }
        //[Route("accessToken")]
        public async Task<string> AccessToken()
        {
            var result = await dtManager.GetAccessToken();

            return result;
        }

        //[Route("getJsapiTicket")]
        [HttpPost]
        public async Task<string> GetJsapiTicket()
        {
            return await dtManager.GetJsapiTicket(); 
        }

    }

    public /*static*/ class Helper
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="strMemo"></param>
        public static void WriteLog(string strMemo)
        {
            string directoryPath = HttpContext.Current.Server.MapPath(@"\Logs");
            string fileName = directoryPath + @"\log" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            StreamWriter sr = null;
            try
            {
                if (!File.Exists(fileName))
                {
                    sr = File.CreateText(fileName);
                }
                else
                {
                    sr = File.AppendText(fileName);
                }
                sr.WriteLine(DateTime.Now + ": " + strMemo);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }

        /// <summary>
        /// 返回一个八位的随机号，用于签名
        /// </summary>
        /// <returns></returns>
        public static string randNonce()
        {
            var result = "";
            var random = new Random((int)DateTime.Now.Ticks);
            const string textArray = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            for (var i = 0; i < 8; i++)
            {
                result = result + textArray.Substring(random.Next() % textArray.Length, 1);
            }

            return result;
        }

        /// <summary>
        /// 时间戳的随机数
        /// </summary>
        /// <returns></returns>
        public static string timeStamp()
        {
            DateTime dt1 = Convert.ToDateTime("1970-01-01 00:00:00");
            TimeSpan ts = DateTime.Now - dt1;
            return Math.Ceiling(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// state 随机数
        /// </summary>
        /// <returns></returns>
        public static string state()
        {
            Random ran = new Random();
            int RandKey = ran.Next(100, 999);
            return RandKey.ToString();
        }



    }

    public class EnterpriseBusiness
    {
        public EnterpriseBusiness()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 拿到企业令牌
        /// </summary>
        /// <param name="CorpId"></param>
        /// <param name="CorpSecret"></param>
        /// <returns></returns>
        public static DingTalkConfig GetToken(string CorpId, string CorpSecret)
        {
            string tagUrl = "https://oapi.dingtalk.com/gettoken?corpid=" + CorpId + "&corpsecret=" + CorpSecret;
            string result = HttpHelper.Get(tagUrl);

            var tokenModel = JsonConvert.DeserializeObject<DingTalkConfig>(result);
            return tokenModel;
        }

        /// <summary>
        /// 拿到当前登录的用户
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        //public static UserModel GetCurrentUser(string access_token, string code)
        //{
        //    string tagUrl = "https://oapi.dingtalk.com/user/getuserinfo?access_token=" + access_token + "&code=" + code;
        //    string result = HttpHelper.Get(tagUrl);

        //    var userModel = JsonConvert.DeserializeObject<UserModel>(result);
        //    return userModel;
        //}

        /// <summary>
        /// 拿到Tickets
        /// </summary>
        /// <param name="CorpId"></param>
        /// <param name="CorpSecret"></param>
        /// <returns></returns>
        public static string GetTickets(string AccessToken)
        {
            string url = "https://oapi.dingtalk.com/get_jsapi_ticket?access_token={0}&type=jsapi";
            url = string.Format(url, AccessToken);
            string result = HttpHelper.Get(url);
            var jsApiTicket = JsonConvert.DeserializeObject<JsApiTicket>(result);

            return jsApiTicket.ticket;
        }

    }

    /// 通过GET方式获取页面的方法
    public static class HttpHelper
    {
        /// <summary>
        /// 通过GET方式获取页面的方法
        /// </summary>
        /// <param name="urlString">请求的URL</param>
        /// <param name="encoding">页面编码</param>
        /// <returns></returns>
        public static string Get(string urlString)
        {
            //定义局部变量
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebRespones = null;
            Stream stream = null;
            string htmlString = string.Empty;

            //请求页面
            try
            {
                httpWebRequest = WebRequest.Create(urlString) as HttpWebRequest;
            }
            //处理异常
            catch (Exception ex)
            {
                throw new Exception("建立页面请求时发生错误！", ex);
            }
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";
            //获取服务器的返回信息
            try
            {
                httpWebRespones = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebRespones.GetResponseStream();
            }
            //处理异常
            catch (Exception ex)
            {
                throw new Exception("接受服务器返回页面时发生错误！", ex);
            }
            StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
            //读取返回页面
            try
            {
                htmlString = streamReader.ReadToEnd();
            }
            //处理异常
            catch (Exception ex)
            {
                throw new Exception("读取页面数据时发生错误！", ex);
            }
            //释放资源返回结果
            streamReader.Close();
            stream.Close();
            return htmlString;
        }


        /// <summary>
        /// 后台post事件
        /// 
        /// 调用方法：
        /// string url = "https://oapi.dingtalk.com/department/create?access_token=" + tokenstring;
        /// string param = "{\"access_token\":\"" + tokenstring + "\",\"name\":\"研发二部\",\"parentid\":\"1\",\"order\":\"2\",\"createDeptGroup\":\"false\"}";
        /// string callback = PostMoths(url, param);
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Post(string url, string param)
        {
            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "POST";
            request.ContentType = "application/json;charset=UTF-8";
            string paraUrlCoded = param;
            byte[] payload;
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(payload, 0, payload.Length);
            writer.Close();
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }

    }

    public class JsApiTicket
    {
        [DataMember(Order = 0)]
        public string errcode { get; set; }
        [DataMember(Order = 1)]
        public string errmsg { get; set; }
        [DataMember(Order = 2)]
        public string ticket { get; set; }
        [DataMember(Order = 3)]
        public int expires_in { get; set; }
    }
}




    




using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;

namespace Ding

{
    /// <summary>
    /// 提供与Web请求时可使用的工具类:包括Url解析、Url/Html编码、获取IP地址、返回http状态码
    /// </summary>
    public static class WebHelper
    {
        #region IPAddress

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns>返回获取的ip地址</returns>
        public static string GetIp()
        {
            return GetIp(HttpContext.Current);
        }

        /// <summary>
        /// 透过代理获取真实IP
        /// </summary>
        /// <param name="httpContext">HttpContext</param>
        /// <returns>返回获取的ip地址</returns>
        public static string GetIp(HttpContext httpContext)
        {
            string result = string.Empty;
            if (httpContext == null)
                return result;

            // 透过代理取真实IP
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;

            if (result == "::1")
                result = "127.0.0.1";

            return result;
        }

        #endregion
    }
}

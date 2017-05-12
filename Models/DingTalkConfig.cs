using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Ding.Models
{
    public class DingTalkConfig
    {
        public string CorpID { get; set; } = "ding6b3d79b9451858fd35c2f4657eb6378f";
        public string CorpSecret { get; set; } = "Bs-htVAVqwf3Iw0-gsmV5HzdZ5MdjfXa_iJSmBXGdtgES77b1C6CS5F5BYpnDqPr";
        public string AccessToken { get; set; } = ConfigurationManager.AppSettings["AccessToken"].ToString();
        public DateTime LastUpdateTime { get; set; }
    }
}
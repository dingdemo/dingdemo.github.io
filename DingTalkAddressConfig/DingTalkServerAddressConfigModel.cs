﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ding
{
    public partial class DingTalkServerAddressConfig
    {
        public string GetDepartmentListUrl { get; private set; }
        public string GetDepartmentDetailUrl { get; private set; }
        public string CreateDepartmentUrl { get; private set; }
        public string UpdateDepartmentUrl { get; private set; }
        public string DeleteDepartmentUrl { get; private set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi.Models
{
    public class ResponseTemp
    {
        public int temp { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string city_name { get; set; }
    }

}
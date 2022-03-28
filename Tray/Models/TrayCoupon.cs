﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoRockye.Tray.Models
{
    public class TrayCoupon
    {
        public string code { get; set; }
        public string discount { get; set; }

        public TrayCoupon()
        {
            code = "";
            discount = "0";
        }
    }
}

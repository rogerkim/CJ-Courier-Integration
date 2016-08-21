using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MangoCarrierSystem.Models
{
    public class OrderStatus
    {
        public enum Code : int
        {
            Error,
            New,
            Verified
        };
    }
}
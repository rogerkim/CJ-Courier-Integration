using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MangoCarrierSystem.Models
{
    public class MainViewModel
    {
        public string ID { get; set; }

        //Order
        public string BOX_NUMBER { get; set; }
        public string SHIPMENT_DATE { get; set; }
        public string ORDER { get; set; }
        public string FULL_NAME { get; set; }
        public string PHONE { get; set; }
        public string COUNTRY { get; set; }
        public string ZIPCODE { get; set; }
        public string LOCATION { get; set; }
        public string PROVINCE { get; set; }
        public string ADDRESS { get; set; }
        public string STATUS { get; set; }

        //Shipping Label
        public string RecevierZipcode { get; set; }
        public string ReceiverShortAddress1 { get; set; }
        public string ReceiverShortAddress2 { get; set; }
        public string AgencyName { get; set; }
        public string DriverName { get; set; }
        public string DriverCode { get; set; }
        public string MainTerminalCode { get; set; }
        public string SubTerminalCode { get; set; }
        public string TrackingNumber { get; set; }
    }
}
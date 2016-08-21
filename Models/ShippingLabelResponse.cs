using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MangoCarrierSystem.Models
{
    [DataContract]
    public class ShippingLabelResponse
    {
        [DataMember]
        public string RecevierZipcode { get; set; }
        [DataMember]
        public string ReceiverShortAddress1 { get; set; }
        [DataMember]
        public string ReceiverShortAddress2 { get; set; }
        [DataMember]
        public string AgencyName { get; set; }
        [DataMember]
        public string DriverName { get; set; }
        [DataMember]
        public string DriverCode { get; set; }
        [DataMember]
        public string MainTerminalCode { get; set; }
        [DataMember]
        public string SubTerminalCode { get; set; }
        [DataMember]
        public string TrackingNumber { get; set; }
    }
}
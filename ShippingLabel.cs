//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MangoCarrierSystem
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShippingLabel
    {
        public int ID { get; set; }
        public string TrackingNumber { get; set; }
        public string RecevierZipcode { get; set; }
        public string ReceiverShortAddress1 { get; set; }
        public string ReceiverShortAddress2 { get; set; }
        public string DriverName { get; set; }
        public string DriverCode { get; set; }
        public string MainTerminalCode { get; set; }
        public string SubTerminalCode { get; set; }
        public Nullable<int> OrderId { get; set; }
        public string AgencyName { get; set; }
    
        public virtual Order Order { get; set; }
    }
}

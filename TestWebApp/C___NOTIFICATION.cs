//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestWebApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class C___NOTIFICATION
    {
        public long ID { get; set; }
        public int APP_ID { get; set; }
        public System.DateTime INSERTED { get; set; }
        public Nullable<System.DateTime> LASTUPDATED { get; set; }
        public byte NOTIFICATION_TYPE_ID { get; set; }
        public string SUBJECT { get; set; }
        public string MESSAGE { get; set; }
        public byte STATUS { get; set; }
    
        public virtual C___APPLICATION C___APPLICATION { get; set; }
    }
}

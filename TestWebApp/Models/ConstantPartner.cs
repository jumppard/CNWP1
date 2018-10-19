using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class ConstantPartner
    {
        public int Id { get; set; }
        public string PartnerName { get; set; }
        public string ConstantName { get; set; }
        public string ConstantValue { get; set; }
        public DateTime Inserted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string ConstantDescription { get; set; }
        public int? PartnerId { get; internal set; }
    }
}
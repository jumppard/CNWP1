using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class Partner
    {
        public int ID { get; set; }
        public byte PartnerTypeID { get; set; }
        public string Code { get; set; }
        public string OrganizationName { get; set; }
        public int RwOrganizationId { get; set; }
        public DateTime CreationDateTime { get; set; }
    }
}
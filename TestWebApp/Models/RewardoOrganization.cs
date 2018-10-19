using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class RewardoOrganization
    {
        public short        OrganizationId      { get; set; }
        public string       Name                { get; set; }
        public string       LongName            { get; set; }
        public DateTime     InsertDate          { get; set; }
        public DateTime?    LastUpdateDate      { get; set; }
        public int          Status              { get; set; }
    }
}
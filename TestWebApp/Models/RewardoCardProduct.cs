using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class RewardoCardProduct
    {
        public short        CardProductID       { get; set; }
        public short        OrganizationID      { get; set; }
        public string       CardProductName     { get; set; }
        public Decimal      TransactionLimit    { get; set; }
        public Decimal      DailyLimit          { get; set; }
        public Decimal?     NextDailyLimit      { get; set; }
        public DateTime     InsertDate          { get; set; }
        public DateTime?    LastUpdateDate      { get; set; }
        public int          Status              { get; set; }
    }
}
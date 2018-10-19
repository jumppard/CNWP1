using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.ViewModel
{
    public class RewardoCardViewModel
    {
        public long? CardId { get; set; }
        public short OrganizationId { get; set; }
        public int MerchantId { get; set; }
        public long CardPoolId { get; set; }
        public short CardProductId { get; set; }
        public long CustomerId { get; set; }
        public long? CardCRMId { get; set; }
        public int? PackageId { get; set; }
        public int? OrderInPackage { get; set; }
        public string Cardno { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal? TransactionLimit { get; set; }
        public decimal? DailyLimit { get; set; }
        public decimal? NextDailyLimit { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int Status { get; set; }
    }
}
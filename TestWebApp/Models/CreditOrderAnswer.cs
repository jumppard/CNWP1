using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class CreditOrderAnswer
    {
        public long ID { get; set; }
        public string PartnerName { get; set; }
        public long? AnswerId { get; set; }
        public long? RequestId { get; set; }
        public long? TransactionId { get; set; }
        public DateTime? ChangeTime { get; set; }
        public string RequestStatus { get; set; }
        public string RequestMessage { get; set; }
        public decimal? TransactionFinal { get; set; }
        public decimal? TransactionBalance { get; set; }
    }
}
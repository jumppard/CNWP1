using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class CardTransferAnswer
    {
        public long ID { get; set; }
        public int? PartnerId { get; set; }
        public long? AnswerId { get; set; }
        public long? RequestId { get; set; }
        public string RequestStatus { get; set; }
        public string Message { get; set; }
        public long? AuthorizationIdFromCard { get; set; }
        public long? AuthorizationIdToCard { get; set; }
        public string RequestMessage { get; set; }
        public DateTime? ChangeTime { get; set; }
        public Decimal? CurrentBalanceFromCard { get; set; }
        public Decimal? CurrentBalanceToCard { get; set; }
    }
}
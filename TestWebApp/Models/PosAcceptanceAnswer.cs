using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    public class PosAcceptanceAnswer
    {
        [Key]
        public long ID { get; set; }
        public string PartnerName { get; set; }
        public long? AnswerId { get; set; }
        public long? RequestId { get; set; }
        public string RequestStatus { get; set; }
        public string RequestMessage { get; set; }
        public string StatusAfterRequest { get; set; }
        public DateTime? InsertDateTime { get; set; }
    }
}
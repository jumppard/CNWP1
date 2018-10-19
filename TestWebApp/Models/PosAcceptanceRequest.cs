using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace TestWebApp.Models
{
    public class PosAcceptanceRequest
    {
        [Key]
        public long ID { get; set; }
        public long? RequestId { get; set; }
        public string PartnerName { get; set; }
        public long? PartnerRequestId { get; set; }
        public int? TerminalTypeId { get; set; }
        public string ExternalTerminalId { get; set; }
        public string NewAcceptState { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public DateTime? AnswerDateTime { get; set; }
        public string RequestStatus { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountTypeIdNAME { get; set; }
    }
}
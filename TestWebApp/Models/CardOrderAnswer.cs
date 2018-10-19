using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    public class CardOrderAnswer
    {
        [Key]
        public long ID { get; set; }
        public int? PartnerId { get; set; }
        public long? AnswerId { get; set; }
        public long? RequestId { get; set; }
        public DateTime? InsertDateTime { get; set; }
        public DateTime? ChangeDateTime { get; set; }
        public int? RequestStatus { get; set; }
        public string Message { get; set; }
        public string Additional_column1 { get; set; }
    }
}
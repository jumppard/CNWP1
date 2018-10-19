using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.ComponentModel.DataAnnotations;


namespace TestWebApp.Models
{
    public class CardOrderRequest
    {
        [Key]
        public long ID { get; set; }
        public long? RequestID { get; set; }
        public int? PartnerId { get; set; }
        public string PartnerRequestId { get; set; }
        public short? CardTypeId { get; set; }
        public string Content { get; set; }
        public int? CardCount { get; set; }

        public string CarrierTypeId { get; set; }
        public string EnvelopeTypeId { get; set; }
        public string PlasticTypeId { get; set; }
        public string AttachementId { get; set; }
        public string PackageAttachementId { get; set; }


        public string ValidTo { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? RequestStatus { get; set; }
        public bool? StatusFinal { get; set; }
        public string SlaDeadLine { get; set; }
        public bool? IsProcessing { get; set; }

        public string OrderType { get; set; } // {"cancel","normal"}
        public string DeliveryListId { get; set; }

        // TODO ostatne properties
    }
}
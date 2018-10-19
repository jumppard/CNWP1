using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TestWebApp.Models;

namespace TestWebApp.ViewModel
{
    public class CardOrderViewModel
    {
        // REQ002 columns
        public long ID { get; set; }
        public long? RequestID { get; set; }
        public int? PartnerId { get; set; }
        public string PartnerName { get; set; } // TODO
        public string PartnerRequestId { get; set; }
        public short? CardTypeId { get; set; }
        public string CardTypeName { get; set; } // TODO
        public string Content { get; set; }
        public int? CardCount { get; set; }
        public string ValidTo { get; set; }
        public DateTime? CreationDateTime { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? RequestStatus { get; set; }
        public string RequestStatusName { get; set; } // TODO
        public bool? StatusFinal { get; set; }
        public string SlaDeadLine { get; set; }

        public string CarrierTypeId { get; set; }
        public string EnvelopeTypeId { get; set; }
        public string PlasticTypeId { get; set; }
        public string AttachementId { get; set; }
        public string PackageAttachementId { get; set; }

        // ANS002 columns
        public string Message { get; set; }

        // others info
        // public string OpenOrderButton = "Open"; // button
        public string RowColor { get; set; }
        public bool TriggerOnLoad = false;
        public string TriggerOnLoadMessage = "";

        public string ValidStatusDate { get; set; }
        public string DeclinedStatusDate { get; set; }
        public string ReceivedStatusDate { get; set; }
        public string SentStatusDate { get; set; }
        public string CanceledStatusDate { get; set; }
        public string CancelDeclinedStatusDate { get; set; }
        public bool? isProcessing { get; set; }

        public string OrderType { get; set; } // {"cancel","normal"}
        public string DeliveryListId { get; set; }

        public string CarrierTypeIdConstantValue { get; set; }
        public string EnvelopeTypeIdConstantValue { get; set; }
        public string PlasticTypeIdConstantValue { get; set; }
        public string AttachementIdConstantValue { get; set; }
        public string PackageAttachementIdConstantValue { get; set; }
        public string DeliveryListIdConstantValue { get; set; }

        public CardOrderDetailsPhotoGallery CardOrderDetailsPhotoGallery = new CardOrderDetailsPhotoGallery();

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.ViewModel
{
    public class CardTransferViewModel
    {
        // REQ008 columns
        public long         ID                      { get; set; }
        public long?        RequestId               { get; set; }
        public int?         PartnerId               { get; set; }
        public string       PartnerName             { get; set; }
        public string       PartnerRequestId        { get; set; }
        public string       TransferType            { get; set; }
        public string       Content                 { get; set; }
        public string       TransferCase            { get; set; }
        public string       BalanceTransfer         { get; set; }
        public string       BonusTransfer           { get; set; }
        public string       CardnoFromFinalStatus   { get; set; }
        public long?        CardnoFrom              { get; set; }
        public long?        CardnoTo                { get; set; }
        public DateTime?    CreationDateTime        { get; set; }
        public DateTime?    LastUpdateDate          { get; set; }
        public string       RequestStatus           { get; set; }
        public byte?        Succesfull              { get; set; }
        public bool?        IsProcessing            { get; set; }
        public DateTime?    SlaDeadline             { get; set; }
        public int          AccountTypeId           { get; set; }
        public short?       AccountTypefinalStatus  { get; set; }
        public string       AccountTypeIdName       { get; set; }


        // ANS004 columns
        public string       Message             { get; set; }

        // others info
        // public string OpenOrderButton = "Open"; // button
        public string       RowColor            { get; set; }
    }
}
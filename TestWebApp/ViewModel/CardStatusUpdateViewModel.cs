using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.ViewModel
{
    public class CardStatusUpdateViewModel
    {
        // REQ004 columns
        public long             ID                  { get; set; }
        public string           PartnerName         { get; set; }
        public long?            RequestId           { get; set; }
        public long?            PartnerRequestId    { get; set; }
        public long?            ExternalCardId      { get; set; }
        public string           RequestedStateId    { get; set; }
        public decimal?         DailyLimit          { get; set; }
        public decimal?         TransactionLimit    { get; set; }
        public DateTime?        CreationDateTime    { get; set; }
        public DateTime?        AnswerDateTime      { get; set; }
        public string           RequestStatus       { get; set; }
        public int              AccountTypeId       { get; set; }
        public string           AccountTypeIdName   { get; set; }

        // ANS004 columns
        public string           Message             { get; set; }

        // others info
        // public string OpenOrderButton = "Open"; // button
        public string           RowColor            { get; set; }
    }
}
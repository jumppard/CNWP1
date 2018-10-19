using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.ViewModel
{
    public class PosAcceptanceViewModel
    {
        // REQ001 columns
        public long         ID                  { get; set; }
        public long?        RequestId           { get; set; }
        public string       PartnerName           { get; set; }
        public long?        PartnerRequestId    { get; set; }
        public int?         TerminalTypeId      { get; set; }
        public string       ExternalTerminalId  { get; set; }
        public string       NewAcceptState      { get; set; }
        public DateTime?    CreationDateTime    { get; set; }
        public DateTime?    AnswerDateTime      { get; set; }
        public string       RequestStatus       { get; set; }
        public string       StatusAfterRequest  { get; set; }
        public int          AccountTypeId       { get; set; }
        public string       AccountTypeIdName   { get; set; }

        // ANS001 columns
        public string       Message             { get; set; }

        // others info
        // public string OpenOrderButton = "Open"; // button
        public string       RowColor            { get; set; }
    }
}
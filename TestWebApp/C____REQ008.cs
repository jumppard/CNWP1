//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestWebApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class C____REQ008
    {
        public long ID { get; set; }
        public Nullable<long> REQUEST_ID { get; set; }
        public Nullable<int> PARTNER_ID { get; set; }
        public string PARTNER_REQUEST_ID { get; set; }
        public short TRANSFER_TYPE { get; set; }
        public string CONTENT { get; set; }
        public Nullable<short> TRANSFER_CASE { get; set; }
        public Nullable<bool> BALANCE_TRANSFER { get; set; }
        public Nullable<bool> BONUS_TRANSFER { get; set; }
        public Nullable<short> CARDNO_FROM_FINAL_STATUS { get; set; }
        public Nullable<long> CARDNO_FROM { get; set; }
        public Nullable<long> CARDNO_TO { get; set; }
        public Nullable<System.DateTime> CREATION_DATETIME { get; set; }
        public Nullable<System.DateTime> LAST_UPDATE { get; set; }
        public Nullable<byte> REQUEST_STATUS { get; set; }
        public byte SUCCESSFULL { get; set; }
        public Nullable<bool> IS_PROCESSING { get; set; }
        public Nullable<System.DateTime> SLA_DEADLINE { get; set; }
        public int ACCOUNT_TYPE_ID { get; set; }
        public Nullable<short> ACCOUNT_TYPE_FINAL_STATUS { get; set; }
    }
}
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
    
    public partial class CONSTANT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CONSTANT()
        {
            this.CALLCENTER = new HashSet<CALLCENTER>();
            this.CALLCENTER1 = new HashSet<CALLCENTER>();
            this.CALLCENTER2 = new HashSet<CALLCENTER>();
            this.CALLCENTER3 = new HashSet<CALLCENTER>();
            this.CALLCENTER4 = new HashSet<CALLCENTER>();
            this.CALLCENTER5 = new HashSet<CALLCENTER>();
            this.CALLCENTER6 = new HashSet<CALLCENTER>();
            this.CAMPAIGN = new HashSet<CAMPAIGN>();
            this.CAMPAIGN1 = new HashSet<CAMPAIGN>();
            this.CAMPAIGN2 = new HashSet<CAMPAIGN>();
            this.CAMPAIGN3 = new HashSet<CAMPAIGN>();
            this.CAMPAIGN4 = new HashSet<CAMPAIGN>();
            this.CAMPAIGNCUSTOMER = new HashSet<CAMPAIGNCUSTOMER>();
            this.CAMPAIGNMESSAGE = new HashSet<CAMPAIGNMESSAGE>();
            this.CARD_TRANSFER = new HashSet<CARD_TRANSFER>();
            this.CARD_TRANSFER1 = new HashSet<CARD_TRANSFER>();
            this.CARD_TRANSFER2 = new HashSet<CARD_TRANSFER>();
            this.CARD_TRANSFER3 = new HashSet<CARD_TRANSFER>();
            this.CARD_TRANSFER4 = new HashSet<CARD_TRANSFER>();
            this.CORPORATECUSTOMER = new HashSet<CORPORATECUSTOMER>();
            this.CUSTOMERPRIVATECHILDINFO = new HashSet<CUSTOMERPRIVATECHILDINFO>();
            this.MERCHANT = new HashSet<MERCHANT>();
            this.MERCHANT1 = new HashSet<MERCHANT>();
            this.MERCHANT2 = new HashSet<MERCHANT>();
            this.MERCHANT_CARDTYPE_PERMISSION = new HashSet<MERCHANT_CARDTYPE_PERMISSION>();
            this.MERCHANT_CARDTYPE_PERMISSION1 = new HashSet<MERCHANT_CARDTYPE_PERMISSION>();
            this.MERCHANT_CARDTYPE_PERMISSION2 = new HashSet<MERCHANT_CARDTYPE_PERMISSION>();
            this.MERCHANT_CARDTYPE_PERMISSION3 = new HashSet<MERCHANT_CARDTYPE_PERMISSION>();
            this.MERCHANT_CARDTYPE_PERMISSION4 = new HashSet<MERCHANT_CARDTYPE_PERMISSION>();
            this.MERCHANT_CARDTYPE_PERMISSION5 = new HashSet<MERCHANT_CARDTYPE_PERMISSION>();
            this.MERCHANT_CARDTYPE_PERMISSION6 = new HashSet<MERCHANT_CARDTYPE_PERMISSION>();
            this.USERTEMPLATE = new HashSet<USERTEMPLATE>();
            this.USERTEMPLATE_PAGE_MAP = new HashSet<USERTEMPLATE_PAGE_MAP>();
            this.USERTEMPLATE1 = new HashSet<USERTEMPLATE>();
        }
    
        public int CONSTANT_ID { get; set; }
        public string CONSTANTNAME { get; set; }
        public string CONSTANTVALUE { get; set; }
        public string CONSTANTDISPLAY { get; set; }
        public Nullable<int> CONSTANTLANGUAGE { get; set; }
        public Nullable<short> DISPLAYORDER { get; set; }
        public System.DateTime INSERTDATE { get; set; }
        public int INSERTUSER_ID { get; set; }
        public Nullable<System.DateTime> LASTUPDATEDATE { get; set; }
        public Nullable<int> LASTUPDATEUSER_ID { get; set; }
        public int STATUS { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CALLCENTER> CALLCENTER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CALLCENTER> CALLCENTER1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CALLCENTER> CALLCENTER2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CALLCENTER> CALLCENTER3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CALLCENTER> CALLCENTER4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CALLCENTER> CALLCENTER5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CALLCENTER> CALLCENTER6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAMPAIGN> CAMPAIGN { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAMPAIGN> CAMPAIGN1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAMPAIGN> CAMPAIGN2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAMPAIGN> CAMPAIGN3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAMPAIGN> CAMPAIGN4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAMPAIGNCUSTOMER> CAMPAIGNCUSTOMER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAMPAIGNMESSAGE> CAMPAIGNMESSAGE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CARD_TRANSFER> CARD_TRANSFER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CARD_TRANSFER> CARD_TRANSFER1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CARD_TRANSFER> CARD_TRANSFER2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CARD_TRANSFER> CARD_TRANSFER3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CARD_TRANSFER> CARD_TRANSFER4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CORPORATECUSTOMER> CORPORATECUSTOMER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CUSTOMERPRIVATECHILDINFO> CUSTOMERPRIVATECHILDINFO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MERCHANT> MERCHANT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MERCHANT> MERCHANT1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MERCHANT> MERCHANT2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MERCHANT_CARDTYPE_PERMISSION> MERCHANT_CARDTYPE_PERMISSION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MERCHANT_CARDTYPE_PERMISSION> MERCHANT_CARDTYPE_PERMISSION1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MERCHANT_CARDTYPE_PERMISSION> MERCHANT_CARDTYPE_PERMISSION2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MERCHANT_CARDTYPE_PERMISSION> MERCHANT_CARDTYPE_PERMISSION3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MERCHANT_CARDTYPE_PERMISSION> MERCHANT_CARDTYPE_PERMISSION4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MERCHANT_CARDTYPE_PERMISSION> MERCHANT_CARDTYPE_PERMISSION5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MERCHANT_CARDTYPE_PERMISSION> MERCHANT_CARDTYPE_PERMISSION6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USERTEMPLATE> USERTEMPLATE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USERTEMPLATE_PAGE_MAP> USERTEMPLATE_PAGE_MAP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USERTEMPLATE> USERTEMPLATE1 { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class ConstantUniversal
    {
        [Key]
        public int Id { get; set; }
        public string ConstantName { get; set; }
        public int ConstantValue { get; set; }
        public string ConstantDescription { get; set; }
        public DateTime InsertDateTime { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string AdditionalValue { get; set; }
    }
}
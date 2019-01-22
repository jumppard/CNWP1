using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    public class Application
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public Boolean ByPartner { get; set; }
        public int? PartnerId { get; set; }
        public Boolean Status { get; set; }
        public DateTime? LastUpdated { get; set; }
        public Boolean IsPeriodicallyExecuted { get; set; }
        public int? TimeInterval { get; set; }
        public int? TolerancePeriod { get; set; }
        public DateTime Inserted { get; set; }
    }
}
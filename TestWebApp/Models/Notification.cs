using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestWebApp.Models
{
    public class Notification
    {
        [Key]
        public long ID { get; set; }
        public int AppId { get; set; }
        public DateTime Inserted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public byte NotificationTypeId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public byte Status { get; set; }
    }
}
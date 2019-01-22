using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.ViewModel
{
    public class NotificationViewModel
    {
        public long ID { get; set; }
        public int AppId { get; set; }
        public string PartnerName { get; set; }
        public DateTime Inserted { get; set; }
        public DateTime? LastUpdated { get; set; }
        public byte NotificationTypeId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public byte Status { get; set; }

        public string AppName { get; set; }
        public string NotificationTypeName { get; set; }
        public string StatusInfo { get; set; }
    }
}
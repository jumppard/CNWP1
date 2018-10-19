using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.ViewModel
{
    public class RewardoCardListViewModel
    {
        public List<RewardoCardViewModel> rclvm = new List<RewardoCardViewModel>(); // rewardo cards list view model (rclvm)
        public string info { get; set; } // if all XML cards are already in Rewardo => info = "Everything's OK" else "Uncompleted order in Rewardo..."
        public string infoColor { get; set; }
        public uint? packageId { get; set; }
        public long? orderId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class RewardoConstant
    {
        public int ConstantId { get; set; }
        public string ConstantName { get; set; }
        public string ConstantValue { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int Status { get; set; }
    }
}
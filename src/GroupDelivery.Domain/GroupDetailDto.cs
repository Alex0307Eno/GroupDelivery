using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    public class GroupDetailDto
    {
        public int GroupId { get; set; }
        public string StoreName { get; set; }

        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime Deadline { get; set; }

        public List<string> MenuImages { get; set; }
    }
}

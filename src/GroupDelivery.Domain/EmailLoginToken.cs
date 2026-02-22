using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupDelivery.Domain
{
    [Table("tbEmailLoginToken")]
    public class EmailLoginToken
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [StringLength(64)]
        public string Token { get; set; }

        public DateTime ExpireAt { get; set; }
        public DateTime? UsedAt { get; set; }
    }

}

using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALEF.Models
{
    public partial class TblProduct
    {
        public int product_id { get; set; }

        public string product_name { get; set; } = null!;

        public int category_id { get; set; }

        public decimal price { get; set; }

        public int quantity { get; set; }

        public int user_id { get; set; }

        public virtual TblCategory Category { get; set; } = null!;
        public virtual TblUser User { get; set; } = null!;
    }

}

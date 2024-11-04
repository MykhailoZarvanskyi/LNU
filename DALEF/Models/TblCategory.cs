using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALEF.Models
{
    public partial class TblCategory
    {
        public int category_id { get; set; }

        public string category_name { get; set; } 

        public string? category_description { get; set; }

        public virtual ICollection<TblProduct> Products { get; set; } = new List<TblProduct>();
    }

}

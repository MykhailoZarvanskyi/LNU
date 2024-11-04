using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALEF.Models
{
    public partial class TblUser
    {
        public int user_id { get; set; }

        public string user_name { get; set; } = null!;

        public string user_password { get; set; } = null!;

        public string role { get; set; } = null!;

        public virtual ICollection<TblProduct> Products { get; set; } = new List<TblProduct>();
    }

}

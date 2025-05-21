using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX_copy.Data.Entities
{
    public class ItemImage
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int? Order { get; set; }
    }
}

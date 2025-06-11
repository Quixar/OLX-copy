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

        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }

        public Guid? ProductGroupId { get; set; }
        public ProductGroup? ProductGroup { get; set; }

        public string ImageUrl { get; set; } = null!;
        public int? Order { get; set; }
    }
}

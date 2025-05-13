using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX_copy.Data.Entities
{
    public class ProductGroup
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime? DeletedAt { get; set; }


        public ProductGroup? ParentGroup { get; set; }
        public List<Product> Products { get; set; } = [];
        public List<ItemImage> Images { get; set; } = [];
    }
}

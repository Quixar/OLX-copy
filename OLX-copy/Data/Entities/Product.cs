using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX_copy.Data.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public string? ImageUrl { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }
        public DateTime? DeletedAt { get; set; }


        public ProductGroup ProductGroup { get; set; } = null!;
        public List<ItemImage> Images { get; set; } = [];

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}

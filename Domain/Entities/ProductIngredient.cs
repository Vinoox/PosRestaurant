using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductIngredient
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }


        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; }

        public int Quantity { get; set; }
    }
}

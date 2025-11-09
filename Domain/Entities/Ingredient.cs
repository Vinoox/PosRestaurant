using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Ingredient : AuditableEntity
    {
        public string Name { get; set; }
        public Unit Unit {get; set;}
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }

        public Ingredient() { }
    }
}

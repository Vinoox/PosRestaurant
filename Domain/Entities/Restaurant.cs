using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;

namespace Domain.Entities
{
    public class Restaurant : AuditableEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; private set; } = null!;

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<User> Staff { get; set; } = new List<User>();

        //public ICollection<Order> Orders { get; set; } = new List<Order>();

        public Restaurant()
        {
        }
    }
}

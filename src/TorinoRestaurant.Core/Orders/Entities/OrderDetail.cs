using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Products.Entities;

namespace TorinoRestaurant.Core.Orders.Entities
{
    public class OrderDetail : AggregateRoot
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerQuantity { get; set; }
        public decimal Total { get; set; }
        public string Note { get; set; }
        public virtual Order Order{ get; set; }
        public virtual Product Product{ get; set; }
    }
}
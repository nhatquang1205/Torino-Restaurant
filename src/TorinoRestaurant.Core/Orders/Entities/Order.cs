using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Orders.DomainEvents;
using TorinoRestaurant.Core.Orders.ValuesObject;

namespace TorinoRestaurant.Core.Orders.Entities
{
    public sealed class Order : AggregateRoot
    {
        private Order(string code, int tableHistoryId, decimal totalPrice, bool isExportTax, string taxInformation, PaymentMethod paymentMethod, OrderStatus orderStatus)
        {
            Code = code;
            TableHistoryId = tableHistoryId;
            TotalPrice = totalPrice;
            IsExportTax = isExportTax;
            TaxInformation = taxInformation;
            PaymentMethod = paymentMethod;
            OrderStatus = orderStatus;
            OrderDetails = [];
        }

        private Order()
        {
            OrderDetails = [];
        }

        public static Order Create(string code, int tableHistoryId, decimal totalPrice, bool isExportTax, string taxInformation, PaymentMethod paymentMethod, OrderStatus orderStatus)
        {
            // validation should go here before the aggregate is created
            // an aggregate should never be in an invalid state
            var order = new Order(code, tableHistoryId, totalPrice, isExportTax, taxInformation, paymentMethod, orderStatus);
            order.PublishCreated();
            return order;
        }

        private void PublishCreated()
        {
            AddDomainEvent(new OrderCreatedDomainEvent(Id, Code, TotalPrice));
        }

        public string Code { get; set; }
        public long TableHistoryId { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsExportTax { get; set; }
        public string TaxInformation { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DineInTableHistory TableHistory { get; set; }
        public ICollection<OrderDetail> OrderDetails{ get; set; }
    }
}
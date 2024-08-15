using CSharpFunctionalExtensions;
using TorinoRestaurant.Core.Abstractions.Exceptions;
using TorinoRestaurant.Core.Abstractions.Guards;

namespace TorinoRestaurant.Core.Orders.ValuesObject
{
    public sealed class OrderStatus : ValueObject
    {
        private OrderStatus(int status)
        {
            Status = status;
        }
        public static OrderStatus Create(int status)
        {
            Guard.Against.ValueOutOfRange(status, 1, 3, message: "Order status must be from 1 to 4");
            return new OrderStatus(status);
        }
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Status;
        }
        public string GetOrderStatusName()
        {
            return Status switch
            {
                1 => "Đang hoạt động",
                2 => "Đã in bill",
                3 => "Hoàn thành",
                4 => "Huỷ",
                _ => throw new DomainException("Invalid Order Status"),
            };
        }
        public int Status { get; set; }
    }
}
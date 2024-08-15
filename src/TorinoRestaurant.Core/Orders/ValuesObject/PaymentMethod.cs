using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using TorinoRestaurant.Core.Abstractions.Exceptions;
using TorinoRestaurant.Core.Abstractions.Guards;

namespace TorinoRestaurant.Core.Orders.ValuesObject
{
    public sealed class PaymentMethod : ValueObject
    {
        private PaymentMethod(int type)
        {
            Type = type;
        }
        public static PaymentMethod Create(int type)
        {
            Guard.Against.ValueOutOfRange(type, 1, 3, message: "Type must be from 1 to 3");
            return new PaymentMethod(type);
        }
        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Type;
        }
        public string GetPaymentMethodName()
        {
            return Type switch
            {
                1 => "Tiền mặt",
                2 => "Chuyển khoản",
                3 => "VISA",
                _ => throw new DomainException("Invalid Payment Method Type"),
            };
        }
        public int Type { get; set; }
    }
}
using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Orders.DomainEvents;
using TorinoRestaurant.Core.Users.Entities;

namespace TorinoRestaurant.Core.Orders.Entities
{
    public class DineInTableHistory : AggregateRoot
    {
        private DineInTableHistory(long tableId, DateTimeOffset timeStart, long staffId, long? customerId)
        {
            DineInTableId = tableId;
            TimeStart = timeStart;
            StaffId = staffId;
            CustomerId = customerId;
        }

        private DineInTableHistory()
        {
        }

        public static DineInTableHistory Create(long tableId, DateTimeOffset timeStart, long staffId, long? customerId)
        {
            // validation should go here before the aggregate is created
            // an aggregate should never be in an invalid state
            var dineInTableHistory = new DineInTableHistory(tableId, timeStart, staffId, customerId);
            dineInTableHistory.PublishCreated();
            return dineInTableHistory;
        }

        private void PublishCreated()
        {
            AddDomainEvent(new DineInTableHistoryCreatedDomainEvent(Id, DineInTableId, TimeStart, StaffId, CustomerId));
        }

        public void SetTimeEnd(DateTimeOffset timeEnd)
        {
            TimeEnd = timeEnd;
        }

        public long DineInTableId { get; set; }
        public DateTimeOffset TimeStart { get; set; }
        public DateTimeOffset? TimeEnd { get; set; }
        public long StaffId { get; set; }
        public long? CustomerId { get; set; }
        public virtual User Staff { get; set; }
        public virtual User Customer { get; set; }
        public virtual DineInTable DineInTable { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
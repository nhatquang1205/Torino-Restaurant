using TorinoRestaurant.Core.Abstractions.Entities;
using TorinoRestaurant.Core.Orders.DomainEvents;

namespace TorinoRestaurant.Core.Orders.Entities
{
    public class DineInTable : AggregateRoot
    {
        private DineInTable(string name, int sort)
        {
            Name = name;
            Sort = sort;
            IsActive = true;
            Histories = [];
        }

        private DineInTable()
        {
        }

        public static DineInTable Create(string name, int sort)
        {
            // validation should go here before the aggregate is created
            // an aggregate should never be in an invalid state
            var dineInTable = new DineInTable(name, sort);
            dineInTable.PublishCreated();
            return dineInTable;
        }

        private void PublishCreated()
        {
            AddDomainEvent(new DineInTableCreatedDomainEvent(Id, Name, Sort));
        }

        public string Name { get; set; }
        public int Sort { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<DineInTableHistory> Histories { get; set; }
    }
}
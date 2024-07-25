namespace TorinoRestaurant.Core.Abstractions.Entities
{
    public abstract class AggregateRoot : AggregateRoot<long> {}

    public abstract class AggregateRoot<T> : EntityBase<T>, IAuditableEntity
    {
        public DateTime Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }
    }

}
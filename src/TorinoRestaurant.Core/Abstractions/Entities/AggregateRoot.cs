namespace TorinoRestaurant.Core.Abstractions.Entities
{
    public abstract class AggregateRoot : AggregateRoot<long> {}

    public abstract class AggregateRoot<T> : EntityBase<T>, IAuditableEntity, ISoftDelete
    {
        public DateTimeOffset Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTimeOffset? DeletedOn { get; set; }

        public string? DeletedBy { get; set; }

        public bool DelFlag { get; set; }
    }
}
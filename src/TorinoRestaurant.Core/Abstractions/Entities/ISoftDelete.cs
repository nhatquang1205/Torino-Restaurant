namespace TorinoRestaurant.Core.Abstractions.Entities;

public interface ISoftDelete
{
    DateTime? DeletedOn { get; set; }
    string? DeletedBy { get; set; }
}
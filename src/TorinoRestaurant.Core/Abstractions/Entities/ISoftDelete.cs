namespace TorinoRestaurant.Core.Abstractions.Entities;

public interface ISoftDelete
{
    bool DelFlag { get; set; }
    DateTimeOffset? DeletedOn { get; set; }
    string? DeletedBy { get; set; }
}
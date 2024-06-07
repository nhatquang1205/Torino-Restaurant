using TorinoRestaurant.Application.Abstractions.Services;

namespace TorinoRestaurant.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}

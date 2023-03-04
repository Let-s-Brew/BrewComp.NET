using NodaTime;

namespace BrewComp.Data;

public class Competition
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public DateInterval CompetitionDates { get; set; }
    public DateInterval DropOffDates { get; set; }
    public DateInterval ShippingDates { get; set; }
    public DateInterval RegistrationDates { get; set; }

}


using NodaTime;

namespace BrewComp.Data;

public class Competition
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public Interval CompetitionDates { get; set; }
    public Interval DropOffDates { get; set; }
    public Interval ShippingDates { get; set; }
    public Interval RegistrationDates { get; set; }
    public Interval EntryRegistrationDates { get; set; }


}


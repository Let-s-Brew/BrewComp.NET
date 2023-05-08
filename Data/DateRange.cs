using System.Text.Json.Serialization;

namespace BrewComp.Data;

/// <summary>
/// Represents a range between two dates. If the <c>endDate</c> is <c>null</c> then it is assumed to be an open ended range (until infinity... and beyond?)
/// Recommend to store all times in UTC and then convert at time of display.
/// </summary>
public record struct DateRange
{
    public static readonly string DefaultFormat = "yyyy/MM/dd HH:mmK";

    public DateTime StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    /// <summary>
    /// Constructor for a DateRange. Can be open ended by passing <c>null</c> for <c>endDate</c>
    /// </summary>
    /// <param name="startDate">The starting DateTime for this Range</param>
    /// <param name="endDate">The ending DateTime for this Range, if null, it assumes an open ended range.</param>
    /// <exception cref="ArgumentException">Throws ArgumentException if endDate is before startDate</exception>
    [JsonConstructor]
    public DateRange(DateTime startDate, DateTime? endDate)
    {
        if (endDate.HasValue && endDate < startDate) throw new ArgumentException("endDate cannot be before startDate");
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>
    /// Constructs an open ended DateRange starting from <paramref name="startDate">startDate</paramref>
    /// </summary>
    /// <param name="startDate"><c cref="DateTime">DateTime</c> to start this range</param>
    public DateRange(DateTime startDate) : this(startDate, null) { }

    /// <summary>
    /// Attempts to construct a DateRange from parsing the <paramref name="startDate"/> string. Will throw a <c cref="FormatException">FormatException</c> if the string cannot be parsed.
    /// </summary>
    /// <param name="startDate"></param>
    public DateRange(string startDate)
        :this(DateTime.Parse(startDate), null)
    {

    }

    public DateRange(string startDate, string? endDate) 
        :this(DateTime.Parse(startDate), string.IsNullOrWhiteSpace(endDate) ? null : DateTime.Parse(endDate))
    {

    }
    /// <summary>
    /// Checks if a certain <c>DateTime</c> falls within this <c>DateRange</c>
    /// </summary>
    /// <param name="value"><c>DateTime</c> to check</param>
    /// <returns><c>true</c> if <c>value</c> is between the StartDate (inclusive) and EndDate (exclusive) of this DateRange</returns>
    public bool Contains(DateTime value)
    {
        if(!EndDate.HasValue) return value >= StartDate;

        return value >= StartDate && value < EndDate;
    }

    /// <summary>
    /// Checks if another <c cref="DateRange">DateRange</c> intersects with this date range.
    /// </summary>
    /// <param name="other">The <c cref="DateRange">DateRange</c> to compare with this one.</param>
    /// <returns><c>true</c> if they intersect, false otherwise</returns>
    public bool Intersects(DateRange other)
    {
        //Other date range starts first
        if(other.StartDate < this.StartDate)
        {
            // Open ended range
            if (!other.EndDate.HasValue) return true;
            else if (other.EndDate < this.StartDate) return false;

            //If we get here, other.EndDate exists, and is beyond this.StartDate, so we intersect.
            return true;
        } else //This date range starts first (or start on same date)
        {
            //Open Ended Range
            if (!this.EndDate.HasValue) return true;
            else if(this.EndDate < other.StartDate) return false;

            //If we get here, this.EndDate exists, and is beyond the other StartDate, so we intersect
            return true;
        }
    }

    /// <summary>
    /// Creates a new <c cref="DateRange">DateRange</c> from the intersection of the two Date Ranges. i.e. only where they overlap.
    /// </summary>
    /// <param name="other">The DateRange to intersect with this one</param>
    /// <returns>A new <c cref="DateRange">DateRange</c> with the overlapping dates, or <c>null</c> if they do not overlap</returns>
    public DateRange? Intersect(DateRange other)
    {
        if(!this.Intersects(other)) return null;

        var start = new[] {this.StartDate, other.StartDate}.Max();
        if (!this.EndDate.HasValue && !other.EndDate.HasValue)
            return new DateRange(start, null);

        if(this.EndDate.HasValue && other.EndDate.HasValue)
        {
            var endSet = new[] {this.EndDate.Value, other.EndDate.Value };
            return new DateRange(start, endSet.Min());
        }

        // We know at least one of the enddates has a value if we get to this point
        if (!this.EndDate.HasValue) 
            return new DateRange(start, other.EndDate!.Value);

        return new DateRange(start, this.EndDate.Value);
    }

    /// <summary>
    /// Returns a new <c cref="DateRange">DateRange</c> encapsulating both <c cref="DateRange">DateRange</c>s.
    /// Even if they do not intersect, the new <c cref="DateRange">DateRange</c> will span both ranges. i.e. the earliest Starting Date, and latest Ending Date.
    /// </summary>
    /// <param name="other">The <c cref="DateRange">DateRange</c> to Join with this one</param>
    /// <returns>A new <c cref="DateRange">DateRange</c> spanning both ranges.</returns>
    public DateRange Join(DateRange other)
    {
        var start = new[] { this.StartDate, other.StartDate }.Min();
        if (!this.EndDate.HasValue || !other.EndDate.HasValue)
            return new DateRange(start, null);

        if (this.EndDate > other.EndDate)
            return new DateRange(start, this.EndDate);

        return new DateRange(start, other.EndDate);
        
    }


    public override string ToString()
    {
        return ToString(DefaultFormat);
    }

    public string ToString(string dateFormat)
    {
        var start = StartDate.ToString(dateFormat);
        var end = EndDate is null ? "the end of time" : EndDate.Value.ToString(dateFormat);
        return $"{start} - {end}";
    }
}

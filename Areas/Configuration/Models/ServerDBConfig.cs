namespace BrewComp.Areas.Configuration.Models;

public class ServerDBConfig
{
    //DB Info
    public string DBType { get; set; } = "postgres"; // Unused currently, may support choice of backed DB in the future
    public string DBUser { get; set; } = "brewcomp";
    public string DBPass { get; set; } = "brewcomp";
    public string DBHost { get; set; } = "localhost";
    public string DBName { get; set; } = "BrewComp.NET";
    public int DBPort { get; set; } = 5432;
}
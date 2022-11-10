namespace MISA.QLTH.API.Models;

public class FilterRequest
{
    public Dictionary<string, string> Filters { get; set; }
    public Dictionary<string, string> Sorts { get; set; }
}
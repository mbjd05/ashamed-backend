namespace AshamedApp.Application.DTOs;

public class TimeRangeRequest
{
    public string Topic { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
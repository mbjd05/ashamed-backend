using AshamedApp.Application.DTOs;
using AshamedApp.Application.Validators;
using Xunit;

namespace AshamedApp.Tests;

public class TimeRangeRequestValidatorTests
{
    private readonly TimeRangeRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Topic_Is_Empty()
    {
        var result = _validator.Validate(new TimeRangeRequest { Topic = "", Start = DateTime.Now.AddDays(-1), End = DateTime.Now });
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("empty"));
    }

    [Fact]
    public void Should_Have_Error_When_StartDate_Is_After_EndDate()
    {
        var result = _validator.Validate(new TimeRangeRequest { Topic = "Test", Start = DateTime.Now.AddDays(-1), End = DateTime.Now.AddDays(-2) });
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("earlier"));
    }

    [Fact]
    public void Should_Pass_When_Valid_Times_Are_Provided()
    {
        var result = _validator.Validate(new TimeRangeRequest
        {
            Topic = "TestTopic",
            Start = DateTime.Now.AddDays(-1),
            End = DateTime.Now
        });
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_Have_Error_When_Date_Range_Exceeds_One_Year()
    {
        var result = _validator.Validate(new TimeRangeRequest
        {
            Topic = "TestTopic",
            Start = DateTime.Now.AddYears(-1).AddDays(-1),
            End = DateTime.Now.AddYears(-1).AddDays(366)
        });
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("1 year"));
    }

    [Fact]
    public void Should_Pass_When_Start_And_End_Are_Same_Date()
    {
        var result = _validator.Validate(new TimeRangeRequest
        {
            Topic = "TestTopic",
            Start = DateTime.Now,
            End = DateTime.Now
        });
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Should_Have_Error_When_Start_Date_Is_In_The_Future()
    {
        var result = _validator.Validate(new TimeRangeRequest
        {
            Topic = "TestTopic",
            Start = DateTime.Now.AddDays(1),
            End = DateTime.Now
        });
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("future"));
    }

    // 5. **Test for End Date in the Future**
    [Fact]
    public void Should_Have_Error_When_End_Date_Is_In_The_Future()
    {
        var result = _validator.Validate(new TimeRangeRequest
        {
            Topic = "TestTopic",
            Start = DateTime.Now,
            End = DateTime.Now.AddDays(1)
        });
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("future"));
    }

    [Fact]
    public void Should_Have_Error_When_Topic_Is_Null()
    {
        var result = _validator.Validate(new TimeRangeRequest
        {
            Topic = null,
            Start = DateTime.Now.AddDays(-1),
            End = DateTime.Now
        });
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains("empty"));
    }

    [Fact]
    public void Should_Pass_When_Date_Range_Is_Exactly_One_Year()
    {
        var start = DateTime.Now.AddYears(-1);
        var end = start.AddDays(365); // Exactly one year apart

        var result = _validator.Validate(new TimeRangeRequest
        {
            Topic = "TestTopic",
            Start = start,
            End = end
        });

        Assert.True(result.IsValid);
    }
}
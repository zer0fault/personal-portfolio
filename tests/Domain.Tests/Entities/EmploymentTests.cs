using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class EmploymentTests
{
    [Fact]
    public void Employment_ShouldInitialize_WithDefaultValues()
    {
        // Arrange & Act
        var employment = new Employment();

        // Assert
        employment.Id.Should().Be(0);
        employment.CompanyName.Should().BeEmpty();
        employment.JobTitle.Should().BeEmpty();
        employment.StartDate.Should().Be(default);
        employment.EndDate.Should().BeNull();
        employment.Responsibilities.Should().Be("[]");
        employment.Achievements.Should().Be("[]");
        employment.Technologies.Should().Be("[]");
        employment.DisplayOrder.Should().Be(0);
        employment.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Employment_ShouldAllowSettingProperties()
    {
        // Arrange
        var employment = new Employment();
        var startDate = new DateTime(2020, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        // Act
        employment.CompanyName = "Duck Creek Technologies";
        employment.JobTitle = "Software Engineer";
        employment.StartDate = startDate;
        employment.EndDate = endDate;
        employment.Responsibilities = "[\"Develop features\", \"Write tests\"]";
        employment.Achievements = "[\"Improved performance by 30%\"]";
        employment.Technologies = "[\"C#\", \"ASP.NET Core\", \"Azure\"]";
        employment.DisplayOrder = 1;

        // Assert
        employment.CompanyName.Should().Be("Duck Creek Technologies");
        employment.JobTitle.Should().Be("Software Engineer");
        employment.StartDate.Should().Be(startDate);
        employment.EndDate.Should().Be(endDate);
        employment.Responsibilities.Should().Be("[\"Develop features\", \"Write tests\"]");
        employment.Achievements.Should().Be("[\"Improved performance by 30%\"]");
        employment.Technologies.Should().Be("[\"C#\", \"ASP.NET Core\", \"Azure\"]");
        employment.DisplayOrder.Should().Be(1);
    }

    [Fact]
    public void Employment_ShouldSupportCurrentPosition_WithNullEndDate()
    {
        // Arrange
        var employment = new Employment
        {
            CompanyName = "Current Company",
            JobTitle = "Senior Engineer",
            StartDate = DateTime.UtcNow.AddYears(-2)
        };

        // Act & Assert
        employment.EndDate.Should().BeNull();
    }

    [Fact]
    public void Employment_ShouldSupportSoftDelete()
    {
        // Arrange
        var employment = new Employment
        {
            CompanyName = "Test Company"
        };

        // Act
        employment.IsDeleted = true;

        // Assert
        employment.IsDeleted.Should().BeTrue();
    }
}

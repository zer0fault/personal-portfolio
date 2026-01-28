using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class ProjectTests
{
    [Fact]
    public void Project_ShouldInitialize_WithDefaultValues()
    {
        // Arrange & Act
        var project = new Project();

        // Assert
        project.Id.Should().Be(0);
        project.Title.Should().BeEmpty();
        project.ShortDescription.Should().BeEmpty();
        project.FullDescription.Should().BeEmpty();
        project.Technologies.Should().Be("[]");
        project.GitHubUrl.Should().BeNull();
        project.LiveDemoUrl.Should().BeNull();
        project.DisplayOrder.Should().Be(0);
        project.Status.Should().Be(ProjectStatus.Draft);
        project.IsDeleted.Should().BeFalse();
        project.Images.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Project_ShouldAllowSettingProperties()
    {
        // Arrange
        var project = new Project();
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow.AddMinutes(5);

        // Act
        project.Id = 1;
        project.Title = "Portfolio Website";
        project.ShortDescription = "A modern portfolio built with .NET 9";
        project.FullDescription = "This is a comprehensive portfolio showcasing .NET expertise";
        project.Technologies = "[\"C#\", \"Blazor\", \"Azure\"]";
        project.GitHubUrl = "https://github.com/user/repo";
        project.LiveDemoUrl = "https://example.com";
        project.DisplayOrder = 1;
        project.Status = ProjectStatus.Published;
        project.CreatedDate = createdDate;
        project.ModifiedDate = modifiedDate;
        project.IsDeleted = false;

        // Assert
        project.Id.Should().Be(1);
        project.Title.Should().Be("Portfolio Website");
        project.ShortDescription.Should().Be("A modern portfolio built with .NET 9");
        project.FullDescription.Should().Be("This is a comprehensive portfolio showcasing .NET expertise");
        project.Technologies.Should().Be("[\"C#\", \"Blazor\", \"Azure\"]");
        project.GitHubUrl.Should().Be("https://github.com/user/repo");
        project.LiveDemoUrl.Should().Be("https://example.com");
        project.DisplayOrder.Should().Be(1);
        project.Status.Should().Be(ProjectStatus.Published);
        project.CreatedDate.Should().Be(createdDate);
        project.ModifiedDate.Should().Be(modifiedDate);
        project.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Project_ShouldSupportSoftDelete()
    {
        // Arrange
        var project = new Project
        {
            Title = "Test Project",
            Status = ProjectStatus.Published
        };

        // Act
        project.IsDeleted = true;

        // Assert
        project.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Project_CanHaveMultipleImages()
    {
        // Arrange
        var project = new Project { Title = "Test Project" };
        var image1 = new ProjectImage { ImagePath = "/assets/projects/img1.webp", IsThumbnail = true };
        var image2 = new ProjectImage { ImagePath = "/assets/projects/img2.webp", IsThumbnail = false };

        // Act
        project.Images.Add(image1);
        project.Images.Add(image2);

        // Assert
        project.Images.Should().HaveCount(2);
        project.Images.Should().Contain(img => img.IsThumbnail);
    }

    [Theory]
    [InlineData(ProjectStatus.Draft)]
    [InlineData(ProjectStatus.Published)]
    [InlineData(ProjectStatus.Archived)]
    public void Project_ShouldSupportAllStatuses(ProjectStatus status)
    {
        // Arrange
        var project = new Project();

        // Act
        project.Status = status;

        // Assert
        project.Status.Should().Be(status);
    }
}

using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class SettingsTests
{
    [Fact]
    public void Settings_ShouldInitialize_WithDefaultValues()
    {
        // Arrange & Act
        var settings = new Settings();

        // Assert
        settings.Id.Should().Be(0);
        settings.Key.Should().BeEmpty();
        settings.Value.Should().BeEmpty();
        settings.Category.Should().BeEmpty();
        settings.IsDeleted.Should().BeFalse();
        settings.LastModified.Should().Be(default);
    }

    [Fact]
    public void Settings_ShouldAllowSettingProperties()
    {
        // Arrange
        var settings = new Settings();
        var lastModified = DateTime.UtcNow;
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow.AddMinutes(5);

        // Act
        settings.Id = 1;
        settings.Key = "HeroHeadline";
        settings.Value = "Welcome to My Portfolio";
        settings.Category = "Hero";
        settings.LastModified = lastModified;
        settings.CreatedDate = createdDate;
        settings.ModifiedDate = modifiedDate;
        settings.IsDeleted = false;

        // Assert
        settings.Id.Should().Be(1);
        settings.Key.Should().Be("HeroHeadline");
        settings.Value.Should().Be("Welcome to My Portfolio");
        settings.Category.Should().Be("Hero");
        settings.LastModified.Should().Be(lastModified);
        settings.CreatedDate.Should().Be(createdDate);
        settings.ModifiedDate.Should().Be(modifiedDate);
        settings.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Settings_ShouldSupportSoftDelete()
    {
        // Arrange
        var settings = new Settings
        {
            Key = "TestKey",
            Value = "TestValue",
            Category = "Test"
        };

        // Act
        settings.IsDeleted = true;

        // Assert
        settings.IsDeleted.Should().BeTrue();
    }

    [Theory]
    [InlineData("HeroHeadline", "Welcome!", "Hero")]
    [InlineData("AboutBio", "I am a developer", "About")]
    [InlineData("GitHubUrl", "https://github.com/user", "Social")]
    public void Settings_ShouldStoreVariousSettingTypes(string key, string value, string category)
    {
        // Arrange
        var settings = new Settings();

        // Act
        settings.Key = key;
        settings.Value = value;
        settings.Category = category;

        // Assert
        settings.Key.Should().Be(key);
        settings.Value.Should().Be(value);
        settings.Category.Should().Be(category);
    }

    [Fact]
    public void Settings_LastModified_ShouldTrackChanges()
    {
        // Arrange
        var settings = new Settings
        {
            Key = "TestKey",
            Value = "Original Value",
            Category = "Test",
            LastModified = DateTime.UtcNow.AddDays(-1)
        };
        var oldLastModified = settings.LastModified;

        // Act
        settings.Value = "Updated Value";
        settings.LastModified = DateTime.UtcNow;

        // Assert
        settings.LastModified.Should().BeAfter(oldLastModified);
        settings.Value.Should().Be("Updated Value");
    }
}

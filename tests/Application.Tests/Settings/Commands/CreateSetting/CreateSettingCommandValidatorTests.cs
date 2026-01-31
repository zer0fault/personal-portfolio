using Application.Settings.Commands.CreateSetting;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Settings.Commands.CreateSetting;

public class CreateSettingCommandValidatorTests
{
    private readonly CreateSettingCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Key_Is_Empty()
    {
        // Arrange
        var command = new CreateSettingCommand
        {
            Key = "",
            Value = "Test Value",
            Category = "Test"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Key");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateSettingCommand
        {
            Key = new string('a', 101),
            Value = "Test Value",
            Category = "Test"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Key");
    }

    [Fact]
    public void Should_Have_Error_When_Value_Is_Empty()
    {
        // Arrange
        var command = new CreateSettingCommand
        {
            Key = "TestKey",
            Value = "",
            Category = "Test"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Value");
    }

    [Fact]
    public void Should_Have_Error_When_Value_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateSettingCommand
        {
            Key = "TestKey",
            Value = new string('a', 2001),
            Category = "Test"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Value");
    }

    [Fact]
    public void Should_Have_Error_When_Category_Is_Empty()
    {
        // Arrange
        var command = new CreateSettingCommand
        {
            Key = "TestKey",
            Value = "Test Value",
            Category = ""
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Category");
    }

    [Fact]
    public void Should_Have_Error_When_Category_Exceeds_MaxLength()
    {
        // Arrange
        var command = new CreateSettingCommand
        {
            Key = "TestKey",
            Value = "Test Value",
            Category = new string('a', 51)
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Category");
    }

    [Fact]
    public void Should_Pass_Validation_With_Valid_Command()
    {
        // Arrange
        var command = new CreateSettingCommand
        {
            Key = "HeroHeadline",
            Value = "Welcome to My Portfolio",
            Category = "Hero"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData("HeroHeadline", "Welcome!", "Hero")]
    [InlineData("AboutBio", "I am a software developer", "About")]
    [InlineData("GitHubUrl", "https://github.com/user", "Social")]
    public void Should_Pass_Validation_With_Various_Valid_Settings(string key, string value, string category)
    {
        // Arrange
        var command = new CreateSettingCommand
        {
            Key = key,
            Value = value,
            Category = category
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}

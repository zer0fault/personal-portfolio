using Application.Settings.Commands.UpdateSetting;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Settings.Commands.UpdateSetting;

public class UpdateSettingCommandValidatorTests
{
    private readonly UpdateSettingCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Zero()
    {
        // Arrange
        var command = new UpdateSettingCommand
        {
            Id = 0,
            Key = "TestKey",
            Value = "Test Value",
            Category = "Test"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Negative()
    {
        // Arrange
        var command = new UpdateSettingCommand
        {
            Id = -1,
            Key = "TestKey",
            Value = "Test Value",
            Category = "Test"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Id");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Is_Empty()
    {
        // Arrange
        var command = new UpdateSettingCommand
        {
            Id = 1,
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
    public void Should_Have_Error_When_Value_Is_Empty()
    {
        // Arrange
        var command = new UpdateSettingCommand
        {
            Id = 1,
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
    public void Should_Have_Error_When_Category_Is_Empty()
    {
        // Arrange
        var command = new UpdateSettingCommand
        {
            Id = 1,
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
    public void Should_Pass_Validation_With_Valid_Command()
    {
        // Arrange
        var command = new UpdateSettingCommand
        {
            Id = 1,
            Key = "HeroHeadline",
            Value = "Updated Welcome Message",
            Category = "Hero"
        };

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}

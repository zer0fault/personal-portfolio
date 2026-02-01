using Application.Common.Interfaces;
using Application.Employment.Commands.UpdateEmployment;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Employment.Commands.UpdateEmployment;

public class UpdateEmploymentCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly UpdateEmploymentCommandHandler _handler;

    public UpdateEmploymentCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new UpdateEmploymentCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_UpdateEmployment_WithValidData()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Original Company",
            JobTitle = "Original Title",
            StartDate = new DateTime(2020, 1, 1),
            EndDate = new DateTime(2021, 12, 31),
            Responsibilities = "[\"Old responsibility\"]",
            Achievements = "[\"Old achievement\"]",
            Technologies = "[\"Old Tech\"]",
            DisplayOrder = 1,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            ModifiedDate = DateTime.UtcNow.AddDays(-1)
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        var command = new UpdateEmploymentCommand
        {
            Id = employment.Id,
            CompanyName = "Updated Company",
            JobTitle = "Updated Title",
            StartDate = new DateTime(2022, 1, 1),
            EndDate = new DateTime(2023, 12, 31),
            Responsibilities = new List<string> { "New responsibility", "Additional task" },
            Achievements = new List<string> { "Major achievement" },
            Technologies = new List<string> { "C#", ".NET", "Azure" },
            DisplayOrder = 2
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var updatedEmployment = await _context.EmploymentHistory.FindAsync(employment.Id);
        updatedEmployment.Should().NotBeNull();
        updatedEmployment!.CompanyName.Should().Be("Updated Company");
        updatedEmployment.JobTitle.Should().Be("Updated Title");
        updatedEmployment.StartDate.Should().Be(new DateTime(2022, 1, 1));
        updatedEmployment.EndDate.Should().Be(new DateTime(2023, 12, 31));
        updatedEmployment.DisplayOrder.Should().Be(2);
        updatedEmployment.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenEmploymentNotFound()
    {
        // Arrange
        var command = new UpdateEmploymentCommand
        {
            Id = 999,
            CompanyName = "Test Company",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Employment entry not found");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenEmploymentIsDeleted()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Deleted Company",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = "[]",
            Achievements = "[]",
            Technologies = "[]",
            IsDeleted = true,
            DisplayOrder = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        var command = new UpdateEmploymentCommand
        {
            Id = employment.Id,
            CompanyName = "Updated",
            JobTitle = "Updated",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Employment entry not found");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenCompanyNameIsEmpty()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Original",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = "[]",
            Achievements = "[]",
            Technologies = "[]",
            IsDeleted = false,
            DisplayOrder = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        var command = new UpdateEmploymentCommand
        {
            Id = employment.Id,
            CompanyName = "",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Company name is required");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenJobTitleIsEmpty()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Test Company",
            JobTitle = "Original",
            StartDate = DateTime.UtcNow,
            Responsibilities = "[]",
            Achievements = "[]",
            Technologies = "[]",
            IsDeleted = false,
            DisplayOrder = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        var command = new UpdateEmploymentCommand
        {
            Id = employment.Id,
            CompanyName = "Test Company",
            JobTitle = "",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Job title is required");
    }

    [Fact]
    public async Task Handle_Should_UpdateModifiedDate_NotCreatedDate()
    {
        // Arrange
        var originalCreatedDate = DateTime.UtcNow.AddDays(-10);
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Original",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = "[]",
            Achievements = "[]",
            Technologies = "[]",
            IsDeleted = false,
            DisplayOrder = 1,
            CreatedDate = originalCreatedDate,
            ModifiedDate = originalCreatedDate
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        var command = new UpdateEmploymentCommand
        {
            Id = employment.Id,
            CompanyName = "Updated",
            JobTitle = "Senior Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedEmployment = await _context.EmploymentHistory.FindAsync(employment.Id);
        updatedEmployment!.CreatedDate.Should().Be(originalCreatedDate);
        updatedEmployment.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_UpdateResponsibilities_Properly()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Test Company",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = "[\"Old task\"]",
            Achievements = "[]",
            Technologies = "[]",
            IsDeleted = false,
            DisplayOrder = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        var command = new UpdateEmploymentCommand
        {
            Id = employment.Id,
            CompanyName = "Test Company",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string> { "New task 1", "New task 2", "New task 3" },
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedEmployment = await _context.EmploymentHistory.FindAsync(employment.Id);
        updatedEmployment!.Responsibilities.Should().Contain("New task 1");
        updatedEmployment.Responsibilities.Should().Contain("New task 2");
        updatedEmployment.Responsibilities.Should().Contain("New task 3");
    }

    [Fact]
    public async Task Handle_Should_UpdateEndDate_ToNull_ForCurrentPosition()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Test Company",
            JobTitle = "Developer",
            StartDate = new DateTime(2020, 1, 1),
            EndDate = new DateTime(2023, 12, 31),
            Responsibilities = "[]",
            Achievements = "[]",
            Technologies = "[]",
            IsDeleted = false,
            DisplayOrder = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        var command = new UpdateEmploymentCommand
        {
            Id = employment.Id,
            CompanyName = "Test Company",
            JobTitle = "Senior Developer",
            StartDate = new DateTime(2020, 1, 1),
            EndDate = null,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedEmployment = await _context.EmploymentHistory.FindAsync(employment.Id);
        updatedEmployment!.EndDate.Should().BeNull();
        updatedEmployment.JobTitle.Should().Be("Senior Developer");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private class TestDbContext : DbContext, IApplicationDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectImage> ProjectImages { get; set; } = null!;
        public DbSet<Domain.Entities.Employment> EmploymentHistory { get; set; } = null!;
        public DbSet<ContactSubmission> ContactSubmissions { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<Domain.Entities.Settings> Settings { get; set; } = null!;
    }
}

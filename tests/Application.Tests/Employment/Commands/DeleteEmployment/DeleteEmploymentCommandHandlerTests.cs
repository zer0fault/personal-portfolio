using Application.Common.Interfaces;
using Application.Employment.Commands.DeleteEmployment;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Employment.Commands.DeleteEmployment;

public class DeleteEmploymentCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly DeleteEmploymentCommandHandler _handler;

    public DeleteEmploymentCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new DeleteEmploymentCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_SoftDeleteEmployment()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Test Company",
            JobTitle = "Software Engineer",
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

        var command = new DeleteEmploymentCommand { Id = employment.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var deletedEmployment = await _context.EmploymentHistory.FindAsync(employment.Id);
        deletedEmployment.Should().NotBeNull();
        deletedEmployment!.IsDeleted.Should().BeTrue();
        deletedEmployment.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenEmploymentNotFound()
    {
        // Arrange
        var command = new DeleteEmploymentCommand { Id = 999 };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Employment entry not found");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenEmploymentAlreadyDeleted()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Already Deleted",
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

        var command = new DeleteEmploymentCommand { Id = employment.Id };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Employment entry not found");
    }

    [Fact]
    public async Task Handle_Should_NotPhysicallyDeleteEmployment()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Test Company",
            JobTitle = "Senior Developer",
            StartDate = new DateTime(2020, 1, 1),
            EndDate = new DateTime(2023, 12, 31),
            Responsibilities = "[\"Task 1\",\"Task 2\"]",
            Achievements = "[\"Achievement 1\"]",
            Technologies = "[\"C#\",\".NET\"]",
            IsDeleted = false,
            DisplayOrder = 1,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        var command = new DeleteEmploymentCommand { Id = employment.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedEmployment = await _context.EmploymentHistory
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.Id == employment.Id);

        deletedEmployment.Should().NotBeNull();
        deletedEmployment!.CompanyName.Should().Be("Test Company");
        deletedEmployment.JobTitle.Should().Be("Senior Developer");
    }

    [Fact]
    public async Task Handle_Should_PreserveAllData_WhenSoftDeleting()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Preserved Company",
            JobTitle = "Tech Lead",
            StartDate = new DateTime(2019, 5, 15),
            EndDate = new DateTime(2022, 8, 30),
            Responsibilities = "[\"Lead team\",\"Code review\"]",
            Achievements = "[\"Delivered project\"]",
            Technologies = "[\"React\",\"Node.js\"]",
            IsDeleted = false,
            DisplayOrder = 3,
            CreatedDate = DateTime.UtcNow.AddDays(-30),
            ModifiedDate = DateTime.UtcNow.AddDays(-5)
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        var originalCreatedDate = employment.CreatedDate;

        var command = new DeleteEmploymentCommand { Id = employment.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedEmployment = await _context.EmploymentHistory
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(e => e.Id == employment.Id);

        deletedEmployment.Should().NotBeNull();
        deletedEmployment!.CompanyName.Should().Be("Preserved Company");
        deletedEmployment.JobTitle.Should().Be("Tech Lead");
        deletedEmployment.StartDate.Should().Be(new DateTime(2019, 5, 15));
        deletedEmployment.EndDate.Should().Be(new DateTime(2022, 8, 30));
        deletedEmployment.DisplayOrder.Should().Be(3);
        deletedEmployment.CreatedDate.Should().Be(originalCreatedDate);
        deletedEmployment.Responsibilities.Should().Contain("Lead team");
        deletedEmployment.Technologies.Should().Contain("React");
    }

    [Fact]
    public async Task Handle_Should_UpdateModifiedDate_WhenDeleting()
    {
        // Arrange
        var originalModifiedDate = DateTime.UtcNow.AddDays(-7);
        var employment = new Domain.Entities.Employment
        {
            CompanyName = "Test Company",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = "[]",
            Achievements = "[]",
            Technologies = "[]",
            IsDeleted = false,
            DisplayOrder = 1,
            CreatedDate = DateTime.UtcNow.AddDays(-30),
            ModifiedDate = originalModifiedDate
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        var command = new DeleteEmploymentCommand { Id = employment.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedEmployment = await _context.EmploymentHistory.FindAsync(employment.Id);
        deletedEmployment!.ModifiedDate.Should().NotBe(originalModifiedDate);
        deletedEmployment.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
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

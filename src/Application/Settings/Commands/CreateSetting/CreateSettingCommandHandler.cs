using Application.Common.Interfaces;
using MediatR;

namespace Application.Settings.Commands.CreateSetting;

/// <summary>
/// Handler for CreateSettingCommand
/// </summary>
public class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateSettingCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = new Domain.Entities.Settings
        {
            Key = request.Key,
            Value = request.Value,
            Category = request.Category,
            LastModified = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Settings.Add(setting);
        await _context.SaveChangesAsync(cancellationToken);

        return setting.Id;
    }
}

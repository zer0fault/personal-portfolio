using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Settings.Commands.UpdateSetting;

/// <summary>
/// Handler for UpdateSettingCommand
/// </summary>
public class UpdateSettingCommandHandler : IRequestHandler<UpdateSettingCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateSettingCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = await _context.Settings
            .Where(s => !s.IsDeleted)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (setting == null)
        {
            throw new KeyNotFoundException($"Setting with ID {request.Id} not found");
        }

        setting.Key = request.Key;
        setting.Value = request.Value;
        setting.Category = request.Category;
        setting.LastModified = DateTime.UtcNow;
        setting.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

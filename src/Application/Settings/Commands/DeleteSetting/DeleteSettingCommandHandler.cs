using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Settings.Commands.DeleteSetting;

/// <summary>
/// Handler for DeleteSettingCommand
/// </summary>
public class DeleteSettingCommandHandler : IRequestHandler<DeleteSettingCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteSettingCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = await _context.Settings
            .Where(s => !s.IsDeleted)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (setting == null)
        {
            throw new KeyNotFoundException($"Setting with ID {request.Id} not found");
        }

        // Soft delete
        setting.IsDeleted = true;
        setting.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

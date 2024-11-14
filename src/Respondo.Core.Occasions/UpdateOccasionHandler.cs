using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Occasions.Persistence;
using Wolverine;

namespace Respondo.Core.Occasions;

public sealed record UpdateOccasionHandler
{
    private readonly IMessageContext _context;
    private readonly OccasionDbContext _db;
    private readonly ILogger<UpdateOccasionHandler> _logger;

    public UpdateOccasionHandler(IMessageContext context, OccasionDbContext db, ILogger<UpdateOccasionHandler> logger)
    {
        _context = context;
        _db = db;
        _logger = logger;
    }

    public async Task Handle(UpdateOccasion request, CancellationToken cancellationToken)
    {
        var occasion = await _db.Occasions.FirstOrDefaultAsync(occasion => occasion.Id == request.OccasionId,
            cancellationToken);

        if (occasion is null)
        {
            _logger.LogError("Occasion with id {OccasionId} was not found, but update was requested", request.OccasionId);
            return;
        }
        
        if (request.Name is { Length: > 0 } name)
        {
            occasion.Name = name;
        }
        
        if (request.BaseUrl is { Length: > 0 } baseUrl)
        {
            occasion.BaseUrl = baseUrl;
        }
        
        if (request.Logo is { Length: > 0 } logo)
        {
            occasion.Logo = logo;
        }

        if (_db.ChangeTracker.HasChanges())
        {
            await _db.SaveChangesAsync(cancellationToken);

            await _context.PublishAsync(new OccasionUpdated() { OccasionId = occasion.Id });
        }
    }
}
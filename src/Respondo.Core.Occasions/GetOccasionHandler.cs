using Microsoft.EntityFrameworkCore;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Occasions.Persistence;

namespace Respondo.Core.Occasions;

public sealed record GetOccasionHandler
{
    public async Task<GetOccasionResponse?> Handle(GetOccasion request, OccasionDbContext context)
    {
        var occasion = await context.Occasions
            .Where(occasion => occasion.Profile.Id == request.ProfileId)
            .FirstOrDefaultAsync(occasion => occasion.Id == request.Id);

        if (occasion is null)
        {
            return default;
        }

        return new GetOccasionResponse
        {
            Id = occasion.Id,
            Name = occasion.Name
        };
    }
}
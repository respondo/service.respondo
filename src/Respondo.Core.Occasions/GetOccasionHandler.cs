using Microsoft.EntityFrameworkCore;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Occasions.Persistence;

namespace Respondo.Core.Occasions;

public sealed record GetOccasionHandler
{
    public async Task<GetOccasionResponse?> Handle(GetOccasion request, OccasionDbContext context)
    {
        var query = context.Occasions
            .AsNoTracking()
            .Where(occasion => occasion.Profile.Id == request.ProfileId)
            .Where(occasion => occasion.Id == request.Id)
            .Select(occasion => new GetOccasionResponse
            {
                Id = occasion.Id,
                Name = occasion.Name,
                BaseUrl = occasion.BaseUrl,
                Logo = occasion.Logo
            });

        return await query.FirstOrDefaultAsync();

    }
}
using Microsoft.EntityFrameworkCore;
using Respondo.Core.Occasions.Contracts;
using Respondo.Core.Occasions.Persistence;

namespace Respondo.Core.Occasions;

public sealed record GetOccasionsHandler
{
    public async Task<GetOccasionsResponse> Handle(GetOccasions request, OccasionDbContext context)
    {
        var query = context.Occasions
            .AsNoTracking()
            .Where(occasion => occasion.Profile.Id == request.ProfileId)
            .Select(occasion => new GetOccasionsResponse.Occasion
            {
                Id = occasion.Id,
                Name = occasion.Name
            });
        
        var occasions = await query.ToListAsync();
        
        return new GetOccasionsResponse
        {
            Occasions = occasions
        };
    }
}
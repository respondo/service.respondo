using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record GetPartiesHandler
{
    public async Task<GetPartiesResponse> Handle(GetParties request, PartiesDbContext context)
    {
        var query = context.Parties
            .Where(party => party.Occasion.Profile.Id == request.ProfileId)
            .Where(party => party.Occasion.Id == request.OccasionId)
            .Select(party => new GetPartiesResponse.Party()
            {
                Id = party.Id,
                Name = party.Name,
                Email = party.Email,
                Members = party.Members.Select(member => new GetPartiesResponse.Party.Member
                {
                    Id = member.Id,
                    Name = member.Name,
                })
            });

        var parties = await query.ToListAsync();

        return new GetPartiesResponse
        {
            Parties = parties
        };
    }
}
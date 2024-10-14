using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record GetPartyHandler
{
    public async Task<GetPartyResponse?> Handle(GetParty request, PartiesDbContext context)
    {
        
        var party = context.Parties
            .Where(party => party.Occasion.Profile.Id == request.ProfileId)
            .Where(party => party.Id == request.Id)
            .Select(party => new GetPartyResponse
            {
                Id = party.Id,
                Name = party.Name,
                Email = party.Email,
                Members = party.Members.Select(member => new GetPartyResponse.Member
                {
                    Id = member.Id,
                    Name = member.Name,
                }).ToList()
            });

        return await party.FirstOrDefaultAsync();
    }
}
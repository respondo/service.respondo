using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Entities;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record AddMemberToPartyHandler
{
    public async Task<MemberAddedToParty?> Handle(AddMemberToParty request, PartiesDbContext context)
    {
        var party = await context.Parties
            .Where(party => party.Occasion.Profile.Id == request.ProfileId)
            .FirstOrDefaultAsync(party => party.Id == request.PartyId);

        if (party == null)
        {
            return default;
        }
        
        var member = new Member
        {
            Id = Guid.CreateVersion7(TimeProvider.System.GetUtcNow()),
            Name = request.Name,
            Party = party
        };
        
        await context.Members.AddAsync(member);
        
        await context.SaveChangesAsync();
        
        return new MemberAddedToParty
        {
            Id = member.Id
        };
    }
}
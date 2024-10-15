using Microsoft.EntityFrameworkCore;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record DeletePartyMemberHandler
{
    public async Task<PartyMemberDeleted?> Handle(DeletePartyMember request, PartiesDbContext context)
    {
        var member = await context.Members
            .Where(member => member.Party.Occasion.Profile.Id == request.ProfileId)
            .Where(member => member.Party.Id == request.PartyId)
            .FirstOrDefaultAsync(member => member.Id == request.Id);
        
        if (member is null)
        {
            return default;
        }
        
        context.Members.Remove(member);
        
        await context.SaveChangesAsync();
        
        return new PartyMemberDeleted { Id = member.Id };
    }
}
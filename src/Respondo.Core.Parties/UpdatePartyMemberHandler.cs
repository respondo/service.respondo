using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Respondo.Core.Parties.Contracts;
using Respondo.Core.Parties.Persistence;

namespace Respondo.Core.Parties;

public sealed record UpdatePartyMemberHandler
{
    public async Task<PartyMemberUpdated?> Handle(UpdatePartyMember request, PartiesDbContext context, ILogger<UpdatePartyMemberHandler> logger)
    {
        var member = await context.Members
            .Where(member => member.Party.Occasion.Profile.Id == request.ProfileId)
            .FirstOrDefaultAsync(member => member.Id == request.MemberId);

        if (member == null)
        {
            logger.LogError("Member with id {MemberId} not found but update was requested", request.MemberId);
            
            return default;
        }
        
        if (member.Name != request.Name)
        {
            member.Name = request.Name;
        }

        if (context.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync();
            
            return new PartyMemberUpdated
            {
                Id = member.Id
            };
        }

        return default;
    }
}
namespace Respondo.Core.Parties.Contracts;

/// <summary>
///     Event emitted when a party is deleted.
/// </summary>
public sealed record PartyDeleted
{
    /// <summary>
    ///     Id of the deleted party.
    /// </summary>
    public required Guid PartyId { get; init; }
    
    public required Guid OccasionId { get; init; }
    
}
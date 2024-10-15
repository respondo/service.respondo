namespace Respondo.Core.Parties.Contracts;

/// <summary>
///     Request for deleting a party.
/// </summary>
public sealed record DeleteParty
{
    /// <summary>
    ///     Id of the party to delete.
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    ///     Id of the profile.
    /// </summary>
    public required Guid ProfileId { get; init; }
}
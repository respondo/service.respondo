namespace Respondo.Core.Parties.Contracts;

/// <summary>
///     Event emitted when a party is deleted.
/// </summary>
public sealed record PartyDeleted
{
    /// <summary>
    ///     Id of the deleted party.
    /// </summary>
    public required Guid Id { get; init; }
}
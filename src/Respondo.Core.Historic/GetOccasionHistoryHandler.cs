using Marten;
using Microsoft.Extensions.Logging;
using Respondo.Core.Historic.Aggregates;
using Respondo.Core.Historic.Contracts;
using Respondo.Core.Historic.Extensions;

namespace Respondo.Core.Historic;

public sealed record GetOccasionHistoryHandler
{
    private readonly IQuerySession _session;
    private readonly ILogger<GetOccasionHistoryHandler> _logger;

    public GetOccasionHistoryHandler(IQuerySession session, ILogger<GetOccasionHistoryHandler> logger)
    {
        _session = session;
        _logger = logger;
    }

    public async Task<GetOccasionHistoryResponse?> Handle(GetOccasionHistory request, CancellationToken cancellationToken)
    {
        var occasionHistory = await _session.LoadAsync<OccasionHistory>(request.OccasionId, cancellationToken);

        if (occasionHistory is null)
        {
            _logger.LogWarning("Occasion {OccasionId} not found", request.OccasionId);
            return default;
        }

        return new GetOccasionHistoryResponse
        {
            OccasionId = occasionHistory.Id,
            History = occasionHistory.History.Select(@event => @event.ToResponseEvent()).ToList()
        };
    }
}
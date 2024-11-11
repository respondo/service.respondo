using Respondo.Core.Historic.Aggregates;
using Respondo.Core.Historic.Contracts;

namespace Respondo.Core.Historic.Extensions;

public static class OccasionHistoryExtensions
{
    public static GetOccasionHistoryResponse.Event ToResponseEvent(this OccasionHistory.Event e)
    {
        return new GetOccasionHistoryResponse.Event
        {
            Log = e.Log,
            Timestamp = e.Timestamp
        };
    }
}
using System.Collections.Concurrent;
using web_app.Controllers;

namespace web_app;

public static class InMemoryStore
{
    public static readonly ConcurrentDictionary<string, BatchStatus> BatchStatuses = new();
}
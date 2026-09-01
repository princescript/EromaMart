namespace Server.Helpers;

using Microsoft.AspNetCore.Http;
using System.Text;

public interface IRedisKeyHelper
{
    string Generate();
}

public class RedisKeyHelper : IRedisKeyHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RedisKeyHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Generate()
    {
        var request = _httpContextAccessor.HttpContext?.Request;

        if (request == null)
            throw new InvalidOperationException("HTTP context is not available.");

        // Controller name
        var controller = request.RouteValues["controller"]?.ToString();

        // Action name
        var action = request.RouteValues["action"]?.ToString();

        if (string.IsNullOrWhiteSpace(controller))
            throw new InvalidOperationException("Controller not found.");

        controller = controller
            .Replace("Controller", "")
            .ToLowerInvariant();

        action = string.IsNullOrWhiteSpace(action)
            ? "default"
            : action.ToLowerInvariant();

        var key = new StringBuilder();

        key.Append(controller);
        key.Append(":");
        key.Append(action);

        // Query parameters
        if (request.Query.Count > 0)
        {
            foreach (var query in request.Query.OrderBy(x => x.Key))
            {
                key.Append(":");
                key.Append(query.Key.ToLowerInvariant());
                key.Append("=");
                key.Append(query.Value.ToString());
            }
        }

        return key.ToString();
    }
}
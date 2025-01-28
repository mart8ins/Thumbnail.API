using System.Text;

namespace Thumbnail.API.Middlewares
{
    public class AuthentificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public AuthentificationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization header missing.");
                return;
            }

            string authHeaderValue = authHeader.ToString();

            if (!authHeaderValue.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid Authorization header.");
                return;
            }

            string encodedCredentials = authHeaderValue.Substring("Basic ".Length).Trim();
            string decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            string[] parts = decodedCredentials.Split(':', 2);

            if (parts.Length != 2 || !IsValidCredentials(parts[0], parts[1]))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            await _next(context);
        }

        private bool IsValidCredentials(string username, string password)
        {
            return username == _configuration["ThumbnailApiUsername"] && password == _configuration["ThumbnailApiPassword"];
        }
    }
}

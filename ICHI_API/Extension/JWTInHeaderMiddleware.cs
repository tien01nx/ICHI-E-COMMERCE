namespace ICHI_CORE.Filter
{
    public class JWTInHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        public JWTInHeaderMiddleware (RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var name = "Jwt";
            var cookie = context.Request.Cookies[name];
            if (cookie != null)
            {
                if (!context.Request.Headers.ContainsKey("Authorization"))
                    context.Request.Headers.Append("Authorization", "Bearer " + cookie);
            }
            await _next(context);
        }
    }
}

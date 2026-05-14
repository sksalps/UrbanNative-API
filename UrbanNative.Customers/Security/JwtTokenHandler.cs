
using System.Net.Http.Headers;
namespace UrbanNative.Customers.Security
{
    
    public class JwtTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static class CustomClaims
        {
            public const string Jwt = "JWT";
        }
        protected override Task<HttpResponseMessage> SendAsync(     HttpRequestMessage request,         CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null && context.User.Identity?.IsAuthenticated == true)
            {
                //var token = _httpContextAccessor.HttpContext?.Request.Cookies["CustomerAuthToken"];
                var token = context.User.FindFirst(CustomClaims.Jwt)?.Value;
                if (token == null) { return base.SendAsync(request, cancellationToken); }

                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

            }
            return base.SendAsync(request, cancellationToken);            
        }
    }

    
    
}

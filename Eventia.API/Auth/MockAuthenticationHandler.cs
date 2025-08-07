using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Eventia.API.Auth
{
    public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public MockAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Aquí puedes cambiar el rol simulado dinámicamente si quieres
            var identity = new ClaimsIdentity("Mock");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "mock-user-id"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "mock@eventia.com"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Administrador")); // o Supervisor, Agente

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "MockScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}

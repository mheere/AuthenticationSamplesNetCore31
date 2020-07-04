//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Options;
//using System;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;
//using System.Net.Http.Headers;
//using System.Security.Claims;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Mvc;
//using fusion.listeners.fapi.jwt;

//namespace fusion.listeners.fapi
//{

//    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//    {
//        private readonly IHttpContextAccessor _httpContext;
//        private readonly IUserManagementService _userManagementService;
//        private ILoggerFactory _logger;
//        private ILogger<BasicAuthenticationHandler> _logger2;

//        public BasicAuthenticationHandler(
//            IOptionsMonitor<AuthenticationSchemeOptions> options,
//            ILoggerFactory logger,
//            ILogger<BasicAuthenticationHandler> logger2,
//            UrlEncoder encoder,
//            ISystemClock clock,
//            IUserManagementService userService,
//            IHttpContextAccessor accessor)
//            : base(options, logger, encoder, clock)
//        {
//            _logger = logger;
//            _logger2 = logger2;
//            _userManagementService = userService;
//            _httpContext = accessor;
//        }

//        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//        {
//            if (!Request.Headers.ContainsKey("Authorization"))
//            {
//                //_logger.LogWarning("Authorisation Header is Missing!");
//                return AuthenticateResult.Fail("Missing Authorization Header");
//            }

//            string username = "** not set **";
//            string password;
//            string ipAddress = "** not set **";

//            try
//            {
//                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
//                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
//                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
//                username = credentials[0];
//                password = credentials[1];
//                ipAddress = _httpContext.HttpContext.Connection.RemoteIpAddress.ToString();
                
//                // now check agains our injected user checker
//                if (!_userManagementService.IsValidUser(username, password))
//                {
//                    _logger2.LogWarning($"Logon failed for User {username} from IP Address {ipAddress}");
//                    return AuthenticateResult.Fail("Invalid Username or Password");
//                }

//            }
//            catch
//            {
//                //_logger.LogError("Error decoding authorisation header");
//                _logger2.LogWarning($"Invalid Authorization Header for {username} from IP Address {ipAddress}");
//                return AuthenticateResult.Fail("Invalid Authorization Header");
//            }

//            // create a new claims collection
//            var claims = new[] {
//                new Claim(ClaimTypes.Sid, "XXX111"),
//                new Claim(ClaimTypes.Name, username)
//            };

//            var identity = new ClaimsIdentity(claims, Scheme.Name);
//            var principal = new ClaimsPrincipal(identity);
//            var ticket = new AuthenticationTicket(principal, Scheme.Name);
//            //_logger.LogInformation($"Logon Success for User {username} from IP Address {ipAddress}");

//            // Create Cookie - this will be returned with the response & can be used as auth on subsequent requests
//            await Request.HttpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
//            {
//                IsPersistent = true,
//                ExpiresUtc = DateTimeOffset.Now.AddMinutes(5)   // NOTE - Cookie Expiration set here, but SlidingExpiration set in Startup.cs
//            }).ConfigureAwait(false);

//            return AuthenticateResult.Success(ticket);
//        }
//    }
//}

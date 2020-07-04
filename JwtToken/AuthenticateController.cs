using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthSamples
{
    /// <summary>
    /// This controller's ONLY task is to Authenticate incoming JWT token requests.
    /// If successfull it returns an OK status (200) and the jwt token as the payload
    /// if not, it returns Unauthorized (401)
    /// The actual mechanism to verify the user credentials is injected (IJwtAuthenticateService) and can
    /// therefore easily be substituted by specifying another control in the StartUp class.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IJwtAuthenticateService _authService;
        private readonly ILogger<AuthenticateController> _logger;

        /// <summary>
        /// We need our own implementation of the AuthenticationService and a logger...
        /// </summary>
        public AuthenticateController(IJwtAuthenticateService authService, ILogger<AuthenticateController> logger)
        {
            _logger = logger;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost, Route("request")]
        public IActionResult RequestToken([FromBody] JwtTokenRequest request)
        {
            if (request == null) return Unauthorized("Invalid Request");

            if (!ModelState.IsValid)
            {
                _logger.LogInformation("JWT Authenticate Error - incoming request for jwt token was Invalid.");
                return Unauthorized(ModelState);
            }

            string token;
            if (_authService.IsAuthenticated(request, out token))
            {
                _logger.LogInformation("JWT Authenticate OK - incoming request authorised for: " + request.Username);
                return Ok(token);
            }

            // 
            _logger.LogInformation("JWT Authenticate Error - incoming request denied for: " + request.Username);
            return Unauthorized("Invalid Request");
        }
    }
}

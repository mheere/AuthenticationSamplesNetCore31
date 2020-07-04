using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthSamples
{
    public interface IJwtAuthenticateService
    {
        bool IsAuthenticated(JwtTokenRequest request, out string token);
    }

    /// <summary>
    /// The IsAuthenticated method return a JWT token if the user cred were ok.  The JwtSecurityToken (token) needs a
    /// variety of info like the Issuer, Audience, claim, etc that is largely retrieved from the appsettings.json
    /// </summary>
    public class JWTAuthenticationService : IJwtAuthenticateService
    {
        private readonly IUserService _userManagementService;
        private readonly TokenManagement _tokenManagement;
        private readonly ILogger<JWTAuthenticationService> _logger;

        public JWTAuthenticationService(IUserService service, IOptions<TokenManagement> tokenManagement, ILogger<JWTAuthenticationService> logger)
        {
            _logger = logger;
            _userManagementService = service;
            _tokenManagement = tokenManagement.Value;
        }

        public bool IsAuthenticated(JwtTokenRequest request, out string token)
        {
            token = string.Empty;

            try
            {
                // this can't happen but because these stupid VS 'code enhancers' are flagging it lets add some unnecessary clutter code in 
                if (request == null) return false;

                // now check agains our injected user checker
                if (!_userManagementService.IsValidUser(request.Username, request.Password)) return false;

                var claim = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var jwtToken = new JwtSecurityToken(
                    _tokenManagement.Issuer,
                    _tokenManagement.Audience,
                    claim,
                    expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                    signingCredentials: credentials
                );
            
                // had to do this (can't remember why so quickly..)
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            
                // https://stackoverflow.com/questions/50590432/jwt-securitytokeninvalidsignatureexception-using-rs256-pii-is-hidden
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error occurred in JWT IsAuthenticated - " + ex.Message);
            }

            return false;

        }
    }


}

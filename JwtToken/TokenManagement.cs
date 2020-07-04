using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AuthSamples
{
    /// <summary>
    /// Handy object that maps to the definition we placed in the appsettings.json
    /// </summary>
    [JsonObject("tokenManagement")]
    public class TokenManagement
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("accessExpiration")]
        public int AccessExpiration { get; set; }

        [JsonProperty("refreshExpiration")]
        public int RefreshExpiration { get; set; }

    }

    /// <summary>
    /// This token request is for JWT only - it specifies what should be submitted to us 
    /// in the body of the incoming message
    /// </summary>
    public class JwtTokenRequest
    {
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }


        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}

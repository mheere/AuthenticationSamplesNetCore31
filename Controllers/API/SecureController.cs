using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using AuthSamples.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace AuthSamples.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {

        private readonly ILogger<SecureController> _logger;

        public SecureController(ILogger<SecureController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("ping")]
        public async Task<string> Ping()
        {
            _logger.LogTrace("Incoming ping, returned pong");

            // 
            return await Task.FromResult("pong").ConfigureAwait(false);
        }

        [Route("Test")]
        [HttpPost]
        [BasicAuth]        
        public MyData Test()
        {
            var data = new MyData();
            data.Status = "OK - using single BasicAuth decorator (TypeFilterAttribute)";
            data.FirstName = "Lisa";
            data.LastName = "Heeremans";
            return data;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("CheckDataJWT")]
        [Description("1. Checks Data with JWT tokens")]
        public async Task<MyData> CheckDataJWT([FromBody] MyData data)
        {
            try
            {
                await Task.Delay(500);
                data.Status = "OK - using JwtBearer token";
                data.FirstName = "Amy ('" + data.FirstName + "')";
                return data;
            }
            catch (Exception ex) { return new MyData { LastName = ex.Message };  }
        }

        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpPost]
        [Route("CheckDataBA")]
        [Description("2. Checks data with BasicAuth")]
        public async Task<MyData> CheckDataBA([FromBody] MyData data)
        {
            try
            {
                await Task.Delay(200);
                data.Status = "OK - using BasicAuthentication";
                data.FirstName = "Kim";
                return data;
            }
            catch (Exception ex) { return new MyData { LastName = ex.Message }; }
        }

        [Authorize]
        [HttpPost]
        [Route("CheckData")]
        [Description("3. Checks data with the default!")]
        public async Task<MyData> CheckData([FromBody] MyData data)
        {
            // THIS ONE IS STILL NOT WORKING -------------------
            try
            {
                await Task.Delay(200);
                data.Status = "OK - using the Default Authorize";
                data.FirstName = "Default";
                return data;
            }
            catch (Exception ex) { return new MyData { LastName = ex.Message }; }
        }

    }
}
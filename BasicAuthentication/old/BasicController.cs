//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
////using s5_core.Controllers.Authentication;
//using System;
//using System.Threading.Tasks;

//namespace fusion.listeners.fapi
//{
//    [Route("api/[controller]")]
//    public class BasicController : Controller
//    {
//        private readonly IHttpContextAccessor _httpContext;
//        private readonly ILogger<BasicController> _logger;

//        public BasicController(IHttpContextAccessor accessor, ILogger<BasicController> logger)
//        {
//            _logger = logger;
//            _httpContext = accessor;
//        }

//        // ------------------------------------------------------------------------
//        // --- BasicAuthentication ---
//        // ------------------------------------------------------------------------
//        [HttpGet]
//        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
//        [Route("ObtainCookie")]
//        public async Task<IActionResult> ObtainCookie()
//        {
//            return await Task.FromResult(new JsonResult(new { response = "You were authenticated and a cookie was returned." }));
//        }

//        [HttpGet]
//        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
//        [Route("TestBasicAuth")]
//        public async Task<IActionResult> TestBasicAuth()
//        {
//            return await Task.FromResult(new JsonResult(new { response = "You were authenticated with basic auth headers!" }));
//        }
        
//        [HttpGet]
//        [Authorize(AuthenticationSchemes = "BasicAuthentication,Cookies")]
//        [Route("TestBasicOrCookieAuth")]
//        public async Task<IActionResult> TestBasicOrCookieAuth()
//        {
//            return await Task.FromResult(new JsonResult(new { response = "You were authenticated with a yummy cookie or basic auth!" }));
//        }

//        //[HttpGet]
//        //[Authorize(AuthenticationSchemes = "Cookies")]
//        //[Route("DestroyCookie")]
//        //public async Task<IActionResult> DestroyAuthCookie()
//        //{
//        //    //await _authService.Logout();
//        //    return Ok();
//        //}

//    }
//}

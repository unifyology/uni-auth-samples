using Microsoft.AspNetCore.Mvc;


namespace UnifyAuth.Sample.ProxyTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IPAddressController : ControllerBase
    {
        private readonly ILogger<IPAddressController> _logger;

        public IPAddressController(ILogger<IPAddressController> logger)
        {
            _logger = logger;
        }

        public IEnumerable<string> Get()
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress;
            string? ip = remoteIp?.MapToIPv4().ToString();
            var scheme = HttpContext.Request.Scheme;
            string sch = scheme.ToString();
            var host = HttpContext.Request.Host;
            string ho = host.ToString();
            return new string[] { ip, sch, ho };
        }
    }
}
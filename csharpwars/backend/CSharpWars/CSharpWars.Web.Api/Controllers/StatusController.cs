using System.Threading.Tasks;
using CSharpWars.Web.Api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CSharpWars.Web.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ApiController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StatusController> _logger;

        public StatusController(IConfiguration configuration, ILogger<StatusController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public Task<IActionResult> GetStatus()
        {
            _logger.LogInformation($"CONNECTIONSTRING: {_configuration.GetValue<string>("CONNECTIONSTRING")}");
            _logger.LogInformation($"KEY_VAULT: {_configuration.GetValue<string>("KEY_VAULT")}");
            _logger.LogInformation($"CLIENT_ID: {_configuration.GetValue<string>("CLIENT_ID")}");
            _logger.LogInformation($"CLIENT_SECRET: {_configuration.GetValue<string>("CLIENT_SECRET")}");

            return Success();
        }
    }
}
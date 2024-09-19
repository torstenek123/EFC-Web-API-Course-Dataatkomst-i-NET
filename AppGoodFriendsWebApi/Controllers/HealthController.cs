using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Seido.Utilities.SeedGenerator;
using Models;
using Models.DTO;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class HealthController : Controller
    {
        const string _seedSource = "./friends-seeds.json";

        // GET: health/apptest
        [HttpGet()]
        [ActionName("AppTest")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult AppTest()
        {
            string sRet = "AppTest\n";

            var fn = Path.GetFullPath(_seedSource);
            var _seeder = new csSeedGenerator(fn);

            return Ok(sRet);
        }

        // GET: health/heartbeat
        [HttpGet()]
        [ActionName("Heartbeat")]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult Heartbeat()
        {
            //to verify the layers are accessible
            string sRet = $"\nLayer access:\n{csAppConfig.Heartbeat}" +
                $"\n{csLoginService.Heartbeat}" +
                $"\n{csJWTService.Heartbeat}";
                
            //to verify secret access source
            sRet += $"\n\nSecret source:\n{csAppConfig.SecretSource}";

            //to verify connection strings can be read from appsettings.json
            sRet += $"\n\nDbConnections:\nDbLocation: {csAppConfig.DbSetActive.DbLocation}" +
                $"\nDbServer: {csAppConfig.DbSetActive.DbServer}";

            sRet += "\nDbUserLogins in DbSet:";
            foreach (var item in csAppConfig.DbSetActive.DbLogins)
            {
                sRet += $"\n   DbUserLogin: {item.DbUserLogin}" +
                    $"\n   DbConnection: {item.DbConnection}\n   ConString: <secret>";
            }

            //_logger.LogInformation(sRet);
            return Ok(sRet);
        }

        //GET: health/log
        [HttpGet()]
        [ActionName("Log")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<csLogMessage>))]
        public async Task<IActionResult> Log([FromServices] ILoggerProvider _loggerProvider)
        {
            //Note the way to get the LoggerProvider, not the logger from Services via DI
            if (_loggerProvider is csInMemoryLoggerProvider cl)
            {
                return Ok(await cl.MessagesAsync);
            }
            return Ok("No messages in log");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        #region constructors
        public HealthController()
        {
        }
        /*
        public HealthController(IFriendsService service)
        {
            _service = service;
        }
        
        public HealthController(IFriendsService service, ILogger<FriendsController> logger)
        {
            _service = service;
            _logger = logger;
        }
        */
        #endregion
    }
}

/* Exercise
1. Add below structue to appsettings.json
  "MyName": {
    "FirstName": "your name",
    "LastName": "your name",
    "Age": your_age
  },
2. Modify Configuration.csAppConfig.cs to read MyName structure
3. Modify HealthController so Heartbeat service also writes your full name and age
*/


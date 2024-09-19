﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Configuration;
using Models;
using Models.DTO;

using Services;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GuestController : Controller
    {
        ILoginService _loginService = null;
        ILogger<GuestController> _logger = null;

        //GET: api/guest/info
        [HttpGet()]
        [ActionName("Info")]
        [ProducesResponseType(200, Type = typeof(gstusrInfoAllDto))]
        public async Task<IActionResult> Info()
        {
            return BadRequest("Not implemented");
        }


        //POST: api/guest/LoginUser
        [HttpPost]
        [ActionName("LoginUser")]
        [ProducesResponseType(200, Type = typeof(loginUserSessionDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> LoginUser([FromBody] loginCredentialsDto userCreds)
        {
            return BadRequest("Not implemented");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        #region constructors
        /*
        public GuestController(IFriendsService friendService, ILogger<GuestController> logger)
        {
            _friendService = friendService;
            _logger = logger;
        }
        */
        /*
        public GuestController(IFriendsService friendService, ILoginService loginService, ILogger<GuestController> logger)
        {
            _friendService = friendService;
            _loginService = loginService;

            _logger = logger;
        }
        */
        #endregion
    }
}


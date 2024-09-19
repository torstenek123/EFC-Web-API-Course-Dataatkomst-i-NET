using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Models;
using Models.DTO;
using Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SeedController : Controller
    {

        ISeedDataService _service = null;
        ILogger<SeedController> _logger;

        //GET: api/admin/seed?count={count}
        [HttpGet()]
        [ActionName("MasterSeed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> MasterSeed()
        {
            try
            {
                _service.MasterSeed();
                return Ok("Seeded");           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        #region constructors
        
        public SeedController(ISeedDataService service, ILogger<SeedController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        

        
        #endregion
    }
}


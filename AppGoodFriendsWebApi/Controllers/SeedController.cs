//Author: Torsten Ek
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


namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SeedController : Controller
    {

        IAttractionDataService _service = null;
        ILogger<SeedController> _logger;

        //GET: 
        [HttpGet()]
        [ActionName("CreateSeed")]
        [ProducesResponseType(200, Type = typeof(seedInfoDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateSeed()
        {
            try
            {
                var _info = await _service.CreateSeed();
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("RemoveSeed")]
        [ProducesResponseType(200, Type = typeof(deleteSeedInfoDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> RemoveSeed(string seeded = "true")
        {
            try
            {
                bool _seeded = bool.Parse(seeded);
                var _info = await _service.RemoveSeed(_seeded);
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #region constructors
        
        public SeedController(IAttractionDataService service, ILogger<SeedController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        

        
        #endregion
    }
}


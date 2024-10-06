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
    public class CreateController : Controller
    {

        IAttractionDataService _service = null;
        ILogger<CreateController> _logger = null;


        [HttpPost()]
        [ActionName("CreateLocality")]
        [ProducesResponseType(200, Type = typeof(csLocalityCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateLocality([FromBody] csLocalityCUdto item)
        {
            try
            {
            
                var _item = await _service.CreateLocalityAsync(item);
                _logger.LogInformation($"Locality {_item.LocalityId} created");
                return Ok(_item);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        [ActionName("CreateAttraction")]
        [ProducesResponseType(200, Type = typeof(csAttractionCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateAttraction([FromBody] csAttractionCUdto item)
        {
            try
            {
            
                var _item = await _service.CreateAttractionAsync(item);
                _logger.LogInformation($"Attraction {_item.AttractionId} created");
                return Ok(_item);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        [ActionName("CreateUser")]
        [ProducesResponseType(200, Type = typeof(csAttractionCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateUser([FromBody] csUserCUdto item)
        {
            try
            {
            
                var _item = await _service.CreateUserAsync(item);
                _logger.LogInformation($"User {_item.UserId} created");
                return Ok(_item);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        [ActionName("CreateComment")]
        [ProducesResponseType(200, Type = typeof(csCommentCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateComment([FromBody] csCommentCUdto item)
        {
            try
            {
            
                var _item = await _service.CreateCommentAsync(item);
                _logger.LogInformation($"Comment {_item.CommentId} created");
                return Ok(_item);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region constructors
        
        public CreateController(IAttractionDataService service, ILogger<CreateController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion
    }
}


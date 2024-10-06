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
    public class UpdateController : Controller
    {

        IAttractionDataService _service = null;
        ILogger<UpdateController> _logger = null;

        [HttpPut("{id}")]
        [ActionName("UpdateLocality")]
        [ProducesResponseType(200, Type = typeof(csLocalityCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateLocality(string id, [FromBody] csLocalityCUdto item)
        {
            try
            {
                var _id = Guid.Parse(id);
                if(item.LocalityId != _id) throw new ArgumentException($"{_id} does not equal {item.LocalityId}");
                var _item = await _service.UpdateLocality(item);
                _logger.LogInformation($"Locality {_id} updated");
                return Ok(_item);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        [ActionName("UpdateAttraction")]
        [ProducesResponseType(200, Type = typeof(csAttractionCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateAttraction(string id, [FromBody] csAttractionCUdto item)
        {
            try
            {
                var _id = Guid.Parse(id);
                if(item.AttractionId != _id) throw new ArgumentException($"{_id} does not equal {item.AttractionId}");
                var _item = await _service.UpdateAttraction(item);
                _logger.LogInformation($"Attraction {_id} updated");
                return Ok(_item);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ActionName("UpdateUser")]
        [ProducesResponseType(200, Type = typeof(csUserCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] csUserCUdto item)
        {
            try
            {
                var _id = Guid.Parse(id);
                if(item.UserId != _id) throw new ArgumentException($"{_id} does not equal {item.UserId}");
                var _item = await _service.UpdateUser(item);
                _logger.LogInformation($"User {_id} updated");
                return Ok(_item);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ActionName("UpdateComment")]
        [ProducesResponseType(200, Type = typeof(csCommentCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateComment(string id, [FromBody] csCommentCUdto item)
        {
            try
            {
                var _id = Guid.Parse(id);
                if(item.CommentId != _id) throw new ArgumentException($"{_id} does not equal {item.CommentId}");
                var _item = await _service.UpdateComment(item);
                _logger.LogInformation($"Comment {_id} updated");
                return Ok(_item);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region constructors
        
        public UpdateController(IAttractionDataService service, ILogger<UpdateController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion
    }
}


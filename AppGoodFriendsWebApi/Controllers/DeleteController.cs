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
    public class DeleteController : Controller
    {

        IAttractionDataService _service = null;
        ILogger<DeleteController> _logger = null;


        [HttpDelete("{id}")]
        [ActionName("DeleteLocality")]
        [ProducesResponseType(200, Type = typeof(ILocality))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteLocality(string id)
        {
            try
            {

                Guid LocalityId = Guid.Parse(id);
                var _info = await _service.DeleteLocalityAsync(LocalityId);
                
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [ActionName("DeleteAttraction")]
        [ProducesResponseType(200, Type = typeof(IAttraction))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteAttraction(string id)
        {
            try
            {

                Guid attractionId = Guid.Parse(id);
                var _info = await _service.DeleteAttraction(attractionId);
                
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ActionName("DeleteUser")]
        [ProducesResponseType(200, Type = typeof(IUser))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {

                Guid userId = Guid.Parse(id);
                var _info = await _service.DeleteUser(userId);
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [ActionName("DeleteComment")]
        [ProducesResponseType(200, Type = typeof(IComment))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteComment(string id)
        {
            try
            {

                Guid commentId = Guid.Parse(id);
                var _info = await _service.DeleteComment(commentId);
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region constructors
        
        public DeleteController(IAttractionDataService service, ILogger<DeleteController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion
    }
}


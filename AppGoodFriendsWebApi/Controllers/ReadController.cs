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
    public class ReadController : Controller
    {

        IAttractionDataService _service = null;
        ILogger<ReadController> _logger = null;


        [HttpGet()]
        [ActionName("ReadLocality")]
        [ProducesResponseType(200, Type = typeof(ILocality))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadLocalityAsync(string localityId,  string flat = "true")
        {
            try
            {

                bool _flat = bool.Parse(flat);
                Guid _localityId = Guid.Parse(localityId);
                var _info = await _service.ReadLocalityAsync(_flat, _localityId);
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet()]
        [ActionName("ReadAttraction")]
        [ProducesResponseType(200, Type = typeof(IAttraction))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadAttractionAsync(string attractionId,  string flat = "true")
        {
            try
            {

                bool _flat = bool.Parse(flat);
                Guid _attractionId = Guid.Parse(attractionId);
                var _info = await _service.ReadAttractionAsync(_flat, _attractionId);
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet()]
        [ActionName("ReadUser")]
        [ProducesResponseType(200, Type = typeof(IUser))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadUserAsync(string userId,  string flat = "true")
        {
            try
            {

                bool _flat = bool.Parse(flat);
                Guid _userId = Guid.Parse(userId);
                var _info = await _service.ReadUserAsync(_flat, _userId);
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadComment")]
        [ProducesResponseType(200, Type = typeof(IComment))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadCommentAsync(string commentId,  string flat = "true")
        {
            try
            {

                bool _flat = bool.Parse(flat);
                Guid _commentId = Guid.Parse(commentId);
                var _info = await _service.ReadCommentAsync(_flat, _commentId);
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadLocalityDto")]
        [ProducesResponseType(200, Type = typeof(csLocalityCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadLocalityDtoAsync(string id)
        {
            try
            {
                var _id = Guid.Parse(id);
                var item = await _service.ReadLocalityAsync(false, _id);
                if (item == null) 
                {
                    return BadRequest($"Attraction with id {id} does not exist");
                }
                var dto = new csLocalityCUdto(item);
                return Ok(dto);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadAttractionDto")]
        [ProducesResponseType(200, Type = typeof(csAttractionCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadAttractionDtoAsync(string id)
        {
            try
            {
                var _id = Guid.Parse(id);
                var item = await _service.ReadAttractionAsync(false, _id);
                if (item == null) 
                {
                    return BadRequest($"Attraction with id {id} does not exist");
                }
                var dto = new csAttractionCUdto(item);
                return Ok(dto);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadUserDto")]
        [ProducesResponseType(200, Type = typeof(csUserCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadUserDtoAsync(string id)
        {
            try
            {
                var _id = Guid.Parse(id);
                var item = await _service.ReadUserAsync(false, _id);
                if (item == null) 
                {
                    return BadRequest($"Attraction with id {id} does not exist");
                }
                var dto = new csUserCUdto(item);
                return Ok(dto);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadCommentDto")]
        [ProducesResponseType(200, Type = typeof(csCommentCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadCommentDtoAsync(string id)
        {
            try
            {
                var _id = Guid.Parse(id);
                var item = await _service.ReadCommentAsync(false, _id);
                if (item == null) 
                {
                    return BadRequest($"Attraction with id {id} does not exist");
                }
                var dto = new csCommentCUdto(item);
                return Ok(dto);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //returns attractions matching filter
        [HttpGet()]
        [ActionName("ReadAttractions")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadAllAttractionsAsync(string seeded = "true", string flat = "true",
                                                            string pageNr = "0", string pageSize = "10",
                                                            string filter = null)
        {
            try
            {
                bool _seeded = bool.Parse(seeded);
                bool _flat = bool.Parse(flat);
                int _pageNr = int.Parse(pageNr);
                int _pageSize = int.Parse(pageSize);
                var _info = await _service.ReadAllAttractionsAsync(_seeded, _flat, _pageNr, _pageSize, filter?.Trim()?.ToLower() );
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //reads users and their comments
        [HttpGet()]
        [ActionName("ReadUsers")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IUser>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadAllUsersAsync(string seeded = "true", string flat = "true",
                                                            string pageNr = "0", string pageSize = "10")
        {
            try
            {
                bool _seeded = bool.Parse(seeded);
                bool _flat = bool.Parse(flat);
                int _pageNr = int.Parse(pageNr);
                int _pageSize = int.Parse(pageSize);
                var _info = await _service.ReadAllUsersAsync(_seeded, _flat, _pageNr, _pageSize);
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET: reading all attractions with no comments
        [HttpGet()]
        [ActionName("ReadAttractionsNoComments")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadAllNoCommentsAsync()
        {
            try
            {
                var _info = await _service.ReadAllNoCommentsAsync();
                return Ok(_info);           
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        #region constructors
        
        public ReadController(IAttractionDataService service, ILogger<ReadController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion
    }
}


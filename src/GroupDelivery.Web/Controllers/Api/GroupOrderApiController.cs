using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain.Entities;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/groups")]
    public class GroupOrderApiController : ControllerBase
    {
        private readonly GroupOrderService _service;

        public GroupOrderApiController(GroupOrderService service)
        {
            _service = service;
        }

        // GET: api/groups
        [HttpGet]
        public async Task<IActionResult> GetActiveGroups()
        {
            try
            {
                List<GroupOrder> groups =
                    await _service.GetActiveGroupsAsync();   

                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/groups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup(int id)
        {
            GroupOrder group =
                await _service.GetGroupDetailAsync(id);     

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }
    }
}

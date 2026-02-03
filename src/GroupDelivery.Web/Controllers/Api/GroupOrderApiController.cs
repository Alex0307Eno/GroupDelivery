using GroupDelivery.Application.Abstractions;
using GroupDelivery.Application.Services;
using GroupDelivery.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroupDelivery.Web.Controllers.Api
{
    [ApiController]
    [Route("api/groups")]
    public class GroupOrderApiController : ControllerBase
    {
        private readonly IGroupOrderService _groupOrderService;
        public GroupOrderApiController(IGroupOrderService groupOrderService)
        {
            _groupOrderService = groupOrderService;
        }

        // GET: api/groups
        [HttpGet]
        public async Task<IActionResult> GetActiveGroups()
        {
            try
            {
                List<GroupOrder> groups =
                    await _groupOrderService.GetActiveGroupsAsync();   

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
                await _groupOrderService.GetGroupDetailAsync(id);     

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }
    }
}

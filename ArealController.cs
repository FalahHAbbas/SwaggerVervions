using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Common.Base;
using TicketSystem.Common.Helpers;
using TicketSystem.Common.Utils;
using TicketSystem.Models.Dbs;
using TicketSystem.Models.Forms.Customers;
using TicketSystem.Services;

namespace TicketSystem.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AreaController : BaseController {
    private readonly IAreaService _areaService;


    public AreaController(IAreaService areaService) { _areaService = areaService; }

    [HttpPost]
    [ProducesResponseType(typeof(SingleResponse<Area>), 200)]
    [ProducesResponseType(typeof(SingleResponse<>), 400)]
    public async Task<IActionResult> Add([FromBody] AreaForm customer) =>
        Ok(await _areaService.Add(customer));

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(SingleResponse<Area>), 200)]
    [ProducesResponseType(typeof(SingleResponse<>), 400)]
    public async Task<IActionResult> Update(Guid id, [FromBody] AreaForm customer) =>
        Ok(await _areaService.Update(customer, id));

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(SingleResponse<Customer>), 200)]
    [ProducesResponseType(typeof(SingleResponse<>), 400)]
    public async Task<IActionResult> Delete(Guid id) => Ok(await _areaService.Remove(id));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Responses<Area>), 200)]
    [ProducesResponseType(typeof(SingleResponse<>), 400)]
    public async Task<IActionResult> Get(Guid id) => Ok(await _areaService.Get(id));

    [HttpGet]
    [ProducesResponseType(typeof(Responses<Area>), 200)]
    [ProducesResponseType(typeof(SingleResponse<>), 400)]
    public async Task<IActionResult> GetAll(int pageNumber = 1) =>
        Ok(await _areaService.GetAll(pageNumber), pageNumber);
}
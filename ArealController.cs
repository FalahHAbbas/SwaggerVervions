using Microsoft.AspNetCore.Mvc;

namespace SwaggerVervions.Path1;

[Route("api/Path1/[controller]/[action]")]
[ApiController]
public class AreaController : ControllerBase {

    [HttpPost]
    public async Task<IActionResult> GetAll() {
        var result =  new{
            Name = "sdf",
            Code = "dfsd",
            Description = "sdfd",
            Status = "sdfsd",
        };
        return Ok(result);
    }
}
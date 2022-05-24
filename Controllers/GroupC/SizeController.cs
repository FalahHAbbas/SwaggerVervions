using Microsoft.AspNetCore.Mvc;

namespace SwaggerVervions.Controllers.GroupC;

[Route("api/Path2/[controller]/[action]")]
[ApiController]
public class SizeController : ControllerBase {

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
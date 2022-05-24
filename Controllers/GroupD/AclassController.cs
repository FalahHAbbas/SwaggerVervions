using Microsoft.AspNetCore.Mvc;

namespace SwaggerVervions.Controllers.GroupD;

[Route("api/Path1/[controller]/[action]")]
[ApiController]
public class AclassController : ControllerBase {

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
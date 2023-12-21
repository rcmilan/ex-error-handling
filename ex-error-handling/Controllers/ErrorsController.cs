using ex_error_handling.CustomExceptions;
using Microsoft.AspNetCore.Mvc;

namespace ex_error_handling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [HttpPost("division")]
        public async Task<ActionResult> Post(int p, int q)
        {
            var result = p / q;

            return Ok(result);
        }
        
        [HttpPost("throws")]
        public async Task<ActionResult> Post()
        {
            throw new MyCustomException("MyCustomException");
        }
    }
}

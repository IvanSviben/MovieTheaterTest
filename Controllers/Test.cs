using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World!");
    }
    [HttpGet("Ping")]
    public IActionResult Ping()
    {
        return Ok("Pong");
    }
}
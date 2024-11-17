using System.Net;
using Microsoft.AspNetCore.Mvc;
using ROLAP.Process.Interfaces;

namespace ROLAP.WebService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CubeController : Controller
{
    private IProcessor _cubeProcessor;
    public CubeController(IProcessor processor)
    {
        _cubeProcessor = processor;
    }
    [HttpGet]
    public async Task<IActionResult> Get(string query)
    {
        try
        {
            var res = await _cubeProcessor.ProcessQuery(query);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError,ex.Message);
        }
    }
}
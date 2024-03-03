using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BetterBeatSaber.Server.Controllers;

[Route("server")]
[ApiController]
public sealed class ServerController(
    IOptions<Server.Server.ServerOptions> options
) : ControllerBase {
    
    [HttpGet]
    public ActionResult GetServer() =>
        Ok($"{options.Value.Address}:{options.Value.Port}");

}
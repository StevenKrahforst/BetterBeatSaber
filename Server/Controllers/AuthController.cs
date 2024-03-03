using BetterBeatSaber.Server.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace BetterBeatSaber.Server.Controllers;

[Route("auth")] // move to server
[ApiController]
public sealed class AuthController(
    IPlayerService playerService,
    ISteamService steamService
) : ControllerBase {

    public const uint AppId = 620980u;
    
    [HttpPost]
    public async Task<ActionResult> Authenticate() {
        
        Request.EnableBuffering();
        Request.Body.Position = 0;

        using var streamReader = new StreamReader(HttpContext.Request.Body);

        var ticket = await streamReader.ReadToEndAsync();
        
        
        return Ok();
    }

}
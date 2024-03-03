using BetterBeatSaber.Server.Models;
using BetterBeatSaber.Server.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace BetterBeatSaber.Server.Controllers;

[Route("players")]
[ApiController]
public sealed class PlayerController(
    IPlayerService playerService
) : ControllerBase {

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(ulong id) {
        var player = await playerService.GetPlayer(id);
        return player is not null ? Ok(player) : NotFound();
    }

}
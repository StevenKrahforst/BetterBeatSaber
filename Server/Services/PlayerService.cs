using BetterBeatSaber.Server.Models;
using BetterBeatSaber.Server.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace BetterBeatSaber.Server.Services;

public sealed class PlayerService(
    DatabaseContext context
) : IPlayerService {

    public Task<Player?> GetPlayer(ulong id) =>
        context.Players.FirstOrDefaultAsync(player => player.Id == id);

}
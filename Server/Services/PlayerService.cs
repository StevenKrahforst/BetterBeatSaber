using BetterBeatSaber.Server.Models;
using BetterBeatSaber.Server.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace BetterBeatSaber.Server.Services;

public sealed class PlayerService(
    DatabaseContext context
) : IPlayerService {

    public Task<Player?> GetPlayer(ulong id) =>
        context.Players.FirstOrDefaultAsync(player => player.Id == id);

    public async Task<Player?> GetAndUpdateOrCreatePlayer(ulong id, string name) {

        var player = await GetPlayer(id);
        if (player == null) {
            
            player = new Player {
                Id = id,
                Name = name
            };

            context.Players.Add(player);

        } else {
            player.Name = name;
        }

        await context.SaveChangesAsync();

        return player;

    }

}
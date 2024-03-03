using BetterBeatSaber.Server.Models;

namespace BetterBeatSaber.Server.Services.Interfaces;

public interface IPlayerService {

    public Task<Player?> GetPlayer(ulong id);

}
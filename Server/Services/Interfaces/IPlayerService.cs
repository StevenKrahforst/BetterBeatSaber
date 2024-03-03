using BetterBeatSaber.Server.Models;

namespace BetterBeatSaber.Server.Services.Interfaces;

public interface IPlayerService {

    public Task<Player?> GetPlayer(ulong id);
    
    public Task<Player?> GetAndUpdateOrCreatePlayer(ulong id, string name);

}
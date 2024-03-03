using BetterBeatSaber.Server.Services.Steam;

namespace BetterBeatSaber.Server.Services.Interfaces;

public interface ISteamService {
    
    public Task<(AuthResponseParams?, AuthResponseError?)> Authenticate(uint appId, string ticket);
    public Task<IEnumerable<ProfileResponsePlayer>> GetPlayerSummaries(params ulong[] steamIds);
    public Task<ProfileResponsePlayer?> GetPlayerSummary(ulong steamId);

}
namespace BetterBeatSaber.Server.Services.Steam;

public interface ISteamResponse<T> {

    public T Response { get; set; }

}
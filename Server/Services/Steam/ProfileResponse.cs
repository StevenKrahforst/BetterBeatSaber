using Newtonsoft.Json;

namespace BetterBeatSaber.Server.Services.Steam;

// ReSharper disable StringLiteralTypo

public record ProfileResponse(
    [JsonProperty("players")] ProfileResponsePlayer[] Players
);

public record ProfileResponsePlayer(
    [JsonProperty("steamid")] string SteamId,
    [JsonProperty("communityvisibilitystate")] byte CommunityVisibilityState,
    [JsonProperty("profilestate")] byte ProfileState,
    [JsonProperty("personaname")] string PersonaName,
    [JsonProperty("profileurl")] string ProfileUrl,
    [JsonProperty("avatar")] string AvatarUrl,
    [JsonProperty("avatarmedium")] string AvatarMediumUrl,
    [JsonProperty("avatarfull")] string AvatarFullUrl,
    [JsonProperty("avatarhash")] string AvatarHash,
    [JsonProperty("personastate")] byte PersonaState,
    [JsonProperty("primaryclanid")] string PrimaryClanId,
    [JsonProperty("timecreated")] long TimeCreated,
    [JsonProperty("personastateflags")] int PersonaStateFlags,
    [JsonProperty("loccountrycode")] string LocationCountryCode,
    [JsonProperty("locstatecode")] string LocationStateCode,
    [JsonProperty("loccityid")] int LocationCityId
);
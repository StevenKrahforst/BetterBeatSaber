using System;
using System.Threading.Tasks;

using Discord;

using Zenject;

namespace BetterBeatSaber.Online.Manager;

public sealed class DiscordManager : IInitializable, ITickable {

    private const long ApplicationId = 1028705518778327200L;
    
    private global::Discord.Discord? _discord;
    
    public Activity? CurrentActivity { get; private set; }
    
    public void Initialize() =>
        _discord = new global::Discord.Discord(ApplicationId, (ulong) CreateFlags.Default);

    public void Tick() =>
        _discord?.RunCallbacks();

    public Task<Result> UpdateActivity(Activity? activity, bool updateStart = true) {
        
        if (activity is null)
            return ClearActivity();
        
        if (updateStart) {
            activity = activity.Value with {
                Timestamps = new ActivityTimestamps {
                    Start = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                }
            };
        }
        
        var promise = new TaskCompletionSource<Result>();
        _discord?.GetActivityManager().UpdateActivity(activity.Value, result => {
            if (result == Result.Ok)
                CurrentActivity = activity;
            promise.SetResult(result);
        });
        
        return promise.Task;
        
    }

    public Task<Result> UpdateCurrentActivity(
        string? details = null,
        string? state = null,
        string? largeImageKey = null,
        string? largeImageText = null,
        string? smallImageKey = null,
        string? smallImageText = null
    ) {
        if (CurrentActivity != null) {
            return UpdateActivity(CurrentActivity.Value with {
                Details = details ?? string.Empty,
                State = state ?? string.Empty,
                Assets = new ActivityAssets {
                    LargeImage = largeImageKey ?? string.Empty,
                    LargeText = largeImageText ?? string.Empty,
                    SmallImage = smallImageKey ?? string.Empty,
                    SmallText = smallImageText ?? string.Empty
                }
            });
        } else {
            return UpdateActivity(new Activity {
                Details = details ?? string.Empty,
                State = state ?? string.Empty,
                Assets = new ActivityAssets {
                    LargeImage = largeImageKey ?? string.Empty,
                    LargeText = largeImageText ?? string.Empty,
                    SmallImage = smallImageKey ?? string.Empty,
                    SmallText = smallImageText ?? string.Empty
                }
            });
        }
    }
    
    public Task<Result> UpdateLargeImage(string largeImageKey, string largeImageText) {

        var activity = CurrentActivity ?? new Activity();
        
        activity = activity with {
            Assets = new ActivityAssets {
                LargeImage = largeImageKey,
                LargeText = largeImageText,
                SmallImage = activity.Assets.SmallImage ?? string.Empty,
                SmallText = activity.Assets.SmallText ?? string.Empty
            }
        };
        
        return UpdateActivity(activity);
        
    }
    
    public Task<Result> UpdateSmallImage(string largeImageKey, string largeImageText) {

        var activity = CurrentActivity ?? new Activity();
        
        activity = activity with {
            Assets = new ActivityAssets {
                LargeImage = largeImageKey,
                LargeText = largeImageText,
                SmallImage = activity.Assets.SmallImage ?? string.Empty,
                SmallText = activity.Assets.SmallText ?? string.Empty
            }
        };
        
        return UpdateActivity(activity);
        
    }

    public Task<Result> ClearActivity() {
        var promise = new TaskCompletionSource<Result>();
        _discord?.GetActivityManager().ClearActivity(result => {
            if(result == Result.Ok)
                CurrentActivity = null;
            promise.SetResult(result);
        });
        return promise.Task;
    }

}
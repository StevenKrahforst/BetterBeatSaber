using System;

using BetterBeatSaber.Online.Manager;

using Discord;

using JetBrains.Annotations;

using Zenject;

namespace BetterBeatSaber.Online.Installers;

public sealed class MenuInstaller : Zenject.Installer {

    [UsedImplicitly]
    [Inject]
    private readonly DiscordManager _discordManager = null!;

    public override void InstallBindings() {
        
        Console.WriteLine("AAAAAA");
        
        _discordManager.UpdateActivity(new Activity {
            Name = "Beat Saber",
            Details = "Relaxing in the Menu",
            State = "UwU",
            Assets = new ActivityAssets {
                LargeImage = "menu",
                LargeText = "In Menu",
                SmallImage = "logo",
                SmallText = $"Better Beat Saber"
            }
        });
        
    }

}
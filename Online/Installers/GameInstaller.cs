using System;

using BetterBeatSaber.Online.Extensions;
using BetterBeatSaber.Online.Manager;

using JetBrains.Annotations;

using Zenject;

namespace BetterBeatSaber.Online.Installers;

public sealed class GameInstaller : Zenject.Installer {

    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<EnvironmentHider>().AsSingle();
    }
    
    private sealed class EnvironmentHider : IInitializable {

        [UsedImplicitly]
        [Inject]
        private readonly GameplayCoreSceneSetupData _setupData = null!;

        [UsedImplicitly]
        [Inject]
        private readonly DiscordManager _discordManager = null!;
        
        public void Initialize() {
            _discordManager.UpdateCurrentActivity(_setupData.difficultyBeatmap.level.songName, largeImageKey: $"https://cdn.beatsaver.com/{_setupData.difficultyBeatmap.level.GetHash().ToLower()}.jpg");
            Console.WriteLine("L: " + _setupData.difficultyBeatmap.level.levelID);
        }

    }

}
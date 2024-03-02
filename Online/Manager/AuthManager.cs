using System;
using System.Collections;

using BetterBeatSaber.Online.Utilities;
using BetterBeatSaber.Shared;
using BetterBeatSaber.Shared.Models;
using BetterBeatSaber.Utilities;

using JetBrains.Annotations;

using UnityEngine;

using Zenject;

namespace BetterBeatSaber.Online.Manager;

internal sealed class AuthManager : Manager<AuthManager>, IInitializable {

    [Inject, UsedImplicitly]
    private readonly IPlatformUserModel _platformUserModel = null!;
    
    [Inject, UsedImplicitly]
    private readonly NetworkManager _networkManager = null!;
    
    [Inject, UsedImplicitly]
    private readonly SharedCoroutineStarter _sharedCoroutineStarter = null!;
    
    public event Action<Player>? OnAuthenticated;
    public event Action? OnFailed;
    
    public Player? Player { get; private set; }
    
    public void Initialize() =>
        _sharedCoroutineStarter.StartCoroutine(Auth());
    
    public IEnumerator Auth() {
        
        yield return new WaitUntil(() => SteamManager.Initialized);

        var ticketTask = _platformUserModel.GetUserAuthToken();
        yield return new WaitUntil(() => ticketTask.IsCompleted);

        /*yield return _networkManager.SendRequest(new AuthRequest {
            Ticket = ticketTask.Result.token ?? string.Empty
        }, out AuthResponse response);

        if (!response.Success || !Player.HasValue) {
            OnFailed?.Invoke();
            yield break;
        }

        Player = response.Player;
        
        OnAuthenticated?.Invoke(Player.Value);*/
        
    }

}
using BetterBeatSaber.Interop;

using Zenject;

namespace BetterBeatSaber.Extensions;

public static class DiContainerExtensions {

    public static void BindInterop<T>(this DiContainer container) where T : Interop<T> =>
        container.BindInterfacesAndSelfTo<T>().AsSingle();

}
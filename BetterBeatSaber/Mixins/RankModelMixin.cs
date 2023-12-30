using System;

using BetterBeatSaber.Mixin;

namespace BetterBeatSaber.Mixins;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming

[Mixin(typeof(RankModel))]
internal static class RankModelMixin {

    [MixinMethod(nameof(GetRankName), MixinAt.Post)]
    private static void GetRankName(ref string __result) {
        Enum.TryParse<RankModel.Rank>(__result, out var rank);
        if (BetterBeatSaberConfig.Instance.ScoreHudModifier.CustomRanks.ContainsKey(rank) && BetterBeatSaberConfig.Instance.ScoreHudModifier.CustomRanks.TryGetValue(rank, out var config) && config.Enable) {
            __result = config.Name;
        }
    }

}
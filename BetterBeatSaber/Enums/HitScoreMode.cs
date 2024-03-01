using System;

namespace BetterBeatSaber.Enums;

[Flags]
internal enum HitScoreMode : byte {

    Accuracy = 2,
    Total = 4,
    
    TimeDependency = 8,
    
    AccuracyWithTimeDependency = Accuracy | TimeDependency,
    TotalWithTimeDependency = Total | TimeDependency
    
}
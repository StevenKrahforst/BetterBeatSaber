using System;

namespace BetterBeatSaber.Enums;

[Flags]
internal enum HitScoreMode : byte {

    Accuracy = 1,
    Total = 2,
    
    TimeDependency = 3,
    
    AccuracyWithTimeDependency = Accuracy | TimeDependency,
    TotalWithTimeDependency = Total | TimeDependency
    
}
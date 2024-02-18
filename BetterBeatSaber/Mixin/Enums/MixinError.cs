namespace BetterBeatSaber.Mixin.Enums;

public enum MixinError {

    // General
    Conflict,

    // Mixin / Mixin Type
    PluginNotFound,
    TypeNotFound,
    
    // Original Method
    OriginalMethodNotFound,
    OriginalMethodAmbiguousMatch,
    
    // Patching
    UnsupportedOperation,
    
    UnknownError

}
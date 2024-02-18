using System;

using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixin.Exceptions;

public class MixinObjectException(
    MixinError error,
    MixinObject obj,
    string message,
    Exception? innerException = null
) : MixinException(error, message, innerException) {

    public MixinObject Object { get; } = obj;

    public MixinObjectException(MixinError error, MixinObject obj, Exception? innerException = null) : this(error, obj, innerException?.Message ?? error.ToString(), innerException) { }

}
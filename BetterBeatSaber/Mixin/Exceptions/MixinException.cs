using System;

using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixin.Exceptions;

public abstract class MixinException(
    MixinError error,
    string message,
    Exception? innerException = null
) : Exception($"{error.ToString()}: {message}", innerException) {

    public MixinError Error { get; } = error;

    protected MixinException(MixinError error, Exception? innerException = null) : this(error, innerException != null ? $"{error.ToString()}: {innerException.Message}" : error.ToString(), innerException) {}

}
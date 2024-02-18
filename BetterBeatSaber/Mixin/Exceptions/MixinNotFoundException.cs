using System;

using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixin.Exceptions;

public sealed class MixinNotFoundException(
    MixinError error,
    string message,
    Exception? innerException = null
) : MixinException(error, message, innerException);
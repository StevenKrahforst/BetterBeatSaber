using System;

namespace BetterBeatSaber.Mixin.TypeResolvers;

public sealed class TypeProvider(Type type) : TypeResolver(type.FullName) {

    public Type Type { get; } = type;

    public override Type ResolveType() =>
        Type;

}
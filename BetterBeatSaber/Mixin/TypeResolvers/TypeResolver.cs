using System;

namespace BetterBeatSaber.Mixin.TypeResolvers;

public abstract class TypeResolver(string typeName) {

    protected string TypeName { get; } = typeName;

    public abstract Type ResolveType();

}
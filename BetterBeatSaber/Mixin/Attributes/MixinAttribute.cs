using System;

using BetterBeatSaber.Mixin.TypeResolvers;

namespace BetterBeatSaber.Mixin.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class MixinAttribute(
    TypeResolver typeResolver,
    string[] conflictsWith
) : Attribute {

    public TypeResolver TypeResolver { get; } = typeResolver;
    public string[] ConflictsWith { get; } = conflictsWith;

    public MixinAttribute(Type type, params string[] conflictsWith) : this(new TypeProvider(type), conflictsWith) { }

    public MixinAttribute(string typeName, params string[] conflictsWith) : this(new AppDomainResolver(typeName), conflictsWith) { }

    public MixinAttribute(string plugin, string typeName, params string[] conflictsWith) : this(new PluginResolver(plugin, typeName), conflictsWith) { }

}
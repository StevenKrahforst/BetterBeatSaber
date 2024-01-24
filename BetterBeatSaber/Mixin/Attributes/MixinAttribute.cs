using System;
using System.Linq;

namespace BetterBeatSaber.Mixin.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MixinAttribute : Attribute {

    public Type? Type { get; set; }
    public string[] ConflictsWith { get; set; }

    // ReSharper disable once MemberCanBeProtected.Global
    public MixinAttribute(string typeName, params string[] conflictsWith)  {
        // ReSharper disable once VirtualMemberCallInConstructor
        Type = ResolveType(typeName);
        ConflictsWith = conflictsWith;
    }

    public MixinAttribute(Type type, params string[] conflictsWith) {
        Type = type;
        ConflictsWith = conflictsWith;
    }

    protected virtual Type? ResolveType(string typeName) =>
        AppDomain.CurrentDomain.GetAssemblies().Reverse().Select(assembly => assembly.GetType(typeName)).FirstOrDefault(type => type != null);
    
    public virtual bool ShouldRun() => true;

}
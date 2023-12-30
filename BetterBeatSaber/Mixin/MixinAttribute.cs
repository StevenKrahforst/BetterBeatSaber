using System;
using System.Linq;

namespace BetterBeatSaber.Mixin;

[AttributeUsage(AttributeTargets.Class)]
public class MixinAttribute : Attribute {

    public Type? Type { get; set; }

    // ReSharper disable once MemberCanBeProtected.Global
    public MixinAttribute(string typeName)  {
        // ReSharper disable once VirtualMemberCallInConstructor
        Type = ResolveType(typeName);
    }

    public MixinAttribute(Type type) {
        Type = type;
    }

    protected virtual Type? ResolveType(string typeName) =>
        AppDomain.CurrentDomain.GetAssemblies().Reverse().Select(assembly => assembly.GetType(typeName)).FirstOrDefault(type => type != null);
    
    public virtual bool ShouldRun() => true;

}
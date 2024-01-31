using System;
using System.Linq;

using BetterBeatSaber.Mixin.Enums;

namespace BetterBeatSaber.Mixin.TypeResolvers;

public sealed class AppDomainResolver(string typeName) : TypeResolver(typeName) {

    public override Type ResolveType() {
        
        var type = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .Reverse()
            .Select(assembly => assembly.GetType(TypeName))
            .FirstOrDefault(type => type != null);
        
        if(type == null)
            throw new MixinException(MixinError.TypeNotFound, $"Type {TypeName} not found in current AppDomain");
        
        return type;
        
    }

}
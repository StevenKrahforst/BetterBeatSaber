using BetterBeatSaber.Server.Models;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

namespace BetterBeatSaber.Server;

public sealed class DatabaseContext : DbContext {

    [UsedImplicitly]
    public DbSet<Player> Players { get; set; } = null!;

}
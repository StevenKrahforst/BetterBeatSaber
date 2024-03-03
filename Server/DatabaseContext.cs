using BetterBeatSaber.Server.Models;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

namespace BetterBeatSaber.Server;

public sealed class DatabaseContext : DbContext {

    public DatabaseContext() { }
    public DatabaseContext(DbContextOptions options) : base(options) { }

    [UsedImplicitly]
    public DbSet<Player> Players { get; set; } = null!;

}
using System.ComponentModel.DataAnnotations;

namespace BetterBeatSaber.Server.Models;

public sealed class Player {

    [Key]
    public ulong Id { get; init; }

    [MaxLength(64)]
    public required string Name { get; set; }
    
}
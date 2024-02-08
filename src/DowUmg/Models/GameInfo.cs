using System.Collections.Generic;
using DowUmg.Data.Entities;

namespace DowUmg.Models
{
    public class GameInfo
    {
        public GameOptions Options { get; set; } = null!;
        public List<GameRule> Rules { get; set; } = new List<GameRule>();
    }
}

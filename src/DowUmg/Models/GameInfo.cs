using System.Collections.Generic;
using DowUmg.Data.Entities;

namespace DowUmg.Models
{
    public class GameInfo(GameOptions options)
    {
        public GameOptions Options { get; set; } = options;
        public List<GameRule> Rules { get; set; } = [];
    }
}

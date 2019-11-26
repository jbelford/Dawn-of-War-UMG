using Microsoft.EntityFrameworkCore;

namespace DowUmg.Data.Entities
{
    [Owned]
    public class CustomRule
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Details { get; set; } = null!;
    }
}

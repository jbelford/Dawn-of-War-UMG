using DowUmg.Services.Interfaces;

namespace DowUmg.Services.Models
{
    public class Alliance : InfoObject
    {
        public int Id { get; }
        public IImageSource Image { get; }
    }
}

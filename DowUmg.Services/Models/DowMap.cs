using DowUmg.Services.Interfaces;

namespace DowUmg.Services.Models
{
    public class DowMap : InfoObject
    {
        public byte Players { get; }
        public byte Size { get; }
        public IImageSource Image { get; }
    }
}

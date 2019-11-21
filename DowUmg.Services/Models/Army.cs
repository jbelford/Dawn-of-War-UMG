using DowUmg.Services.Interfaces;

namespace DowUmg.Services.Models
{
    public class Army : InfoObject
    {
        public string Race { get; }
        public Alliance Alliance { get; }
        public IImageSource Image { get; }
    }
}

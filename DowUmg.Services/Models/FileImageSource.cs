using DowUmg.Services.Interfaces;

namespace DowUmg.Services.Models
{
    public class FileImageSource : IImageSource
    {
        private string path;

        public FileImageSource(string path)
        {
            this.path = path;
        }
    }
}

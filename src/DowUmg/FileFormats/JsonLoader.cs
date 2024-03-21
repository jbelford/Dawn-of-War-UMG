using System.IO;
using Newtonsoft.Json;

namespace DowUmg.FileFormats
{
    internal class JsonLoader<T> : IFileLoader<T>
    {
        public T Load(string filePath)
        {
            using var r = new StreamReader(filePath);
            string json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(json);
        }

        public void Write(string path, T obj)
        {
            var fileInfo = new FileInfo(path);
            fileInfo.Directory.Create();

            string json = JsonConvert.SerializeObject(obj);
            using var w = new StreamWriter(path);
            w.Write(json);
        }
    }
}

using Newtonsoft.Json;
using System.IO;

namespace DowUmg.Services
{
    public class DataLoader
    {
        public T LoadJson<T>(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public void SaveJson(string path, object obj)
        {
            CreateDirectoryPath(path);
            string json = JsonConvert.SerializeObject(obj);
            using (StreamWriter w = new StreamWriter(path))
            {
                w.Write(json);
            }
        }

        private void CreateDirectoryPath(string path)
        {
            var fileInfo = new FileInfo(path);
            fileInfo.Directory.Create();
        }
    }
}

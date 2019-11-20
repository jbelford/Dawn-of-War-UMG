using Newtonsoft.Json;
using System.IO;

namespace DowUmg.Services
{
    public class DataLoader
    {
        public T Load<T>(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public void Save(string path, object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            using (StreamWriter w = new StreamWriter(path))
            {
                w.Write(json);
            }
        }
    }
}
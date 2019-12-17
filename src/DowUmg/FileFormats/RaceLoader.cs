using DowUmg.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace DowUmg.FileFormats
{
    internal class RaceFile
    {
        public RaceFile(string name, string desc, bool playable)
        {
            Name = name;
            Description = desc;
            Playable = playable;
        }

        public string Description { get; }
        public string Name { get; }
        public bool Playable { get; }
    }

    internal class RaceLoader : IFileLoader<RaceFile>
    {
        private const uint RaceDetailsHash = 954215234;
        private const uint NameHash = 2683408083;
        private const uint DescHash = 3018961026;
        private const uint PlayableHash = 1272208871;

        public RaceFile Load(string filePath)
        {
            return Load(File.OpenRead(filePath));
        }

        public RaceFile Load(Stream stream)
        {
            var rgd = new RgdReader();
            RgdFile file = rgd.Read(stream);
            try
            {
                if (file.Entries[RaceDetailsHash] is RgdEntry<Dictionary<uint, IRgdEntry>> details
                    && details.Value[NameHash] is RgdEntry<string> name
                    && details.Value[DescHash] is RgdEntry<string> desc
                    && details.Value[PlayableHash] is RgdEntry<bool> playable)
                {
                    return new RaceFile(name.Value, desc.Value, playable.Value);
                }
                throw new IOException("Race details is malformatted");
            }
            catch (KeyNotFoundException ex)
            {
                throw new IOException("Could not read race details due to missing key", ex);
            }
        }
    }
}

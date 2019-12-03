using DowUmg.Interfaces;
using System.IO;
using System.Text;

namespace DowUmg.FileFormats
{
    public class MapFile
    {
        public int Players { get; set; }
        public int Size { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }

    /**
    * Chunky files are LITTLE-ENDIAN
    *
    * Chunk Header Format:
    * 4 byte chunk type "DATA"
    * 4 byte chunk ID "WHMD"
    * 4 byte chunk version
    * 4 byte chunk size
    * 4 byte name size
    * X byte name
    *
    * Map Chunk Format:
    * 4 byte player size
    * 4 byte map size
    * 4 byte mod/folder/something size
    * X byte mod/folder/something
    * 4 16-bit map name size
    * X byte map
    * 4 16-bit map description size
    * X byte description
    */

    public class MapLoader : IFileLoader<MapFile>
    {
        /// <summary>
        /// Loads information from an SGB map file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="IOException" />
        public MapFile Load(string filePath)
        {
            return Load(File.OpenRead(filePath));
        }

        public MapFile Load(Stream stream)
        {
            using var reader = new BinaryReader(stream);

            string chunky = Encoding.UTF8.GetString(reader.ReadBytes(12));
            if (!"Relic Chunky".Equals(chunky))
            {
                throw new IOException($"Not a Relic Chunky");
            }

            // Unknown yet whether DATAWHMD always starts here or not.
            reader.BaseStream.Seek(44, SeekOrigin.Begin);

            string label = Encoding.UTF8.GetString(reader.ReadBytes(8));
            if (!"DATAWMHD".Equals(label))
            {
                throw new IOException($"Could not read due to malformatted data");
            }

            reader.BaseStream.Seek(8, SeekOrigin.Current);

            int namesize = reader.ReadInt32();

            reader.BaseStream.Seek(namesize, SeekOrigin.Current);

            int players = reader.ReadInt32();
            int size = reader.ReadInt32();
            int modNameSize = reader.ReadInt32();

            reader.BaseStream.Seek(modNameSize, SeekOrigin.Current);

            int nameLength = reader.ReadInt32();
            string name = Encoding.Unicode.GetString(reader.ReadBytes(nameLength * 2));

            int descLength = reader.ReadInt32();
            string description = Encoding.Unicode.GetString(reader.ReadBytes(descLength * 2));

            return new MapFile
            {
                Name = name,
                Description = description,
                Players = players,
                Size = size
            };
        }
    }
}

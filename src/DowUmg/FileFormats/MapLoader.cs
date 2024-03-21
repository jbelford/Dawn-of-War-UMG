using System;
using System.IO;
using System.Text;
using DowUmg.Constants;

namespace DowUmg.FileFormats
{
    /**
    * Chunky files are LITTLE-ENDIAN
    *
    * First 11 bytes are UTF8 "Chunky File"
    *
    * 12 bytes unknown
    *
    * 8 bytes "FOLDSCEN"
    * 4 byte YEAR
    * 4 byte unknown - could be time of the year / size of the folder
    * 4 byte size of some  text before reaching DATAWHMD
    * X bytes of some text
    *
    * 8 bytes "DATAWHMD"
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

    public class MapFile : IEquatable<MapFile?>
    {
        public string FileName { get; set; } = null!;
        public int Players { get; set; }
        public int Size { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        #region Generated

        public override bool Equals(object? obj)
        {
            return Equals(obj as MapFile);
        }

        public bool Equals(MapFile? other)
        {
            return other is not null
                && FileName == other.FileName
                && Players == other.Players
                && Size == other.Size
                && Name == other.Name
                && Description == other.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FileName, Players, Size, Name, Description);
        }

        #endregion
    }

    internal class MapLoader : IFileLoader<MapFile>
    {
        /// <summary>
        /// Loads information from an SGB map file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="IOException" />
        public MapFile Load(string filePath)
        {
            return Load(File.OpenRead(filePath), filePath);
        }

        public MapFile Load(Stream stream, string filePath)
        {
            using var reader = new BinaryReader(stream);

            string chunky = Encoding.UTF8.GetString(reader.ReadBytes(12));
            if (!"Relic Chunky".Equals(chunky))
            {
                throw new IOException($"Not a Relic Chunky");
            }

            reader.BaseStream.Seek(12, SeekOrigin.Current);
            string foldscen = Encoding.UTF8.GetString(reader.ReadBytes(8));
            if (!"FOLDSCEN".Equals(foldscen))
            {
                throw new IOException($"Could not read due to malformatted data");
            }

            reader.BaseStream.Seek(8, SeekOrigin.Current);
            reader.BaseStream.Seek(reader.ReadInt32(), SeekOrigin.Current);

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
                Size = (int)RoundMapSize(size),
                FileName = Path.GetFileNameWithoutExtension(filePath)
            };
        }

        private MapSize RoundMapSize(int mapSize) =>
            mapSize switch
            {
                (< 192) => MapSize.TINY,
                (< 386) => MapSize.SMALL,
                (< 769) => MapSize.MEDIUM,
                _ => MapSize.LARGE
            };
    }
}

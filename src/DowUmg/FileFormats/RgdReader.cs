using DowUmg.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DowUmg.FileFormats
{
    /**
     * RGD File Format:
     *
     * RGD files are pre-compiled LUA used by Dawn of War to supposedly improve performance.
     * They are stored as Relic Chunky files and usually only have one chunk item
     * "DATAAEGD"
     *
     * Chunky Header:
     * 16 bytes "Relic Chunky\x0D\x0A\x1A\x00"
     * 4 byte version
     * 4 byte unknown
     * Version 3 only:
     *   4 byte unknown
     *   4 byte unknown
     *   4 byte unknown
     *
     * Rgd Chunk:
     * 4 byte chunky type "DATA"
     * 4 byte chunky id "AEGD"
     * 4 byte version
     * 4 byte chunk length
     * 4 byte chunk name length
     * X byte chunk name
     * Version 3 only:
     *   4 byte unknown
     *   4 byte unknown
     *
     * 4 byte crc
     * 4 byte data length
     * x byte data
     *
     * Rgd Data Entry Header:
     * 4 byte key count
     * 4 byte hash
     * 4 byte type
     *
     *
     *
     *
     * Rgd Data Types:
     *
     * table (A list of sub entries)
     * int
     * float
     * string (utf8 / utf16le)
     * bool
     *
     */

    internal interface IRgdEntry
    {
        public uint Hash { get; }
    }

    internal enum RgdDataType : int
    {
        Float = 0,
        Integer = 1,
        Bool = 2,
        String = 3,
        WString = 4,
        Table = 100,
        NoData = 254
    }

    internal class RgdFile
    {
        public RgdFile(Dictionary<uint, IRgdEntry> entries)
        {
            Entries = entries;
        }

        public Dictionary<uint, IRgdEntry> Entries { get; }
    }

    internal class RgdEntry<T> : IRgdEntry
    {
        public RgdEntry(uint hash, T value)
        {
            Hash = hash;
            Value = value;
        }

        public T Value { get; }

        public uint Hash { get; }
    }

    internal class RgdReader
    {
        public RgdFile Read(Stream stream)
        {
            byte[] dataBuff;

            using (var reader = new BinaryReader(stream))
            {
                string chunky = Encoding.UTF8.GetString(reader.ReadBytes(12));
                if (!"Relic Chunky".Equals(chunky))
                {
                    throw new IOException("Not a Relic Chunky");
                }

                reader.BaseStream.Seek(4, SeekOrigin.Current);

                int rgdVersion = reader.ReadInt32();
                bool isV3 = rgdVersion == 3;

                int skipIntegers = isV3 ? 4 : 1;
                reader.BaseStream.Seek(skipIntegers * 4, SeekOrigin.Current);

                string datachunk = Encoding.UTF8.GetString(reader.ReadBytes(8));
                if (!"DATAAEGD".Equals(datachunk))
                {
                    throw new IOException("Could not read due to malformatted data");
                }

                reader.BaseStream.Seek(8, SeekOrigin.Current);
                reader.BaseStream.Seek(reader.ReadInt32(), SeekOrigin.Current);

                if (isV3)
                {
                    reader.BaseStream.Seek(8, SeekOrigin.Current);
                }

                reader.BaseStream.Seek(4, SeekOrigin.Current);

                dataBuff = reader.ReadBytes(reader.ReadInt32());
            }

            return new RgdFile(ReadEntries(dataBuff, 0));
        }

        private Dictionary<uint, IRgdEntry> ReadEntries(in byte[] data, int pos)
        {
            int keyCount = BitConverter.ToInt32(data, pos);
            int newPos = pos + 4;
            int dataOffset = newPos + keyCount * (3 * 4);

            var entries = new Dictionary<uint, IRgdEntry>(keyCount);

            for (int i = 0; i < keyCount; ++i)
            {
                IRgdEntry entry = ReadEntry(data, newPos + (i * 3 * 4), dataOffset);
                entries[entry.Hash] = entry;
            }

            return entries;
        }

        private IRgdEntry ReadEntry(in byte[] data, int pos, int dataOffset)
        {
            uint hash = BitConverter.ToUInt32(data, pos);
            int type = BitConverter.ToInt32(data, pos + 4);
            int entryOffset = BitConverter.ToInt32(data, pos + 8);

            int startIndex = dataOffset + entryOffset;
            return ((RgdDataType)type) switch
            {
                RgdDataType.Float => new RgdEntry<float>(hash, BitConverter.ToSingle(data, startIndex)),
                RgdDataType.Integer => new RgdEntry<int>(hash, BitConverter.ToInt32(data, startIndex)),
                RgdDataType.Bool => new RgdEntry<bool>(hash, BitConverter.ToBoolean(data, startIndex)),
                RgdDataType.String => new RgdEntry<string>(hash, Parsing.GetAsciiString(data, startIndex)),
                RgdDataType.WString => new RgdEntry<string>(hash, Parsing.GetUnicodeString(data, startIndex)),
                RgdDataType.Table => new RgdEntry<Dictionary<uint, IRgdEntry>>(hash, ReadEntries(data, startIndex)),
                _ => throw new Exception($"Unknown data type encountered {type}"),
            };
        }
    }
}

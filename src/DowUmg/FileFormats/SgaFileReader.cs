//===== SGA File Format =====
//0x000000 - Identifier ASCII String(8)
//0x000008 - Version(4)
//0x00000C - Unknown(16) (First MD5 hash)
//0x00001C - Archive Type UNICODE String(128)
//0x00009C - Majorly Unknown(16) (Second MD5 hash)
//0x0000AC - Data Header Size(4)
//0x0000B0 - Data Offset(4)
//0x0000B4 - Platform (ONLY if Version V4)

//0x0000B4 - TOC Offset(4) (Offset from B4)
//0x0000B8 - TOC Count(2)
//0x0000BA - Dir Offset(4) (Offset from B4) - Points to an array of Directory Info
//0x0000BE - Dir Count(2)
//0x0000C0 - File Offset(4) (Offset from B4) - Points to an array of File Info
//0x0000C4 - File Count(2)
//0x0000C6 - Item Offset(4) (Offset from B4)
//0x0000CA - Item Count(2)

//0x0000CC - TOC Alias ASCII String(64) (Addr: B4 + TOC Offset)
//0x00010C - TOC Start Name ASCII String(64)
//0x00014C - TOC Start Dir(2)
//0x00014E - TOC End Dir(2)
//0x000150 - TOC Start File(2)
//0x000152 - TOC End File(2)
//0x000154 - TOC Folder Offset(4)

//"Directory Info"
//0x000 - Name Offset(4) (+180+ItemOffset)
//0x004 - Sub Dir ID Begin(2)
//0x006 - Sub Dir ID End(2) (Begin->End-1)
//0x008 - File ID Begin(2)
//0x010 - File ID End(2) (Begin+1->End)=8

//"File Info"
//0x000 - Name Offset(4) (+181+ItemOffset)
//0x004 - Unknown(4)
//0x008 - Data Offset(4) (+ B0 Data Offset)
//0x00C - Unknown(4)
//0x010 - Data Len(4)

using DowUmg.Extensions;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DowUmg.FileFormats
{
    /// <summary>
    /// Header for SGA file
    /// <para>0x000000 - Identifier ASCII String(8)</para>
    /// <para>0x000008 - Version(4)</para>
    /// <para>0x00000C - Unknown(16) (First MD5 hash)</para>
    /// <para>0x00001C - Archive Type UNICODE String(128)</para>
    /// <para>0x00009C - Majorly Unknown(16) (Second MD5 hash)</para>
    /// <para>0x0000AC - Data Header Size(4)</para>
    /// <para>0x0000B0 - Data Offset(4)</para>
    /// <para>0x0000B4 - Platform (ONLY if Version V4)</para>
    /// </summary>
    internal struct SgaFileHeader
    {
        public string Identifier;
        public uint Version;
        public byte[] ToolMD5;
        public string ArchiveType;
        public byte[] MD5;
        public uint DataHeaderSize;
        public uint DataOffset;
        public uint Platform;
    }

    /// <summary>
    /// Header for the data contents
    /// <para>0x0000B4 - TOC Offset(4) (Offset from B4) - Where in the data header ToCs start</para>
    /// <para>0x0000B8 - TOC Count(2)</para>
    /// <para>0x0000BA - Dir Offset(4) (Offset from B4) - Where in the data header directories start</para>
    /// <para>0x0000BE - Dir Count(2)</para>
    /// <para>0x0000C0 - File Offset(4) (Offset from B4) - Where in the data header files start</para>
    /// <para>0x0000C4 - File Count(2)</para>
    /// <para>0x0000C6 - Item Offset(4) (Offset from B4) - Where in the data header strings start</para>
    /// <para>0x0000CA - Item Count(2)</para>
    /// </summary>
    internal struct SgaDataHeaderInfo
    {
        public uint ToCOffset;
        public ushort ToCCount;
        public uint DirOffset;
        public ushort DirCount;
        public uint FileOffset;
        public ushort FileCount;
        public uint ItemOffset;
        public ushort ItemCount;
    }

    /// <summary>
    /// <para>0x0000CC - TOC Alias ASCII String(64) (Addr: B4 + TOC Offset)</para>
    /// <para>0x00010C - TOC Start Name ASCII String(64)</para>
    /// <para>0x00014C - TOC Start Dir(2)</para>
    /// <para>0x00014E - TOC End Dir(2)</para>
    /// <para>0x000150 - TOC Start File(2)</para>
    /// <para>0x000152 - TOC End File(2)</para>
    /// <para>0x000154 - TOC Folder Offset(4)</para>
    /// </summary>
    internal struct SgaToCInfo
    {
        public string Alias;
        public string BaseDirName;
        public ushort StartDir;
        public ushort EndDir;
        public ushort StartFile;
        public ushort EndFile;
        public uint FolderOffset;
    }

    /// <summary>
    /// Directory Info
    /// <para>0x000 - Name Offset(4) (+180+ItemOffset)</para>
    /// <para>0x004 - Sub Dir ID Begin(2)</para>
    /// <para>0x006 - Sub Dir ID End(2) (Begin->End-1)</para>
    /// <para>0x008 - File ID Begin(2)</para>
    /// <para>0x010 - File ID End(2) (Begin+1->End)=8</para>
    /// </summary>
    internal struct SgaDirInfo
    {
        public uint NameOffset;
        public ushort SubDirBegin;
        public ushort SubDirEnd;
        public ushort FileBegin;
        public ushort FileEnd;
    }

    /// <summary>
    /// File Info v2
    /// <para>0x000 - Name Offset(4) (+181+ItemOffset)</para>
    /// <para>0x004 - Flags (4) (0x00 = Uncompressed, 0x10 = zlib large file, 0x20 = zlib small file)</para>
    /// <para>0x008 - Data Offset(4) (+ B0 Data Offset)</para>
    /// <para>0x00C - Data Length Compressed(4)</para>
    /// <para>0x010 - Data Length (4)</para>
    /// File Info v4
    /// <para>0x000 - Name Offset(4) (+181+ItemOffset)</para>
    /// <para>0x004 - Data Offset(4) (+ B0 Data Offset)</para>
    /// <para>0x008 - Data Length Compressed(4)</para>
    /// <para>0x00C - Data Length (4)</para>
    /// <para>0x010 - Modification Time (4)</para>
    /// <para>0x014 - Flags (4) (0x00 = Uncompressed, 0x10 = zlib large file, 0x20 = zlib small file)</para>
    /// </summary>
    internal struct SgaFileInfo
    {
        public uint NameOffset;
        public uint Flags;
        public uint DataOffset;
        public uint DataLengthCompressed;
        public uint DataLength;
    }

    public class InvalidSgaException : IOException
    {
        public InvalidSgaException(string message) : base(message)
        {
        }
    }

    public class SgaFileReader : IDisposable
    {
        private readonly BinaryReader reader;

        private readonly SgaFileHeader header;
        private readonly SgaDirectory[] directories;

        /// <summary>
        /// Create a new SgaFileReader. Will immediately read the header information
        /// for the file and throw <see cref="InvalidSgaException"/> if stream is not a valid SGA.
        /// </summary>
        /// <param name="stream">A SGA file stream</param>
        /// <exception cref="IOException" />
        public SgaFileReader(string filePath)
        {
            this.reader = new BinaryReader(File.OpenRead(filePath));
            try
            {
                this.header = ReadFileHeader();
                byte[] dataHeaderBuffer = ReadDataHeader(header);
                SgaDataHeaderInfo dataHeaderInfo = ReadDataHeaderInfo(dataHeaderBuffer);

                SgaFile[] files = ReadFiles(header, dataHeaderInfo, dataHeaderBuffer);
                SgaDirectory[] directories = ReadDirs(files, dataHeaderInfo, dataHeaderBuffer);

                var tocs = ReadTocs(directories, dataHeaderInfo, dataHeaderBuffer);
                if (tocs.Length != 1)
                {
                    throw new IOException($"SGA file does not have exactly 1 TOC {filePath}");
                }

                this.directories = directories;
            }
            catch (IOException ex)
            {
                Dispose();
                throw ex;
            }
        }

        public void Dispose()
        {
            this.reader.Dispose();
        }

        public IObservable<SgaRawFile> GetScenarioImages()
        {
            return GetFiles(@"scenarios\mp", @"((_icon|_mm)(_custom)?)\.tga$");
        }

        public IObservable<SgaRawFile> GetWinConditions()
        {
            return GetFiles(@"scar\winconditions", @"_local\.lua$");
        }

        public IObservable<SgaRawFile> GetLocales()
        {
            return GetFiles(@"Locale\English", @"\.ucs");
        }

        private IObservable<SgaRawFile> GetFiles(string directoryPath, string filePattern)
        {
            int pos = this.directories.BinarySearch((directory) => directoryPath.CompareTo(directory.Name));
            if (pos < 0)
            {
                return Observable.Empty<SgaRawFile>();
            }

            SgaDirectory dir = this.directories[pos];

            return Observable.Create<SgaRawFile>((observer, cancellationToken) => Task.Factory.StartNew(
                () =>
                {
                    var reg = new Regex(filePattern);
                    SgaFile[] files = dir.Files.Where(x => reg.IsMatch(x.Name)).ToArray();

                    foreach (var file in files)
                    {
                        observer.OnNext(ReadFile(file));
                    }

                    observer.OnCompleted();
                },
                cancellationToken,
                TaskCreationOptions.None,
                TaskScheduler.Default));
        }

        private SgaRawFile ReadFile(SgaFile file)
        {
            this.reader.BaseStream.Seek(Convert.ToInt32(this.header.DataOffset) + file.Info.DataOffset, SeekOrigin.Begin);

            byte[] data = this.reader.ReadBytes(Convert.ToInt32(file.Info.DataLengthCompressed));

            if (file.Info.DataLength != file.Info.DataLengthCompressed)
            {
                using var outputStream = new MemoryStream();
                using (var inputStream = new InflaterInputStream(new MemoryStream(data)))
                {
                    inputStream.CopyTo(outputStream);
                }
                data = outputStream.ToArray();
            }

            return new SgaRawFile(file.Name, data);
        }

        /// <summary>
        /// Read file header data using Binary Reader. Assumes binary reader is located at beginning
        /// of header info.
        /// </summary>
        /// <exception cref="IOException"/>
        private SgaFileHeader ReadFileHeader()
        {
            string identifier = Encoding.ASCII.GetString(this.reader.ReadBytes(8));
            if (!"_ARCHIVE".Equals(identifier))
            {
                throw new InvalidSgaException($"File Identifier '{identifier}' should be '_ARCHIVE'");
            }

            var header = new SgaFileHeader
            {
                Identifier = identifier,
                Version = this.reader.ReadUInt32(),
                ToolMD5 = this.reader.ReadBytes(16),
                ArchiveType = Encoding.Unicode.GetString(this.reader.ReadBytes(128)).TrimEnd('\0'),
                MD5 = this.reader.ReadBytes(16),
                DataHeaderSize = this.reader.ReadUInt32(),
                DataOffset = this.reader.ReadUInt32()
            };

            if (header.Version != 2 && header.Version != 4)
            {
                throw new InvalidSgaException("SGA Version is not supported");
            }

            if (header.Version == 4)
            {
                header.Platform = this.reader.ReadUInt32();
                // Value of 1 means Win32, x86 or Little Endian.
                // It is unknown what a value not of 1 is so we should throw an error.
                if (header.Platform != 1)
                {
                    throw new InvalidSgaException($"Unknown Platform '{header.Platform}'");
                }
            }

            return header;
        }

        /// <summary>
        /// Reads the Data header into buffer and verifies the checksum.
        /// </summary>
        private byte[] ReadDataHeader(SgaFileHeader header)
        {
            var data = this.reader.ReadBytes(Convert.ToInt32(header.DataHeaderSize));

            // Here we validate that the checksum matches. If this step passes there is good odds that we are
            // indeed reading an SGA file.
            using MD5 md5 = MD5.Create();

            byte[] key = Encoding.ASCII.GetBytes("DFC9AF62-FC1B-4180-BC27-11CCE87D3EFF");

            var buf = new byte[key.Length + data.Length];
            key.CopyTo(buf, 0);
            data.CopyTo(buf, key.Length);

            byte[] md5Main = md5.ComputeHash(buf);

            if (!md5Main.SequenceEqual(header.MD5))
            {
                throw new InvalidSgaException("Header MD5 does not match");
            }

            return data;
        }

        private SgaDataHeaderInfo ReadDataHeaderInfo(in byte[] dataHeaderBuffer)
        {
            return new SgaDataHeaderInfo
            {
                ToCOffset = BitConverter.ToUInt32(dataHeaderBuffer, 0),
                ToCCount = BitConverter.ToUInt16(dataHeaderBuffer, 4),
                DirOffset = BitConverter.ToUInt32(dataHeaderBuffer, 6),
                DirCount = BitConverter.ToUInt16(dataHeaderBuffer, 10),
                FileOffset = BitConverter.ToUInt32(dataHeaderBuffer, 12),
                FileCount = BitConverter.ToUInt16(dataHeaderBuffer, 16),
                ItemOffset = BitConverter.ToUInt32(dataHeaderBuffer, 18),
                ItemCount = BitConverter.ToUInt16(dataHeaderBuffer, 22)
            };
        }

        private SgaToc[] ReadTocs(in SgaDirectory[] directories, SgaDataHeaderInfo dataHeaderInfo, in byte[] dataHeaderBuffer)
        {
            var tocs = new SgaToc[dataHeaderInfo.ToCCount];

            for (int i = 0; i < tocs.Length; ++i)
            {
                int offset = Convert.ToInt32(dataHeaderInfo.ToCOffset) + i * 140;

                SgaToCInfo info = new SgaToCInfo
                {
                    Alias = Encoding.ASCII.GetString(dataHeaderBuffer, offset, 64).TrimEnd('\0'),
                    BaseDirName = Encoding.ASCII.GetString(dataHeaderBuffer, offset + 64, 64).TrimEnd('\0'),
                    StartDir = BitConverter.ToUInt16(dataHeaderBuffer, offset + 128),
                    EndDir = BitConverter.ToUInt16(dataHeaderBuffer, offset + 130),
                    StartFile = BitConverter.ToUInt16(dataHeaderBuffer, offset + 132),
                    EndFile = BitConverter.ToUInt16(dataHeaderBuffer, offset + 134),
                    FolderOffset = BitConverter.ToUInt32(dataHeaderBuffer, offset + 136)
                };

                tocs[i] = new SgaToc(directories[info.StartDir], info);
            }

            return tocs;
        }

        private SgaDirectory[] ReadDirs(in SgaFile[] files, SgaDataHeaderInfo dataHeaderInfo, in byte[] dataHeaderBuffer)
        {
            var dirs = new SgaDirectory[dataHeaderInfo.DirCount];

            for (int i = 0; i < dirs.Length; ++i)
            {
                int offset = Convert.ToInt32(dataHeaderInfo.DirOffset) + i * 12;

                var info = new SgaDirInfo
                {
                    NameOffset = BitConverter.ToUInt32(dataHeaderBuffer, offset),
                    SubDirBegin = BitConverter.ToUInt16(dataHeaderBuffer, offset + 4),
                    SubDirEnd = BitConverter.ToUInt16(dataHeaderBuffer, offset + 6),
                    FileBegin = BitConverter.ToUInt16(dataHeaderBuffer, offset + 8),
                    FileEnd = BitConverter.ToUInt16(dataHeaderBuffer, offset + 10)
                };

                string name = GetAsciiString(dataHeaderBuffer, Convert.ToInt32(dataHeaderInfo.ItemOffset + info.NameOffset));

                var dir = new SgaDirectory(name, info);

                for (int j = info.FileBegin; j < info.FileEnd; ++j)
                {
                    dir.Files.Add(files[j]);
                }

                dirs[i] = dir;
            }

            for (int i = 0; i < dirs.Length; ++i)
            {
                SgaDirectory dir = dirs[i];
                for (int j = dir.Info.SubDirBegin; j < dir.Info.SubDirEnd; ++j)
                {
                    dir.Directories.Add(dirs[j]);
                }
            }

            return dirs;
        }

        private SgaFile[] ReadFiles(SgaFileHeader header, SgaDataHeaderInfo dataHeaderInfo, in byte[] dataHeaderBuffer)
        {
            var files = new SgaFile[dataHeaderInfo.FileCount];

            int infoSize = header.Version == 2 ? 20 : 22;

            for (int i = 0; i < files.Length; ++i)
            {
                int offset = Convert.ToInt32(dataHeaderInfo.FileOffset) + i * infoSize;

                SgaFileInfo info;

                if (header.Version == 2)
                {
                    info = new SgaFileInfo
                    {
                        NameOffset = BitConverter.ToUInt32(dataHeaderBuffer, offset),
                        Flags = BitConverter.ToUInt32(dataHeaderBuffer, offset + 4),
                        DataOffset = BitConverter.ToUInt32(dataHeaderBuffer, offset + 8),
                        DataLengthCompressed = BitConverter.ToUInt32(dataHeaderBuffer, offset + 12),
                        DataLength = BitConverter.ToUInt32(dataHeaderBuffer, offset + 16)
                    };
                }
                else
                {
                    info = new SgaFileInfo
                    {
                        NameOffset = BitConverter.ToUInt32(dataHeaderBuffer, offset),
                        DataOffset = BitConverter.ToUInt32(dataHeaderBuffer, offset + 4),
                        DataLengthCompressed = BitConverter.ToUInt32(dataHeaderBuffer, offset + 8),
                        DataLength = BitConverter.ToUInt32(dataHeaderBuffer, offset + 12),
                        Flags = BitConverter.ToUInt16(dataHeaderBuffer, offset + 20)
                    };
                }

                string name = GetAsciiString(dataHeaderBuffer, Convert.ToInt32(dataHeaderInfo.ItemOffset + info.NameOffset));

                files[i] = new SgaFile(name, info);
            }

            return files;
        }

        /// <summary>
        /// Returns the null-terminated string at the offset in the buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private string GetAsciiString(in byte[] buffer, int offset)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    return new string((sbyte*)(p + offset));
                }
            }
        }

        public class SgaRawFile
        {
            public SgaRawFile(string name, byte[] data)
            {
                Name = name;
                Data = data;
            }

            public string Name { get; }
            public byte[] Data { get; }
        }
    }

    internal class SgaToc
    {
        public SgaToc(SgaDirectory root, SgaToCInfo info)
        {
            RootDirectory = root;
            Info = info;
        }

        public SgaDirectory RootDirectory { get; }
        public SgaToCInfo Info { get; }
    }

    internal class SgaDirectory
    {
        public SgaDirectory(string name, SgaDirInfo info)
        {
            Name = name;
            Info = info;
        }

        public SgaDirInfo Info { get; }
        public List<SgaDirectory> Directories { get; } = new List<SgaDirectory>();
        public List<SgaFile> Files { get; } = new List<SgaFile>();
        public string Name { get; }
    }

    internal class SgaFile
    {
        public SgaFile(string name, SgaFileInfo info)
        {
            Name = name;
            Info = info;
        }

        public SgaFileInfo Info { get; }
        public string Name { get; }
    }
}

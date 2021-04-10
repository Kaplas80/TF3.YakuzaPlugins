// Copyright (c) 2021 Kaplas
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
namespace TF3.YarhlPlugin.YakuzaCommon.Converters.Par
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaCommon.Enums;
    using TF3.YarhlPlugin.YakuzaCommon.Formats;
    using TF3.YarhlPlugin.YakuzaCommon.Types;
    using Yarhl.FileFormat;
    using Yarhl.FileSystem;
    using Yarhl.IO;

    /// <summary>
    /// Serializes PAR archives.
    /// </summary>
    public class Writer : IConverter<NodeContainerFormat, BinaryFormat>, IInitializer<WriterParameters>
    {
        private WriterParameters _writerParameters = new ();

        /// <summary>
        /// Initializes the writer parameters.
        /// </summary>
        /// <param name="parameters">Writer configuration.</param>
        public void Initialize(WriterParameters parameters) => _writerParameters = parameters;

        /// <summary>
        /// Converts a NodeContainerFormat into a BinaryFormat.
        /// </summary>
        /// <param name="source">Input format.</param>
        /// <returns>The node container format.</returns>
        /// <exception cref="ArgumentNullException">Thrown if source is null.</exception>
        public virtual BinaryFormat Convert(NodeContainerFormat source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Reorder nodes
            source.Root.SortChildren((x, y) =>
                string.CompareOrdinal(x.Name.ToLowerInvariant(), y.Name.ToLowerInvariant()));

            // Fill node indexes
            FillNodeIndexes(source.Root);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            DataStream stream = _writerParameters.OutputStream ?? DataStreamFactory.FromMemory();

            stream.Position = 0;

            var writer = new DataWriter(stream)
            {
                DefaultEncoding = Encoding.GetEncoding(1252),
                Endianness = _writerParameters.Endianness == Endianness.LittleEndian
                    ? EndiannessMode.LittleEndian
                    : EndiannessMode.BigEndian,
            };

            var directories = new List<Node>();
            var files = new List<Node>();
            uint fileOffset = 0;

            directories.Add(source.Root);
            foreach (Node node in Navigator.IterateNodes(source.Root))
            {
                if (!node.IsContainer)
                {
                    if (node.Stream.Length > 0xFFFFFFFF)
                    {
                        throw new FormatException("Can not add files over 4GB");
                    }

                    uint compressedSize = (uint)node.Stream.Length;
                    fileOffset = RoundSize(fileOffset, compressedSize);

                    files.Add(node);
                }
                else
                {
                    directories.Add(node);
                }
            }

            var header = new FileHeader
            {
                Magic = "PARC",
                PlatformId = _writerParameters.PlatformId,
                Endianness = _writerParameters.Endianness,
                SizeExtended = 0,
                Relocated = 0,
                Version = _writerParameters.Version,
                Size = 0,
            };

            uint directoryStartOffset = (uint)(0x20 + (0x40 * (directories.Count + files.Count)));
            uint fileStartOffset = (uint)(directoryStartOffset + (0x20 * directories.Count));
            var index = new ParIndex
            {
                DirectoryCount = (uint)directories.Count,
                DirectoryStartOffset = directoryStartOffset,
                FileCount = (uint)files.Count,
                FileStartOffset = fileStartOffset,
            };

            uint headerSize = RoundSize((uint)((0x20 * files.Count) + fileStartOffset));
            writer.Stream.SetLength(RoundSize(fileOffset + headerSize));

            uint currentOffset = headerSize;

            if (_writerParameters.WriteDataSize)
            {
                header.Size = headerSize + fileOffset;
            }

            writer.WriteOfType(header);
            writer.WriteOfType(index);

            for (int i = 0; i < directories.Count; i++)
            {
                Node node = directories[i];
                writer.Write(node.Name, 0x40, false);

                long returnPosition = writer.Stream.Position;
                _ = writer.Stream.Seek(directoryStartOffset + (i * 0x20), System.IO.SeekOrigin.Begin);

                var directoryInfo = new ParDirectoryInfo
                {
                    SubdirectoryCount = (uint)node.Tags["SubdirectoryCount"],
                    SubdirectoryStartIndex = (uint)node.Tags["SubdirectoryStartIndex"],
                    FileCount = (uint)node.Tags["FileCount"],
                    FileStartIndex = (uint)node.Tags["FileStartIndex"],
                    RawAttributes = (uint)node.Tags["RawAttributes"],
                };

                writer.WriteOfType(directoryInfo);
                writer.WritePadding(0x00, 0x20);

                _ = writer.Stream.Seek(returnPosition, System.IO.SeekOrigin.Begin);
            }

            for (int i = 0; i < files.Count; i++)
            {
                Node node = files[i];
                writer.Write(node.Name, 0x40, false);

                long returnPosition = writer.Stream.Position;
                _ = writer.Stream.Seek(fileStartOffset + (i * 0x20), System.IO.SeekOrigin.Begin);

                ParFile file = node.GetFormatAs<ParFile>();
                currentOffset = RoundOffset(currentOffset, file.FileInfo.CompressedSize);
                file.FileInfo.DataOffset = currentOffset;
                file.FileInfo.ExtendedOffset = 0;

                if (node.Tags.ContainsKey("RawAttributes"))
                {
                    file.FileInfo.RawAttributes = node.Tags["RawAttributes"];
                }
                else if (node.Tags.ContainsKey("FileInfo"))
                {
                    FileInfo info = node.Tags["FileInfo"];
                    file.FileInfo.RawAttributes = (uint)info.Attributes;
                }
                else
                {
                    file.FileInfo.RawAttributes = 0x20;
                }

                if (node.Tags.ContainsKey("Timestamp"))
                {
                    file.FileInfo.Timestamp = node.Tags["Timestamp"];
                }
                else if (node.Tags.ContainsKey("FileInfo"))
                {
                    FileInfo info = node.Tags["FileInfo"];
                    var time = new DateTimeOffset(info.LastWriteTime);
                    file.FileInfo.Timestamp = time.ToUnixTimeSeconds();
                }
                else
                {
                    DateTimeOffset now = DateTimeOffset.Now;
                    file.FileInfo.Timestamp = now.ToUnixTimeSeconds();
                }

                writer.WriteOfType(file.FileInfo);

                _ = writer.Stream.Seek(currentOffset, SeekOrigin.Begin);
                node.Stream.WriteTo(writer.Stream);

                currentOffset += file.FileInfo.CompressedSize;

                _ = writer.Stream.Seek(returnPosition, System.IO.SeekOrigin.Begin);
            }

            return new BinaryFormat(stream);
        }

        private static void FillNodeIndexes(Node root)
        {
            uint subdirectoryIndex = 1;
            uint fileIndex = 0;

            (uint rootDirectoryCount, uint rootFileCount) = FillDirectoryInfo(root, subdirectoryIndex, fileIndex);

            subdirectoryIndex += rootDirectoryCount;
            fileIndex += rootFileCount;

            foreach (Node node in Navigator.IterateNodes(root))
            {
                if (!node.IsContainer)
                {
                    continue;
                }

                (uint directoryCount, uint fileCount) = FillDirectoryInfo(node, subdirectoryIndex, fileIndex);

                subdirectoryIndex += directoryCount;
                fileIndex += fileCount;
            }
        }

        private static (uint directoryCount, uint fileCount) FillDirectoryInfo(Node node, uint directoryIndex, uint fileIndex)
        {
            uint directoryCount = (uint)node.Children.Count(x => x.IsContainer);
            uint fileCount = (uint)node.Children.Count(x => !x.IsContainer);

            node.Tags["SubdirectoryStartIndex"] = directoryIndex;
            node.Tags["SubdirectoryCount"] = directoryCount;
            node.Tags["FileStartIndex"] = fileIndex;
            node.Tags["FileCount"] = fileCount;

            return (directoryCount, fileCount);
        }

        private static uint RoundSize(uint offset, uint size) => size + RoundOffset(offset, size);

        private static uint RoundSize(uint size) => (size + 0x07FF) & 0xFFFFF800;

        [ExcludeFromCodeCoverage]
        private static uint RoundOffset(uint offset, uint size)
        {
            if (RoundNeeded(offset, size))
            {
                offset = RoundSize(offset);
            }

            return offset;
        }

        private static bool RoundNeeded(uint offset, uint size) => size + (offset & 0x07FF) >= 0x800;
    }
}

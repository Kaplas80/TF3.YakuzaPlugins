// Copyright (c) 2022 Kaplas
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
    using TF3.YarhlPlugin.YakuzaCommon.Enums;
    using Yarhl.IO;

    /// <summary>
    /// Parameters for PAR writer.
    /// </summary>
    public class WriterParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriterParameters"/> class.
        /// </summary>
        public WriterParameters()
        {
            PlatformId = Platform.PlayStation3;
            Endianness = Endianness.BigEndian;
            Version = 0x00020001;
            WriteDataSize = false;
            DotRoot = true;
            SortWithUpperInvariant = false;
            OutputStream = null;
        }

        /// <summary>
        /// Gets or sets the platform id.
        /// </summary>
        public Platform PlatformId { get; set; }

        /// <summary>
        /// Gets or sets the compression algorithm endianness.
        /// </summary>
        public Endianness Endianness { get; set; }

        /// <summary>
        /// Gets or sets the version (usually 0x00020001).
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the data size is written in header (only in Kenzan).
        /// </summary>
        public bool WriteDataSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the root name is a dot or a full name.
        /// </summary>
        public bool DotRoot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use ToUpperInvariant or ToLowerInvariant (default) when sorting node names.
        /// </summary>
        /// <remarks>Kenzan relies on the file order and some files will not be loaded without this option enabled.</remarks>
        public bool SortWithUpperInvariant { get; set; }

        /// <summary>
        /// Gets or sets the DataStream to write the file.
        /// </summary>
        /// <remarks>It can be null. In that case, it will be written in memory.</remarks>
        public DataStream OutputStream { get; set; }
    }
}

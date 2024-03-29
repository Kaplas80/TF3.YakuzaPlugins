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
namespace TF3.YarhlPlugin.YakuzaCommon.Types
{
    using Yarhl.IO.Serialization.Attributes;

    /// <summary>
    /// Par file info.
    /// </summary>
    [Serializable]
    public class ParFileInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParFileInfo"/> class.
        /// </summary>
        public ParFileInfo()
        {
            Flags = 0;
            OriginalSize = 0;
            CompressedSize = 0;
            DataOffset = 0;
            RawAttributes = 0;
            ExtendedOffset = 0;
            Timestamp = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParFileInfo"/> class.
        /// </summary>
        /// <param name="info">Object to copy.</param>
        public ParFileInfo(ParFileInfo info)
        {
            Flags = info.Flags;
            OriginalSize = info.OriginalSize;
            CompressedSize = info.CompressedSize;
            DataOffset = info.DataOffset;
            RawAttributes = info.RawAttributes;
            ExtendedOffset = info.ExtendedOffset;
            Timestamp = info.Timestamp;
        }

        /// <summary>
        /// Gets or sets the file flags.
        /// </summary>
        /// <remarks>For now, only 0x80000000 (IsCompressed).</remarks>
        public uint Flags { get; set; }

        /// <summary>
        /// Gets or sets the original file size.
        /// </summary>
        public uint OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the compressed file size.
        /// </summary>
        public uint CompressedSize { get; set; }

        /// <summary>
        /// Gets or sets the offset of the data inside the PAR archive (lower part).
        /// </summary>
        public uint DataOffset { get; set; }

        /// <summary>
        /// Gets or sets the file attributes.
        /// </summary>
        public uint RawAttributes { get; set; }

        /// <summary>
        /// Gets or sets the offset of the data inside the PAR archive (higher part).
        /// </summary>
        public uint ExtendedOffset { get; set; }

        /// <summary>
        /// Gets or sets the file timestamp (Unix format).
        /// </summary>
        /// <remarks>Number of seconds from 1970/01/01.</remarks>
        public long Timestamp { get; set; }

        /// <summary>
        /// Check if file is compressed.
        /// </summary>
        /// <returns>True if the file is compressed.</returns>
        public bool IsCompressed() => (Flags & 0x80000000) == 0x80000000;
    }
}

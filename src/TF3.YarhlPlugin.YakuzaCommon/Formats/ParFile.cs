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

namespace TF3.YarhlPlugin.YakuzaCommon.Formats
{
    using TF3.YarhlPlugin.YakuzaCommon.Types;
    using Yarhl.IO;

    /// <summary>
    /// File inside a PAR container.
    /// </summary>
    public class ParFile : BinaryFormat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParFile"/> class.
        /// </summary>
        /// <param name="stream">Binary stream.</param>
        public ParFile(System.IO.Stream stream)
            : base(stream, 0, stream.Length)
        {
            FileInfo = new ParFileInfo
            {
                OriginalSize = (uint)stream.Length,
                CompressedSize = (uint)stream.Length,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParFile"/> class.
        /// </summary>
        /// <param name="fileInfo">File parameters.</param>
        /// <param name="stream">Binary stream.</param>
        public ParFile(ParFileInfo fileInfo, System.IO.Stream stream)
            : base(stream, 0, stream.Length)
        {
            FileInfo = fileInfo;
        }

        /// <summary>
        /// Gets or sets the file parameters.
        /// </summary>
        public ParFileInfo FileInfo { get; set; }

        /// <inheritdoc />
        public override object DeepClone()
        {
            DataStream newStream = DataStreamFactory.FromMemory();
            Stream.WriteTo(newStream);
            return new ParFile(new ParFileInfo(FileInfo), newStream);
        }
    }
}

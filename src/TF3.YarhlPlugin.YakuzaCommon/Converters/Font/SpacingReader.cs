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

namespace TF3.YarhlPlugin.YakuzaCommon.Converters.Font
{
    using System;
    using TF3.YarhlPlugin.YakuzaCommon.Enums;
    using TF3.YarhlPlugin.YakuzaCommon.Formats;
    using TF3.YarhlPlugin.YakuzaCommon.Types;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Reads the vwf table from exe.
    /// </summary>
    public class SpacingReader : IConverter<BinaryFormat, CharacterSpacingTable>, IInitializer<ReaderParameters>
    {
        private ReaderParameters _parameters = null;

        /// <summary>
        /// Initializes the reader parameters.
        /// </summary>
        /// <param name="parameters">Reader configuration.</param>
        public void Initialize(ReaderParameters parameters) => _parameters = parameters;

        /// <summary>
        /// Reads the exe and extracts the character spacing table.
        /// </summary>
        /// <param name="source">The exe binary format.</param>
        /// <returns>The spacing table.</returns>
        public CharacterSpacingTable Convert(BinaryFormat source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (_parameters == null)
            {
                throw new InvalidOperationException("Uninitialized");
            }

            long offset = _parameters.Offset;
            if (offset < 0 || offset > source.Stream.Length)
            {
                throw new InvalidOperationException("Table offset is outside the source.");
            }

            source.Stream.Seek(offset);
            var reader = new DataReader(source.Stream)
            {
                Endianness = _parameters.Endianness == Endianness.LittleEndian
                    ? EndiannessMode.LittleEndian
                    : EndiannessMode.BigEndian,
            };

            var result = new CharacterSpacingTable
            {
                TableOffset = offset,
            };

            for (int i = 0; i < 256; i++)
            {
                result[i] = reader.Read<CharacterSpacing>();
            }

            return result;
        }
    }
}

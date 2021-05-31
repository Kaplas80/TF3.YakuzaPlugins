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
    using TF3.YarhlPlugin.YakuzaCommon.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Character spacing table replacer.
    /// </summary>
    public class SpacingWriter : IConverter<BinaryFormat, BinaryFormat>, IInitializer<CharacterSpacingTable>
    {
        private CharacterSpacingTable _table;

        /// <summary>
        /// Converter initializer.
        /// </summary>
        /// <remarks>
        /// Initialization is mandatory.
        /// </remarks>
        /// <param name="parameters">New spacing table.</param>
        public void Initialize(CharacterSpacingTable parameters) => _table = parameters;

        /// <summary>
        /// Replace the character spacing table in Yakuza exe.
        /// </summary>
        /// <remarks>
        /// Only the character table is modified.
        /// </remarks>
        /// <param name="source">The original BinaryFormat.</param>
        /// <returns>The BinaryFormat with the new spacing table.</returns>
        public BinaryFormat Convert(BinaryFormat source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (_table == null)
            {
                throw new InvalidOperationException("Uninitialized");
            }

            source.Stream.Seek(_table.TableOffset);

            var writer = new DataWriter(source.Stream)
            {
                Endianness = EndiannessMode.LittleEndian,
            };

            for (int i = 0; i < 256; i++)
            {
                writer.WriteOfType(_table[i]);
            }

            return source;
        }
    }
}

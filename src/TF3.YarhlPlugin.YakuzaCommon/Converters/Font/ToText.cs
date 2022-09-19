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

namespace TF3.YarhlPlugin.YakuzaCommon.Converters.Font
{
    using System;
    using System.Text;
    using TF3.YarhlPlugin.YakuzaCommon.Formats;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Convert a CharacterSpacingTable into a human readable format.
    /// </summary>
    public class ToText : IConverter<CharacterSpacingTable, BinaryFormat>
    {
        /// <summary>
        /// Convert a CharacterSpacingTable into a human readable format.
        /// </summary>
        /// <param name="source">The character spacing table.</param>
        /// <returns>The text BinaryFormat.</returns>
        public BinaryFormat Convert(CharacterSpacingTable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var dataStream = DataStreamFactory.FromMemory();
            var writer = new TextDataWriter(dataStream, Encoding.UTF8);

            writer.WriteLine("TableOffset={0}", source.TableOffset);
            writer.WriteLine();

            for (int i = 0; i < 256; i++)
            {
                writer.WriteLine("0x{0:X2}={1}", i, source[i].ToString());
            }

            return new BinaryFormat(dataStream);
        }
    }
}

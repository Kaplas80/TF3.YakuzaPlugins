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
    using System.Text;
    using TF3.YarhlPlugin.YakuzaCommon.Formats;
    using TF3.YarhlPlugin.YakuzaCommon.Types;
    using Yarhl.FileFormat;
    using Yarhl.IO;

    /// <summary>
    /// Convert a text file into a CharacterSpacingTable.
    /// </summary>
    public class FromText : IConverter<BinaryFormat, CharacterSpacingTable>
    {
        /// <summary>
        /// Convert a text file into a CharacterSpacingTable.
        /// </summary>
        /// <param name="source">The text BinaryFormat.</param>
        /// <returns>The character spacing table.</returns>
        public CharacterSpacingTable Convert(BinaryFormat source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var result = new CharacterSpacingTable();

            source.Stream.Seek(0);
            var reader = new TextDataReader(source.Stream, Encoding.UTF8);

            while (!source.Stream.EndOfStream)
            {
                string line = reader.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                string[] split = line.Split('=');
                if (split.Length != 2)
                {
                    throw new FormatException("Bad text format");
                }

                if (split[0] == "TableOffset")
                {
                    result.TableOffset = long.Parse(split[1]);
                }
                else
                {
                    int chrIndex = System.Convert.ToInt32(split[0].Replace("0x", string.Empty), 16);
                    result[chrIndex] = new CharacterSpacing(split[1]);
                }
            }

            return result;
        }
    }
}

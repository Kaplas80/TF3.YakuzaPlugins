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

namespace TF3.YarhlPlugin.YakuzaCommon.Formats
{
    using TF3.YarhlPlugin.YakuzaCommon.Types;
    using Yarhl.FileFormat;

    /// <summary>
    /// Yakuza font character spacing table.
    /// </summary>
    public class CharacterSpacingTable : IFormat
    {
        private readonly CharacterSpacing[] _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSpacingTable"/> class.
        /// </summary>
        public CharacterSpacingTable()
        {
            _table = new CharacterSpacing[256];
            TableOffset = 0;
        }

        /// <summary>
        /// Gets or sets the table offset inside the file.
        /// </summary>
        public long TableOffset { get; set; }

        /// <summary>
        /// Gets the <see cref="CharacterSpacing"/> of a char.
        /// </summary>
        /// <param name="character">The character.</param>
        public CharacterSpacing this[char character]
        {
            get
            {
                return _table[character];
            }

            set
            {
                _table[character] = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="CharacterSpacing"/> of a char.
        /// </summary>
        /// <param name="index">The character index (ASCII).</param>
        public CharacterSpacing this[int index]
        {
            get
            {
                return _table[index];
            }

            set
            {
                _table[index] = value;
            }
        }
    }
}

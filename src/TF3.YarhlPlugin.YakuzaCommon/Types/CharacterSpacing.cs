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
    using System;
    using System.Globalization;

    /// <summary>
    /// Yakuza font character spacing.
    /// </summary>
    [Yarhl.IO.Serialization.Attributes.Serializable]
    public class CharacterSpacing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSpacing"/> class.
        /// </summary>
        public CharacterSpacing()
        {
            TopLeft = 0;
            TopRight = 0;
            MiddleLeft = 0;
            MiddleRight = 0;
            BottomLeft = 0;
            BottomRight = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSpacing"/> class.
        /// </summary>
        /// <param name="values">Values in a space separated string.</param>
        public CharacterSpacing(string values)
        {
            string[] split = values.Split(' ');
            if (split.Length != 6)
            {
                throw new FormatException("Inssuficient values.");
            }

            TopLeft = Convert.ToSingle(split[0], CultureInfo.InvariantCulture);
            TopRight = Convert.ToSingle(split[1], CultureInfo.InvariantCulture);
            MiddleLeft = Convert.ToSingle(split[2], CultureInfo.InvariantCulture);
            MiddleRight = Convert.ToSingle(split[3], CultureInfo.InvariantCulture);
            BottomLeft = Convert.ToSingle(split[4], CultureInfo.InvariantCulture);
            BottomRight = Convert.ToSingle(split[5], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the top left space.
        /// </summary>
        public float TopLeft { get; set; }

        /// <summary>
        /// Gets or sets the top right space.
        /// </summary>
        public float TopRight { get; set; }

        /// <summary>
        /// Gets or sets the middle left space.
        /// </summary>
        public float MiddleLeft { get; set; }

        /// <summary>
        /// Gets or sets the middle right space.
        /// </summary>
        public float MiddleRight { get; set; }

        /// <summary>
        /// Gets or sets the bottom left space.
        /// </summary>
        public float BottomLeft { get; set; }

        /// <summary>
        /// Gets or sets the bottom right space.
        /// </summary>
        public float BottomRight { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return FormattableString.Invariant($"{TopLeft} {TopRight} {MiddleLeft} {MiddleRight} {BottomLeft} {BottomRight}");
        }
    }
}

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
    using Yarhl.FileSystem;

    /// <summary>
    /// Replace the font.
    /// </summary>
    public class Replacer : IConverter<NodeContainerFormat, NodeContainerFormat>, IInitializer<NodeContainerFormat>
    {
        private NodeContainerFormat newFontContainer = null;

        /// <summary>
        /// Converter initializer.
        /// </summary>
        /// <remarks>
        /// Initialization is mandatory.
        /// </remarks>
        /// <param name="parameters">Font replacement.</param>
        public void Initialize(NodeContainerFormat parameters) => newFontContainer = parameters;

        /// <summary>
        /// Replace font files.
        /// </summary>
        /// <param name="source">The original font asset.</param>
        /// <returns>The replaced font.</returns>
        public NodeContainerFormat Convert(NodeContainerFormat source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.Root.Children.Count != 2)
            {
                throw new InvalidOperationException("Font assets need 2 nodes.");
            }

            if (newFontContainer.Root.Children.Count != 2)
            {
                throw new InvalidOperationException("Font assets need 2 nodes.");
            }

            Node newDds = null;
            Node newSpacing = null;

            foreach (Node child in newFontContainer.Root.Children)
            {
                if (child.Name.EndsWith("dds"))
                {
                    newDds = child;
                }

                if (child.Name.EndsWith("txt"))
                {
                    newSpacing = child;
                }
            }

            if (newDds == null || newSpacing == null)
            {
                throw new InvalidOperationException("Missing format in Font Asset.");
            }

            foreach (Node child in source.Root.Children)
            {
                if (child.Name.EndsWith("dds"))
                {
                    child.ChangeFormat(newDds.Format);
                }
                else if (child.Name.EndsWith("exe"))
                {
                    child.TransformWith<SpacingWriter, CharacterSpacingTable>(newSpacing.Format as CharacterSpacingTable);
                }
                else
                {
                    throw new InvalidOperationException("Unknown format in Font Asset.");
                }
            }

            return source;
        }
    }
}

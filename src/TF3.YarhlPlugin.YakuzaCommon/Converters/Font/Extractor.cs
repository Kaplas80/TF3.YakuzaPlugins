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
    using Yarhl.FileFormat;
    using Yarhl.FileSystem;

    /// <summary>
    /// Converts font asset nodes to binary.
    /// </summary>
    public class Extractor : IConverter<NodeContainerFormat, NodeContainerFormat>, IInitializer<ReaderParameters>
    {
        private ReaderParameters _parameters = null;

        /// <summary>
        /// Initializes the reader parameters.
        /// </summary>
        /// <param name="parameters">Reader configuration.</param>
        public void Initialize(ReaderParameters parameters) => _parameters = parameters;

        /// <summary>
        /// Converts font asset nodes to binary.
        /// </summary>
        /// <param name="source">The font asset.</param>
        /// <returns>The font asset with all its children in BinaryFormat.</returns>
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

            // The DDS node doesn't change
            // The CharacterSpacingTable change to BinaryFormat (text).
            foreach (Node child in source.Root.Children)
            {
                if (child.Name.EndsWith("dds"))
                {
                    continue;
                }

                if (child.Name.EndsWith("exe"))
                {
                    child.TransformWith<SpacingReader, ReaderParameters>(_parameters).TransformWith<ToText>();
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

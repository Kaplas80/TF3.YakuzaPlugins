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

namespace FontBuilder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using BCnEncoder.Encoder;
    using BCnEncoder.ImageSharp;
    using BCnEncoder.Shared;
    using BCnEncoder.Shared.ImageFiles;
    using CommandLine;
    using SixLabors.Fonts;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Drawing.Processing;
    using SixLabors.ImageSharp.PixelFormats;
    using SixLabors.ImageSharp.Processing;
    using TF3.YarhlPlugin.YakuzaCommon.Formats;
    using TF3.YarhlPlugin.YakuzaCommon.Types;

    /// <summary>
    /// Font builder.
    /// </summary>
    internal static class Program
    {
        private const int OutputWidthYakuza6 = 512;
        private const int OutputWidthKiwami2 = 1024;
        private const int OutputHeight = 1024;

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">args.</param>
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(BuildFont);
        }

        private static void BuildFont(Options options)
        {
            if (!File.Exists(options.Font))
            {
                Console.WriteLine($"Font not found: {options.Font}");
                return;
            }

            if (Directory.Exists(options.Output))
            {
                Console.Write("Output directory already exists. Overwrite (y/N)? ");
                string overwrite = Console.ReadLine();

                if (!string.Equals(overwrite, "Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("Cancelled by user.");
                    return;
                }

                Directory.Delete(options.Output, true);
            }

            var mapping = BuildMapping(options.CodePage);
            var output = BuildOutputImage(options.Mode, out int charWidth, out int charHeight);

            var spacingTable = new CharacterSpacingTable
            {
                TableOffset = -1,
            };

            for (int i = 0; i < 0x100; i++)
            {
                spacingTable[i] = new CharacterSpacing();
            }

            var fontCollection = new FontCollection();
            FontFamily family = fontCollection.Add(options.Font);
            Font font = family.CreateFont(charHeight - (charHeight / 8));

            int x = 0;
            int y = 0;
            for (byte i = 0x20; i < 0x80; i++)
            {
                (Image<L8> img, CharacterSpacing spacing) = RenderCharacter(options.Mode, mapping[i], font, charWidth, charHeight, 3);
                output.Mutate(o => o.DrawImage(img, new Point(x * charWidth, y * charHeight), 1f));
                spacingTable[i] = spacing;
                img.Dispose();
                x++;
                if (x == 16)
                {
                    x = 0;
                    y++;
                }
            }

            x = 1;
            y = 8;

            for (int i = 0xA1; i < 0x100; i++)
            {
                (Image<L8> img, CharacterSpacing spacing) = RenderCharacter(options.Mode, mapping[i], font, charWidth, charHeight, 3);
                output.Mutate(o => o.DrawImage(img, new Point(x * charWidth, y * charHeight), 1f));
                spacingTable[i] = spacing;
                img.Dispose();
                x++;
                if (x == 16)
                {
                    x = 0;
                    y++;
                }
            }

            spacingTable[0x20].TopLeft = 0.3f;
            spacingTable[0x20].MiddleLeft = 0.3f;
            spacingTable[0x20].BottomLeft = 0.3f;
            spacingTable[0x20].TopRight = 0.3f;
            spacingTable[0x20].MiddleRight = 0.3f;
            spacingTable[0x20].BottomRight = 0.3f;

            if (!Directory.Exists(options.Output))
            {
                Directory.CreateDirectory(options.Output);
            }

            SaveDds(output, options.Output);
            SaveTable(spacingTable, options.Output);
        }

        private static Dictionary<int, string> BuildMapping(int codePage)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding(codePage);

            var mapping = new Dictionary<int, string>();
            for (byte i = 0x20; i < 0x80; i++)
            {
                byte[] data = { i };
                mapping[i] = encoding.GetString(data);
            }

            for (int i = 0xA1; i < 0x100; i++)
            {
                byte[] data = { (byte)i };
                mapping[(byte)i] = encoding.GetString(data);
            }

            mapping[0x5C] = "¥";
            mapping[0x7F] = "®";

            return mapping;
        }

        private static Image<Rgba32> BuildOutputImage(string mode, out int charWidth, out int charHeight)
        {
            switch (mode)
            {
                case "kiwami2":
                    {
                        charWidth = OutputWidthKiwami2 / 16;
                        charHeight = OutputHeight / 16;
                        return new Image<Rgba32>(OutputWidthKiwami2, OutputHeight);
                    }

                case "y6":
                    {
                        charWidth = OutputWidthYakuza6 / 16;
                        charHeight = OutputHeight / 16;
                        return new Image<Rgba32>(OutputWidthYakuza6, OutputHeight);
                    }

                default: throw new ArgumentOutOfRangeException("mode");
            }
        }

        private static (Image<L8>, CharacterSpacing) RenderCharacter(string mode, string character, Font font, int width, int height, int defaultSeparation)
        {
            var image = new Image<L8>(width * 2, height);

            FontRectangle size = TextMeasurer.Measure(character, new TextOptions(font));

            // Add a "tall" character to align all the baselines
            image.Mutate(x => x
                .DrawText(string.Concat(character, "       Í"), font, Color.White, new PointF(1f, 0f))
                .Crop(Math.Max((int)size.Width, width), height));

            if (size.Width >= width)
            {
                image.Mutate(x => x.Resize(width, height));
            }

            const float defaultValue = 0.8f;
            int spcHeight = height / 3;

            int tlPixel = FindPixelLeft(image, new Rectangle(0, 0, width, spcHeight), defaultSeparation);
            int mlPixel = FindPixelLeft(image, new Rectangle(0, spcHeight, width, spcHeight), defaultSeparation);
            int blPixel = FindPixelLeft(image, new Rectangle(0, 2 * spcHeight, width, spcHeight), defaultSeparation);
            int trPixel = FindPixelRight(image, new Rectangle(0, 0, width, spcHeight), defaultSeparation);
            int mrPixel = FindPixelRight(image, new Rectangle(0, spcHeight, width, spcHeight), defaultSeparation);
            int brPixel = FindPixelRight(image, new Rectangle(0, 2 * spcHeight, width, spcHeight), defaultSeparation);

            CharacterSpacing charSpacing = BuildCharacterSpacing(mode, defaultValue, width, (tlPixel, mlPixel, blPixel), (trPixel, mrPixel, brPixel));
            return (image, charSpacing);
        }

        private static CharacterSpacing BuildCharacterSpacing(string mode, float defaultValue, int width, (int top, int middle, int bottom) leftPixels, (int top, int middle, int bottom) rightPixels)
        {
            float tl = defaultValue;
            float ml = defaultValue;
            float bl = defaultValue;
            float tr = defaultValue;
            float mr = defaultValue;
            float br = defaultValue;

            if (leftPixels.top != int.MaxValue)
            {
                tl = 3f * leftPixels.top / width;
                if (tl < -0.1f)
                {
                    tl = -0.1f;
                }

                if (tl > 0.8f)
                {
                    tl = 0.8f;
                }
            }

            if (leftPixels.middle != int.MaxValue)
            {
                ml = 3f * leftPixels.middle / width;

                if (ml < -0.1f)
                {
                    ml = -0.1f;
                }

                if (ml > 0.8f)
                {
                    ml = 0.8f;
                }
            }

            if (leftPixels.bottom != int.MaxValue)
            {
                bl = 3f * leftPixels.bottom / width;

                if (bl < -0.1f)
                {
                    bl = -0.1f;
                }

                if (bl > 0.8f)
                {
                    bl = 0.8f;
                }
            }

            switch (mode)
            {
                case "kiwami2":
                    {
                        if (rightPixels.top != int.MinValue)
                        {
                            float temp = 3f * rightPixels.top / width;
                            tr = 1.5f - temp;
                        }

                        if (rightPixels.middle != int.MinValue)
                        {
                            float temp = 3f * rightPixels.middle / width;
                            mr = 1.5f - temp;
                        }

                        if (rightPixels.bottom != int.MinValue)
                        {
                            float temp = 3f * rightPixels.bottom / width;
                            br = 1.5f - temp;
                        }

                        break;
                    }

                case "y6":
                    {
                        if (rightPixels.top != int.MinValue)
                        {
                            float temp = 1f * rightPixels.top / width;
                            tr = 3f * (1f - temp);

                            if (tr < -0.1f)
                            {
                                tr = -0.1f;
                            }

                            if (tr > 1.17f)
                            {
                                tr = 1.17f;
                            }
                        }

                        if (rightPixels.middle != int.MinValue)
                        {
                            float temp = 1f * rightPixels.middle / width;
                            mr = 3f * (1f - temp);

                            if (mr < -0.1f)
                            {
                                mr = -0.1f;
                            }

                            if (mr > 1.17f)
                            {
                                mr = 1.17f;
                            }
                        }

                        if (rightPixels.bottom != int.MinValue)
                        {
                            float temp = 1f * rightPixels.bottom / width;
                            br = 3f * (1f - temp);

                            if (br < -0.1f)
                            {
                                br = -0.1f;
                            }

                            if (br > 1.17f)
                            {
                                br = 1.17f;
                            }
                        }

                        break;
                    }

                default: throw new ArgumentOutOfRangeException("mode");
            }

            return new CharacterSpacing
            {
                TopLeft = Round(tl),
                MiddleLeft = Round(ml),
                BottomLeft = Round(bl),
                TopRight = Round(tr),
                MiddleRight = Round(mr),
                BottomRight = Round(br),
            };
        }

        private static int FindPixelLeft(Image<L8> image, Rectangle searchArea, int margin)
        {
            int result = int.MaxValue;

            image.ProcessPixelRows(accessor =>
            {
                for (int y = searchArea.Y; y < searchArea.Y + searchArea.Height; y++)
                {
                    Span<L8> pixelRow = accessor.GetRowSpan(y);

                    for (int x = searchArea.X; x < searchArea.X + searchArea.Width; x++)
                    {
                        ref L8 pixel = ref pixelRow[x];
                        if (pixel.PackedValue == 0x00)
                        {
                            continue;
                        }

                        result = Math.Min(result, x);
                        break;
                    }
                }
            });

            return result == int.MaxValue ? int.MaxValue : (result - margin);
        }

        private static int FindPixelRight(Image<L8> image, Rectangle searchArea, int margin)
        {
            int result = int.MinValue;

            image.ProcessPixelRows(accessor =>
            {
                for (int y = searchArea.Y; y < searchArea.Y + searchArea.Height; y++)
                {
                    Span<L8> pixelRow = accessor.GetRowSpan(y);

                    for (int x = searchArea.X + searchArea.Width - 1; x >= searchArea.X; x--)
                    {
                        ref L8 pixel = ref pixelRow[x];
                        if (pixel.PackedValue == 0x00)
                        {
                            continue;
                        }

                        result = Math.Max(result, x);
                        break;
                    }
                }
            });

            return result == int.MinValue ? int.MinValue : result + margin;
        }

        private static float Round(float val)
        {
            return (float)Math.Round(val * 20) / 20;
        }

        private static void SaveDds(Image<Rgba32> image, string path)
        {
            var encoder = new BcEncoder()
            {
                OutputOptions =
                {
                    GenerateMipMaps = false,
                    Quality = CompressionQuality.Balanced,
                    Format = CompressionFormat.Bc4,
                    FileFormat = OutputFileFormat.Dds,
                },
            };

            DdsFile dds = encoder.EncodeToDds(image);
            using var fs = new FileStream(Path.Combine(path, "yakuza.dds"), FileMode.CreateNew);
            dds.Write(fs);
        }

        private static void SaveTable(CharacterSpacingTable table, string path)
        {
            var bin = (Yarhl.IO.BinaryFormat)Yarhl.FileFormat.ConvertFormat.With<TF3.YarhlPlugin.YakuzaCommon.Converters.Font.ToText>(table);
            bin.Stream.WriteTo(Path.Combine(path, "yakuza_spacing.txt"));
        }

        private sealed class Options
        {
            [Option('f', "font", Required = true, HelpText = "TTF font file to use.")]
            public string Font { get; set; }

            [Option('m', "mode", Required = true, HelpText = "Destination game. Valid values are: kiwami2 and y6")]
            public string Mode { get; set; }

            [Option("codepage", Required = false, Default = 28591, HelpText = "Code page number")]
            public int CodePage { get; set; }

            [Option('o', "output-dir", Required = true, HelpText = "Output directory.")]
            public string Output { get; set; }
        }
    }
}

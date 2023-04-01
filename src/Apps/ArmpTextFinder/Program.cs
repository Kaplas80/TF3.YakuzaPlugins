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

namespace ArmpTextFinder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using CommandLine;
    using Standart.Hash.xxHash;
    using Yarhl.FileSystem;

    /// <summary>
    /// Identify the armp files with translatable strings inside a directory.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">args.</param>
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(FindFiles);
        }

        private static void FindFiles(Options options)
        {
            if (!Directory.Exists(options.Input))
            {
                Console.WriteLine($"Input directory not found: {options.Input}");
                return;
            }

            if (!string.IsNullOrEmpty(options.Output) && File.Exists(options.Output))
            {
                Console.Write("Output file already exists. Overwrite (y/N)? ");
                string overwrite = Console.ReadLine();

                if (!string.Equals(overwrite, "Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("Cancelled by user.");
                    return;
                }

                File.Delete(options.Output);
            }

            var files = Directory.EnumerateFiles(options.Input, "*.bin", SearchOption.AllDirectories);

            List<string> lines = new List<string>();

            foreach (string file in files)
            {
                Node n = NodeFactory.FromFile(file);

                var hash = xxHash64.ComputeHash(n.Stream);
                n.Stream.Position = 0;

                string filename = Path.GetFileNameWithoutExtension(file);
                string relativePath = Path.GetRelativePath(options.Input, file).Replace("\\", "/");

                try
                {
                    n.TransformWith<TF3.YarhlPlugin.YakuzaCommon.Converters.Armp.Reader>();
                    n.TransformWith<TF3.YarhlPlugin.YakuzaCommon.Converters.Armp.ExtractStrings>();

                    Yarhl.Media.Text.Po po = n.GetFormatAs<Yarhl.Media.Text.Po>();

                    if (po.Entries.Count > 0)
                    {
                        lines.Add(@$"{{
  ""Id"": ""{filename}.bin"",
  ""OutputNames"": [""text/{filename}.po""],
  ""Files"": [
    {{
      ""Name"": ""{filename}.bin"",
      ""ContainerId"": ""root"",
      ""Path"": ""data/db/e/{relativePath}"",
      ""Checksum"": ""0x{hash:X16}"",
      ""Readers"": [
        {{
          ""TypeName"": ""TF3.YarhlPlugin.YakuzaCommon.Converters.Armp.Reader"",
          ""ParameterId"": """"
        }}
      ],
      ""Writers"": [
        {{
          ""TypeName"": ""TF3.YarhlPlugin.YakuzaCommon.Converters.Armp.Writer"",
          ""ParameterId"": """"
        }}
      ]
    }}
  ],
  ""Extractors"": [
    {{
      ""TypeName"": ""TF3.YarhlPlugin.YakuzaCommon.Converters.Armp.ExtractStrings"",
      ""ParameterId"": ""poHeader""
    }},
    {{
      ""TypeName"": ""Yarhl.Media.Text.Po2Binary"",
      ""ParameterId"": """"
    }}
  ],
  ""TranslationMergers"": [
    {{
      ""TypeName"": ""TF3.YarhlPlugin.YakuzaCommon.Converters.Po.Merger"",
      ""ParameterId"": ""poHeader""
    }}
  ],
  ""Translator"": ""TF3.YarhlPlugin.YakuzaCommon.Converters.Armp.Translate""
}}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR in {file}:");
                    Console.WriteLine(e);
                }
            }

            var text = $"[{string.Join(",\n", lines)}]";
            File.WriteAllText(options.Output, text);
        }

        private sealed class Options
        {
            [Option('i', "input-dir", Required = true, HelpText = "Directory with armp files.")]
            public string Input { get; set; }

            [Option('o', "output", Required = true, HelpText = "Output file.")]
            public string Output { get; set; }
        }
    }
}

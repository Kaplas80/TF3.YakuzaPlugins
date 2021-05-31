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

namespace FontSpacingEditor
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using BCnEncoder.Decoder;
    using BCnEncoder.ImageSharp;
    using BCnEncoder.Shared;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;
    using TF3.YarhlPlugin.YakuzaCommon.Formats;
    using Yarhl.FileSystem;

    /// <summary>
    /// App main window.
    /// </summary>
    public partial class MainForm : Form
    {
        private string _currentTableFile;
        private CharacterSpacingTable _charTable;
        private System.Drawing.Image _fontImage;
        private int? _selectedChar;
        private System.Drawing.Bitmap _selectedCharBitmap;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            _currentTableFile = string.Empty;
            _charTable = null;
            _fontImage = null;
            _selectedChar = null;
            _selectedCharBitmap = null;
        }

        private static void ShowError(string message)
        {
            MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static float FindPixelLeft(System.Drawing.Bitmap bitmap, Rectangle searchArea, System.Drawing.Color nullColor, float defaultValue, int charWidth, int margin)
        {
            int result = int.MaxValue;

            for (int y = searchArea.Y; y < searchArea.Y + searchArea.Height; y++)
            {
                for (int x = searchArea.X; x < searchArea.X + searchArea.Width; x++)
                {
                    if (bitmap.GetPixel(x, y).ToArgb() == nullColor.ToArgb())
                    {
                        continue;
                    }

                    result = Math.Min(result, x);
                    break;
                }
            }

            return result == int.MaxValue ? defaultValue : Round((result - margin) * 3f / charWidth);
        }

        private static float FindPixelRight(System.Drawing.Bitmap bitmap, Rectangle searchArea, System.Drawing.Color nullColor, float defaultValue, int charWidth, int margin)
        {
            int result = int.MinValue;

            for (int y = searchArea.Y; y < searchArea.Y + searchArea.Height; y++)
            {
                for (int x = searchArea.X + searchArea.Width - 1; x >= searchArea.X; x--)
                {
                    if (bitmap.GetPixel(x, y).ToArgb() == nullColor.ToArgb())
                    {
                        continue;
                    }

                    result = Math.Max(result, x);
                    break;
                }
            }

            return result == int.MinValue ? defaultValue : Round(1.5f - ((result + margin) * 3f / charWidth));
        }

        private static float Round(float val)
        {
            return (float)Math.Round(val * 20) / 20;
        }

        private void LoadFontButtonClick(object sender, EventArgs e)
        {
            openFileDialog.FileName = string.Empty;
            openFileDialog.Filter = "Yakuza font images (*.dds; *.png)|*.dds;*.png";

            DialogResult dialogResult = openFileDialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            LoadFontImage(openFileDialog.FileName);
        }

        private void LoadTableButtonClick(object sender, EventArgs e)
        {
            openFileDialog.FileName = string.Empty;
            openFileDialog.Filter = "Yakuza font table (*.txt)|*.txt|All files (*.*)|*.*";

            DialogResult dialogResult = openFileDialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            LoadFontTable(openFileDialog.FileName);
        }

        private void FontPictureMouseDown(object sender, MouseEventArgs e)
        {
            // Check if font is loaded
            if (_fontImage == null)
            {
                return;
            }

            // Identify clicked character
            int charWidth = fontPictureBox.Width / 16;
            int charHeight = fontPictureBox.Height / 16;

            int column = e.X / charWidth;
            int row = e.Y / charHeight;

            if (row + 0x2 > 0xF)
            {
                // Clicked on a invalid character
                return;
            }

            _selectedChar = (row * 0x10) + 0x20 + column;

            // Highlight selected char
            fontPictureBox.Invalidate();

            // Load character info
            UpdateCharInfo();
            UpdateCharImage();
        }

        private void UpdateCharImage()
        {
            if (_fontImage == null || _selectedChar == null)
            {
                charPictureBox.Image = null;
                _selectedCharBitmap = null;
                return;
            }

            int charWidth = _fontImage.Width / 16;
            int charHeight = _fontImage.Height / 16;

            int row = (int)(_selectedChar / 16) - 2;
            int col = (int)(_selectedChar % 16);

            var p = new System.Drawing.Point { X = col * charWidth, Y = row * charHeight };

            var rect = new System.Drawing.Rectangle(p.X, p.Y, charWidth, charHeight);
            _selectedCharBitmap = new System.Drawing.Bitmap(charWidth, charHeight);

            using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(_selectedCharBitmap))
            {
                var destRect = new System.Drawing.Rectangle(0, 0, charWidth, charHeight);
                gr.DrawImage(_fontImage, destRect, rect, System.Drawing.GraphicsUnit.Pixel);
            }

            charPictureBox.Image = _selectedCharBitmap;
        }

        private void UpdateCharInfo()
        {
            if (_charTable == null || _selectedChar == null)
            {
                topLeftNumericUpDown.Value = 0;
                topLeftNumericUpDown.Enabled = false;
                middleLeftNumericUpDown.Value = 0;
                middleLeftNumericUpDown.Enabled = false;
                bottomLeftNumericUpDown.Value = 0;
                bottomLeftNumericUpDown.Enabled = false;
                topRightNumericUpDown.Value = 0;
                topRightNumericUpDown.Enabled = false;
                middleRightNumericUpDown.Value = 0;
                middleRightNumericUpDown.Enabled = false;
                bottomRightNumericUpDown.Value = 0;
                bottomRightNumericUpDown.Enabled = false;

                return;
            }

            TF3.YarhlPlugin.YakuzaCommon.Types.CharacterSpacing charInfo = _charTable[_selectedChar.Value];

            topLeftNumericUpDown.Enabled = true;
            middleLeftNumericUpDown.Enabled = true;
            bottomLeftNumericUpDown.Enabled = true;
            topRightNumericUpDown.Enabled = true;
            middleRightNumericUpDown.Enabled = true;
            bottomRightNumericUpDown.Enabled = true;

            topLeftNumericUpDown.Value = (decimal)charInfo.TopLeft;
            middleLeftNumericUpDown.Value = (decimal)charInfo.MiddleLeft;
            bottomLeftNumericUpDown.Value = (decimal)charInfo.BottomLeft;
            topRightNumericUpDown.Value = (decimal)charInfo.TopRight;
            middleRightNumericUpDown.Value = (decimal)charInfo.MiddleRight;
            bottomRightNumericUpDown.Value = (decimal)charInfo.BottomRight;
        }

        private void FontPicturePaint(object sender, PaintEventArgs e)
        {
            if (_selectedChar == null)
            {
                return;
            }

            int charWidth = fontPictureBox.Width / 16;
            int charHeight = fontPictureBox.Height / 16;

            int row = (int)(_selectedChar / 16) - 2;
            int col = (int)(_selectedChar % 16);

            using var pen = new System.Drawing.Pen(System.Drawing.Color.Yellow, 1);
            e.Graphics.DrawRectangle(pen, col * charWidth, row * charWidth, charWidth, charHeight);
        }

        private void CharPicturePaint(object sender, PaintEventArgs e)
        {
            if (_charTable == null || _selectedChar == null)
            {
                return;
            }

            int charWidth = charPictureBox.Width;
            int charHeight = charPictureBox.Height;

            TF3.YarhlPlugin.YakuzaCommon.Types.CharacterSpacing charInfo = _charTable[_selectedChar.Value];

            float topLeft = charInfo.TopLeft * charWidth / 3f;
            float topRight = (0.5f * charWidth) - (charInfo.TopRight * 0.333f * charWidth);
            float middleLeft = charInfo.MiddleLeft * 0.333f * charWidth;
            float middleRight = (0.5f * charWidth) - (charInfo.MiddleRight * 0.333f * charWidth);
            float bottomLeft = charInfo.BottomLeft * 0.333f * charWidth;
            float bottomRight = (0.5f * charWidth) - (charInfo.BottomRight * 0.333f * charWidth);

            using (var pen = new System.Drawing.Pen(System.Drawing.Color.Yellow, 1))
            {
                e.Graphics.DrawLine(pen, topLeft, 0, topLeft, charHeight * 0.333f);
                e.Graphics.DrawLine(pen, middleLeft, charHeight * 0.333f, middleLeft, charHeight * 0.666f);
                e.Graphics.DrawLine(pen, bottomLeft, charHeight * 0.666f, bottomLeft, charHeight);
            }

            using (var pen = new System.Drawing.Pen(System.Drawing.Color.Blue, 1))
            {
                e.Graphics.DrawLine(pen, topRight, 0, topRight, charHeight * 0.333f);
                e.Graphics.DrawLine(pen, middleRight, charHeight * 0.333f, middleRight, charHeight * 0.666f);
                e.Graphics.DrawLine(pen, bottomRight, charHeight * 0.666f, bottomRight, charHeight);
            }
        }

        private void MarginValueChanged(object sender, EventArgs e)
        {
            if (sender is not NumericUpDown nudObject)
            {
                return;
            }

            if (_charTable == null || _selectedChar == null)
            {
                return;
            }

            string id = nudObject.Tag.ToString();

            // Identify changed value
            switch (id)
            {
                case "TopLeft":
                    _charTable[_selectedChar.Value].TopLeft = Convert.ToSingle(nudObject.Value);
                    break;
                case "TopRight":
                    _charTable[_selectedChar.Value].TopRight = Convert.ToSingle(nudObject.Value);
                    break;
                case "MiddleLeft":
                    _charTable[_selectedChar.Value].MiddleLeft = Convert.ToSingle(nudObject.Value);
                    break;
                case "MiddleRight":
                    _charTable[_selectedChar.Value].MiddleRight = Convert.ToSingle(nudObject.Value);
                    break;
                case "BottomLeft":
                    _charTable[_selectedChar.Value].BottomLeft = Convert.ToSingle(nudObject.Value);
                    break;
                case "BottomRight":
                    _charTable[_selectedChar.Value].BottomRight = Convert.ToSingle(nudObject.Value);
                    break;
            }

            // Redraw margin marker
            charPictureBox.Invalidate();
        }

        private void SaveTableButtonClick(object sender, EventArgs e)
        {
            saveFileDialog.InitialDirectory = Path.GetDirectoryName(_currentTableFile);
            saveFileDialog.FileName = Path.GetFileName(_currentTableFile);

            DialogResult dialogResult = saveFileDialog.ShowDialog(this);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            SaveFontTable(saveFileDialog.FileName);

            _currentTableFile = saveFileDialog.FileName;
        }

        private void AutomaticValuesButtonClick(object sender, EventArgs e)
        {
            // This method searches the first "non-black" pixel in each of the character areas and
            // sets the margin values for them.
            if (_selectedChar == null || _selectedCharBitmap == null)
            {
                return;
            }

            int margin = Convert.ToInt32(marginNumericUpDown.Value);
            int width = _selectedCharBitmap.Width;
            int height = _selectedCharBitmap.Height / 3;
            const float defaultValue = 0.4f;
            System.Drawing.Color nullColor = System.Drawing.Color.Black;

            _charTable[_selectedChar.Value].TopLeft = FindPixelLeft(_selectedCharBitmap, new Rectangle(0, 0, width, height), nullColor, defaultValue, width, margin);
            _charTable[_selectedChar.Value].MiddleLeft = FindPixelLeft(_selectedCharBitmap, new Rectangle(0, height, width, height), nullColor, defaultValue, width, margin);
            _charTable[_selectedChar.Value].BottomLeft = FindPixelLeft(_selectedCharBitmap, new Rectangle(0, 2 * height, width, height), nullColor, defaultValue, width, margin);
            _charTable[_selectedChar.Value].TopRight = FindPixelRight(_selectedCharBitmap, new Rectangle(0, 0, width, height), nullColor, defaultValue, width, margin);
            _charTable[_selectedChar.Value].MiddleRight = FindPixelRight(_selectedCharBitmap, new Rectangle(0, height, width, height), nullColor, defaultValue, width, margin);
            _charTable[_selectedChar.Value].BottomRight = FindPixelRight(_selectedCharBitmap, new Rectangle(0, 2 * height, width, height), nullColor, defaultValue, width, margin);

            topLeftNumericUpDown.Value = (decimal)_charTable[_selectedChar.Value].TopLeft;
            middleLeftNumericUpDown.Value = (decimal)_charTable[_selectedChar.Value].MiddleLeft;
            bottomLeftNumericUpDown.Value = (decimal)_charTable[_selectedChar.Value].BottomLeft;
            topRightNumericUpDown.Value = (decimal)_charTable[_selectedChar.Value].TopRight;
            middleRightNumericUpDown.Value = (decimal)_charTable[_selectedChar.Value].MiddleRight;
            bottomRightNumericUpDown.Value = (decimal)_charTable[_selectedChar.Value].BottomRight;
        }

        private void LoadFontImage(string imageFile)
        {
            string extension = Path.GetExtension(imageFile);

            try
            {
                switch (extension.ToLowerInvariant())
                {
                    case ".dds":
                        LoadDdsFontImage(imageFile);
                        break;
                    case ".png":
                        LoadPngFontImage(imageFile);
                        break;
                    default:
                        throw new Exceptions.ImageLoadException("Unknown image extension.");
                }
            }
            catch (Exceptions.ImageLoadException e)
            {
                ShowError(e.Message);
            }
        }

        private void LoadDdsFontImage(string imageFile)
        {
            try
            {
                using FileStream fs = File.OpenRead(imageFile);

                BcDecoder decoder = new ();
                decoder.OutputOptions.Bc4Component = ColorComponent.Luminance;

                using Image<Rgba32> image = decoder.DecodeToImageRgba32(fs);
                using MemoryStream outFs = new ();
                image.SaveAsBmp(outFs);

                _fontImage = System.Drawing.Image.FromStream(outFs);
                fontPictureBox.Image = _fontImage;
            }
            catch (Exception e)
            {
                throw new Exceptions.ImageLoadException("Error opening image", e);
            }
        }

        private void LoadPngFontImage(string imageFile)
        {
            try
            {
                _fontImage = System.Drawing.Image.FromFile(imageFile);

                fontPictureBox.Image = _fontImage;
            }
            catch (Exception e)
            {
                throw new Exceptions.ImageLoadException("Error opening image", e);
            }
        }

        private void LoadFontTable(string fontFile)
        {
            Node node = NodeFactory.FromFile(fontFile);
            node.TransformWith<TF3.YarhlPlugin.YakuzaCommon.Converters.Font.FromText>();

            _charTable = node.GetFormatAs<CharacterSpacingTable>();
        }

        private void SaveFontTable(string fontFile)
        {
            var bin = (Yarhl.IO.BinaryFormat)Yarhl.FileFormat.ConvertFormat.With<TF3.YarhlPlugin.YakuzaCommon.Converters.Font.ToText>(_charTable);
            bin.Stream.WriteTo(fontFile);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace WordCloudCsharp
{
    public static class WordcloudExtension
    {
        private static event Action<double> OnProgress;

        #region method

        public static FastImage CropImage(FastImage img)
        {
            var cropRect = new Rectangle(1, 1, img.Width - 1, img.Height - 1);
            var src = img.Bitmap;
            var target = new Bitmap(cropRect.Width, cropRect.Height);

            using (var g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                 cropRect,
                                 GraphicsUnit.Pixel);
            }
            return new FastImage(target);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Image ResizeImage(Image image, int width, int height)
        {
            if (image.Width == width && image.Height == height)
                return image;
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            var bmpdata = destImage.LockBits(new Rectangle(0, 0, destImage.Width, destImage.Height), ImageLockMode.ReadOnly, destImage.PixelFormat);
            var len = bmpdata.Height * bmpdata.Stride;
            var buf = new byte[len];
            Marshal.Copy(bmpdata.Scan0, buf, 0, len);
            for (var i = 0; i < width * height * 4; i++)
            {
                if (buf[i] == 255 || buf[i] == 0)
                    continue;
                if (i % 4 == 3)
                    continue;
                if (buf[i] > 0)
                    buf[i] = 255;
                else
                    buf[i] = 0;
            }
            Marshal.Copy(buf, 0, bmpdata.Scan0, len);
            destImage.UnlockBits(bmpdata);

            return destImage;
        }

        public static bool CheckMaskValid(Image mask)
        {
            bool valid;
            using (var bmp = new Bitmap(mask))
            {
                var bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
                var len = bmpdata.Height * bmpdata.Stride;
                var buf = new byte[len];
                Marshal.Copy(bmpdata.Scan0, buf, 0, len);
                valid = buf.Count(b => b != 0 && b != 255) == 0;
                bmp.UnlockBits(bmpdata);
            }
            return valid;
        }
        /// <summary>
        /// generate image
        /// <para>生成图片</para>
        /// </summary>
        /// <param name="words">List of words ordered by occurance.</param>
        /// <param name="freqs">List of frequecies.</param>
        /// <returns>Image of word cloud.</returns>
        public static Image Draw(this WordCloud wordCloud, IList<string> words, IList<int> freqs)
        {
            return Draw(wordCloud, words, freqs, Color.White, null);
        }

        /// <summary>
        /// Draws the specified word cloud with background color spicified given list of words and frequecies
        /// </summary>
        /// <param name="words">List of words ordered by occurance.</param>
        /// <param name="freqs">List of frequecies.</param>
        /// <param name="bgcolor">Specified background color</param>
        /// <returns>Image of word cloud.</returns>
        public static Image Draw(this WordCloud wordCloud, IList<string> words, IList<int> freqs, Color bgcolor)
        {
            return Draw(wordCloud, words, freqs, bgcolor, null);
        }

        /// <summary>
        /// Draws the specified word cloud with background spicified given list of words and frequecies
        /// </summary>
        /// <param name="words">List of words ordered by occurance.</param>
        /// <param name="freqs">List of frequecies.</param>
        /// <param name="img">Specified background image</param>
        /// <returns>Image of word cloud.</returns>
        public static Image Draw(this WordCloud wordCloud, IList<string> words, IList<int> freqs, Image img)
        {
            return Draw(wordCloud, words, freqs, Color.White, img);
        }

        #endregion

        #region private method
        /// <summary>
        /// Draws the specified word cloud given list of words and frequecies
        /// </summary>
        /// <param name="words">List of words ordered by occurance.</param>
        /// <param name="freqs">List of frequecies.</param>
        /// <param name="bgcolor">Backgroud color of the output image</param>
        /// <param name="img">Backgroud image of the output image</param>
        /// <returns>Image of word cloud.</returns>
        /// <exception cref="System.ArgumentException">
        /// Arguments null.
        /// or
        /// Must have the same number of words as frequencies.
        /// </exception>
        private static Image Draw(WordCloud wordCloud, IList<string> words, IList<int> freqs, Color bgcolor, Image? img = null)
        {
            var fontSize = wordCloud.MaxFontSize;
            if (words == null || freqs == null)
            {
                throw new ArgumentException("Arguments null.");
            }
            if (words.Count != freqs.Count)
            {
                throw new ArgumentException("Must have the same number of words as frequencies.");
            }

            Bitmap result;
            if (img == null)
                result = new Bitmap(wordCloud.WorkImage.Width, wordCloud.WorkImage.Height);
            else
            {
                if (img.Size != wordCloud.WorkImage.Bitmap.Size)
                    throw new Exception("The backgroud img should be with the same size with the mask");
                result = new Bitmap(img);
            }

            using (var gworking = Graphics.FromImage(wordCloud.WorkImage.Bitmap))
            using (var gresult = Graphics.FromImage(result))
            {
                if (img == null)
                    gresult.Clear(bgcolor);
                gresult.TextRenderingHint = TextRenderingHint.AntiAlias;
                gworking.TextRenderingHint = TextRenderingHint.AntiAlias;
                var lastProgress = 0.0d;
                for (var i = 0; i < words.Count; ++i)
                {
                    var progress = (double)i / words.Count;
                    if (progress - lastProgress > 0.01)
                    {
                        ShowProgress(progress);
                        lastProgress = progress;
                    }
                    if (!wordCloud.UseRank)
                    {
                        fontSize = (float)Math.Min(fontSize, 100 * Math.Log10(freqs[i] + 100));
                    }

                    var format = new StringFormat();
                    if (wordCloud.AllowVertical)
                    {
                        if (wordCloud.Random.Next(0, 2) == 1)
                            format.FormatFlags = StringFormatFlags.DirectionVertical;
                    }

                    Point p;
                    var foundPosition = false;
                    Font font;
                    var size = new SizeF();
                    Debug.WriteLine("Word: " + words[i]);
                    do
                    {
                        font = new Font(wordCloud.Fontname, fontSize, GraphicsUnit.Pixel);
                        size = gworking.MeasureString(words[i], font, new PointF(0, 0), format);
                        Debug.WriteLine("Search with font size: " + fontSize);
                        foundPosition = wordCloud.Map.GetRandomUnoccupiedPosition((int)size.Width, (int)size.Height, out p);
                        if (!foundPosition) fontSize -= wordCloud.FontStep;
                    } while (fontSize > 0 && !foundPosition);
                    Debug.WriteLine("Found pos: " + p);
                    if (fontSize <= 0) break;
                    gworking.DrawString(words[i], font, new SolidBrush(wordCloud.FontColor), p.X, p.Y, format);
                    gresult.DrawString(words[i], font, new SolidBrush(wordCloud.FontColor), p.X, p.Y, format);
                    wordCloud.Map.Update(wordCloud.WorkImage, p.X, p.Y);
                }
            }
            wordCloud.WorkImage.Dispose();
            return result;
        }

        private static void ShowProgress(double progress)
        {
            OnProgress?.Invoke(progress);
        }
        #endregion
    }
}

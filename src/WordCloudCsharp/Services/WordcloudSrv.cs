using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace WordCloudCsharp
{
    /// <summary>
    /// Wordcloud Service
    /// <para>词云实现接口</para>
    /// </summary>
    public class WordcloudSrv : IWordcloud
    {
        #region disposable

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        ~WordcloudSrv()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// <seealso cref="IWordcloud.GetWordCloud(int, int, bool, Color?, float, int, Image?, bool, string?)"/>
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="useRank"></param>
        /// <param name="fontColor"></param>
        /// <param name="maxFontSize"></param>
        /// <param name="fontStep"></param>
        /// <param name="mask"></param>
        /// <param name="allowVerical"></param>
        /// <param name="fontname"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public WordCloud GetWordCloud(int width, int height, bool useRank = false, Color? fontColor = null, float maxFontSize = -1, int fontStep = 1, Image? mask = null, bool allowVerical = false, string? fontname = null)
        {
            var wordCloud = new WordCloud()
            {
                MaxFontSize = maxFontSize < 0 ? height : maxFontSize,
                FontStep = fontStep,
                FontColor = fontColor ?? GetRandomColor(),
                UseRank = useRank,
                Random = new Random(Environment.TickCount),
                AllowVertical = allowVerical,
                Fontname = fontname,
            };
            if (mask == null)
            {
                wordCloud.Map = new OccupancyMap(width, height);
                wordCloud.WorkImage = new FastImage(width, height, PixelFormat.Format32bppArgb);
            }
            else
            {
                mask = WordcloudExtension.ResizeImage(mask, width, height);
                if (!WordcloudExtension.CheckMaskValid(mask))
                    throw new Exception("Mask is not a valid black-white image");
                wordCloud.Map = new OccupancyMap(mask);
                wordCloud.WorkImage = new FastImage(mask);
            }
            return wordCloud;
        }

        #region private method
        /// <summary>
        /// Gets a random color.
        /// </summary>
        /// <returns>Color</returns>
        private Color GetRandomColor()
        {
            return Color.FromArgb(new Random().Next(0, 255), new Random().Next(0, 255), new Random().Next(0, 255));
        }
        #endregion
    }
}

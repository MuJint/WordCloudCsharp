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
        /// <summary>
        /// dispose
        /// </summary>
        /// <param name="disposing"></param>
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
        /// <summary>
        /// destructor
        /// </summary>
        ~WordcloudSrv()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        /// <summary>
        /// dispose
        /// </summary>
        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// get wordcloud object
        /// <para>得到一个wordcloud操作对象</para>
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="useRank">User input order instead of frequency</param>
        /// <param name="fontColor">fontColor</param>
        /// <param name="maxFontSize">maxFontSize</param>
        /// <param name="fontStep">Amount to decrement font size each time a word won't fit.</param>
        /// <param name="mask">use mask image by generate wordcloud</param>
        /// <param name="allowVerical">If allow vertical drawing</param>
        /// <param name="fontname">fontname</param>
        /// <returns><seealso cref="IWordcloud.GetWordCloud(int, int, bool, Color?, float, int, Image?, bool, string?)"/></returns>
        /// <exception cref="Exception"></exception>
        public WordCloud GetWordCloud(int width, int height, bool useRank = false, Color? fontColor = null, float maxFontSize = -1, int fontStep = 1, Image? mask = null, bool allowVerical = false, string? fontname = null)
        {
            var wordCloud = new WordCloud()
            {
                MaxFontSize = maxFontSize < 0 ? height : maxFontSize,
                FontStep = fontStep,
                FontColor = fontColor,
                UseRank = useRank,
                Random = new Random(Environment.TickCount),
                AllowVertical = allowVerical,
                Fontname = fontname,
            };

            //now can use background
            if (mask == null)
            {
                wordCloud.Map = new OccupancyMap(width, height);
                wordCloud.WorkImage = new FastImage(width, height, PixelFormat.Format32bppArgb);
            }
            else
            {
                //use mask.can't be used background
                mask = mask.ResizeImage(width, height);
                if (mask.CheckMaskValid() is false)
                    throw new Exception("Mask is not a valid black-white image");
                wordCloud.Map = new OccupancyMap(mask);
                wordCloud.WorkImage = new FastImage(mask);
            }
            return wordCloud;
        }
    }
}

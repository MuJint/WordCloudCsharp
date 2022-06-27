using System;
using System.Drawing;

namespace WordCloudCsharp
{
    /// <summary>
    /// wordcloud interface
    /// <para>wordcloud接口</para>
    /// </summary>
    public interface IWordcloud : IDisposable
    {
        /// <summary>
        /// generate wordcloud object
        /// </summary>
        /// <param name="width">The width of word cloud.</param>
        /// <param name="height">The height of word cloud.</param>
        /// <param name="useRank">if set to <c>true</c> will ignore frequencies for best fit.</param>
        /// <param name="fontColor">Color of the font.</param>
        /// <param name="maxFontSize">Maximum size of the font.</param>
        /// <param name="fontStep">The font step to use.</param>
        /// <param name="mask">mask image</param>
        /// <param name="allowVerical">allow vertical text</param>
        /// <param name="fontname">font name</param>
        /// <returns>operate wordcloud object</returns>
        WordCloud GetWordCloud(int width, int height, bool useRank = false, Color? fontColor = null, float maxFontSize = -1, int fontStep = 1, Image? mask = null, bool allowVerical = false, string? fontname = null);
    }
}

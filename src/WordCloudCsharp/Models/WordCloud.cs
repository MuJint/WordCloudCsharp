using System;
using System.Drawing;

namespace WordCloudCsharp
{
    /// <summary>
    /// wordcloud object
    /// </summary>
    public class WordCloud
    {
        #region property
        /// <summary>
        /// Gets font colour or random if font wasn't set
        /// </summary>
        public Color FontColor { get; set; }


        /// <summary>
        /// Used to select random colors.
        /// </summary>
        public Random Random { get; set; }


        /// <summary>
        /// Working image.
        /// </summary>
        public FastImage WorkImage { get; set; }


        /// <summary>
        /// Keeps track of word positions using integral image.
        /// </summary>
        public OccupancyMap Map { get; set; }


        /// <summary>
        /// Gets or sets the maximum size of the font.
        /// </summary>
        public float MaxFontSize { get; set; }


        /// <summary>
        /// User input order instead of frequency
        /// </summary>
        public bool UseRank { get; set; }

        /// <summary>
        /// Amount to decrement font size each time a word won't fit.
        /// </summary>
        public int FontStep { get; set; }

        /// <summary>
        /// If allow vertical drawing 
        /// </summary>
	    public bool AllowVertical { get; set; }

        public string? Fontname
        {
            get { return _fontname ?? "Microsoft Sans Serif"; }
            set
            {
                _fontname = value;
                if (value == null) return;
                using var f = new Font(value, 12, FontStyle.Regular);
                _fontname = f.FontFamily.Name;
            }
        }

        private string? _fontname;

        #endregion
    }
}

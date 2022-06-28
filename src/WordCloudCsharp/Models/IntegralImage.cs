using System;

namespace WordCloudCsharp
{
    public class IntegralImage
    {
        #region property & constructors
        public int OutputImgWidth { get; set; }

        public int OutputImgHeight { get; set; }

        protected uint[,] Integral { get; set; }

        public IntegralImage(int outputImgWidth, int outputImgHeight)
        {
            Integral = new uint[outputImgWidth, outputImgHeight];
            OutputImgWidth = outputImgWidth;
            OutputImgHeight = outputImgHeight;
        }

        public IntegralImage(FastImage image)
        {
            Integral = new uint[image.Width, image.Height];
            OutputImgWidth = image.Width;
            OutputImgHeight = image.Height;
            InitMask(image);
        }

        #endregion

        #region private method
        private void InitMask(FastImage image)
        {
            Update(image.CropImage(), 1, 1);
        }
        #endregion

        public void Update(FastImage image, int posX, int posY)
        {
            if (posX < 1) posX = 1;
            if (posY < 1) posY = 1;
            var pixelSize = Math.Min(3, image.PixelFormatSize);

            for (var i = posY; i < image.Height; ++i)
            {
                for (var j = posX; j < image.Width; ++j)
                {
                    byte pixel = 0;
                    for (var p = 0; p < pixelSize; ++p)
                    {
                        pixel |= image.Data[i * image.Stride + j * image.PixelFormatSize + p];
                    }
                    Integral[j, i] = pixel + Integral[j - 1, i] + Integral[j, i - 1] - Integral[j - 1, i - 1];
                }
            }
        }

        public ulong GetArea(int xPos, int yPos, int sizeX, int sizeY)
        {
            ulong area = Integral[xPos, yPos] + Integral[xPos + sizeX, yPos + sizeY];
            area -= Integral[xPos + sizeX, yPos] + Integral[xPos, yPos + sizeY];
            return area;
        }
    }
}

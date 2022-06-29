using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WordCloudCsharp
{
    /// <summary>
    /// structure image
    /// </summary>
    public class FastImage : IDisposable
    {
        private bool disposedValue;

        #region property

        /// <summary>
        /// Width
        /// </summary>
        public int Width => Bitmap.Width;

        /// <summary>
        /// Height
        /// </summary>
        public int Height => Bitmap.Height;

        /// <summary>
        /// PixelFormatSize
        /// <para>像素格式化大小</para>
        /// </summary>
        public int PixelFormatSize { get; set; }

        /// <summary>
        /// GCHandle
        /// <para>gc handle</para>
        /// </summary>
        public GCHandle Handle { get; set; }

        /// <summary>
        /// Stride
        /// </summary>
        public int Stride { get; set; }

        /// <summary>
        /// data
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// bitmap
        /// </summary>
        public Bitmap Bitmap { get; set; }
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="format"></param>
        public FastImage(int width, int height, PixelFormat format)
        {
            PixelFormatSize = Image.GetPixelFormatSize(format) / 8;
            Stride = width * PixelFormatSize;

            Data = new byte[Stride * height];
            Handle = GCHandle.Alloc(Data, GCHandleType.Pinned);
            var pData = Marshal.UnsafeAddrOfPinnedArrayElement(Data, 0);
            Bitmap = new Bitmap(width, height, Stride, format, pData);
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="image"></param>
        public FastImage(Image image) : this(image.Width, image.Height, image.PixelFormat)
        {
            var bmp = new Bitmap(image);
            var bmpdatain = bmp.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), ImageLockMode.ReadOnly, Bitmap.PixelFormat);
            Marshal.Copy(bmpdatain.Scan0, Data, 0, Data.Length);
            bmp.UnlockBits(bmpdatain);
        }

        #region disposable

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
                    // TODO: 释放托管状态(托管对象)
                    Handle.Free();
                    Bitmap.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        /// <summary>
        /// the destructor
        /// </summary>
        ~FastImage()
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
    }
}

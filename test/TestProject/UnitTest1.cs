using System.Drawing;
using System.Drawing.Imaging;
using WordCloudCsharp;

namespace TestProject
{
    public class UnitTest1
    {
        readonly List<string> words = new() { "Sign", "Xia", "张三2", "签到", "在吗", "上网啊" };
        readonly List<int> feq = new() { 1, 2, 1, 1, 1, 1 };

        [Fact]
        public void TestInteration()
        {
            try
            {
                for (int i = 0; i < 10000; i++)
                {
                    using var images2 = ISingletion<WordcloudSrv>.Instance
                         .GetWordCloud(200, 200)
                         .Draw(new List<string>() { "Sign", "Xia" }, new List<int>() { 1, 2 });
                    images2.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Fact]
        public void Test2()
        {
            using var images2 = ISingletion<WordcloudSrv>.Instance
                 .GetWordCloud(200, 200, useRank: true)
                 .Draw(words, feq);
            images2.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }

        [Fact]
        public void Test3()
        {
            using var images2 = ISingletion<WordcloudSrv>.Instance
                 .GetWordCloud(200, 200, useRank: true, fontColor: Color.AliceBlue)
                 .Draw(words, feq);
            images2.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }

        [Fact]
        public void Test4()
        {
            using var images2 = ISingletion<WordcloudSrv>.Instance
                 .GetWordCloud(500, 400,fontColor:Color.Black,maxFontSize:24)
                 .Draw(words, feq);
            images2.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }

        [Fact]
        public void Test5()
        {
            using var images2 = ISingletion<WordcloudSrv>.Instance
                 .GetWordCloud(500, 400, fontColor: Color.Black, fontStep: 5)
                 .Draw(words, feq);
            images2.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }

        [Fact]
        public void Test6()
        {
            using var images2 = ISingletion<WordcloudSrv>.Instance
                 .GetWordCloud(500, 400, fontColor: Color.DarkRed, allowVerical: true)
                 .Draw(words, feq);
            images2.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }

        [Fact]
        public void Test7()
        {
            using var images2 = ISingletion<WordcloudSrv>.Instance
                 .GetWordCloud(500, 400, fontColor: Color.DarkRed,fontname:"fangsong")
                 .Draw(words, feq);
            images2.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }

        [Fact]
        public void Test8()
        {
            //imgMark need vaild black/white  ? 需要有效的黑或白蒙板图片
            using var fileStream = new FileStream(@"C:\Users\86152\Desktop\1-21061PUH3.jpg", FileMode.Open);
            var img = Image.FromStream(fileStream);
            using var images2 = ISingletion<WordcloudSrv>.Instance
                 .GetWordCloud(580, 387, fontColor: Color.Pink, fontname: "kaiti", mask: img)
                 .Draw(words, feq);
            images2.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }
    }
}
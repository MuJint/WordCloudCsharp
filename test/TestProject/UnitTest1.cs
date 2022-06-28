using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using System.Drawing.Imaging;
using WordCloudCsharp;
using JiebaNet.Segmenter;
using JiebaNet.Segmenter.Common;

namespace TestProject
{
    public class UnitTest1
    {
        readonly ServiceProvider provider = new ServiceCollection()
                                     .AddSingleton<IWordcloud, WordcloudSrv>()
                                 .BuildServiceProvider();
        readonly List<string> words = new() { "Sign", "Xia", "张三2", "签到", "在吗", "上网啊" };
        readonly List<int> feq = new() { 1, 2, 1, 1, 1, 1 };

        [Fact]
        public void TestInteration()
        {
            try
            {
                for (int i = 0; i < 10000; i++)
                {
                    var s = ISingletion<WordcloudSrv>.Instance
                         .GetWordCloud(200, 200);

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
                 .GetWordCloud(500, 400, fontColor: Color.Black, maxFontSize: 24)
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
                 .GetWordCloud(500, 400, fontColor: Color.DarkRed, fontname: "fangsong")
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

        [Fact]
        public void TestIOC()
        {

            //di  ?注入
            //services.AddSingleton<IWordcloud, WordcloudSrv>();

            //ctor ?构造函数注入
            //readonly IWordcloud _wordcloud;
            //public XXX(IWordcloud wordcloud)
            //{
            //    _wordcloud = wordcloud;
            //}

            var _wordcloud = provider.GetService<IWordcloud>();

            using var images = _wordcloud.GetWordCloud(4000, 4000)
                             .Draw(new List<string>() { "Sign", "Xia" }, new List<int>() { 1, 2 });
            images.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }

        [Fact]
        public void TestJiebaAndWordCloud()
        {
            //Jieba.Net Chinese word segmentation, generate pictures  ?结巴中文分词，生成图片
            var s = "在数学和计算机科学之中，算法（algorithm）为任何良定义的具体计算步骤的一个序列，常用于计算、数据处理和自动推理。精确而言，算法是一个表示为有限长列表的有效方法。算法应包含清晰定义的指令用于计算函数。";
            var seg = new JiebaSegmenter();
            var freqs = new Counter<string>(seg.Cut(s));
            var wordKeys = new List<string>();
            var ints = new List<int>();
            var filterFreqs = freqs.Count >= 20 ? freqs?.MostCommon(20) : freqs?.MostCommon(freqs.Count - 1);
            foreach (var pair in filterFreqs)
            {
                wordKeys.Add(pair.Key);
                ints.Add(pair.Value);
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }

            var service = provider.GetService<IWordcloud>();

            var images = service.GetWordCloud(300, 300, true).Draw(wordKeys, ints);
            images.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }
    }
}
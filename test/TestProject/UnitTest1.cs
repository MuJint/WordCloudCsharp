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
        readonly List<string> words = new() { "Sign", "Xia", "����2", "ǩ��", "����", "������" };
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
                 .GetWordCloud(1242, 768, useRank: true)
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
            //imgMark need vaild black/white  ? ��Ҫ��Ч�ĺڰ��ɰ�ͼƬ
            using var fileStream = new FileStream(@"Image/heart.png", FileMode.Open);
            var img = Image.FromStream(fileStream);
            using var images2 = ISingletion<WordcloudSrv>.Instance
                 .GetWordCloud(580, 387, fontname: "huawenxingkai", mask: img)
                 .Draw(words, feq);
            images2.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }

        [Fact]
        public void TestIOC()
        {

            //di  ?ע��
            //services.AddSingleton<IWordcloud, WordcloudSrv>();

            //ctor ?���캯��ע��
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
            //Jieba.Net Chinese word segmentation, generate pictures  ?������ķִʣ�����ͼƬ
            var s = "����ѧ�ͼ������ѧ֮�У��㷨��algorithm��Ϊ�κ�������ľ�����㲽���һ�����У������ڼ��㡢���ݴ�����Զ�������ȷ���ԣ��㷨��һ����ʾΪ���޳��б����Ч�������㷨Ӧ�������������ָ�����ڼ��㺯����";
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

            using var fileStream = new FileStream(@"Image/heart.png", FileMode.Open);
            var img = Image.FromStream(fileStream);

            var images = service.GetWordCloud(1242, 768, mask: img).Draw(wordKeys, ints);
            images.Save($"D:\\foreach\\{Guid.NewGuid()}.png", ImageFormat.Png);
        }

        [Fact]
        public void TestBackgroundImg()
        {
#warning background and mask can't be used at the same time.the width of the background image is equal to the width of the input
            //����ͼƬ���ܺ��ɲ�ͬʱʹ�á����ұ���ͼƬ�ÿ�ߵ�������ÿ��

            var service = provider.GetService<IWordcloud>();
            using var fileStream2 = new FileStream(@"Image/background.png", FileMode.Open);
            var img2 = Image.FromStream(fileStream2);
            var images = service.GetWordCloud(1242, 766).Draw(words, feq, img: img2);
        }
    }
}
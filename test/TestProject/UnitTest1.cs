using System.Drawing.Imaging;
using WordCloudCsharp;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
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
    }
}
// See https://aka.ms/new-console-template for more information
using System.Drawing.Imaging;
using WordCloudCsharp;

Console.WriteLine("Hello, World!");
using var images = ISingletion<WordcloudSrv>.Instance
                     .GetWordCloud(4000, 4000)
                     .Draw(new List<string>() { "Sign", "Xia" }, new List<int>() { 1, 2 });
images.Save($"D:\\{Guid.NewGuid()}.png", ImageFormat.Png);
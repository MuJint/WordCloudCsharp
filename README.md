# Notice
This project comes from another author.Wait to merge into the main line

* [WordCloudSharp](https://github.com/AmmRage/WordCloudSharp)
* ?  Suspected fix GC problem, waiting for test

# Ported Repository Info

This project is ported from a Codeplex hosted repository.

* [orig repo](http://wordcloud.codeplex.com/)

* [orig author](http://www.codeplex.com/site/users/view/briancullen)

# Install

[![NuGet](https://img.shields.io/nuget/v/WordCloudCsharp.svg)](https://www.nuget.org/packages/WordCloudCsharp)

# Usage
* NameSpace ?命名空间
```
    using WordCloudCsharp;
```
* Singletion ?自带单例
```
    using var images = ISingletion<WordcloudSrv>.Instance
                     .GetWordCloud(4000, 4000)
                     .Draw(new List<string>() { "Sign", "Xia" }, new List<int>() { 1, 2 });
    images.Save($"D:\\{Guid.NewGuid()}.png", ImageFormat.Png);
```
* IOC ?IOC使用
```
    //di  ?注入
    services.AddSingleton<IWordcloud, WordcloudSrv>();

    //ctor ?构造函数注入
    readonly IWordcloud _wordcloud;
    public XXX(IWordcloud wordcloud)
    {
        _wordcloud = wordcloud;
    }
    
    using var images = _wordcloud.GetWordCloud(4000, 4000)
                     .Draw(new List<string>() { "Sign", "Xia" }, new List<int>() { 1, 2 });
    images.Save($"D:\\{Guid.NewGuid()}.png", ImageFormat.Png);
```
for more details, please ref to the usage in UnitTest1. ?更多使用用例请查看 UnitTest1

# Generate Img

* Without Mask
<br />
<br />
<img width="200" src="test/TestProject/Output/7c382f5e-dc25-446a-a74b-a0db2b8b1edf.png" alt="7c382f5e-dc25-446a-a74b-a0db2b8b1edf.png">

* With Mask
<br />
<br />
<img width="200" src="test/TestProject/Output/4a05f0b6-ef03-4a18-81d2-2119ce0b1da6.png" alt="4a05f0b6-ef03-4a18-81d2-2119ce0b1da6.png">
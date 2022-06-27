# Notice
This project comes from another author.Wait to merge into the main line

* [WordCloudSharp](https://github.com/AmmRage/WordCloudSharp)
* ?Suspected fix GC problem, waiting for test

# Ported Repository Info

This project is ported from a Codeplex hosted repository.

* [orig repo](http://wordcloud.codeplex.com/)

* [orig author](http://www.codeplex.com/site/users/view/briancullen)

# Build

![GitHub Actions]()

# Install

[![NuGet](https://img.shields.io/nuget/v/WordCloudSharp.svg)]()

# Usage

```
    using var images = ISingletion<WordcloudSrv>.Instance
                     .GetWordCloud(4000, 4000)
                     .Draw(new List<string>() { "Sign", "Xia" }, new List<int>() { 1, 2 });
    images.Save($"D:\\{Guid.NewGuid()}.png", ImageFormat.Png);
```

for more details, please ref to the usage in UnitTest1

# Examples

without mask: 

![alt text][without]

[without]: https://github.com/AmmRage/WordCloudSharp/blob/master/images/exmaple.jpg "without mask"

with mask: 

![alt text][with]

[with]: https://github.com/AmmRage/WordCloudSharp/blob/master/images/example_with_mask.jpg "with mask"

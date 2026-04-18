using System.Collections.Generic;
using congruent.Helpers;

namespace congruent.Tests;

public class HelperTests
{
    [Fact]
    public void GetLabelTextTest()
    {
       string url = "https://newlibre.com/comicreader";
       Console.WriteLine($"url tab text: {url.CreateTabText()}");
       url = "https://newlibre.com/";
       Console.WriteLine($"url tab text: {url.CreateTabText()}");
       url = "http://rpix.local";
       Console.WriteLine($"url tab text: {url.CreateTabText()}");
       url = "https://newlibre.com/maze";
       Console.WriteLine($"url tab text: {url.CreateTabText()}");
       url = "https://computerworld.com";
       Console.WriteLine($"url tab text: {url.CreateTabText()}");
       url = "https://archive.org";
       Console.WriteLine($"url tab text: {url.CreateTabText()}");
    }
}

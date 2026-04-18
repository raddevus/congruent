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
    }
}

using System.Collections.Generic;
using congruent.Models;

namespace congruent.Tests;

public class BookmarkTests
{
    [Fact]
    public void Test1()
    {
      List<Bookmark> allBookmarks = new();
      allBookmarks.Add(new Bookmark());

      Console.WriteLine(allBookmarks[0].BookmarkPath);
    }
}

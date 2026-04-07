using System.Collections.Generic;
using congruent.Models;

namespace congruent.Tests;

public class BookmarkTests
{
    [Fact]
    public void Test1()
    {
      List<Bookmark> allBookmarks = new();
      // set bookmarkpath to the test debug directory
      allBookmarks.Add(new Bookmark(){
           BookmarkPath=AppContext.BaseDirectory
      });
      Console.WriteLine($"base path => {allBookmarks[0].BookmarkPath}");
    }
}

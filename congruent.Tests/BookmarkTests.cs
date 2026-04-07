using System.Collections.Generic;
using congruent.Models;

namespace congruent.Tests;

public class BookmarkTests
{
    [Fact]
    async public Task ExamineBookmarksFromFile()
    {
      List<Bookmark> allBookmarks = new();
      // set bookmarkpath to the test debug directory
      allBookmarks.Add(new Bookmark(){
           BookmarkPath=AppContext.BaseDirectory
      });
      Console.WriteLine($"base path => {allBookmarks[0].BookmarkPath}");

      var bmList = await allBookmarks[0].LoadFromFile();
      Console.WriteLine($"bm.Count {bmList.Count}");
    }
}

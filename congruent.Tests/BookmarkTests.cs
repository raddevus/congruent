using System.Collections.Generic;
using congruent.Models;

namespace congruent.Tests;

public class BookmarkTests
{
    [Fact]
    async public Task ExamineBookmarksFromFile()
    {
      Bookmark bm = new(){
           BookmarkPath=AppContext.BaseDirectory
      };

      Console.WriteLine($"base path => {bm.BookmarkPath}");

      var bmList = await bm.LoadFromFile();
      Console.WriteLine($"bm.Count {bmList.Count}");
      foreach (Bookmark b in bmList){
         Console.WriteLine($"b.Title : {b.Title}");
         Console.WriteLine($"b.Children : {b.Children.Count}");
      }
    }
}

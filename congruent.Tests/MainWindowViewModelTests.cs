using System.Collections.ObjectModel;
using System.Collections.Generic;
using congruent.Models;
using congruent.ViewModels;

namespace congruent.Tests;

public class MainWindowViewModelTests
{
    [Fact]
    async public Task FindBookmarkTest(){
       MainWindowViewModel vm = new();
       Console.WriteLine("I did it!");

       Bookmark bm = new(){
           BookmarkPath=AppContext.BaseDirectory
      };

      Console.WriteLine($"base path => {bm.BookmarkPath}");

      var bmList = await bm.LoadFromFile();
      foreach (Bookmark b in bmList){
         vm.AllBookmarks.Add(b);
      }
      Console.WriteLine($"Test is ready. Have {vm.AllBookmarks.Count} bookmarks loaded.");

    }
}

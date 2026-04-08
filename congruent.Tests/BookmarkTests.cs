using System.Collections.ObjectModel;
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
      ObservableCollection<Bookmark> allBms = new();
      foreach (Bookmark b in bmList){
         allBms.Add(b);
      }
      var targetCounter = allBms.Count;
      var counter = 0;
      foreach (Bookmark b in allBms){
         counter++;
         Console.WriteLine($"counter: {targetCounter}");
         Console.WriteLine($"b.Title : {b.Title}");
//       foreach (Bookmark i in 
         Console.WriteLine($"b.Children : {b.Children.Count}");
         if (counter == targetCounter){
            Console.WriteLine("in here...");
            allBms = b.Children as ObservableCollection<Bookmark>;
            targetCounter = b.Children.Count;
            counter = 0;
         }
      }
    }
}

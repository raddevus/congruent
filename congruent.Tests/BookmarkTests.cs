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
      List<Bookmark> allBms = new();
      foreach (Bookmark b in bmList){
         allBms.Add(b);
      }
      var targetCounter = allBms.Count;
      var counter = 0;
      for (int x = 0;x < allBms.Count; x++){
         counter++;
         Console.WriteLine($"x: {x} counter: {targetCounter}");
         Console.WriteLine($"b.Title : {allBms[x].Title}");
         Console.WriteLine($"b.Children : {allBms[x].Children.Count}");
            foreach (Bookmark i in allBms[x].Children){ allBms.Add(i);}
         if (counter == targetCounter){
            Console.WriteLine($"allBms.Count {allBms.Count}");
            targetCounter = allBms[x].Children.Count;
            
            Console.WriteLine($"targetCounter: {targetCounter}");
            counter = 0;
         }
      }
    }
}

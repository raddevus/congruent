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
      var resultBm = await vm.FindTargetBookmark("Favorites");
      if (resultBm != null){
         Console.WriteLine($"Title {resultBm.Title} : hashcode : {resultBm.GetHashCode()}");
      }
      resultBm = await vm.FindTargetBookmark("one-2");
      if (resultBm != null){
         Console.WriteLine($"Title {resultBm.Title} : hashcode : {resultBm.GetHashCode()}");
      }
      resultBm = await vm.FindTargetBookmark("allos");
      if (resultBm != null){
         Console.WriteLine($"Title {resultBm.Title} : hashcode : {resultBm.GetHashCode()}");
      }
      Console.WriteLine("Get allos parent...");
      resultBm = await vm.FindTargetBookmark("allos",true);
      Console.WriteLine($"** Returned ** - {resultBm}");
      if (resultBm != null){
         Console.WriteLine($"Title {resultBm.Title} : hashcode : {resultBm.GetHashCode()}");
      }
    }

    [Fact]
    async public Task FindBookmarkTestBigFile(){
       MainWindowViewModel vm = new();
       Console.WriteLine("I did it!");

       Bookmark bm = new(){
           BookmarkPath=AppContext.BaseDirectory,
           BookmarkFile="7levelsplus.json"
      };

      Console.WriteLine($"base path => {bm.BookmarkPath}");

      var bmList = await bm.LoadFromFile();
      foreach (Bookmark b in bmList){
         vm.AllBookmarks.Add(b);
      }
      Console.WriteLine($"Test is ready. Have {vm.AllBookmarks.Count} bookmarks loaded.");
      var resultBm = await vm.FindTargetBookmark("comicreader", true);
      if (resultBm != null){
         Console.WriteLine($"Title {resultBm.Title} : hashcode : {resultBm.GetHashCode()}");
      }
      resultBm = await vm.FindTargetBookmark("mojiWriter🤓", true);
      if (resultBm != null){
         Console.WriteLine($"Title {resultBm.Title} : hashcode : {resultBm.GetHashCode()}");
      }
    }
    
    [Fact]
    async public Task DoesBookmarkExistTest(){
       MainWindowViewModel vm = new();
       Console.WriteLine("I did it!");

       Bookmark bm = new(){
           BookmarkPath=AppContext.BaseDirectory,
           BookmarkFile="7levelsplus.json"
      };

      Console.WriteLine($"base path => {bm.BookmarkPath}");

      var bmList = await bm.LoadFromFile();
      foreach (Bookmark b in bmList){
         vm.AllBookmarks.Add(b);
      }
      Console.WriteLine($"Test is ready. Have {vm.AllBookmarks.Count} bookmarks loaded.");
      Bookmark t1bm = new(){
         Title="garbage",
         Link="http://garbage.com"
      };
      Console.WriteLine($"##### {t1bm.Title ?? "<empty>"} ====> Does it exist: {await vm.DoesBookmarkExist(t1bm)}");
      t1bm.Link = "http://rpix.local/";
      t1bm.Title = "http://rpix.local/";
      Console.WriteLine($"##### {t1bm.Title ?? "<empty>"} ====> Does it exist: {await vm.DoesBookmarkExist(t1bm)}");
      t1bm.Link = string.Empty;
      t1bm.Title = "level6";
      Console.WriteLine($"##### {t1bm.Title ?? "<empty>"} ====> Does it exist: {await vm.DoesBookmarkExist(t1bm)}");
      t1bm.Link = string.Empty;
      t1bm.Title = "Favorites";
      Console.WriteLine($"##### {t1bm.Title ?? "<empty>"}====> Does it exist: {await vm.DoesBookmarkExist(t1bm)}");
      t1bm.Link = string.Empty;
      t1bm.Title = "SFConservancy";
      Console.WriteLine($"##### {t1bm.Title ?? "<empty>"}====> Does it exist: {await vm.DoesBookmarkExist(t1bm)}");
      t1bm.Link = "sfconservancy.org";
      t1bm.Title = "SFConservancy";
      Console.WriteLine($"##### {t1bm.Title ?? "<empty>"}====> Does it exist: {await vm.DoesBookmarkExist(t1bm)}");
      t1bm.Link = "sfconservancy.org";
      t1bm.Title = null;
      Console.WriteLine($"##### {t1bm.Title ?? "<empty>"}====> Does it exist: {await vm.DoesBookmarkExist(t1bm)}");
    }
    
    [Fact]
    async public Task AddOneBookmarkTest(){
       MainWindowViewModel vm = new();
       Console.WriteLine("I did it!");

      Bookmark folderBm = new(){Title="Favorites"};
      vm.AllBookmarks.Add(folderBm);
      folderBm = await vm.FindTargetBookmark(folderBm.Title);
      if (folderBm == null){Console.WriteLine("Folder doesn't exist."); return;}
      Bookmark? targetBm = new();
      targetBm.Link = "newlibre.com";
      targetBm.Title = "newlibre";
      Console.WriteLine($"{targetBm}");
      Console.WriteLine($"Does {targetBm.Title} exist? : {await vm.DoesBookmarkExist(targetBm)}");
    }

    [Fact]
    async public Task FindFolderTest(){
       MainWindowViewModel vm = new();
       Console.WriteLine("I did it!");

       Bookmark bm = new(){
           BookmarkPath=AppContext.BaseDirectory,
           BookmarkFile="7levelsplus.json"
      };

      Console.WriteLine($"base path => {bm.BookmarkPath}");

      var bmList = await bm.LoadFromFile();
      foreach (Bookmark b in bmList){
         vm.AllBookmarks.Add(b);
      }
      Console.WriteLine($"Test is ready. Have {vm.AllBookmarks.Count} bookmarks loaded.");
      var resultFolder = await vm.FindTargetBookmark("Favorites", false);
      Console.WriteLine(resultFolder);
    }
}

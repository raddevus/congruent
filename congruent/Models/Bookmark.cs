using System;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace congruent.Models;

public class Bookmark : INotifyPropertyChanged {
   public string Title {get;set;}
   public string Link {get;set;}
   public string? FolderName{get;set;}

   [JsonIgnore]
   public string BookmarkPath{get; set;} = Path.GetDirectoryName(Environment.ProcessPath);
   private string BookmarkFile = "bookmarks.json";

   private string iconSource;
   public string IconSource {
        get => iconSource;
        set
        {
            if (iconSource != value)
            {
                iconSource = value;
                OnPropertyChanged(nameof(IconSource));
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public override string ToString(){
       return Title;
    }

   public ObservableCollection<Bookmark> Children { get; set; }
        = new ObservableCollection<Bookmark>();

   async public Task<bool> Save(ObservableCollection<Bookmark> allBookmarks){
      var targetFile = Path.Combine(BookmarkPath,BookmarkFile);
      File.Delete(targetFile);
      var output = JsonSerializer.Serialize(allBookmarks);
      await File.AppendAllTextAsync(targetFile, output);
     if (File.Exists(targetFile)){
        Console.WriteLine($"Success! Wrote Bookmarks file. {targetFile}");
        return true;
     }
     return false;
   }

   async public Task<ObservableCollection<Bookmark>> LoadFromFile(){
      var targetFile = Path.Combine(BookmarkPath,BookmarkFile);
      Console.WriteLine($"targetFile : {targetFile}");
      var allBookmarks = await File.ReadAllTextAsync(targetFile);
      Console.WriteLine("deserializing...");
      var bookmarks =  JsonSerializer.Deserialize<List<Bookmark>>(allBookmarks);
      Console.WriteLine($"keys: {bookmarks}");
      ObservableCollection<Bookmark> bm = new();
      foreach (Bookmark b in bookmarks){ bm.Add(b);} 
      return bm;
   }

}

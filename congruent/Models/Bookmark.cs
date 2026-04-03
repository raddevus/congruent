using System;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

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

}

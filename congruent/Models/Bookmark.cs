using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace congruent.Models;

public class Bookmark{
   string Title {get;set;}
   string Link {get;set;}
   string? FolderName{get;set;}
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

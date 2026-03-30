using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace congruent.Models;

public class Bookmark{
   string Title {get;set;}
   string Link {get;set;}
   string? FolderName{get;set;}

   public ObservableCollection<Bookmark> Children { get; set; }
        = new ObservableCollection<Bookmark>();
}

using System.Collections.ObjectModel;
using congruent.Models;

namespace congruent.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
   public ObservableCollection<WebLink> AllLinks{get;set;} = new();
   public ObservableCollection<Bookmark> AllBookmarks{get;set;} = new();

}

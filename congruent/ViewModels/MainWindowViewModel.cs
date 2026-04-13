using System.Collections.ObjectModel;
using congruent.Models;
using System.Threading.Tasks;

namespace congruent.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
   public ObservableCollection<Bookmark> AllBookmarks{get;set;} = new();

   async public Task<Bookmark> FindTargetBookmark(){
      return new Bookmark();
   }

   async public Task<Bookmark> FindBookmarkParent(){
      return new Bookmark();
   }

}

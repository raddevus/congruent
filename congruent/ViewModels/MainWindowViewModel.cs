using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using congruent.Models;
using System.Threading.Tasks;
using System.Linq;


namespace congruent.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
   public ObservableCollection<Bookmark> AllBookmarks{get;set;} = new();

   async public Task<Bookmark> FindTargetBookmark(
            string currentBookmarkFolder,
            int hashcode, bool isGetParent = false){
            var bm = AllBookmarks?.ToList<Bookmark>().FirstOrDefault(b => b?.Title == currentBookmarkFolder);

            Console.WriteLine($"AllBookmarks.Count: {AllBookmarks.Count}");
            Bookmark? targetBm = null;

            List<Bookmark> allBms = new();
            foreach (Bookmark b in AllBookmarks){
               allBms.Add(b);
            }

            var targetCounter = allBms.Count;
            var counter = 0;
            Bookmark parent = null;
            for (int x = 0;x < allBms.Count; x++){
               // parent is item before counter is incremented
               parent = allBms[x];
               counter++;
               Console.WriteLine($"b.Title : {allBms[x].Title}");

               if (allBms[x].GetHashCode() == hashcode){
                  if (isGetParent){
                     Console.WriteLine($"Found parent: {parent.GetHashCode()} : {parent.Title}");
                     Console.WriteLine($"Found child: {allBms[x].GetHashCode()}");
                     return parent;
                  }
                  targetBm = allBms[x];
                  return targetBm;
               }
               foreach (Bookmark i in allBms[x].Children){
                 Console.WriteLine($"Parent title ==> {allBms[x].Title}");
                  allBms.Add(i);}
               if (counter == targetCounter){
                  targetCounter = allBms[x].Children.Count;
                  counter = 0;
               }
            }

            return null;
      }

   async public Task<Bookmark> FindBookmarkParent(){
      return new Bookmark();
   }

}

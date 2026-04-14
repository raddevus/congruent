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
            //int hashcode,
            bool isGetParent = false
            ){
            var bm = AllBookmarks?.ToList<Bookmark>().FirstOrDefault(b => b?.Title == currentBookmarkFolder);

            Console.WriteLine($"AllBookmarks.Count: {AllBookmarks.Count}");
            Bookmark? targetBm = null;

            List<Bookmark> allBms = new();
            HashSet<Bookmark> allParents = new();
            foreach (Bookmark b in AllBookmarks){
               allBms.Add(b);
            }

            var targetCounter = allBms.Count;
            var counter = 0;
            Bookmark parent = null;
            for (int x = 0;x < allBms.Count; x++){
               counter++;
               Console.WriteLine($"b.Title : {allBms[x].Title}");

               //if (allBms[x].GetHashCode() == hashcode){
               if (allBms[x].Title == currentBookmarkFolder){
                  if (isGetParent){
                     Console.WriteLine($"Found parent: {allParents.ToList<Bookmark>()[allParents.Count-1]?.GetHashCode()} : {allParents.ToList<Bookmark>()[allParents.Count-1]?.Title}");
                     Console.WriteLine($"Found child: {allBms[x].GetHashCode()}");
                     return parent;
                  }
                  targetBm = allBms[x];
                  return targetBm;
               }
               foreach (Bookmark i in allBms[x].Children){
                  allParents.Add(allBms[x]);
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

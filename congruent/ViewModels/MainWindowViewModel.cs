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
            string title,
            //int hashcode,
            bool isGetParent = false
            ){

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
               if (allBms[x].Title == title){
                  if (isGetParent){
                     if (allParents.Count >0){
                     parent = allParents.ToList<Bookmark>()[allParents.Count-1];
                     Console.WriteLine($"Found parent: {parent?.GetHashCode()} : {parent?.Title}");
                     Console.WriteLine($"Found child: {allBms[x].GetHashCode()}");
                     }
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

   async public Task<bool> DoesBookmarkExist(Bookmark inMark){
      //if Link is Null or Empty then we have a parent (folder)
      //otherwise we check to see if a normal bookmark exists
      var outMark = await FindTargetBookmark(inMark.Title, !string.IsNullOrEmpty(inMark.Link));
      var outByLink = await FindTargetBookmarkByLink(inMark.Link); 
      if (outMark == null && outByLink == null){
         return false;
      }
      return true;
   }

   async public Task<Bookmark> FindBookmarkParent(){
      return new Bookmark();
   }

   async public Task<Bookmark> FindTargetBookmarkByLink(
            string link,
            //int hashcode,
            bool isGetParent = false
            ){

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
               if (allBms[x].Link == link){
                  if (isGetParent){
                     if (allParents.Count >0){
                     parent = allParents.ToList<Bookmark>()[allParents.Count-1];
                     Console.WriteLine($"Found parent: {parent?.GetHashCode()} : {parent?.Title}");
                     Console.WriteLine($"Found child: {allBms[x].GetHashCode()}");
                     }
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
}

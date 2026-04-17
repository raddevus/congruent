using System;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Controls;
using Avalonia.Interactivity;  // Adds items necessary for event handlers
using System.Threading.Tasks;
using congruent.ViewModels;
using congruent.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace congruent.Views;

public partial class MainWindow : Window
{

   private TabItem currentTab = null;
   private NativeWebView currentWebView = null;
   private string currentBookmarkFolder = null;

    public MainWindow()
    {
        InitializeComponent();
        this.Closing += async (s,e)  =>  { 
           var vm = (MainWindowViewModel)DataContext;
           await new Bookmark().Save(vm.AllBookmarks);
        };

    }
    
    async protected override void OnOpened(EventArgs e){
       base.OnOpened(e);
       MainWebView.Focus();
       MainWebView.Source = new System.Uri("https://duckduckgo.com");
       currentWebView = MainWebView;
       // following three lines force the initial render of webview
       wnd.Width += 2;
       System.Threading.Thread.Sleep(100);
       wnd.Width -= 2;
       NavPathTB.Focus();
       var vm = (MainWindowViewModel) DataContext;

       var loadedBm = await new Bookmark().LoadFromFile();
       if (loadedBm != null){
          foreach (Bookmark b in loadedBm){
            vm.AllBookmarks.Add(b);
          }
       }
       else{
         var bm = new Bookmark(){
             Title="Favorites", 
             IconSource = "📂",
             };
          vm.AllBookmarks.Add(bm);
       }
         // get the root item of the bookmark tree      
         var rootItem = BookmarkTree.Items[0];
         // You have to convert the bookmark item into a TreeViewItem
         var rootTreeViewItem = BookmarkTree.TreeContainerFromItem(rootItem) as TreeViewItem;

         if (rootTreeViewItem != null)
         {
            Console.WriteLine($"hashcode  : {(rootItem as Bookmark).GetHashCode()} text : {(rootItem as Bookmark).Title}");
             BookmarkTree.ExpandSubTree(rootTreeViewItem);
         }
       // "📝"
    }
      async private void OpenLink_Click(object? sender, RoutedEventArgs e)
      {
          if (BookmarkTree.SelectedItem is Bookmark bm && !string.IsNullOrEmpty(bm.Link))
          {
             Console.WriteLine("opening link");
             CopyToClipboard(bm.Link);
             var result = await PasteToNavPath();
            if (result){
               NavigateToUrl();
            }

          }
      }

      async private void AddNewBookmark_Click(object? sender, RoutedEventArgs e){
         var msg = new BMarkMsgBox("Please add a Link Title (shows up in the treeview) &amp; a Link URL.");
          bool dialogResult = await msg.ShowDialog<bool>(this);
          if (dialogResult)
          {
             Console.WriteLine("dialog result is good");
          var title = msg.LinkTitle;
          var url = msg.LinkUrl;
            // insuring that values are set to some string
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(url)){ return;}
            var vm = (MainWindowViewModel)DataContext;
            // folderBm represents the folder where wee are adding the new link
            Bookmark folderBm = new(){Title=currentBookmarkFolder};
            folderBm = await vm.FindTargetBookmark(folderBm.Title);
            if (folderBm == null){Console.WriteLine("Folder doesn't exist."); return;}
            Bookmark? targetBm = new();
            targetBm.Link = msg.LinkUrl;
            targetBm.Title = msg.LinkTitle;
            Console.WriteLine($"{targetBm}");
            // bmCheck insures that a BM doesn't exist with same title & link
            var bmCheck = await vm.DoesBookmarkExist(targetBm);
            if (!bmCheck){
               Console.WriteLine($"bm : {targetBm}");
               Console.WriteLine($"bm.Title: {targetBm?.Title} folder.title {currentBookmarkFolder}");
               folderBm?.Children.Add(new Bookmark(){
                Title= title, 
                Link = url,
                IconSource = "📝",
                });

               //Added a new bookmark so we save to Bookmarks file
               targetBm.Save(vm.AllBookmarks);
               return;
            }
            Console.WriteLine($"A bookmark with that link already exists.");

          }
      }

      async private Task<Bookmark?> FindTargetBookmark(
            string targetFolderTitle, bool isGetParent = false){
            var vm = (MainWindowViewModel)DataContext;
            var bm = vm.AllBookmarks?.FirstOrDefault(b => b?.Title == currentBookmarkFolder);

            Console.WriteLine($"vm.AllBookmarks.Count: {vm.AllBookmarks.Count}");
            Bookmark? targetBm = null;

            List<Bookmark> allBms = new();
            foreach (Bookmark b in vm.AllBookmarks){
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
               if (allBms[x].Title == targetFolderTitle){
                  if (isGetParent){
                     return parent;
                  }
                  targetBm = allBms[x];
                  Console.WriteLine($"parent.Title: {parent.Title}");
                  return targetBm;
               }
               foreach (Bookmark i in allBms[x].Children){ allBms.Add(i);}
               if (counter == targetCounter){
                  targetCounter = allBms[x].Children.Count;
                  counter = 0;
               }
            }

            return null;
      }

      async private void DeleteBookmark_Click(object? sender, RoutedEventArgs e){
         DeleteBookmark();
      }

      async private void DeleteBookmark(){

        
       if (BookmarkTree.SelectedItem is Bookmark bm && !string.IsNullOrEmpty(bm.Link))
       {
          var vm = (MainWindowViewModel) DataContext;
          var targetBm = await vm.FindTargetBookmarkByLink(bm.Link);
          Console.WriteLine($"**DEL** {targetBm.Link}");
          var parent = await vm.FindTargetBookmark(bm.Title, true);
          if (parent != null){
             Console.WriteLine($"**DEL** parent.Title: {parent.Title}");
             Console.WriteLine($"index : {parent.Children.Count}");
                  // .FirstOrDefault(b => b.GetHashCode() == bm.GetHashCode())}");
             var result = parent.Children.Remove(targetBm);
             Console.WriteLine($"delete result: {result}");
          }
          Console.WriteLine("Successfully deleted.");
       }
      }

      async private void MoveBookmark(object? sender, RoutedEventArgs e){
                  
          if (BookmarkTree.SelectedItem is Bookmark bm && !string.IsNullOrEmpty(bm.Link))
          {

            var msg = new AddFolderMsgBox("Type the folder name you want to create.");
             var vm = (MainWindowViewModel)DataContext;
             bool dialogResult = await msg.ShowDialog<bool>(this);
             if (dialogResult)
             {
                // find the foldername that the user wants to move
                // this link to
                Bookmark? parentBm = null;
                Bookmark? targetBm = await FindTargetBookmark(msg.FolderName);
                if (targetBm == null){
                   // Let user know we couldn't find a folder with the provided Title
                   Console.WriteLine("Couldn't find a matching bookmark folder.");
                   return;
                }
                //We've found the targetBm folder so we add the 
                // selected bm to the Children - before deleting 
                targetBm.Children.Add(bm);
                // Now we need to delete it from the old location.
                // 
                //parentBm = await FindTargetBookmark(msg.FolderName);
                Console.WriteLine("Just remove the original bm - maybe this works!");
//                vm.AllBookmarks.Remove(bm);
                 DeleteBookmark();
             }
          }
          else{
             // Tell user that we cannot move a Folder, only a link
             // 
         }
      }

      async private void NewFolder_Click(object? sender, RoutedEventArgs e)
      {
          if (BookmarkTree.SelectedItem is Bookmark bm && string.IsNullOrEmpty(bm.Link))
          {
             Console.WriteLine($"bm: {bm.Title}");
            var msg = new AddFolderMsgBox("Type the folder name you want to create.");
             var vm = (MainWindowViewModel)DataContext;
             bool dialogResult = await msg.ShowDialog<bool>(this);
             if (dialogResult)
             {
                Console.WriteLine("dialog result is good");
                var folderName = msg.FolderName;
                bm.Children.Add(new Bookmark(){
                Title = folderName, 
                IconSource = "📂",
                });
             }
          }
      }


      private void CopyLink_Click(object? sender, RoutedEventArgs e)
      {
          if (BookmarkTree.SelectedItem is Bookmark bm)
          {
             Console.WriteLine($"bm: {bm.Link}");
             CopyToClipboard(bm.Link);
          }
      }
    private async void TviClick(object? sender, SelectionChangedEventArgs e){
       var targetNode = (sender as TreeView)?.SelectedItem as Bookmark;
       if (string.IsNullOrEmpty(targetNode?.Link)){
             currentBookmarkFolder = targetNode?.Title;
             Console.WriteLine($" currentBookmarkFolder: {currentBookmarkFolder} hashcode: {targetNode?.GetHashCode()}");
         }
    }

   private void Cut_Click(object? sender, RoutedEventArgs e)
   {
      Console.WriteLine("Cutting...");
       if (sender is MenuItem mi &&
           mi.Parent is ContextMenu cm &&
           cm.PlacementTarget is TextBox tb)
           tb.Cut();
   }
   async private void CopyToClipboard(string textToCopy){
      Console.WriteLine("Copying...");
         try{
            var clipboard = AppHelpers.Clipboard.GetClipboard();
            var item = DataTransferItem.CreateText(textToCopy);

            // 2. Create a DataTransfer and add the item
            var transfer = new DataTransfer();
            transfer.Add(item);

         await clipboard!.SetDataAsync(transfer);
      }
      catch (Exception ex){
         Console.WriteLine("Couldn't copy to clipboard.");
      }
   }

   async private void Copy_Click(object? sender, RoutedEventArgs e)
   {
      CopyToClipboard(NavPathTB.Text);
   }

   async private void Paste_Click(object? sender, RoutedEventArgs e)
   {
      Console.WriteLine("Pasting...");
      PasteToNavPath();
   }

   async private void AddNewFavorite(object? sender, RoutedEventArgs e)
   {
       var vm = (MainWindowViewModel) DataContext;
       var bm = vm.AllBookmarks.First(a => a.Title == "Favorites");
       bm?.Children.Add(new Bookmark()
                   {Title=NavPathTB.Text,Link=NavPathTB.Text,IconSource="📝"});

   }

   async private void PasteAndGo(object? sender, RoutedEventArgs e)
   {
      var result = await PasteToNavPath();
      if (result){
         NavigateToUrl();
      }
   }

   async private Task<bool> PasteToNavPath(){

      try{
         var clipboard = AppHelpers.Clipboard.GetClipboard();
         var data = await clipboard.TryGetTextAsync();
         if (data is not null)
         {
             NavPathTB.Text = data;
             Console.WriteLine($"retrieved from clipboard: {data}");
         }
         return true;
      } catch(Exception ex){ 
         Console.WriteLine($"{ex.Message}");
         return false;
      }
   }

   private void NavigationCompleted(object? sender, WebViewNavigationCompletedEventArgs args)
   {
      Console.WriteLine(" **** Nav Completed ****");
       if (args.IsSuccess)
       {
           // Navigation completed successfully
           Console.WriteLine("I'm done.");
       }
   }

  private void NavigateToUrl(){
       var targetUrl = ValidateUrl(NavPathTB.Text);
       if (targetUrl == string.Empty){
          // if it's an invalid URL throw it away & return
          NavPathTB.Text = string.Empty;
          NavPathTB.Focus();
          return;
       }
       NavPathTB.Text = targetUrl;
      AddNewTab(targetUrl.ToString());
  }

   private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
   {
       if (e.Key == Key.Enter)
       {
         NavigateToUrl();
       }
   }

   public string ValidateUrl(string url)
   {
       if (url.StartsWith("file:///", StringComparison.OrdinalIgnoreCase)){
          Console.WriteLine($"return url : {url}");
           return url; 
      }
      if (string.IsNullOrEmpty(url)){return string.Empty;}
       // Check if the URL already has a scheme (http:// or https://)
       if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && 
           !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
       {
           url = "https://".Trim() + url; // Prepend https://
       }

       // Validate the URL is well-formed as an absolute URI
       if (Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && 
           (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
       {
           return url; // Valid URL
       }
       return string.Empty;
   }

   public void AddNewTab(string url, bool isWebView = true)
   {
      TabItem tab = null;
      if (isWebView){
          var webView = new NativeWebView
          {
              Source = new Uri(url)
          };

          currentWebView = webView;
          tab = new TabItem
          {
              Header = url,
              Content = webView
          };
      }
      else{
         tab = new TabItem{
            Header = NavPathTB.Text,
            Content = url
         };
      }


       BrowserTabs.Items.Add(tab);
       BrowserTabs.SelectedItem = tab;
   }
  
   private void OnTabSelectionChanged(object? sender, SelectionChangedEventArgs e)
   {
       if (BrowserTabs?.SelectedItem is TabItem tab)
       {
         // Any time tab is clicked we save it as the currentTab
         // so we can track it in case the user wants to remove it
          currentTab = tab;
           Console.WriteLine($"Selected tab: {tab.Header}");

           if (tab.Content is NativeWebView web)
           {
               Console.WriteLine($"WebView URL: {web.Source}");
               var w = wnd?.Width;
               var h = wnd?.Height;
               Console.WriteLine($"Height {h} Width {w}");
               NavPathTB.Text = web.Source.ToString();
               NavPathTB.Focus();
               NavPathTB.CaretIndex = web.Source.ToString().Length;
               // Following three lines force webview to render
               wnd.Width += 2;
               System.Threading.Thread.Sleep(10);
               wnd.Width -= 2;
               currentWebView = web;

           }
       }
   }


   private void ForwardButtonClick(object? sender, RoutedEventArgs e){
      var result = currentWebView?.GoForward();
      Console.WriteLine($"forward result: {result}");
   }

   private void BackButtonClick(object? sender, RoutedEventArgs e){
      var result = currentWebView?.GoBack();
      Console.WriteLine($"result: {result}");
   }

   private void OnWebViewLoaded(object? sender, RoutedEventArgs e)
   {
//       if (sender is NativeWebView web &&
 //          web.Parent is Control parent)
  //     {
         Console.WriteLine("##### OnWEbViewLoaded #############");
           var web = (sender as NativeWebView);
           
               MainWebView.InvalidateMeasure();
               MainWebView.InvalidateArrange();
               MainWebView.InvalidateVisual();
               BrowserTabs.SelectedItem  = BrowserTabs.Items[0];               
               web.InvalidateMeasure();
               web.InvalidateArrange();
               web.InvalidateVisual();
   //    }
   }

   private void OnCloseTab(object? sender, RoutedEventArgs e){
      Console.WriteLine($"There are {BrowserTabs.Items.Count} tabs open.");
      foreach (var item in BrowserTabs.Items)
      {
          if (item is TabItem tabx){
              Console.WriteLine($"Tab: {tabx.Header}");
          }
      }
      
      BrowserTabs.Items.Remove(currentTab);
   }
   
   async private void ViewSource(object? sender, RoutedEventArgs e){
      ViewSource(currentWebView);
   }


   private async void ViewSource(NativeWebView webView)
   {
      var result =  await webView?.InvokeScript("document.documentElement.outerHTML");
       if (result is string html)
       {
           // Show in a dialog, save to file, open in a new tab, etc.
           Console.WriteLine(html);
           AddNewTab(html, false);
       }
   }

   private void OnDuplicateTab(object? sender, RoutedEventArgs e)
   {
   }

   private void OnReloadTab(object? sender, RoutedEventArgs e)
   {
      currentWebView.Source = new Uri(NavPathTB.Text);
   }


}

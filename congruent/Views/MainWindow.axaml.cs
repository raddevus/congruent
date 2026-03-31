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

namespace congruent.Views;

public partial class MainWindow : Window
{

   private TabItem currentTab = null;
   private NativeWebView currentWebView = null;

    public MainWindow()
    {
        InitializeComponent();
    }
    
    protected override void OnOpened(EventArgs e){
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
       var bm = new Bookmark(){
             Title="Favorites", 
             IconSource = "📂",
             };
      bm.Children.Add(new Bookmark()
                   {Title="allos.dev",Link="https://allos.dev",IconSource="📝"});
      bm.Children.Add(new Bookmark()
                   {Title="cyapass.com",Link="https://cyapass.com",IconSource="📝"});
       vm.AllBookmarks.Add(bm);
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

      private void AddNewBookmark_Click(object? sender, RoutedEventArgs e){

      }
      
      private void NewFolder_Click(object? sender, RoutedEventArgs e)
      {
          if (BookmarkTree.SelectedItem is Bookmark bm && string.IsNullOrEmpty(bm.Link))
          {
             Console.WriteLine($"bm: {bm.Title}");
             bm.Children.Add(new Bookmark(){
             Title= DateTime.Now.ToLongDateString(), 
             IconSource = "📂",
             });
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

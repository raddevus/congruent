using System;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Controls;
using Avalonia.Interactivity;  // Adds items necessary for event handlers
using System.Threading.Tasks;


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
    }


   private async void QuickLinkChanged(object? sender, RoutedEventArgs e){
        }

private void Cut_Click(object? sender, RoutedEventArgs e)
{
   Console.WriteLine("Cutting...");
    if (sender is MenuItem mi &&
        mi.Parent is ContextMenu cm &&
        cm.PlacementTarget is TextBox tb)
        tb.Cut();
}

async private void Copy_Click(object? sender, RoutedEventArgs e)
{
   Console.WriteLine("Copying...");
      try{
         var clipboard = AppHelpers.Clipboard.GetClipboard();
         var item = DataTransferItem.CreateText(NavPathTB.Text);

// 2. Create a DataTransfer and add the item
var transfer = new DataTransfer();
transfer.Add(item);

         await clipboard!.SetDataAsync(transfer);
//          Console.WriteLine($"{clipboard}");
         //clipboard?.SetTextAsync(NavPathTB?.Text ?? string.Empty);
      }
      catch (Exception ex){
         Console.WriteLine("Couldn't copy to clipboard.");
      }

    if (sender is MenuItem mi &&
        mi.Parent is ContextMenu cm &&
        cm.PlacementTarget is TextBox tb)
        tb.Copy();
}

   async private void Paste_Click(object? sender, RoutedEventArgs e)
   {
      Console.WriteLine("Pasting...");
      PasteToNavPath();
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
          if (!QuickLinksLB.Items.Contains(NavPathTB.Text)){
                QuickLinksLB.Items.Add(NavPathTB.Text);
          }
                

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

   public void AddNewTab(string url)
   {
       var webView = new NativeWebView
       {
           Source = new Uri(url)
       };

       currentWebView = webView;

       var tab = new TabItem
       {
           Header = url,
           Content = webView
       };

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

   private void OnDuplicateTab(object? sender, RoutedEventArgs e)
   {
   }

   private void OnReloadTab(object? sender, RoutedEventArgs e)
   {
   }


}

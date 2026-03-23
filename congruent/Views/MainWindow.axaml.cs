using System;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;  // Adds items necessary for event handlers
using Avalonia.LogicalTree;


namespace congruent.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    protected override void OnOpened(EventArgs e){
       base.OnOpened(e);
       MainWebView.Focus();
       MainWebView.Source = new System.Uri("https://duckduckgo.com");
       // following two lines force the initial render of webview
       MainWebView.InvalidateMeasure();
       MainWebView.InvalidateArrange();
    }


   private async void QuickLinkChanged(object? sender, RoutedEventArgs e){
        }
   private void NavigationCompleted(object? sender, WebViewNavigationCompletedEventArgs args)
   {
       if (args.IsSuccess)
       {
           // Navigation completed successfully
           Console.WriteLine("I'm done.");
          if (!QuickLinksLB.Items.Contains(NavPathTB.Text)){
                QuickLinksLB.Items.Add(NavPathTB.Text);
          }
                

       }
   }
   
   private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
   {
       if (e.Key == Key.Enter)
       {
          var targetUrl = ValidateUrl(NavPathTB.Text);
          if (targetUrl == string.Empty){
             // if it's an invalid URL throw it away & return
             NavPathTB.Text = string.Empty;
             NavPathTB.Focus();
             return;
          }
          NavPathTB.Text = targetUrl;
//          MainWebView.Source = new System.Uri(targetUrl);
         AddNewTab(targetUrl.ToString());
       }
   }

   public string ValidateUrl(string url)
   {
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
           Console.WriteLine($"Selected tab: {tab.Header}");

           if (tab.Content is NativeWebView web)
           {
               Console.WriteLine($"WebView URL: {web.Source}");
               NavPathTB.Text = web.Source.ToString();
           }
       }
   }

   private void OnCloseTab(object? sender, RoutedEventArgs e){
     var mix = sender as MenuItem; 
      Console.WriteLine($"got the Close event. {mix.GetType()} : {mix?.Tag?.GetType()}");
      Console.WriteLine($"is it a tab item? : {mix.Tag is TabItem}");
      if (sender is MenuItem mi && mi.Tag is TabItem tab)
      {
        BrowserTabs.Items.Remove(tab);
      }
   }

   private void OnDuplicateTab(object? sender, RoutedEventArgs e)
   {
   }

   private void OnReloadTab(object? sender, RoutedEventArgs e)
   {
   }


}

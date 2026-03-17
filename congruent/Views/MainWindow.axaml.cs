using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;  // Adds items necessary for event handlers


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
          MainWebView.Source = new System.Uri(targetUrl);
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

}

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
    }

        private async void QuickLinkChanged(object? sender, RoutedEventArgs e){
        }
   private void NavigationCompleted(object? sender, WebViewNavigationCompletedEventArgs args)
   {
       if (args.IsSuccess)
       {
           // Navigation completed successfully
           Console.WriteLine("I'm done.");
       }
   }
   
   private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
   {
       if (e.Key == Key.Enter)
       {
         MainWebView.Source = new System.Uri(NavPathTB.Text);
       }
   }

}

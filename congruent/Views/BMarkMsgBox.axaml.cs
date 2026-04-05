using Avalonia.Controls;
using Avalonia.Interactivity;
using congruent.Models;

namespace congruent.Views;

public partial class BMarkMsgBox : Window
{
   public string LinkTitle {get;set;}
   public string LinkUrl {get;set;}
   public BMarkMsgBox(): this("default message") {
   }
    public BMarkMsgBox(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
        // Sets Focus to the SiteKey text box
        this.Opened += (_, __) => {LinkTitleTB.Focus(); };
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       // replace all white-space anywhere in string
       LinkTitle = LinkTitleTB?.Text?.Replace(" ", "") ?? string.Empty;
       LinkUrl = LinkUrlTB?.Text?.Replace(" ", "") ?? string.Empty;
       if (string.IsNullOrEmpty(LinkUrl)){
          MessageText.Text = "The string must be a valid URL and not contain any spaces.";
          LinkUrlTB.Focus();
          return;
       }
       if (string.IsNullOrEmpty(LinkTitle)){
          MessageText.Text = "The string must be at least 1 character and not contain any spaces.";
          LinkTitleTB.Focus();
          return;
       }
       Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}

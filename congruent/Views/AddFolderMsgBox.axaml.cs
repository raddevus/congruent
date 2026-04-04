using Avalonia.Controls;
using Avalonia.Interactivity;
using congruent.Models;

namespace congruent.Views;

public partial class AddFolderMsgBox : Window
{
   public string FolderName {get;set;}

   public AddFolderMsgBox(): this("default message") {
   }
    public AddFolderMsgBox(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
        // Sets Focus to the SiteKey text box
        this.Opened += (_, __) => {FolderNameTB.Focus(); };
    }

    private void Ok_Click(object? sender, RoutedEventArgs e)
    {
       // replace all white-space anywhere in string
       FolderName = FolderNameTB?.Text?.Replace(" ", "") ?? string.Empty;
       if (string.IsNullOrEmpty(FolderName)){
          MessageText.Text = "The string must be at least 1 character and not contain any spaces.";
          FolderNameTB.Focus();
          return;
       }
       Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}

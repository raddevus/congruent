using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Input.Platform;
using Avalonia.Controls.ApplicationLifetimes;

namespace AppHelpers; 
public static class Clipboard {
   public static IClipboard? GetClipboard(){
   var app = Application.Current;
        if (app is null)
            return null;

        var lifetime = app.ApplicationLifetime;

        // Desktop: Windows, Linux, macOS
        if (lifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window }){
           Console.WriteLine($"return clipboard 1 {window.Clipboard}");
            return window?.Clipboard;
        }

        // Single-view: Android, iOS, WASM, or other hosts
        if (lifetime is ISingleViewApplicationLifetime { MainView: { } view })
        {
           Console.WriteLine("in clipboard 2");
            var topLevel = TopLevel.GetTopLevel(view);
            Console.WriteLine($"toplevel: {topLevel}");
            return topLevel?.Clipboard;
        }
         Console.WriteLine("bad deal - it's null");
        // Fallback: try to get a TopLevel from any visual root we can infer later if needed.
        return null;
   }
}

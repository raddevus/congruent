using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using Avalonia.VisualTree;

namespace AppHelpers;

public static class Clipboard
{
    /// <summary>
    /// Gets the platform clipboard in a cross-platform Avalonia 12-safe way.
    /// Returns null if no clipboard is available for the current lifetime.
    /// </summary>
    public static IClipboard? GetClipboard()
    {
        var app = Application.Current;
        if (app is null)
            return null;

        var lifetime = app.ApplicationLifetime;

        // Desktop: Windows, Linux, macOS
        if (lifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
            return window.Clipboard;

        // Single-view: Android, iOS, WASM, or other hosts
        if (lifetime is ISingleViewApplicationLifetime { MainView: { } view })
        {
            var topLevel = TopLevel.GetTopLevel(view);
            return topLevel?.Clipboard;
        }

        // Fallback: try to get a TopLevel from any visual root we can infer later if needed.
        return null;
    }
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Input.Platform;
using Avalonia.Controls.ApplicationLifetimes;

namespace AppHelpers;

public class Clipboard
{
    public static IClipboard Get()
    {
        // Desktop
        if (Application.Current?.ApplicationLifetime
            is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
        {
            return window.Clipboard!;
        }

        // Android / iOS / Mobile
        if (Application.Current?.ApplicationLifetime
            is ISingleViewApplicationLifetime { MainView: { } mainView })
        {
            // Avalonia 12: MainView is Control, but GetVisualRoot() is on IVisual
            if (mainView is IVisual visual)
            {
                var root = visual.GetVisualRoot();
                if (root is TopLevel topLevel)
                    return topLevel.Clipboard!;
            }
        }

        return null!;
    }
}


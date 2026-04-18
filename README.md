### I'm Glad You Asked: Congruent Does Have Tabbed Browsing
2026-03-23 - Congruent got tabbed browsing today. Each time you open a new URL you'll get a new tab.<br>
Yes, soon the tabs will get better styles &amp; better tab text.<br>

Look at that bookmark hierarchy.  Create folders to keep your bookmarks organized.<br>
The following snapshot is Congruent running on Linux.
<img width="1410" height="781" alt="image" src="https://github.com/user-attachments/assets/f5b4f868-0ae0-4216-b426-04cf71c69751" />


### Backlog
- [ ] Add links to the quicklinks control
- [ ] Clean up tab title so it doesn't show entire link text (remove http etc)
- [ ] fix links that open in a new window - discover how to open them in current window
- [ ] allow show source
- [ ] as a user I want to create a new tab without typing a URL
- [ ] as a user I want a way. to save common links i use (bookmarks)

#### As I Began Building
~~Discovered that the webview required a commercial license in Avalonia 11, but is soon coming.~~

~~As of March 11, 2026, the Avalonia WebView is going open-source with Avalonia 12, and will be available to all developers without a commercial license.~~ <br> 
### Discovered Workaround For WebView 03-17-2026
There is a preview version of Avalonia.  Altering the `.csproj` file allows us to build on that version and the WebView works great.<br>
`PackageReference Include="Avalonia" Version="12.0.0-preview2"`

#### Info From Brave AI
~~The AVLIC0001 error occurs because the Avalonia.Controls.WebView package was previously part of Avalonia Accelerate, a commercial offering. <br>
However, the team has announced it will be fully open-sourced in the upcoming Avalonia 12 release.~~

### Project On Hold
~~When the new version releases, I'll come back to this project.~~
**2026-03-21**
I was able to figure out how to use the prerelease 12 version and build the app uing the Avalonia WebView<br>
Everything seems to work great and this one uses the Opens Source license<br>


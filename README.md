#### As I Began Building
~~Discovered that the webview required a commercial license in Avalonia 11, but is soon coming.~~

~~As of March 11, 2026, the Avalonia WebView is going open-source with Avalonia 12, and will be available to all developers without a commercial license.~~ <br> 
### Discovered Workaround For WebView 03-17-2026
There is a preview version of Avalonia.  Altering the `.csproj` file allows us to build on that version and the WebView works great.<br>
`PackageReference Include="Avalonia" Version="12.0.0-preview2"`

#### Info From Brave AI
The AVLIC0001 error occurs because the Avalonia.Controls.WebView package was previously part of Avalonia Accelerate, a commercial offering. <br>
However, the team has announced it will be fully open-sourced in the upcoming Avalonia 12 release. 

### Project On Hold
When the new version releases, I'll come back to this project.

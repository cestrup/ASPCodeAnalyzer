using Microsoft.Win32;

namespace AspCodeAnalyzer {

  public class Preferences {
    public const string VERSION = "1.0";
    private string _registryPath;

    public string EditorPath { get; set; } = "UEdit32.exe";
    public string FilePattern { get; set; } = "\"{0}/{1}\"";
    public string LastUsedPath { get; set; } = "C:\\";

    public void LoadPreferences() {
      var key = Registry.CurrentUser.OpenSubKey( _registryPath, true);
      if ( key == null ) {
        return;
      }
      EditorPath = (string) key.GetValue( "EditorPath");
      FilePattern = (string) key.GetValue( "FilePattern");
      LastUsedPath = (string) key.GetValue( "LastUesedPath");
    }

    public void SavePreferences() {
      var key = Registry.CurrentUser.OpenSubKey( _registryPath, true);
      if ( key == null ) {
        key = Registry.CurrentUser.CreateSubKey( _registryPath);
      }
      key.SetValue( "EditorPath", EditorPath);
      key.SetValue( "FilePattern", FilePattern);
      key.SetValue( "LastUesedPath", LastUsedPath);
    }

    public Preferences() {
      _registryPath = "Software\\evolootion\\AspCodeAnalyzer\\" + VERSION;
    }
			
  }
}

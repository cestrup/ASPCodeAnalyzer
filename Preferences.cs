using System;
using Microsoft.Win32;

namespace AspCodeAnalyzer {

  public class Preferences {
    public const String VERSION = "1.0";
    private String _registryPath;
    private String _editorPath = "UEdit32.exe";
    private String _filePattern = "\"{0}/{1}\"";
    private String _lastUsedPath = "C:\\";


    public String EditorPath {
      get {
        return _editorPath;
      }
      set {
        _editorPath = value;
      }
    }
    public String FilePattern {
      get {
        return _filePattern;
      }
      set {
        _filePattern = value;
      }
    }

    public String LastUsedPath {
      get {
        return _lastUsedPath;
      }
      set {
        _lastUsedPath = value;
      }
    }


    public void LoadPreferences() {
      RegistryKey key = Registry.CurrentUser.OpenSubKey( _registryPath, true);
      if ( key == null ) {
        return;
      }
      _editorPath = (String) key.GetValue( "EditorPath");
      _filePattern = (String) key.GetValue( "FilePattern");
      _lastUsedPath = (String) key.GetValue( "LastUesedPath");
    }

    public void SavePreferences() {
      RegistryKey key = Registry.CurrentUser.OpenSubKey( _registryPath, true);
      if ( key == null ) {
        key = Registry.CurrentUser.CreateSubKey( _registryPath);
      }
      key.SetValue( "EditorPath", _editorPath);
      key.SetValue( "FilePattern", _filePattern);
      key.SetValue( "LastUesedPath", _lastUsedPath);
    }

    public Preferences() {
      _registryPath = "Software\\evolootion\\AspCodeAnalyzer\\" + VERSION;
    }
			
  }
}

using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace AspCodeAnalyzer {
  public class Analyzer {
    private String _basePath;
    private ArrayList _files = new ArrayList();
    private AnalyzerGui _gui;
    private DateTime _start;


    public String BasePath {
      get {
        return _basePath;
      }
      set {
        _basePath = value;
      }
    }

    public ArrayList Files {
      get {
        return _files;
      }
      set {
        _files = value;
      }
    }


    public void AddGeneralError( String pErrorText) {
      _gui.lbError.Items.Add( pErrorText);
      UpdateElapsedTime();
    }


    public AspFile AddAspFile( String pFilename) {
      AspFile res;

      pFilename = pFilename.ToLower();
      for (int i = 0; i < _files.Count; i++) {
        AspFile curFile = (AspFile) _files[ i];
        if (curFile.Filename == pFilename) {
          return curFile;
        }        
      }
      res = new AspFile( pFilename, this);
      _files.Add( res);
      return res;
    }


    private void AddFiles( String pPath) {
      string[] aspFiles;
      string[] incFiles;
      _gui.lblCurrentAction.Text = "Scanning " + pPath;
      UpdateElapsedTime();
      try {
        aspFiles = Directory.GetFiles( pPath, "*.asp");
        incFiles = Directory.GetFiles( pPath, "*.inc");
      }
      catch ( Exception ) {
        return;
      }

      for( int i = 0; i < aspFiles.Length; i++) {
        AddAspFile( aspFiles[ i]);
        _gui.lblAnalyzedFiles.Text = _files.Count.ToString();
        UpdateElapsedTime();
      }
      for( int i = 0; i < incFiles.Length; i++) {
        AddAspFile( incFiles[ i]);
        _gui.lblAnalyzedFiles.Text = _files.Count.ToString();
        UpdateElapsedTime();
      }
      string[] dirs = Directory.GetDirectories( pPath);
      for ( int i = 0; i < dirs.Length; i++) {
        AddFiles( dirs[i]);
      }
    }


    public void Analyze() {
      try {
        _start = DateTime.Now;
        if ( !Directory.Exists( _basePath) ) {
          AddGeneralError( "Directory " + _basePath + " does not exist");          
        } else {
          SetCurrentAction( "Scanning *.asp;*.inc");

          AddFiles( _basePath);
          int i = 0; 
          while ( i < _files.Count) {
            AspFile curFile = (AspFile) _files[ i];
            SetCurrentAction( "Analyzing " + curFile.Filename);
            curFile.Read();
            _gui.lblAnalyzedFiles.Text = String.Format( "{0} / {1}", i + 1, _files.Count);
            UpdateElapsedTime();
            i++;        
          }
          for ( i = 0; i < _files.Count; i++) {
            AspFile curFile = (AspFile) _files[ i];
            SetCurrentAction( "Finding Unused Elements " + curFile.Filename);
            _gui.lblAnalyzedFiles.Text = String.Format( "{0} / {1}", i + 1, _files.Count);
            UpdateElapsedTime();
            curFile.CheckUsedVariables();
            curFile.CheckUsedFunctions();
          }
          for ( i = 0; i < _files.Count; i++) {
            AspFile curFile = (AspFile) _files[ i];
            SetCurrentAction( "Reporting Unused Elements " + curFile.Filename);
            _gui.lblAnalyzedFiles.Text = String.Format( "{0} / {1}", i + 1, _files.Count);
            UpdateElapsedTime();
            curFile.ReportUnusedVariables();
            curFile.ReportUnusedFunctions();
          }

        }
      } catch ( Exception e) {
        AddGeneralError( e.Message);
      }
      _gui.btnGo.Text = "Go";
      SetCurrentAction( "Idle");
    }


    public void PublishResult( Result pResult) {
      if (pResult != null) {
        _gui.lbResult.Items.Insert( 0, pResult);          
      }
      _gui.lblFindings.Text = _gui.lbResult.Items.Count.ToString();
      UpdateElapsedTime();
    }



    public void SetCurrentAction( String pCurrentAction) {
      _gui.lblCurrentAction.Text = pCurrentAction;
      UpdateElapsedTime();
    }

    public void UpdateElapsedTime() {
      _gui.lblElapsedTime.Text = Math.Round( (DateTime.Now - _start).TotalSeconds, 0).ToString() + " s";
    }


    public Analyzer( AnalyzerGui pGui, String pBasePath) {
      _gui = pGui;
      _basePath = pBasePath;
    }
  }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace AspCodeAnalyzer {
  public class Analyzer {
    private AnalyzerGui _gui;
    private DateTime _start;

    public string BasePath { get; set; }
    public List<AspFile> Files { get; set; } = new List<AspFile>();

     public void AddGeneralError( string pErrorText) {
      _gui.lbError.Items.Add( pErrorText);
      UpdateElapsedTime();
    }

    public AspFile AddAspFile( string pFilename) {
      pFilename = pFilename.ToLower();

      foreach (var curFile in Files)
      {
          if (curFile.Filename == pFilename) {
              return curFile;
          }
      }
      var res = new AspFile( pFilename, this);
      Files.Add( res);
      return res;
    }


    private void AddFiles( string pPath) {
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

      foreach (string t in aspFiles)
      {
          AddAspFile( t);
          _gui.lblAnalyzedFiles.Text = Files.Count.ToString();
          UpdateElapsedTime();
      }
      foreach (string t in incFiles)
      {
          AddAspFile( t);
          _gui.lblAnalyzedFiles.Text = Files.Count.ToString();
          UpdateElapsedTime();
      }
      var dirs = Directory.GetDirectories( pPath);
      foreach (string t in dirs)
      {
          AddFiles( t);
      }
    }


    public void Analyze() {
      try {
        _start = DateTime.Now;
        if ( !Directory.Exists( BasePath) ) {
          AddGeneralError( "Directory " + BasePath + " does not exist");
        } else {
          SetCurrentAction( "Scanning *.asp;*.inc");
          var ignoredFiles = new List<string>() {"adovbs.asp"};

          AddFiles( BasePath);

          int i = 0;
          while ( i < Files.Count) {
            var curFile = Files[i];
            SetCurrentAction( "Analyzing " + curFile.Filename);
            curFile.Read();
            _gui.lblAnalyzedFiles.Text = string.Format( "{0} / {1}", i + 1, Files.Count);
            UpdateElapsedTime();
            i++;        
          }

          foreach (var curFile in Files.Where(f => ignoredFiles.Any(s => string.Equals(Path.GetFileName(f.Filename), s, StringComparison.CurrentCultureIgnoreCase))))
          {
            curFile.MarkAllVariablesUsed();
          }

          for ( i = 0; i < Files.Count; i++) {
            var curFile = Files[i];

            SetCurrentAction( "Finding unused variables " + curFile.Filename);
            _gui.lblAnalyzedFiles.Text = string.Format( "{0} / {1}", i + 1, Files.Count);
            UpdateElapsedTime();
            curFile.CheckUsedVariables();

            SetCurrentAction( "Finding unused functions " + curFile.Filename);
            UpdateElapsedTime();
            curFile.CheckUsedFunctions();
          }

          for ( i = 0; i < Files.Count; i++) {
            var curFile = Files[i];
            SetCurrentAction( "Reporting unused elements " + curFile.Filename);
            _gui.lblAnalyzedFiles.Text = string.Format( "{0} / {1}", i + 1, Files.Count);
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



    public void SetCurrentAction( string pCurrentAction) {
      _gui.lblCurrentAction.Text = pCurrentAction;
      UpdateElapsedTime();
    }

    public void UpdateElapsedTime() {
      _gui.lblElapsedTime.Text = Math.Round( (DateTime.Now - _start).TotalSeconds, 0).ToString() + " s";
    }


    public Analyzer( AnalyzerGui pGui, string pBasePath) {
      _gui = pGui;
      BasePath = pBasePath;
    }
  }
}

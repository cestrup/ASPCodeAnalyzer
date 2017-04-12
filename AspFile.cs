using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Diagnostics;


namespace AspCodeAnalyzer {
  public class AspFile  {
    private String _filename;
    private bool _isRead;
    private Analyzer _analyzer;
    private ArrayList _includes = new ArrayList();
    private ArrayList _blocks = new ArrayList();
    private ArrayList _variables = new ArrayList();
    private ArrayList _classes = new ArrayList();
    private ArrayList _functions = new ArrayList();
    private String _scopeText = "";


    public bool IsRead {
      get {
        return _isRead;
      }
    }

    public ArrayList Includes {
      get {
        return _includes;
      }
      set {
        _includes = value;
      }
    }


    public String Filename {
      get {
        return _filename;
      }
      set {
        _filename = value;
      }
    }


    private static String GetSandwichedValue( String pLine, String pPrefix, String pPostfix, int pStartPos) {
      int pos1 = pLine.IndexOf( pPrefix, pStartPos);
      if (pos1 == -1) {
        return null;
      }
      int pos2 = pLine.IndexOf( pPostfix, pos1 + pPrefix.Length);
      if ( pos2 == -1) {
        return null;
      }
      return pLine.Substring( pos1 + pPrefix.Length, pos2 - pos1 - pPrefix.Length);
    }


    private bool IsIncludeLine( String pLine, int pPos) {
      int pos1 = pLine.IndexOf( "#include", pPos);
      if (pos1 == -1) {
        return false;
      }
      pos1 = pLine.IndexOf( "file", pos1);
      if (pos1 == -1) {
        return false;
      }
      String include = GetSandwichedValue( pLine, "\"", "\"", pos1);
      if (include == null) {
        return false;
      }
      String name = Path.GetFullPath( Path.GetDirectoryName( _filename) + "\\" + include);
      if (!File.Exists( name)) {
        _analyzer.AddGeneralError( name + " (referenced by " + _filename  + ") does not exist");
      } else {
        AspFile includeFile = _analyzer.AddAspFile( name);
        _includes.Add( includeFile);
      }
      return true;
    }



    private void PseudoParseCode() {
      String functionName = null;
      String functionType = null;

      BlockReader reader = new BlockReader( _blocks);
      reader.ReadLine();
      while (reader.GetCurLine() != null ) {
        AspTool.AppendVariables( _variables, reader);
        AspTool.CheckSubContext( ref functionName, ref functionType, reader);
        if (functionType == null ) {
          _scopeText += reader.GetCurLine() + Environment.NewLine;
          reader.ReadLine();
        } else if ( functionType == "function" ||  functionType == "sub" ) {
          AspFunction curFunction = new AspFunction( functionName, functionType, this, reader.GetLineNumber() - 1);
          curFunction.PseudoParseCode( reader);
          _functions.Add( curFunction);
        } else if ( functionType == "class" ) {
          AspClass curClass = new AspClass( functionName, this);
          curClass.PseudoParseCode( reader);
          _classes.Add( curClass);
        }
      }
    }


    public void Read() {
      if ( _isRead) {
        return;
      }
      _isRead = true;
      if ( !File.Exists( _filename)) {
        _analyzer.AddGeneralError( "File " + _filename + " does not exist");
        return;
      }
      LineReader lineReader = new LineReader( _filename);
      lineReader.ReadLine();
      int posComment;
      int posCode;

      while (lineReader.CurLine != null) {
        if (lineReader.Eol) {
          posComment = -1;
          posCode = -1;
        }
        else {
          posComment = lineReader.CurLine.IndexOf( "<!--", lineReader.Col);
          posCode = lineReader.CurLine.IndexOf( "<%", lineReader.Col);
        }
 
        if (posComment == -1 && posCode == -1 ) {
          lineReader.ReadLine();
        } else {
          if ( posComment == -1 || ( ( posCode < posComment) &&  posCode != -1 ) ) {
            lineReader.Col = posCode + 2;
            CodeBlock cb = new CodeBlock( lineReader);
            cb.Read();
            _blocks.Add( cb);
          } else {
            lineReader.Col = posComment +  4;
            CommentBlock commBl = new CommentBlock( lineReader);
            commBl.Read();
            IsIncludeLine( commBl.Content, 0);
          }
        }
      }

      lineReader.Close();
      PseudoParseCode();
    }

    public AspFile( String pFilename, Analyzer pAnalyzer) {
      _filename = pFilename;
      _analyzer = pAnalyzer;
    }

    public bool FindFunction( String pFunction) {
      if (AspTool.ContainsIdentifier( _scopeText, pFunction)) {
        return true;
      }
      for (int i = 0; i < _classes.Count; i++) {
        AspClass curClass = (AspClass) _classes[ i];
        if (curClass.FindFunction( pFunction) ) {
          return true;
        }        
      }
      for (int i = 0; i < _functions.Count; i++) {
        AspFunction curFunction = (AspFunction) _functions[ i];
        if (curFunction.FindFunction( pFunction) ) {
          return true;
        }        
      }
      for (int i = 0; i < _includes.Count; i++) {
        AspFile curInclude = (AspFile) _includes[ i];
        if (curInclude.FindFunction( pFunction)) {
          return true;
        }
      }
      return false;
    }


    public bool FindVariable( String pVariable) {
      if (AspTool.ContainsIdentifier( _scopeText, pVariable)) {
        return true;
      }
      for (int i = 0; i < _classes.Count; i++) {
        AspClass curClass = (AspClass) _classes[ i];
        if (curClass.FindVariable( pVariable) ) {
          return true;
        }        
      }
      for (int i = 0; i < _functions.Count; i++) {
        AspFunction curFunction = (AspFunction) _functions[ i];
        if (curFunction.FindVariable( pVariable) ) {
          return true;
        }        
      }
      for (int i = 0; i < _includes.Count; i++) {
        AspFile curInclude = (AspFile) _includes[ i];
        if (curInclude.FindVariable( pVariable)) {
          return true;
        }
      }
      return false;
    }



    public void AddVariables( ArrayList pTransitiveVariableList) {
      pTransitiveVariableList.AddRange( _variables);
      for (int i = 0; i < _includes.Count; i++) {
        AspFile curInclude = (AspFile) _includes[ i];
        curInclude.AddVariables( pTransitiveVariableList);
      }

    }


    public void CheckUsedVariables() {
      ArrayList transitiveVariableList = new ArrayList();
      AddVariables( transitiveVariableList);
      for (int i = 0; i < transitiveVariableList.Count; i++) {
        Variable curVariable = (Variable) transitiveVariableList[ i];
        if (!curVariable.Used) {
          if ( FindVariable( curVariable.Name)) {
            curVariable.Used = true;
          }
        }
      }
    }


    public void ReportUnusedVariables() {
      for (int i = 0; i < _variables.Count; i++) {
        Variable curVariable = (Variable) _variables[ i];
        if (!curVariable.Used) {
          PublishResult( new Result( _filename, curVariable.Row, "Unused Global Variable " + curVariable.Name));
        }
      }
    }




    public void AddFunctions( ArrayList pTransitiveFunctionList) {
      pTransitiveFunctionList.AddRange( _functions);
      for (int i = 0; i < _includes.Count; i++) {
        AspFile curInclude = (AspFile) _includes[ i];
        curInclude.AddFunctions( pTransitiveFunctionList);
      }

    }


    public void CheckUsedFunctions() {
      ArrayList transitiveFunctionList = new ArrayList();
      AddFunctions( transitiveFunctionList);
      for (int i = 0; i < transitiveFunctionList.Count; i++) {
        AspFunction curFunction = (AspFunction) transitiveFunctionList[ i];
        if (!curFunction.Used) {
          if ( FindFunction( curFunction.Name)) {
            curFunction.Used = true;
          }
        }
      }
    }


    public void ReportUnusedFunctions() {
      for (int i = 0; i < _functions.Count; i++) {
        AspFunction curFunction = (AspFunction) _functions[ i];
        if (!curFunction.Used) {
          PublishResult( new Result( _filename, curFunction.Row, "Unused Function " + curFunction.Name));
        }
      }
    }


    public void PublishResult( Result pResult) {
      _analyzer.PublishResult( pResult);
    }


  }
}

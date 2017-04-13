using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace AspCodeAnalyzer {
  public class AspFile  {
    private Analyzer _analyzer;
    private List<CodeBlock> _blocks = new List<CodeBlock>();
    private List<Variable> _variables = new List<Variable>();
    private List<AspClass> _classes = new List<AspClass>();
    private List<AspFunction> _functions = new List<AspFunction>();
    private string _scopeText = "";


    public bool IsRead { get; private set; }
    public List<AspFile> Includes { get; set; } = new List<AspFile>();
    public string Filename { get; set; }

    private static string GetSandwichedValue( string pLine, string pPrefix, string pPostfix, int pStartPos) {
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


    private bool IsIncludeLine( string pLine, int pPos) {
      int pos1 = pLine.IndexOf( "#include", pPos);
      if (pos1 == -1) {
        return false;
      }
      pos1 = pLine.IndexOf( "file", pos1);
      if (pos1 == -1) {
        return false;
      }
      var include = GetSandwichedValue( pLine, "\"", "\"", pos1);
      if (include == null) {
        return false;
      }
      var name = Path.GetFullPath( Path.GetDirectoryName( Filename) + "\\" + include);
      if (!File.Exists( name)) {
        _analyzer.AddGeneralError( name + " (referenced by " + Filename  + ") does not exist");
      } else {
        var includeFile = _analyzer.AddAspFile( name);
        Includes.Add( includeFile);
      }
      return true;
    }



    private void PseudoParseCode() {
      string functionName = null;
      string functionType = null;

      var reader = new BlockReader( _blocks);
      reader.ReadLine();
      while (reader.GetCurLine() != null ) {
        AspTool.AppendVariables( _variables, reader);
        AspTool.CheckSubContext( ref functionName, ref functionType, reader);
        switch (functionType)
        {
            case null:
                _scopeText += reader.GetCurLine() + Environment.NewLine;
                reader.ReadLine();
                break;
            case "function":
            case "sub":
                var curFunction = new AspFunction( functionName, functionType, this, reader.GetLineNumber() - 1);
                curFunction.PseudoParseCode( reader);
                _functions.Add( curFunction);
                break;
            case "class":
                var curClass = new AspClass( functionName, this);
                curClass.PseudoParseCode( reader);
                _classes.Add( curClass);
                break;
        }
      }
    }


    public void Read() {
      if ( IsRead) {
        return;
      }
      IsRead = true;
      if ( !File.Exists( Filename)) {
        _analyzer.AddGeneralError( "File " + Filename + " does not exist");
        return;
      }
      var lineReader = new LineReader( Filename);
      lineReader.ReadLine();

        while (lineReader.CurLine != null) {
          int posComment;
          int posCode;
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
            var cb = new CodeBlock( lineReader);
            cb.Read();
            _blocks.Add( cb);
          } else {
            lineReader.Col = posComment +  4;
            var commBl = new CommentBlock( lineReader);
            commBl.Read();
            IsIncludeLine( commBl.Content, 0);
          }
        }
      }

      lineReader.Close();
      PseudoParseCode();
    }

    public AspFile( string pFilename, Analyzer pAnalyzer) {
      Filename = pFilename;
      _analyzer = pAnalyzer;
    }

    public bool FindFunction( string pFunction) {
      if (AspTool.ContainsIdentifier( _scopeText, pFunction)) {
        return true;
      }
      if (_classes.Any(curClass => curClass.FindFunction( pFunction)))
      {
          return true;
      }
      if (_functions.Any(curFunction => curFunction.FindFunction( pFunction)))
      {
          return true;
      }
      return Includes.Any(curInclude => curInclude.FindFunction(pFunction));
    }


    public bool FindVariable( string pVariable) {
      if (AspTool.ContainsIdentifier( _scopeText, pVariable)) {
        return true;
      }
      if (_classes.Any(curClass => curClass.FindVariable( pVariable)))
      {
          return true;
      }
      if (_functions.Any(curFunction => curFunction.FindVariable( pVariable)))
      {
          return true;
      }
      return Includes.Any(curInclude => curInclude.FindVariable(pVariable));
    }



    public void AddVariables( List<Variable> pTransitiveVariableList)
    {
        pTransitiveVariableList.AddRange( _variables);
        foreach (var curInclude in Includes)
        {
            curInclude.AddVariables( pTransitiveVariableList);
        }
    }


    public void CheckUsedVariables() {
      var transitiveVariableList = new List<Variable>();
      AddVariables( transitiveVariableList);
      foreach (var curVariable in transitiveVariableList)
      {
          if (!curVariable.Used) {
              if ( FindVariable( curVariable.Name)) {
                  curVariable.Used = true;
              }
          }
      }
    }


    public void ReportUnusedVariables()
    {
        foreach (var curVariable in _variables.Where(s => !s.Used))
        {
            PublishResult( new Result( Filename, curVariable.Row, "Unused Global Variable " + curVariable.Name));
        }
    }




    public void AddFunctions( List<AspFunction> pTransitiveFunctionList)
    {
        pTransitiveFunctionList.AddRange( _functions);
        foreach (var curInclude in Includes)
        {
            curInclude.AddFunctions( pTransitiveFunctionList);
        }
    }


    public void CheckUsedFunctions() {
      var transitiveFunctionList = new List<AspFunction>();
      AddFunctions( transitiveFunctionList);

      foreach (var curFunction in transitiveFunctionList)
      {
          if (!curFunction.Used) {
              if ( FindFunction( curFunction.Name)) {
                  curFunction.Used = true;
              }
          }
      }
    }


    public void ReportUnusedFunctions()
    {
        foreach (var curFunction in _functions.Where(s => !s.Used))
        {
            PublishResult( new Result( Filename, curFunction.Row, "Unused Function " + curFunction.Name));
        }
    }


    public void PublishResult( Result pResult) {
      _analyzer.PublishResult( pResult);
    }


  }
}

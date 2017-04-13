using System;
using System.Collections.Generic;

namespace AspCodeAnalyzer {

  public class AspClass {
    private string _name;
    private AspFile _aspFile;
    private List<AspFunction> _functions = new List<AspFunction>();
    private string _scopeText = "";

    public AspClass( string pName, AspFile pAspFile) {
      _name = pName;
      _aspFile = pAspFile;
    }


    public void PseudoParseCode( BlockReader pReader) {
      string functionName = null;
      string functionType = null;

      var variables = new List<Variable>();

      while ( pReader.GetCurLine() != null ) {
        int endPos = pReader.GetCurLine().IndexOf( "end ");
        if ( endPos >= 0 ) {
          if  ( pReader.GetCurLine().IndexOf( "class", endPos) >= 0 ) {
            pReader.ReadLine();
            return;
          }
        }
        AspTool.AppendVariables( variables, pReader);
        AspTool.CheckSubContext( ref functionName, ref functionType, pReader);
        if (functionType == null ) {
          _scopeText += pReader.GetCurLine() + Environment.NewLine;
          pReader.ReadLine();
        } else if ( functionType == "function" ||  functionType == "sub" ) {
          var curFunction = new AspFunction( functionName, functionType, _aspFile, pReader.GetLineNumber() - 1);
          curFunction.PseudoParseCode( pReader);
          _functions.Add( curFunction);
        } else {
          throw new Exception( "Class in Class??: " + _aspFile.Filename + " line: " + (pReader.GetLineNumber()-1).ToString());
        }
      }
    }


    public bool FindFunction( string pFunction) {
      if (AspTool.ContainsIdentifier( _scopeText, pFunction)) {
        return true;
      }
      for (int i = 0; i < _functions.Count; i++) {
        var curFunction = _functions[ i];
        if (curFunction.FindFunction( pFunction) ) {
          return true;
        }        
      }
      return false;
    }


    public bool FindVariable( string pVariable) {
      if (AspTool.ContainsIdentifier( _scopeText, pVariable)) {
        return true;
      }
      for (int i = 0; i < _functions.Count; i++) {
        var curFunction = _functions[ i];
        if (curFunction.FindVariable( pVariable) ) {
          return true;
        }        
      }
      return false;
    }


  }
}

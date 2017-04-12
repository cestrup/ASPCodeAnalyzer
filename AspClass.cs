using System;
using System.Collections;
using System.Diagnostics;

namespace AspCodeAnalyzer {

  public class AspClass {
    private String _name;
    private AspFile _aspFile;
    private ArrayList _functions = new ArrayList();
    private String _scopeText = "";

    public AspClass( String pName, AspFile pAspFile) {
      _name = pName;
      _aspFile = pAspFile;
    }


    public void PseudoParseCode( BlockReader pReader) {
      String functionName = null;
      String functionType = null;

      ArrayList variables = new ArrayList();

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
          AspFunction curFunction = new AspFunction( functionName, functionType, _aspFile, pReader.GetLineNumber() - 1);
          curFunction.PseudoParseCode( pReader);
          _functions.Add( curFunction);
        } else {
          throw new Exception( "Class in Class??: " + _aspFile.Filename + " line: " + (pReader.GetLineNumber()-1).ToString());
        }
      }
    }


    public bool FindFunction( String pFunction) {
      if (AspTool.ContainsIdentifier( _scopeText, pFunction)) {
        return true;
      }
      for (int i = 0; i < _functions.Count; i++) {
        AspFunction curFunction = (AspFunction) _functions[ i];
        if (curFunction.FindFunction( pFunction) ) {
          return true;
        }        
      }
      return false;
    }


    public bool FindVariable( String pVariable) {
      if (AspTool.ContainsIdentifier( _scopeText, pVariable)) {
        return true;
      }
      for (int i = 0; i < _functions.Count; i++) {
        AspFunction curFunction = (AspFunction) _functions[ i];
        if (curFunction.FindVariable( pVariable) ) {
          return true;
        }        
      }
      return false;
    }


  }
}

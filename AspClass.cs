using System;
using System.Collections.Generic;
using System.Linq;

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
        switch (functionType)
        {
            case null:
                _scopeText += pReader.GetCurLine() + Environment.NewLine;
                pReader.ReadLine();
                break;
            case "function":
            case "sub":
                var curFunction = new AspFunction( functionName, functionType, _aspFile, pReader.GetLineNumber() - 1);
                curFunction.PseudoParseCode( pReader);
                _functions.Add( curFunction);
                break;
            default:
                throw new Exception( "Class in Class??: " + _aspFile.Filename + " line: " + (pReader.GetLineNumber()-1).ToString());
        }
      }
    }


    public bool FindFunction( string pFunction) {
      if (AspTool.ContainsIdentifier( _scopeText, pFunction)) {
        return true;
      }
      return _functions.Any(curFunction => curFunction.FindFunction(pFunction));
    }


    public bool FindVariable( string pVariable) {
      if (AspTool.ContainsIdentifier( _scopeText, pVariable)) {
        return true;
      }
      return _functions.Any(curFunction => curFunction.FindVariable(pVariable));
    }


  }
}

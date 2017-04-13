using System;
using System.Collections.Generic;

namespace AspCodeAnalyzer {
  public class AspFunction {
    private string _functionType;
    private AspFile _aspFile;
    private string _scopeText = "";

      public AspFunction(string pName, string pFunctionType, AspFile pAspFile, int pRow) {
      Name = pName;
      _functionType = pFunctionType;
      _aspFile = pAspFile;
      Row = pRow;
    }


    public bool Used { get; set; } = false;
    public string Name { get; set; }
    public int Row { get; set; } = 1;

    private static void CheckVariables( string pLine, List<Variable> pVariables) {
      int i = 0; 
      while (i < pVariables.Count) {
        if (AspTool.ContainsIdentifier( pLine, pVariables[ i].Name)) {
          pVariables.RemoveAt( i);
        } else {
          i++;
        }
      }      
    }


    public void PseudoParseCode( BlockReader pReader) {
      var variables = new List<Variable>();

      while ( pReader.GetCurLine() != null ) {
        AspTool.AppendVariables( variables, pReader);
        CheckVariables( pReader.GetCurLine(), variables);
        int endPos = pReader.GetCurLine().IndexOf( "end ");
        if ( endPos >= 0 ) {          
          if  ( pReader.GetCurLine().IndexOf( _functionType, endPos) >= 0 ) {
            foreach (var curVariable in variables)
            {
                _aspFile.PublishResult( new Result( _aspFile.Filename, curVariable.Row, "Unused Variable " + curVariable.Name));
            }
            pReader.ReadLine();
            return;
          }
        }
        _scopeText += pReader.GetCurLine() + Environment.NewLine;        
        pReader.ReadLine();
      }
    }


    public bool FindFunction( string pFunction) {
      if ( pFunction == Name) {
        // Nicht nach Funktionsaufrufen bzw. Zuweisungen in der gesuchten Funktion suchen
        return false;
      }
      if (AspTool.ContainsIdentifier( _scopeText, pFunction)) {
        return true;
      }
      return false;
    }

    public bool FindVariable( string pVariable) {
      if (AspTool.ContainsIdentifier( _scopeText, pVariable)) {
        return true;
      }
      return false;
    }


  }
}

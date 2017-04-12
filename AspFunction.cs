using System;
using System.Collections;
using System.Diagnostics;

namespace AspCodeAnalyzer {
  public class AspFunction {
    private String _name;
    private String _functionType;
    private AspFile _aspFile;
    private String _scopeText = "";
    private int _row = 1;
    private bool _used = false;

    public AspFunction(String pName, String pFunctionType, AspFile pAspFile, int pRow) {
      _name = pName;
      _functionType = pFunctionType;
      _aspFile = pAspFile;
      _row = pRow;
    }


    public bool Used {
      get {
        return _used;
      }
      set {
        _used = value;
      }
    }



    public String Name {
      get {
        return _name;
      }
      set {
        _name = value;
      }
    }


    public int Row {
      get {
        return _row;
      }
      set {
        _row = value;
      }
    }



    private static void CheckVariables( String pLine, ArrayList pVariables) {
      int i = 0; 
      while (i < pVariables.Count) {
        if (AspTool.ContainsIdentifier( pLine, ((Variable) pVariables[ i]).Name)) {
          pVariables.RemoveAt( i);
        } else {
          i++;
        }
      }      
    }


    public void PseudoParseCode( BlockReader pReader) {

      ArrayList variables = new ArrayList();

      while ( pReader.GetCurLine() != null ) {
        AspTool.AppendVariables( variables, pReader);
        CheckVariables( pReader.GetCurLine(), variables);
        int endPos = pReader.GetCurLine().IndexOf( "end ");
        if ( endPos >= 0 ) {          
          if  ( pReader.GetCurLine().IndexOf( _functionType, endPos) >= 0 ) {
            for (int i = 0; i < variables.Count; i++) {
              Variable curVariable = (Variable) variables[ i];
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


    public bool FindFunction( String pFunction) {
      if ( pFunction == _name) {
        // Nicht nach Funktionsaufrufen bzw. Zuweisungen in der gesuchten Funktion suchen
        return false;
      }
      if (AspTool.ContainsIdentifier( _scopeText, pFunction)) {
        return true;
      }
      return false;
    }

    public bool FindVariable( String pVariable) {
      if (AspTool.ContainsIdentifier( _scopeText, pVariable)) {
        return true;
      }
      return false;
    }


  }
}

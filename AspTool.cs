using System.Collections.Generic;

namespace AspCodeAnalyzer {
  public class AspTool {

    public static bool AppendVariables( List<Variable> pVariables, BlockReader pReader) {
      var found = false;
      string searchText;
      var line = pReader.GetCurLine();
      searchText = "dim";
      var dimIndex = IdentifierPos( line, searchText);
      if (dimIndex == -1) {
        searchText = "const";
        dimIndex = IdentifierPos( line, searchText);
      }
      if (dimIndex == -1 ) {
        return false;
      }
      var objDefPos = line.IndexOf( ":");
      if (objDefPos != -1 ) 
      {
        pReader.SetCurLine( line.Substring( objDefPos));
      } 
      else 
      {
        pReader.SetCurLine( "");
      }
      var vars = line.Substring( dimIndex + searchText.Length).Trim().Split( new char[] {','});
      for (int i = 0; i < vars.Length; i++) {
        var curVar = vars[ i].Trim();
        if (curVar.Length != 0) {
          int j = 0; 
          while ( j < curVar.Length && IsVariableCharacter( curVar[ j]) ) {
            j++;
          }

		  if (j > 0)
			{
				curVar = curVar.Substring(0, j);
				var var = new Variable(curVar, pReader.GetLineNumber());
				pVariables.Add(var);
				found = true;
			}
		}          
      }
      return found;
    }


    public static int IdentifierPos( string pText, string pIdentifier) {
      int pos = pText.IndexOf( pIdentifier);
      while (pos > 0) {
        if (pos == 0 || !AspTool.IsVariableCharacter( pText[ pos - 1 ])) {
          if (pos + pIdentifier.Length >= pText.Length || !AspTool.IsVariableCharacter( pText[ pos + pIdentifier.Length])) {
            return pos;
          }
        }
        pos = pText.IndexOf( pIdentifier, pos + 1);        
      }
      return -1;
    }


    public static bool ContainsIdentifier( string pText, string pIdentifier) {
      int pos = pText.IndexOf( pIdentifier);
      while (pos > 0) {
        if (pos == 0 || ((pText[pos-1] != '.') && !AspTool.IsVariableCharacter( pText[ pos - 1 ]))) {
          if (pos + pIdentifier.Length >= pText.Length || !AspTool.IsVariableCharacter( pText[ pos + pIdentifier.Length])) {
            return true;
          }
        }
        pos = pText.IndexOf( pIdentifier, pos + 1);        
      }
      return false;
    }

    public static void CheckSubContext( ref string pFunctionName, ref string pFunctionType, BlockReader pReader) {
      pFunctionName = null;
      pFunctionType = null;
      int pos;
      pos = pReader.GetCurLine().IndexOf( "sub ");
      if ( pos >= 0 ) {
        if (pos == 0 || !AspTool.IsVariableCharacter( pReader.GetCurLine()[ pos - 1 ])) {
          pFunctionType = "sub";
          int pos1 = pReader.GetCurLine().IndexOf( "(", pos);
          if ( pos1 == -1 ) {
            pFunctionName = pReader.GetCurLine().Substring( pos + 4).Trim();
            pReader.ReadLine();
            return;
          }
          else {
            pFunctionName = pReader.GetCurLine().Substring( pos + 4, pos1 - pos - 4).Trim();
            pReader.ReadLine();
            return;
          }
        }
      }
      pos = pReader.GetCurLine().IndexOf( "function ");
      if ( pos >= 0 ) {
        if (pos == 0 || !AspTool.IsVariableCharacter( pReader.GetCurLine()[ pos - 1 ])) {
          pFunctionType = "function";
          int pos1 = pReader.GetCurLine().IndexOf( "(", pos);
          if ( pos1 == -1 ) {
            pFunctionName = pReader.GetCurLine().Substring( pos + 9).Trim();
            pReader.ReadLine();
            return;
          }
          else {
            pFunctionName = pReader.GetCurLine().Substring( pos + 9, pos1 - pos - 9).Trim();
            pReader.ReadLine();
            return;
          }
        }
      }
      pos = pReader.GetCurLine().IndexOf( "class ");
      if ( pos >= 0 ) {
        if (pos == 0 || !AspTool.IsVariableCharacter( pReader.GetCurLine()[ pos - 1 ])) {
          pFunctionType = "class";
          int pos1 = pReader.GetCurLine().IndexOf( "(", pos);
          if ( pos1 == -1 ) {
            pFunctionName = pReader.GetCurLine().Substring( pos + 6).Trim();
            pReader.ReadLine();
            return;
          }
          else {
            pFunctionName = pReader.GetCurLine().Substring( pos + 6, pos1 - pos - 6).Trim();
            pReader.ReadLine();
            return;
          }
        }
      }
    }

    public static bool IsVariableCharacter( char pChar) {
      pChar = char.ToLower( pChar);
      if (pChar >= 'a' && pChar <= 'z') {
        return true;
      }
      if (pChar >= '0' && pChar <= '9' ) {
        return true;
      }
      if (pChar == '_' ) {
        return true;
      }
      return false;
    }



  }
}

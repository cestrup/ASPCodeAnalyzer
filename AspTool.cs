using System;
using System.Collections;

namespace AspCodeAnalyzer {
  public class AspTool {

    public static bool AppendVariables( ArrayList pVariables, BlockReader pReader) {
      bool found = false;
      String searchText;
      String line = pReader.GetCurLine();
      searchText = "dim";
      int dimIndex = IdentifierPos( line, searchText);
      if (dimIndex == -1) {
        searchText = "const";
        dimIndex = IdentifierPos( line, searchText);
      }
      if (dimIndex == -1 ) {
        return false;
      }
      int objDefPos = line.IndexOf( ":");
      if (objDefPos != -1 ) 
      {
        pReader.SetCurLine( line.Substring( objDefPos));
      } 
      else 
      {
        pReader.SetCurLine( "");
      }
      String[] vars = line.Substring( dimIndex + searchText.Length).Trim().Split( new char[] {','});
      for (int i = 0; i < vars.Length; i++) {
        String curVar = vars[ i].Trim();
        if (curVar.Length != 0) {
          int j = 0; 
          while ( j < curVar.Length && IsVariableCharacter( curVar[ j]) ) {
            j++;
          }
          curVar = curVar.Substring( 0, j);
          Variable var = new Variable( curVar, pReader.GetLineNumber());
          pVariables.Add( var);     
          found = true;
        }          
      }
      return found;
    }


    public static int IdentifierPos( String pText, String pIdentifier) {
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


    public static bool ContainsIdentifier( String pText, String pIdentifier) {
      int pos = pText.IndexOf( pIdentifier);
      while (pos > 0) {
        if (pos == 0 || !AspTool.IsVariableCharacter( pText[ pos - 1 ])) {
          if (pos + pIdentifier.Length >= pText.Length || !AspTool.IsVariableCharacter( pText[ pos + pIdentifier.Length])) {
            return true;
          }
        }
        pos = pText.IndexOf( pIdentifier, pos + 1);        
      }
      return false;
    }

    public static void CheckSubContext( ref String pFunctionName, ref String pFunctionType, BlockReader pReader) {
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
      pChar = Char.ToLower( pChar);
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

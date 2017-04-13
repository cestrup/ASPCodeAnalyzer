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
      foreach (string t in vars)
      {
          var curVar = t.Trim();
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

      while (pos >= 0) {
        if (pos == 0 || !IsVariableCharacter( pText[ pos - 1 ])) {
          if (pos + pIdentifier.Length >= pText.Length || !IsVariableCharacter( pText[ pos + pIdentifier.Length])) {
            return pos;
          }
        }
        pos = pText.IndexOf( pIdentifier, pos + 1);
      }
      return -1;
    }


    public static bool ContainsIdentifier( string pText, string pIdentifier) {
      int pos = pText.IndexOf( pIdentifier);

      while (pos >= 0) {
        if (pos == 0 || ((pText[pos-1] != '.') && !IsVariableCharacter( pText[ pos - 1 ]))) {
          if (pos + pIdentifier.Length >= pText.Length || !IsVariableCharacter( pText[ pos + pIdentifier.Length])) {
            return true;
          }
        }
        pos = pText.IndexOf( pIdentifier, pos + 1);        
      }
      return false;
    }

      private static bool CheckSingleSubContext(string searchType, ref string pFunctionName, ref string pFunctionType, BlockReader pReader)
      {
          int pos = pReader.GetCurLine().IndexOf(searchType + " ");

          if (pos >= 0 ) {
              if (pos == 0 || !IsVariableCharacter( pReader.GetCurLine()[ pos - 1 ])) {
                  pFunctionType = searchType;
                  int pos1 = pReader.GetCurLine().IndexOf( "(", pos);
                  if (pos1 == -1 ) {
                      pFunctionName = pReader.GetCurLine().Substring( pos + searchType.Length + 1).Trim();
                      pReader.ReadLine();
                      return true;
                  }
                  else {
                      pFunctionName = pReader.GetCurLine().Substring( pos + searchType.Length + 1, pos1 - pos - searchType.Length - 1).Trim();
                      pReader.ReadLine();
                      return true;
                  }
              }
          }

          return false;
      }

      public static void CheckSubContext( ref string pFunctionName, ref string pFunctionType, BlockReader pReader)
      {
          pFunctionName = null;
          pFunctionType = null;

          if (CheckSingleSubContext("sub", ref pFunctionName, ref pFunctionType, pReader))
          {
              return;
          }
          if (CheckSingleSubContext("function", ref pFunctionName, ref pFunctionType, pReader))
          {
              return;
          }
          if (CheckSingleSubContext("class", ref pFunctionName, ref pFunctionType, pReader))
          {
              return;
          }
      }

    public static bool IsVariableCharacter (char pChar)
    {
      return (char.IsLetterOrDigit(pChar) || pChar == '_');
    }
  }
}

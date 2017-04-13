using System;
using System.Collections.Generic;

namespace AspCodeAnalyzer 
{
  public class CodeBlock 
  {
    private LineReader _lineReader;

    public int Col { get; set; }
    public int Row { get; set; }
    public List<string> Text { get; } = new List<string>();

    public CodeBlock( LineReader pLineReader)
    {
      _lineReader = pLineReader;
      Col = _lineReader.Col;
      Row = _lineReader.Row;
      Text.Add( "");
    }


    private void ParseLiteral() 
    {
      while (!_lineReader.Eof) 
      { 
        if (_lineReader.Peek1() == '\"' && _lineReader.Peek2() == '\"') 
        {
          _lineReader.Get1();
          _lineReader.Get1();
        } 
        else if ( _lineReader.Peek1() == '\"') 
        {
          _lineReader.Get1();
          AddChar( ' ');
          return;
        } 
        else 
        {
          _lineReader.Get1();
        }
        if (_lineReader.Eol) 
        {
          throw new Exception( _lineReader.Filename + " Zeile " + _lineReader.Row + ":Text nicht auf der aktuellen Zeile abgeschlossen:");
        }
      }
      AddChar( ' ');
    }


    private void AddChar( char pChar) 
    {
      if (pChar == (char) 0) 
      {
        pChar = ' ';
      }
      Text[ Text.Count - 1] = Text[ Text.Count - 1] + pChar;
    }

    public void Read() 
    {
      while (!_lineReader.Eof) 
      { 
        if (_lineReader.Peek1() == '%' && _lineReader.Peek2() == '>' ) 
        {
          _lineReader.Get1();
          _lineReader.Get1();
          return;
        }
        if (_lineReader.Peek1() == '\"') 
        {
          _lineReader.Get1();
          ParseLiteral();
        } 
        else if ( ( _lineReader.Peek1() == '\'' ) 
          || ( _lineReader.Peek1() == 'r' && _lineReader.Peek2() == 'e' && _lineReader.Peek3() == 'm' && _lineReader.Peek4() == ' ' ) 
          || ( _lineReader.Peek1() == '/' && _lineReader.Peek2() == '/' ) ) 
        {
          int pos =_lineReader.CurLine.IndexOf( "%>", _lineReader.Col);
          if ( pos == -1 ) 
          {
            Text.Add( "");
            _lineReader.ReadLine();
          } 
          else 
          {
            _lineReader.Col = pos + 2;
            return;
          }
        } 
        else 
        {
          AddChar( _lineReader.Get1());
        }
        if (_lineReader.Eol) 
        {
          Text.Add( "");
          _lineReader.ReadLine();
        }        
      }
    }

    public void AddText( string pText)
    {
      if (pText.Trim().IndexOf( "\'") == 0 ) 
      {
        Text.Add( "");
      } 
      else if (pText.Trim().IndexOf( "//") == 0 )  
      {
        Text.Add( "");
      }
      else 
      {
        Text.Add( pText);
      }
    }
  }
}

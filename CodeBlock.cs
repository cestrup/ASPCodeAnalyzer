using System;
using System.Collections;
using System.Diagnostics;

namespace AspCodeAnalyzer 
{
  public class CodeBlock 
  {
    private int _col;
    private int _row;
    private ArrayList _text = new ArrayList();
    private LineReader _lineReader;

    public int Col 
    {
      get 
      {
        return _col;
      }
      set 
      {
        _col = value;
      }
    }

    public int Row 
    {
      get 
      {
        return _row;
      }
      set 
      {
        _row = value;
      }
    }

    public ArrayList Text 
    {
      get 
      {
        return _text;
      }
    }

    public CodeBlock( LineReader pLineReader) 
    {
      _lineReader = pLineReader;
      _col = _lineReader.Col;
      _row = _lineReader.Row;
      _text.Add( "");
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
      _text[ _text.Count - 1] = ((String) _text[ _text.Count - 1]) + pChar;
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
            _text.Add( "");
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
          _text.Add( "");
          _lineReader.ReadLine();
        }        
      }
    }

    public void AddText( String pText) 
    {
      if (pText.Trim().IndexOf( "\'") == 0 ) 
      {
        _text.Add( "");
      } 
      else if (pText.Trim().IndexOf( "//") == 0 )  
      {
        _text.Add( "");
      }
      else 
      {
        _text.Add( pText);
      }
    }
  }
}

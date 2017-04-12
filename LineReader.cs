using System;
using System.Text;
using System.IO;

namespace AspCodeAnalyzer {
  public class LineReader {
    StreamReader _reader;
    int _row;
    int _col;
    bool _eof;
    bool _eol;
    String _curLine;
    String _filename;

    public String CurLine {
      get {
        return _curLine;
      }
    }

    public String Filename {
      get {
        return _filename;
      }
    }


    public int Row {
      get {
        return _row;
      }
    }

    public int Col {
      get {
        return _col;
      }
      set {
        _col = value;
      }
    }

    public bool Eof {
      get {
        return _eof;
      }
    }

    public bool Eol {
      get {
        return _eol;
      }
    }

    public LineReader( String pFilename) {
      _filename = pFilename;
      _reader = new StreamReader( _filename, Encoding.Default);
      _eof = false;
      _eol = false;
      _row = 0;
      _curLine = null;
      _col = 0;
    }


    public String ReadLine() {
      _curLine = _reader.ReadLine();
      if ( _curLine == null ) {
        _eol = true;
        _eof = true;
        return null;
      }
      _curLine = _curLine.ToLower().Replace( "\t", "  ");;

      _row++;
      _col = 0;
      _eol = false;
      return _curLine;
    }

    public char Peek1() {
      if (_eol || _eof) {
        return (char)0;
      }
      if (_col >= _curLine.Length) {
        return (char)0;
      }
      return _curLine[ _col];
    }



    public char Peek2() {
      if (_eol || _eof) {
        return (char)0;
      }
      if (_col + 1 >= _curLine.Length) {
        return (char)0;
      }
      return _curLine[ _col + 1];
    }


    public char Peek3() {
      if (_eol || _eof) {
        return (char)0;
      }
      if (_col + 2 >= _curLine.Length) {
        return (char)0;
      }
      return _curLine[ _col + 2];
    }


    public char Peek4() {
      if (_eol || _eof) {
        return (char)0;
      }
      if (_col + 3 >= _curLine.Length) {
        return (char)0;
      }
      return _curLine[ _col + 3];
    }



    public char Get1() {
      if (_eol || _eof) {
        return (char)0;
      }
      if (_col >= _curLine.Length) {
        _eol = true;
        return (char)0;
      }
      char res = _curLine[ _col];
      _col++;
      if (_col >= _curLine.Length) {
        _eol = true;
      }


      return res;
    }


    public void Close() {
      if (_reader != null ) {
        _reader.Close();
      }
    }


  }
}

using System;
using System.Text;
using System.IO;

namespace AspCodeAnalyzer {
  public class LineReader {
    StreamReader _reader;
    public string CurLine { get; private set; }
    public string Filename { get; }
    public int Row { get; private set; }
    public int Col { get; set; }
    public bool Eof { get; private set; }
    public bool Eol { get; private set; }

    public LineReader( string pFilename) {
      Filename = pFilename;
      _reader = new StreamReader( Filename, Encoding.Default);
      Eof = false;
      Eol = false;
      Row = 0;
      CurLine = null;
      Col = 0;
    }


    public string ReadLine() {
      CurLine = _reader.ReadLine();
      if ( CurLine == null ) {
        Eol = true;
        Eof = true;
        return null;
      }
      CurLine = CurLine.ToLower().Replace( "\t", "  ");;

      Row++;
      Col = 0;
      Eol = false;
      return CurLine;
    }

    public char Peek1() {
      if (Eol || Eof) {
        return (char)0;
      }
      if (Col >= CurLine.Length) {
        return (char)0;
      }
      return CurLine[ Col];
    }



    public char Peek2() {
      if (Eol || Eof) {
        return (char)0;
      }
      if (Col + 1 >= CurLine.Length) {
        return (char)0;
      }
      return CurLine[ Col + 1];
    }


    public char Peek3() {
      if (Eol || Eof) {
        return (char)0;
      }
      if (Col + 2 >= CurLine.Length) {
        return (char)0;
      }
      return CurLine[ Col + 2];
    }


    public char Peek4() {
      if (Eol || Eof) {
        return (char)0;
      }
      if (Col + 3 >= CurLine.Length) {
        return (char)0;
      }
      return CurLine[ Col + 3];
    }



    public char Get1() {
      if (Eol || Eof) {
        return (char)0;
      }
      if (Col >= CurLine.Length) {
        Eol = true;
        return (char)0;
      }
      var res = CurLine[ Col];
      Col++;
      if (Col >= CurLine.Length) {
        Eol = true;
      }


      return res;
    }


    public void Close()
    {
        _reader?.Close();
    }
  }
}

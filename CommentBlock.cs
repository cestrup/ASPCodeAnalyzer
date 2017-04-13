using System;

namespace AspCodeAnalyzer {
  public class CommentBlock {
    private LineReader _lineReader;
    public string Content { get; private set; }

    public CommentBlock(LineReader pLineReader) {
      _lineReader = pLineReader;
      Content = "";
    }

    public void Read() {
      while (!_lineReader.Eof) { 
        if (_lineReader.Peek1() == '-' && _lineReader.Peek2() == '-' && _lineReader.Peek3() == '>' ) {
          _lineReader.Get1();
          _lineReader.Get1();
          _lineReader.Get1();
          return;
        } else {
          Content += _lineReader.Get1();
        }
        if (_lineReader.Eol) {
          Content += " ";
          _lineReader.ReadLine();
        }
      }
    }

  }
}

using System;

namespace AspCodeAnalyzer {
  public class CommentBlock {
    private String _content;
    private LineReader _lineReader;


    public String Content {
      get {
        return _content;
      }
    }
    

    public CommentBlock(LineReader pLineReader) {
      _lineReader = pLineReader;
      _content = "";
    }

    public void Read() {
      while (!_lineReader.Eof) { 
        if (_lineReader.Peek1() == '-' && _lineReader.Peek2() == '-' && _lineReader.Peek3() == '>' ) {
          _lineReader.Get1();
          _lineReader.Get1();
          _lineReader.Get1();
          return;
        } else {
          _content += _lineReader.Get1();
        }
        if (_lineReader.Eol) {
          _content += " ";
          _lineReader.ReadLine();
        }
      }
    }

  }
}

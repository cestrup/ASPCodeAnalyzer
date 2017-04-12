using System;

namespace AspCodeAnalyzer {
  public class Result {
    private String _filename;
    private int _row;
    private String _message;


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

    public String Message {
      get { 
        return _message;
      }
    }

    public override string ToString() {
      return _filename + ": Line " + _row.ToString() + ": " + _message;
    }


    public Result( String pFilename, int pRow, String pMessage) {
      _filename = pFilename;
      _row = pRow;
      _message = pMessage;
    }
  }
}

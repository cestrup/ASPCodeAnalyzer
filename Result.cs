using System;

namespace AspCodeAnalyzer {
  public class Result {
    public string Filename { get; }
    public int Row { get; }
    public string Message { get; }

    public override string ToString() {
      return Filename + ": Line " + Row.ToString() + ": " + Message;
    }

    public Result( string pFilename, int pRow, string pMessage) {
      Filename = pFilename;
      Row = pRow;
      Message = pMessage;
    }
  }
}

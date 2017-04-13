using System;

namespace AspCodeAnalyzer {
  public class Variable {
    public string Name { get; }
    public int Row { get; }
    public bool Used { get; set; }

    public Variable( string pName, int pRow) {
      Name = pName;
      Row = pRow;
    }
  }
}

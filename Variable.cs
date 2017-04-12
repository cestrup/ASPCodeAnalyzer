using System;

namespace AspCodeAnalyzer {
  public class Variable {
    private String _name;
    private int _row;
    private bool _used;


    public String Name {
      get { 
        return _name;
      }
    }

    public int Row {
      get { 
        return _row;
      }
    }


    public bool Used {
      get {
        return _used;
      }
      set {
        _used = value;
      }
    }



    public Variable( String pName, int pRow) {
      _name = pName;
      _row = pRow;

    }
  }
}

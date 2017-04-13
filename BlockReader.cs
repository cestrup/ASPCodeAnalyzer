using System;
using System.Collections.Generic;

namespace AspCodeAnalyzer {

  public class BlockReader {
    private List<CodeBlock> _blocks;
    private CodeBlock _curBlock;
    private int _curLineNumber;
    private bool _eof = false;
    private string _curLine;

    public BlockReader( List<CodeBlock> pBlocks) {
      _blocks = pBlocks;
    }

    public void ReadLine() {
      if ( _eof ) {
        _curLine = null;
        return;
      }
      if ( _curBlock == null ) {
        if ( _blocks == null || _blocks.Count == 0 ) {
          _eof = true;
          _curLine = null;
          return;
        }
        _curLineNumber = 0;
        _curBlock = _blocks[ 0];
        _blocks.RemoveAt( 0);
        _curLine = _curBlock.Text[ _curLineNumber];
        return;
      }
      _curLineNumber++;
      if ( _curLineNumber >= _curBlock.Text.Count ) {
        if ( _blocks == null || _blocks.Count == 0 ) {
          _eof = true;
          _curLine = null;
          return;
        }
        _curLineNumber = 0;
        _curBlock = _blocks[ 0];
        _blocks.RemoveAt( 0);
        _curLine = _curBlock.Text[ _curLineNumber];
        return;
      }
      _curLine = _curBlock.Text[ _curLineNumber];
    }



    public int GetLineNumber() {
      if ( _curBlock == null || _eof ) {
        throw new Exception( "Wrong state");
      }
      return _curBlock.Row + _curLineNumber;
    }

    public int GetCol() {
      if ( _curBlock == null || _eof ) {
        throw new Exception( "Wrong state");
      }
      if ( _curLineNumber == 0 ) {
        return _curBlock.Col;
      } else {
        return 0;
      }
    }

    public string GetCurLine() {
      if ( _eof ) {
        return null;
      }
      return _curLine;
    }

    public void SetCurLine( string pLine)
    {
      _curLine = pLine;
      _curBlock.Text[_curLineNumber] = pLine;
    }

  }
}

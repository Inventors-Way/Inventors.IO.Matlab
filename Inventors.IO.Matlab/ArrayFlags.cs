using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Inventors.IO.Matlab
{
  internal class ArrayFlags
  {
    public ArrayFlags()
    {
      complex = false;
      ctype = ClassType.mxDOUBLE;
    }

    public ArrayFlags(bool complex,
                      uint ctype)
    {
      this.complex = complex;
      this.ctype = ctype;
    }

    public void Serialize(BinaryWriter writer)
    {
      uint flag = (complex ? 0x00000800U : 0x0U) |
                  ctype;
      var elem = new Element<uint>(new uint[2]{flag , 0U});
      elem.Serialize(writer);
    }
    
    public bool Complex
    {
      get { return complex; }
    }

    private bool complex;
    private uint ctype;
  }
}

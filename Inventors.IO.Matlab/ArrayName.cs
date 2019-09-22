using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Inventors.IO.Matlab
{
  internal class ArrayName
  {
    public ArrayName(string name)
    {
      this.name = name;
    }

    public void Serialize(BinaryWriter writer)
    {
      var elem = new Element<byte>(DataType.miINT8, new UTF8Encoding(false).GetBytes(name));
      elem.Serialize(writer);
    }

    public string Name { get { return name; } }
    private string name;
  }
}

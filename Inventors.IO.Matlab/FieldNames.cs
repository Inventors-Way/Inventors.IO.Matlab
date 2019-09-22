using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Inventors.IO.Matlab
{
  class FieldNames
  {
    public FieldNames(IMatrix[] fields)
    {
      names = new List<string>();

      foreach (IMatrix m in fields)
      {
        if ( m.Name.Length < MAX_LENGTH)
        {
          names.Add(m.Name);
        }
        else
        {
          throw new InvalidFieldNameException("Field name too long");
        }
      }
    }

    public void Serialize(BinaryWriter writer)
    {
      var elem = new Element<byte>(DataType.miINT8, GetBytes());
      elem.Serialize(writer);
    }

    private byte[] GetBytes()
    {
      byte[] retValue = new byte[names.Count * MAX_LENGTH];
      int ptr = 0;

      foreach (string str in names)
      {
        byte[] strBytes = new UTF8Encoding(false).GetBytes(str.ToCharArray());
        
        foreach (byte b in strBytes)
        {
          retValue[ptr] = b;
          ++ptr;
        }

        for (int i = 0; i < MAX_LENGTH - (strBytes.Length % MAX_LENGTH); ++i)
        {
          retValue[ptr] = (byte)0;
          ++ptr;
        }
      }

      return retValue;
    }

    public static int MAX_LENGTH = 32;
    private List<string> names;
  }
}

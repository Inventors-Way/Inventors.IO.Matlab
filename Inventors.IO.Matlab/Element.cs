using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Inventors.IO.Matlab
{
  internal class Element<T>
  {
    public Element(T[] data)
    {
      this.data = data;
      type = DataType.GetType(typeof(T));
    }

    public Element(uint type, T[] data)
    {
      this.data = data;
      this.type = type;
    }

    public uint Type { get { return type; } }

    public void Serialize(BinaryWriter writer)
    {
      uint size = GetLength();
      MethodInfo method = writer.GetType().GetMethod("Write", new Type[] { typeof(T) });

      if ((size != 0) && (size <= 4))
      {
        writer.Write((ushort) type);
        writer.Write((ushort)size);
        
        foreach (T elem in data)
        {
          method.Invoke(writer, new Object[] { elem });
        }

        if (size % 4 != 0)
        {
          for (uint i = 0; i < 4 - size % 4; ++i)
          {
            writer.Write((byte)0);
          }
        }
      }
      else
      {
        writer.Write(type);
        writer.Write(size);

        foreach (T elem in data)
        {
          method.Invoke(writer, new Object[] { elem });
        }

        if (size % 8 != 0)
        {
          for (uint i = 0; i < 8 - size % 8; ++i)
          {
            writer.Write((byte)0);
          }
        }
      }    
    }

    private uint GetLength()
    {
      return DataType.GetSize(typeof(T)) * ((uint)data.Length);  
    }

    private uint type;
    private T[] data;
  }
}

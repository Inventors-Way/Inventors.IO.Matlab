using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Inventors.IO.Matlab
{
  internal class ArrayDimension
  {
    public ArrayDimension(int size)
    {
      dim = new int[] { size, 1 };
    }

    public ArrayDimension(int n, int m)
    {
      dim = new int[] { n, m };
    }

    public ArrayDimension(Array data)
    {
      dim = GetDimensions(data);
    }

    public void Serialize(BinaryWriter writer)
    {
      var elem = new Element<int>(dim);
      elem.Serialize(writer);
    }

    private int[] GetDimensions(Array a)
    {
      int[] retValue = null;

      if (a.Rank > 1)
      {
        retValue = new int[a.Rank];

        for (int i = 0; i < a.Rank; ++i)
        {
          retValue[i] = a.GetLength(i);
        }
      }
      else
      {
        retValue = new int[] { a.Length, 1 };
      }

      return retValue;
    }

    private int[] dim;
  }
}

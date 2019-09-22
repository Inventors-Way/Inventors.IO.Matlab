using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * Reshape array data from C# format to Matlab format.
   * </summary>
   */
  internal static class Reshaper<T>
  {
    public static T[] Transform(T[,] data)
    {
      T[] retValue = new T[data.Length];      
      int index = 0;

      for (int n = 0; n < data.GetLength(1); ++n)
      {
        for (int m = 0; m < data.GetLength(0); ++m)
        {
          retValue[index] = data[m, n];
          ++index;
        }
      }

      return retValue;
    }

    public static T[] Transform(T[,,] data)
    {
      T[] retValue = new T[data.Length];
      int index = 0;

      for (int k = 0; k < data.GetLength(2); ++k)
      {
        for (int n = 0; n < data.GetLength(1); ++n)
        {
          for (int m = 0; m < data.GetLength(0); ++m)
          {
            retValue[index] = data[m, n, k];
            ++index;
          }
        }
      }

      return retValue;
    }
  }
}

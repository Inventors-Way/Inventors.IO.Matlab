using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventors.IO.Matlab
{
  internal static class DataType
  {
    public const uint miINT8 = 1;
    public const uint miUINT8 = 2;
    public const uint miINT16 = 3;
    public const uint miUINT16 = 4;
    public const uint miINT32 = 5;
    public const uint miUINT32 = 6;
    public const uint miSINGLE = 7;
    public const uint miDOUBLE = 9;
    public const uint miINT64 = 12;
    public const uint miUINT64 = 13;
    public const uint miMATRIX = 14;
    public const uint miCOMPRESSED = 15;
    public const uint miUTF8 = 16;
    public const uint miUTF16 = 17;
    public const uint miUTF32 = 18;

    public static uint GetType(Type type)
    {
      uint retValue = 0;

      switch (type.Name)
      {
        case "SByte": retValue = miINT8; break;
        case "Byte": retValue = miUINT8; break;
        case "UInt16": retValue = miUINT16; break;
        case "Int16": retValue = miINT16; break;
        case "UInt32": retValue = miUINT32; break;
        case "Int32": retValue = miINT32; break;
        case "UInt64": retValue = miUINT64; break;
        case "Int64": retValue = miINT64; break;
        case "Single": retValue = miSINGLE; break;
        case "Double": retValue = miDOUBLE; break;
        case "Char": retValue = miUTF16; break;
        default:
          throw new NotImplementedException("No valid miTYPE for " + type.Name);
      }

      return retValue;
    }

    public static uint GetSize(Type type)
    {
      uint retValue = 0;

      switch (type.Name)
      {
        case "SByte": retValue = 1; break;
        case "Byte": retValue = 1; break;
        case "UInt16": retValue = 2; break;
        case "Int16": retValue = 2; break;
        case "UInt32": retValue = 4; break;
        case "Int32": retValue = 4; break;
        case "UInt64": retValue = 8; break;
        case "Int64": retValue = 8; break;
        case "Single": retValue = 4; break;
        case "Double": retValue = 8; break;
        case "Char": retValue = 2; break;
        default:
          throw new NotImplementedException("No valid miTYPE for " + type.Name);
      }

      return retValue;
    }
  }
}

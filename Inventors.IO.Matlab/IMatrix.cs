using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * Interface for all %Matlab file elements.
   * </summary>
   * <remarks>
   * All classes that are serializable to a %Matlab file must implement this interface.
   * </remarks>
   */
  public abstract class IMatrix
  {
    /**
     * <summary>
     * Serialize to a %Matlab file.
     * </summary>
     * <param name="writer">
     * the stream that is associated with the %Matlab file.
     * </param>
     */
    public void Serialize(BinaryWriter writer)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        using (BinaryWriter msWriter = new BinaryWriter(ms))
        {
          GetPayload(msWriter);

          uint mxPadding = ms.Length % 8 == 0 ? 0U : (uint)(8 - ms.Length % 8);
          uint mxLength = (uint)ms.Length + mxPadding;

          writer.Write(DataType.miMATRIX);
          writer.Write(mxLength);
          writer.Write(ms.ToArray());

          for (uint i = 0; i < mxPadding; ++i)
          {
            writer.Write((byte)0);
          }

          msWriter.Close();
        }
        ms.Close();
      }
    }

    protected abstract void GetPayload(BinaryWriter writer);

    /**
     * <summary>
     * The name of the matlab matrix element.
     * </summary>
     */
    public abstract string Name { get; set; }
  }
}

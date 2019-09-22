using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * A stream for writing IMatrix to a cell element.
   * </summary>
   * <remarks>
   * The MatrixStream class provides support for streaming IMatrix elements to a 
   * cell array in a %Matlab file. Internally it is handled by creating a dummy cell
   * array within the %Matlab file, which has zero size and zero elements. IMatrix 
   * elements can then be written to this cell array by calling the Write() function
   * on the MatrixStream. When all IMatrix elements has been streamed to the cell
   * array it then must be closed by calling the Close() function on the MatrixStream. 
   * When the Close() function is called the MatrixStream will seek back to the beginning
   * of the cell array and write the correct size and number of elements in the cell
   * array to the header of the element. This is needed in order for the file to be 
   * readable by Octave or %Matlab, if it is not called the file will become 
   * corrupted.
   * </remarks>
   */
  public class MatrixStream : IDisposable
  {
    internal MatrixStream(string filename, MatlabFile file, string name)
    {
      this.filename = filename;
      this.file = file;
      this.name = name;
      this.isOpen = false;
      this.startPosition = Open();
      this.count = 0;
    }

    /**
     * <summary>
     * Write a IMatrix element to the stream.
     * </summary>
     * <param name="matrix">
     * the IMatrix to write
     * </param>
     * /throw MatlabFileAccesException
     */
    public void Write(IMatrix matrix)
    {
      if (isOpen)
      {
        using (FileStream fs = new FileStream(filename, FileMode.Append))
        {
          using (BinaryWriter writer = new BinaryWriter(fs))
          {
            matrix.Serialize(writer);
            writer.Close();

            ++count;
          }
          fs.Close();
        }
      }
      else
      {
        throw new MatlabFileAccessException("The stream is not open for writing");
      }
    }

    private long Open()
    {
      long retValue = 0;

      using (FileStream fs = new FileStream(filename, FileMode.Append))
      {
        retValue = fs.Position;

        using (BinaryWriter writer = new BinaryWriter(fs))
        {
          writer.Write(DataType.miMATRIX);
          writer.Write((uint) 0);
          new ArrayFlags(false, ClassType.mxCELL).Serialize(writer);
          new ArrayDimension(0).Serialize(writer);
          new ArrayName(name).Serialize(writer);

          writer.Close();

          isOpen = true;
        }
        fs.Close();
      }

      return retValue;
    }

    public void Dispose()
    {
      Close();
    }

    /**
     * <summary>
     * Closes the MatrixStream.
     * </summary>
     */
    public void Close()
    {
      if (isOpen)
      {
        isOpen = false;

        using (FileStream fs = new FileStream(filename, FileMode.Open))
        {
          fs.Seek(0, SeekOrigin.End);
          long endPosition = fs.Position;
          fs.Seek(startPosition, SeekOrigin.Begin);

          using (BinaryWriter writer = new BinaryWriter(fs))
          {
            writer.Write(DataType.miMATRIX);
            writer.Write((uint)(endPosition - startPosition - 8));
            new ArrayFlags(false, ClassType.mxCELL).Serialize(writer);
            new ArrayDimension(count).Serialize(writer);
            new ArrayName(name).Serialize(writer);

            file.Close();

            writer.Close();
          }
          fs.Close();
        }
      }
    }

    private string filename;
    private MatlabFile file;
    private long startPosition;
    private int count;
    private string name;
    private bool isOpen;
  }
}

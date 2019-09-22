using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * Represent a matlab file.
   * </summary>
   * <remarks>
   * This class represent a matlab file and provide function for writing matrix 
   * elements (Matrix, Cell, Struct) to the file as well as for opening the file 
   * in streaming mode.
   * </remarks>
   * <example>
   * A new %Matlab file can be created by instantiating this class:
   * <code>
   *    MatlabFile file = new MatlabFile("test.mat", true);
   *    file.Write(new Matrix("a", 1.0));
   * </code>
   * This example will create a matlab file named test.mat and write a matrix named
   * a with a single scalar value (1.0) to the file.
   * </example>
   * <example>
   * A %Matlab file can also be opened in a streaming mode in which IMatrix elements
   * can be appended to a Cell element one at a time. While in streaming mode IMatrix 
   * elements can only be written to the file using the MatrixStream returned by the Open()
   * method:
   * <code>
   *    MatlabFile file = new MatlabFile("test.mat", true);
   *    file.Write(new Matrix("a", 1)); // IMatrix "a" is written normally.
   *    
   *    using(MatrixStream stream = file.Open("s1")) // Create a cell element, and open it for streaming
   *    {
   *      stream.Write(new Matrix("", 1)); // This is streamed to the cell element "s"
   *      stream.Write(new Matrix("", 2));
   *    }
   *    
   *    // OR as in most cases of streaming where the using statement is not appropriately:
   *    MatrixStream stream = file.Open("s2");   
   *    
   *    // Write data
   *    stream.Close();
   * </code>
   * </example>
   */
  public class MatlabFile
  {
    /**
     * <summary>
     * Instantiate the class.
     * </summary>
     * <remarks>
     * This will create a new instance of the MatlabFile class and will either created
     * the associated %Matlab file or will set it up for appending elements to it.
     * </remarks>
     * <param name="fileName">
     * name of the %Matlab file.
     * </param>
     * <param name="create">
     * create a new %Matlab file.
     * </param>
     */
    public MatlabFile(string fileName, bool create)
    {
      this.fileName = fileName;
      Locked = false;

      if (create) 
      {
        CreateHeader();
      }
    }

    /**
     * <summary>
     * Write a matrix element to the file.
     * </summary>
     * <param name="matrix">
     * the IMatrix to write to the file.
     * </param>
     */
    public void Write(IMatrix matrix)
    {
      if (!Locked)
      {
        using (FileStream fs = new FileStream(fileName, FileMode.Append))
        {
          using (BinaryWriter writer = new BinaryWriter(fs))
          {
            matrix.Serialize(writer);
            writer.Close();
          }
          fs.Close();
        }
      }
      else
      {
        throw new MatlabFileAccessException("File in streaming mode, write disallowed");
      }
    }

    /**
     * <summary>
     * Open a cell element for streaming.
     * </summary>
     * <param name="name">
     * name of the cell element to create.
     * </param>
     * <returns>
     * A MatrixStream to which IMatrix elements can be streamed
     * </returns>
     */
    public MatrixStream Open(string name)
    {
      if (!Locked)
      {
        Locked = true;
        return stream = new MatrixStream(fileName, this, name);
      }
      else
      {
        throw new MatlabFileAccessException("The file is already open for streaming");
      }
    }

    internal void Close()
    {
      if (Locked)
      {
        Locked = false;
      }
      else
      {
        throw new MatlabFileAccessException("File not opened for streaming");
      }
    }

    private void CreateHeader()
    {
      using (FileStream fs = new FileStream(fileName, FileMode.Create))
      {
        using (BinaryWriter writer = new BinaryWriter(fs))
        {
          byte[] header = new UTF8Encoding(false).GetBytes("MATLAB 5.0 MAT-file, Platform: PCWIN");
          byte[] space = new UTF8Encoding(false).GetBytes(" ");
          writer.Write(header);

          for (uint i = 0; i < 128 - header.Length - 4; ++i)
          {
            writer.Write(space);
          }

          writer.Write((ushort)0x0100);
          writer.Write((ushort)0x4D49);
          writer.Close();
        }
        fs.Close();
      }
    }

    /**
     * <summary>
     * name of the %Matlab file.
     * </summary>
     */
    public string FileName { get { return FileName; } }

    /**
     * <summary>
     * is the file locked for streaming.
     * </summary>
     */
    public bool Locked { get; set; }

    private string fileName;
    private MatrixStream stream;
  }
}

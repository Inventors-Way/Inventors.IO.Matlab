using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * A StringArray that holds a text string.
   * </summary>
   * <remarks>
   * This class represent an UTF8 encoded text string.
   * </remarks>
   * <example>
   * A text string can be written to a %Matlab file with the following code:
   * <code>
   *  MatlabFile file = new MatlabFile("test.mat", true);
   *  file.Write(new StringArray("str", "Hello, World!");
   * </code>
   * </example>
   */
  public class StringArray : IMatrix
  {
    /**
     * <summary>
     * Create a new instance of the class.
     * </summary>
     * <param name="name">
     * the name of the string array in the file
     * </param>
     * <param name="str">
     * the string that the string array should store
     * </param>
     */
    public StringArray(string name, string str)
    {
      byte[] strBytes = new UTF8Encoding(false).GetBytes(str);
      this.flags = new ArrayFlags(false, ClassType.mxCHAR);
      this.dims = new ArrayDimension(1, strBytes.Length);
      this.name = name;
      this.data = new Element<byte>(DataType.miUTF8, strBytes);
    }

    protected override void GetPayload(System.IO.BinaryWriter writer)
    {
      flags.Serialize(writer);
      dims.Serialize(writer);
      new ArrayName(name).Serialize(writer);
      data.Serialize(writer);      
    }

    /**
     * <summary>
     * The name of the matrix.
     * </summary>
     */
    public override string Name { get { return name; } set { name = value; } }

    private ArrayFlags flags;
    private ArrayDimension dims;
    private string name;
    private Element<byte> data;
  }
}

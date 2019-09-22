using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * Structure that contains a set of fields that each are an IMatrix.
   * </summary>
   * <remarks>
   * The Struct class represent %Matlab structures, which contain one or more fields.
   * Each of these fields are a IMatrix element and consequently, it is possible to created
   * nested struct (such as, struct.fiel1.a).
   * </remarks>
   * <example>
   * To create a struct:
   * <code>
   * Struct config = new Struct("config", new IMatrix[]{new Matrix("sample", 1000),
   *                                                    new Matrix("length", 0.5)});
   * Matrix data = new Matrix("data", data_arr);
   * Struct recording = new Struct("recording", new IMatrix[]{config, data});
   * file.Write(recording);
   * </code>
   * </example>
   * 
   */
  public class Struct : IMatrix
  {
    /**
     * <summary>
     * Create a new %Matlab structure.
     * </summary>
     * <param name="name">
     * The name of the struct array
     * </param>
     * <param name="fields">
     * The fields in the structure (the name of each IMatrix becomes the name of the field)
     * </param>
     */
    public Struct(string name, IMatrix[] fields)
    {
      this.name = name;
      this.dim = new Element<int>(new int[]{1,1});
      this.flag = new ArrayFlags(false, ClassType.mxSTRUCT);
      this.fieldLength = new Element<int>(new int[1]{ FieldNames.MAX_LENGTH });
      this.fieldNames = new FieldNames(fields);
      this.fields = fields;
    }

    protected override void GetPayload(BinaryWriter writer)
    {
      flag.Serialize(writer);
      dim.Serialize(writer);
      new ArrayName(name).Serialize(writer);
      fieldLength.Serialize(writer);
      fieldNames.Serialize(writer);

      foreach (IMatrix matrix in fields)
      {
        string saved = matrix.Name;
        matrix.Name = "";
        matrix.Serialize(writer);
        matrix.Name = saved;
      }
    }

    /**
     * <summary>
     * The name of the struct array.
     * </summary>
     */
    public override string Name { get { return name; } set { name = value; } }

    private string name;
    private Element<int> dim;
    private ArrayFlags flag;
    private Element<int> fieldLength;
    private FieldNames fieldNames;
    private IMatrix[] fields;
  }
}

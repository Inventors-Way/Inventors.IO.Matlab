using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * Cell array that can contain all other %Matlab arrays.
   * </summary>
   * <remarks>
   * A matlab cell array consists a 1, 2, or 3 dimensional grid of matlab
   * arrays, where each array can be numeric, structure, or another cell array.
   * Please note that jagged cell arrays can be created by embedding cell arrays
   * within a cell array. 
   * </remarks>
   * <example>
   * To create a cell array and write it to a matlab file:
   * <code>
   * MatlabFile matlabFile = MatlabFile("test.mat", true);
   * Matrix m1 = new Matrix("", new double[]{1,2,3});
   * Matrix m2 = new Matrix("", 2);
   * matlabFile.Write(new Cell("c", new IMatrix[]{m1,m2});
   * </code>
   * When loaded in %Matlab this cell array can be accessed with:
   * <code>
   * c{1} // will print [1 2 3]
   * c{2} // will print 2
   * </code>
   * </example>
   */
  public class Cell : IMatrix
  {
    /**
     * <summary>
     * Create a one-dimensional cell array.
     * </summary>
     * <param name="name">
     * The name of the cell array
     * </param>
     * <param name="data">
     * The one-dimensional array of IMatrix to save in the cell array.
     * </param>
     */
    public Cell(string name, IMatrix[] data)
    {
      this.flags = new ArrayFlags(false, ClassType.mxCELL);
      this.dims = new ArrayDimension(data);
      this.name = name;
      this.data = data;
    }

    /**
     * <summary>
     * Create a two-dimensional cell array.
     * </summary>
     * <param name="name">
     * The name of the cell array
     * </param>
     * <param name="data">
     * The two-dimensional array of IMatrix to save in the cell array.
     * </param>
     */
    public Cell(string name, IMatrix[,] data)
    {
      this.flags = new ArrayFlags(false, ClassType.mxCELL);
      this.dims = new ArrayDimension(data);
      this.name = name;
      this.data = Reshaper<IMatrix>.Transform(data);
    }

    /**
     * <summary>
     * Create a three-dimensional cell array.
     * </summary>
     * <param name="name">
     * The name of the cell array
     * </param>
     * <param name="data">
     * The three-dimensional array of IMatrix to save in the cell array.
     * </param>
     */
    public Cell(string name, IMatrix[,,] data)
    {
      this.flags = new ArrayFlags(false, ClassType.mxCELL);
      this.dims = new ArrayDimension(data);
      this.name = name;
      this.data = Reshaper<IMatrix>.Transform(data);
    }


    protected override void GetPayload(BinaryWriter writer)
    {
      flags.Serialize(writer);
      dims.Serialize(writer);
      new ArrayName(name).Serialize(writer);

      foreach (IMatrix matrix in data)
      {
        matrix.Serialize(writer);
      }
    }

    /**
     * <summary>
     * The name of the cell array.
     * </summary>
     */
    public override string Name { get { return name; } set { name = value; } }

    private ArrayFlags flags;
    private ArrayDimension dims;
    private string name;
    private IMatrix[] data;
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * A matrix containing numerical data.
   * </summary>
   * <remarks>
   * All numerical data is represent by the Matrix class and is in the
   * form of N-dimensional arrays. Different types of Matrix classes are
   * instantiated by calling the appropriate constructor.
   * </remarks>
   */
  public class Matrix : IMatrix
  {
    /**
     * <summary>
     * Create a matrix containing a single real scalar value.
     * <summary>
     * <param name="name">
     * name of the matrix
     * </param>
     * <param name="real">
     * the real scalar value
     * </param>
     */
    public Matrix(string name, double real)
    {
      this.flags = new ArrayFlags(false, ClassType.mxDOUBLE);
      this.dims = new ArrayDimension(new double[]{real});
      this.name = name;
      this.real = new Element<double>(new double[]{real});
      this.imag = null;
    }

    /**
     * <summary>
     * Create a matrix containing a single complex scalar value.
     * <summary>
     * <param name="name">
     * name of the matrix
     * </param>
     * <param name="real">
     * the real part of the scalar value
     * </param>
     * <param name="imag">
     * the imaginary part of the scalar value.
     * </param>
     */
    public Matrix(string name, double real, double imag)
    {
      this.flags = new ArrayFlags(false, ClassType.mxDOUBLE);
      this.dims = new ArrayDimension(new double[] { real });
      this.name = name;
      this.real = new Element<double>(new double[] { real });
      this.imag = new Element<double>(new double[] { imag });
    }

    /** 
     * <summary>
     * Create a matrix containing a column vector of real values. 
     * <summary>
     * <param name="name">
     * name of the matrix
     * </param>
     * <param name="real">
     * 1-dimensional array containing the data for the vector
     * </param>
     */
    public Matrix(string name, double[] real)
    {
      this.flags = new ArrayFlags(false, ClassType.mxDOUBLE);
      this.dims = new ArrayDimension(real);
      this.name = name;
      this.real = new Element<double>(real);
      this.imag = null;
    }

    /** 
     * <summary>
     * Create a matrix containing a column vector of complex values. 
     * <summary>
     * <param name="name">
     * name of the matrix
     * </param>
     * <param name="real">
     * 1-dimensional array containing the real part of for the vector
     * </param>
     * <param name="imag">
     * 1-dimensional array containing the imaginary part of for the vector
     * </param>
     */
    public Matrix(string name, double[] real, double[] imag)
    {
      if (Compatible(real, imag))
      {
        this.flags = new ArrayFlags(true, ClassType.mxDOUBLE);
        this.dims = new ArrayDimension(real);
        this.name = name;
        this.real = new Element<double>(real);
        this.imag = new Element<double>(imag);
      }
      else
      {
        throw new InvalidMatrixDimensionException("Real and imaginary parts of the matrix does not have the same dimensions");
      }
    }

    /** 
     * <summary>
     * Create a matrix containing a 2-dimensial matrix of real values. 
     * <summary>
     * <param name="name">
     * name of the matrix
     * </param>
     * <param name="real">
     * 2-dimensional array containing the data for the vector
     * </param>
     */
    public Matrix(string name, double[,] real)
    {
      this.flags = new ArrayFlags(false, ClassType.mxDOUBLE);
      this.dims = new ArrayDimension(real);
      this.name = name;
      this.real = new Element<double>(Reshaper<double>.Transform(real));
      this.imag = null;      
    }

    /** 
     * <summary>
     * Create a matrix containing a 2-dimensial matrix of complex values. 
     * <summary>
     * <param name="name">
     * name of the matrix
     * </param>
     * <param name="real">
     * 2-dimensional array containing the real data for the vector
     * </param>
     * <param name="imag">
     * 2-dimensional array containing the imaginary data for the vector
     * </param>
     */
    public Matrix(string name, double[,] real, double[,] imag)
    {
      if (Compatible(real, imag))
      {
        this.flags = new ArrayFlags(true, ClassType.mxDOUBLE);
        this.dims  = new ArrayDimension(real);
        this.name  = name;
        this.real  = new Element<double>(Reshaper<double>.Transform(real));
        this.imag  = new Element<double>(Reshaper<double>.Transform(imag));
      }
      else
      {
        throw new InvalidMatrixDimensionException("Real and imaginary parts of the matrix does not have the same dimensions");
      }
    }

    /** 
     * <summary>
     * Create a matrix containing a 3-dimensial matrix of real values. 
     * <summary>
     * <param name="name">
     * name of the matrix
     * </param>
     * <param name="real">
     * 3-dimensional array containing the data for the vector
     * </param>
     */
    public Matrix(string name, double[, ,] real)
    {
      this.flags = new ArrayFlags(false, ClassType.mxDOUBLE);
      this.dims = new ArrayDimension(real);
      this.name = name;
      this.real = new Element<double>(Reshaper<double>.Transform(real));
      this.imag = null;
    }

    /** 
     * <summary>
     * Create a matrix containing a 3-dimensial matrix of complex values. 
     * <summary>
     * <param name="name">
     * name of the matrix
     * </param>
     * <param name="real">
     * 3-dimensional array containing the real data for the vector
     * </param>
     * <param name="imag">
     * 3-dimensional array containing the imaginary data for the vector
     * </param>
     */
    public Matrix(string name, double[, ,] real, double[, ,] imag)
    {
      if (Compatible(real, imag))
      {
        this.flags = new ArrayFlags(true, ClassType.mxDOUBLE);
        this.dims = new ArrayDimension(real);
        this.name = name;
        this.real = new Element<double>(Reshaper<double>.Transform(real));
        this.imag = new Element<double>(Reshaper<double>.Transform(imag));
      }
      else
      {
        throw new InvalidMatrixDimensionException("Real and imaginary parts of the matrix does not have the same dimensions");
      }
    }

    private bool Compatible(Array x, Array y)
    {
      bool retValue = true;

      if (x.Rank == y.Rank)
      {
        for (int i = 0; i < x.Rank; ++i)
        {
          if (x.GetLength(i) != y.GetLength(i))
          {
            retValue = false;
          }
        }
      }
      else
      {
        retValue = false;
      }

      return retValue;
    }

    protected override void GetPayload(BinaryWriter writer)
    {
      flags.Serialize(writer);
      dims.Serialize(writer);
      new ArrayName(name).Serialize(writer);
      real.Serialize(writer);

      if (flags.Complex)
      {
        imag.Serialize(writer);
      }
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
    private Element<double> real;
    private Element<double> imag;
  }
}

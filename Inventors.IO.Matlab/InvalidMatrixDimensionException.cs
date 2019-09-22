using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * Exception that are thrown if a matrix have a wrong dimension.
   * </summary>
   * <remarks>
   * The format for matrixes with imaginary numbers require that the 
   * arrays provided for the real and imaginary part of the numbers 
   * have the same dimensions. This exception is thrown if these 
   * arrays does not have the same dimensions.
   * </remarks>
   */
  public class InvalidMatrixDimensionException : System.ApplicationException
  {
    public InvalidMatrixDimensionException(string message) :
      base(message)
    { }

    public InvalidMatrixDimensionException(string message, Exception inner) :
      base(message, inner)
    { }
  
  }
}

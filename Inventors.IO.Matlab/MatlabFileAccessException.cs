using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * Attempts to write to a MatlabFile or MatrixStream when not opened for writing.
   * </summary>
   * <remarks>
   * This exception is thrown when: 1) a MatlabFile has been opened for streaming
   * and it is attempted to write to this file, 2) a MatrixStream has been closed and it
   * is attempted to write to the MatrixStream, or 3) if a MatlabFile is attempted to be
   * opened for streaming multiple times without the previous MatrixStream has been closed.
   * </remarks>
   */
  public class MatlabFileAccessException : ApplicationException
  {
    public MatlabFileAccessException(string message) :
      base(message)
    { }

    public MatlabFileAccessException(string message, Exception inner) :
      base(message, inner)
    { }  
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventors.IO.Matlab
{
  /**
   * <summary>
   * Exception that is thrown if a field in a struct is too long.
   * </summary>
   * <remarks>
   * At the present %Matlab only support field names in structs that are up
   * to 31 characters (+ a null terminator). This exception will be throw if 
   * it is attempted to create a field name in a Struct that are longer than
   * 31 characters.
   * </remarks>
   */
  public class InvalidFieldNameException : ApplicationException
  {
    public InvalidFieldNameException(string message) :
      base(message)
    { }

    public InvalidFieldNameException(string message, Exception inner) :
      base(message, inner)
    { }
  }
}

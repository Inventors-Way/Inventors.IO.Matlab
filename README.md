# ITLS MatLib
## Introduction
ITLS MatLib is a library for writing matlab or octave mat files that has a small and easy to learn public interface. Despites its simple interface it offers a set of powerful features for creating matlab/octave files:

- Scalar, 1-, 2-, and 3-dimensional numeric arrays.
- Character arrays.
- Struct array's (including composite structs, such as \c record.config.sample ).
- 1-, 2-, and 3-dimensional cell array's that can contain any array type (numeric, character, struct, or another cell array).
- Streaming array's of any type to a 1-dimensional cell array in the file.
- Jagged cell array.

The following guide will provide a tutorial on how to use the library. In this
tutorial it is assumed that the reader is proficient in the use of matlab and C#.

## Create a Matlab file
MatlabFile is the main class for creating matlab files that when instantiated will either create
a new file or append data to an existing file. 

In order to create a file:

```C#
MatlabFile file = new MatlabFile("test.mat", true); // Parameters (name, create?)
```

In order to append to the file instead, instantiate the class with its second parameter set to
false:

```C#
MatlabFile file = new MatlabFile("test.mat", false); // Parameters (name, create?)
```

Once the MatlabFile class is instantiated data can either be written in non-streaming mode
using its Write() method, or it can be opened for streaming data using its Open() method.
The following example illustreated various way data can be written to the file using its
non-streaming mode:

```C#
// Example data
double[] realVec = new double[] {1, 2, 3, 4};
double[] imagVec = new double[] {5, 6, 7, 8};
double[,] matData = new double[2,4] = {realVec, realVec};

// Create some numerical array that can later be written to the file
Matrix a = new Matrix("a", 3);
Matrix b = new Matrix("b", realVec);
Matrix c = new Matrix("c", realVec, imagVec);
Matrix d = new Matrix("d", matData);

file.Write(a); // Write a single scaler (a = 3)
file.Write(b); // Write a real column vector (b = [1 2 3 4]')
file.Write(c); // Write a complex column vector
file.Write(d); // Write a 2-dimensional matrix

file.Write(new Cell("data", new IMatrix[] {a, b, c, d}); // Write a 1-D cell with elements a,b,c,d
file.Write(new Struct("config", new IMatrix[] {a, b, c, d}); // Write a 1-D struct with fields a,b,c,d
```

## Streaming data
The library provide a streaming mode for matlab/octave files with which data can be streamed to
a 1-dimensional cell array. This is particular useful when either the number of cells cannot be known in
advance or they are impractical to store in memory until they can be written to disc. A example of
a typical program that benifits from this functionality would be a data aquisition program, which
records data in the form of sweeps.

The following example illustrates how the file can be opened for streaming and data can be straemed to the
resulting MatrixStream:

```C#
MatrixStream ms = file.Open("data");

while(Sampling)
{
  stream.Write(new Matrix("", getData());
}
stream.Close();
```

Internally this is implemented such that a dummy cell array is created in the file when the 
Open() method is called. The size and number of elements in the cell array is initialy set to
zero. When all elements has been written (streamed) to the cell array and the Close() method is 
called the correct size and number of elements will be written to the header of the cell array.
Consequently, it is not possible to write data to the MatlabFile object while a MatrixStream 
is opened. Attempts to write data to the MatlabFile will then throw a MatrixFileAccessException.

## Jagged Cell Array's
The elements of a cell array (Cell) may contain any object that has IMatrix as its base class. 
Consequently, the elements can be any other element (StringArray, Matrix, Struct, and Cell). 
Consequently it is possible to create jagged cell aray's, where its elements are other cell arrays:

```C#
Cell c1 = new Cell("", new IMatrix[] { new Matrix(1), new Matrix(2)};
Cell c2 = new Cell("", new IMatrix[] { new Matrix(1), new Matrix(2), new Marix(3)};
Cell c = new Cell("c", new IMatrix[] { c1, c2});

file.Write(c); // This will create cell array "c", where length(c{1}) == 2 and length(c{2}) == 3
```

## Composite struct array's
Like cell arrays, each field of struct array may be any object with IMatrix as its base class.
Consequently, each field in a struct can be another struct and arbitrary deep nested levels of
structs can be created.



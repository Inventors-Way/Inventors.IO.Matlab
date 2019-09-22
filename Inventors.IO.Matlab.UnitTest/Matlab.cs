using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inventors.IO.Matlab;

namespace RLAB.UnitTest
{
    [TestClass]
    public class Matlab
    {
        [TestMethod]
        public void PTT_IO_Matlab_BasicCellTest()
        {
            var data = new double[] { 1, 2, 3, 4 };
            var matrix = new Matrix[] { new Matrix("", 2.0),
                              new Matrix("", data),
                              new Matrix("", data) };
            IMatrix m = new Matrix("", 1);
            var FileName = @"basiccell.mat";

            {
                MatlabFile file = new MatlabFile(FileName, true);
                file.Write(new Cell("c1", matrix));
            }

            {
                MatlabFile file = new MatlabFile(FileName, false);
                file.Write(new Cell("c2", new IMatrix[] { new Matrix("", 1), new Matrix("", 2) }));
                file.Write(new Cell("c3", new IMatrix[2, 3] { { m, m, m }, { m, m, m } }));
                file.Write(new Cell("c4", new IMatrix[2, 3, 4] {{{ m, m, m,m }, { m, m, m,m }, { m, m, m,m }},
                                                    {{ m, m, m,m }, { m, m, m,m }, { m, m, m,m }}}));
                file.Write(new Cell("c5", new IMatrix[] { new Cell("", new IMatrix[] { m, m, m }), new Cell("", new IMatrix[] { m, m, m, m }) }));
            }
        }

        [TestMethod]
        public void PTT_IO_Matlab_StreamTest()
        {
            var FileName = "streamed.mat";
            MatlabFile file = new MatlabFile(FileName, true);
            MatrixStream ms = file.Open("s");

            ms.Write(new Matrix("", 1));
            ms.Write(new Matrix("", 2));
            ms.Write(new Matrix("", 3));
            ms.Close();
        }

        [TestMethod]
        public void PTT_IO_Matlab_MatrixTest()
        {
            var r1x4 = new double[] { 1, 2, 3, 4 };
            var i1x4 = new double[] { 4, 3, 2, 1 };

            var r3x4 = new double[3, 4] {{1,2,3,4},
                               {5,6,7,8},
                               {9,10,11,12}};
            var i3x4 = new double[3, 4] {{12,11,10,9},
                               {8,7,6,5},
                               {4,3,2,1}};

            var r2x3x4 = new double[2, 3, 4] {{{1,2,3,4},
                                     {5,6,7,8},
                                     {9,10,11,12}},
                                    {{101,102,103,104},
                                     {105,106,107,108},
                                     {109,110,111,112}}};
            var i2x3x4 = new double[2, 3, 4] {{{201,202,203,204},
                                     {205,206,207,208},
                                     {209,210,211,212}},
                                    {{301,302,303,304},
                                     {305,306,307,308},
                                     {309,310,311,312}}};
            {
                MatlabFile file = new MatlabFile("matrix.mat", true);

                file.Write(new StringArray("str", "Hello, World!"));
                file.Write(new Matrix("m0", 3.5));
                file.Write(new Matrix("m1", r1x4));
                file.Write(new Matrix("m2", r1x4, i1x4));
                file.Write(new Matrix("m3", r3x4));
                file.Write(new Matrix("m4", r3x4, i3x4));
            }
            {
                MatlabFile file = new MatlabFile("matrix.mat", false);

                file.Write(new Matrix("m5", r2x3x4));
                file.Write(new Matrix("m6", r2x3x4, i2x3x4));
            }
        }

        [TestMethod]
        public void PTT_IO_Matlab_StructTest()
        {
            var FileName = "struct.mat";

            // Create the data for the test
            double[] data = new double[] { 1.5, 2, 2.5, 3, 4.5 };
            IMatrix sub = new Struct("config", new IMatrix[] { new Matrix("sample", 1000),
                                                           new Matrix("length", 0.25),
                                                           new Matrix("offset", 0.05)});
            IMatrix celem = new Cell("c", new IMatrix[] { sub, new Matrix("d", 3), new Matrix("data", data) });
            IMatrix top = new Struct("record", new IMatrix[] { sub, new Matrix("data", data), celem });


            MatlabFile file = new MatlabFile(FileName, true);
            file.Write(top);
        }
    }
}

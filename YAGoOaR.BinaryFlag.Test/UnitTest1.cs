using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.BinaryFlag;

namespace YAGoOaR.BinaryFlag.Test
{
    [TestClass]
    public class BinaryFlagTests
    {
        const ulong maxBinaries = 17179868704;
        const ulong minBinaries = 2;
        const ulong middlePoint = 8589934353;
        const ulong offPointOffset = 1;
        const ulong outPointOffset = 1;
        const ulong inPoint1 = 10;

        [TestClass]
        public class initialValueTests
        {
            [TestMethod]
            public void Test_initialValue_True() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(inPoint1, true);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_initialValue_False() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(inPoint1, false);
                Assert.AreEqual(flags.GetFlag(), false);
            }
        }
        [TestClass]
        public class LengthLimitTests
        {
            [TestMethod]
            public void Test_InPoint_middle() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(middlePoint);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_OnPoint_min() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(minBinaries);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_OnPoint_max() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(maxBinaries);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_OffPoint_min() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(minBinaries + offPointOffset);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_OffPoint_max() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(maxBinaries - offPointOffset);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_OutPoint_min_Throw() {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                    MultipleBinaryFlag flags = new MultipleBinaryFlag(minBinaries - outPointOffset);
                });
            }

            [TestMethod]
            public void Test_OutPoint_max_Throw() {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                    MultipleBinaryFlag flags = new MultipleBinaryFlag(maxBinaries + outPointOffset);
                });
            }
        }
    }
}

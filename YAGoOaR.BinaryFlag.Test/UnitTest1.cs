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

        const ulong range1_length = 10;
        const ulong range1_max = range1_length - 1;
        const ulong range1_min = 0;
        const ulong range1_mid = 5;

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
        public class GeneralTests
        {
            [TestMethod]
            public void Test_ResetFlag() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length, true);
                flags.ResetFlag(range1_mid);
                Assert.AreEqual(flags.GetFlag(), false);
            }

            [TestMethod]
            public void Test_SetFlag() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(2, false);
                flags.SetFlag(0);
                flags.SetFlag(1);
                Assert.AreEqual(flags.GetFlag(), true);
            }
        }

        [TestClass]
        public class LengthParamLimitTests
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

        [TestClass]
        public class ResetFlagLimitTests
        {
            [TestMethod]
            public void Test_InPoint_middle() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                flags.ResetFlag(range1_mid);
                Assert.AreEqual(flags.GetFlag(), false);
            }

            [TestMethod]
            public void Test_OnPoint_min() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                flags.ResetFlag(range1_min);
                Assert.AreEqual(flags.GetFlag(), false);
            }

            [TestMethod]
            public void Test_OnPoint_max() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                flags.ResetFlag(range1_max);
                Assert.AreEqual(flags.GetFlag(), false);
            }

            [TestMethod]
            public void Test_OffPoint_min() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                flags.ResetFlag(range1_min + offPointOffset);
                Assert.AreEqual(flags.GetFlag(), false);
            }

            [TestMethod]
            public void Test_OffPoint_max() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                flags.ResetFlag(range1_max - offPointOffset);
                Assert.AreEqual(flags.GetFlag(), false);
            }

            [TestMethod]
            public void Test_OutPoint_min_Throw() {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                    MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                    unchecked {
                        flags.ResetFlag(range1_min - outPointOffset);
                    }
                });
            }

            [TestMethod]
            public void Test_OutPoint_max_Throw() {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                    MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                    unchecked {
                        flags.ResetFlag(range1_max + outPointOffset);
                    }
                });
            }
        }

        [TestClass]
        public class SetFlagLimitTests
        {
            [TestMethod]
            public void Test_InPoint_middle() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                flags.SetFlag(range1_mid);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_OnPoint_min() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                flags.SetFlag(range1_min);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_OnPoint_max() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                flags.SetFlag(range1_max);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_OffPoint_min() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                flags.SetFlag(range1_min + offPointOffset);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_OffPoint_max() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                flags.SetFlag(range1_max - offPointOffset);
                Assert.AreEqual(flags.GetFlag(), true);
            }

            [TestMethod]
            public void Test_OutPoint_min_Throw() {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                    MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                    unchecked {
                        flags.SetFlag(range1_min - outPointOffset);
                    }
                });
            }

            [TestMethod]
            public void Test_OutPoint_max_Throw() {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                    MultipleBinaryFlag flags = new MultipleBinaryFlag(range1_length);
                    unchecked {
                        flags.SetFlag(range1_max + outPointOffset);
                    }
                });
            }
        }

        [TestClass]
        public class DisposedFlagStateTests
        {
            [TestMethod]
            public void Dispose_GetFlag_Expect_Null() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(inPoint1);
                flags.Dispose();
                Assert.AreEqual(flags.GetFlag(), null);
            }
            [TestMethod]
            public void Dispose_Setflag_GetFlag_Expect_Null() {
                MultipleBinaryFlag flags = new MultipleBinaryFlag(inPoint1);
                flags.Dispose();
                flags.ResetFlag(0);
                flags.SetFlag(1);
                Assert.AreEqual(flags.GetFlag(), null);
            }
        }
    }
}

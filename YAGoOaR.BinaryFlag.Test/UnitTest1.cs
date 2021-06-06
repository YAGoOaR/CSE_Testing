using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.BinaryFlag;

namespace YAGoOaR.BinaryFlag.Test
{
    [TestClass]
    public class BinaryFlagTests
    {
        const ulong inPoint1 = 10;

        [TestMethod]
        public void Test_initialValue_True() {
            MultipleBinaryFlag flag = new MultipleBinaryFlag(inPoint1, true);
            Assert.AreEqual(flag.GetFlag(), true);
        }

        [TestMethod]
        public void Test_initialValue_False() {
            MultipleBinaryFlag flag = new MultipleBinaryFlag(inPoint1, false);
            Assert.AreEqual(flag.GetFlag(), false);
        }
    }
}

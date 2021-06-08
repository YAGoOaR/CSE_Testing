using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.PasswordHashingUtils;

namespace YAGoOaR.PasswordHashingUtils.Test
{
    [TestClass]
    public class PasswordHashingUtilsTest
    {
        [TestMethod]
        public void TestMethod1() {
            string hash = PasswordHasher.GetHash("password");
            Assert.IsNull(hash, hash);
        }

        [TestMethod]
        public void TestMethod2() {
            string hash = PasswordHasher.GetHash("password");
            Assert.IsNull(hash, hash);
        }

    }
}

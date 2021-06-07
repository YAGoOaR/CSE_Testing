using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.PasswordHashingUtils;

namespace YAGoOaR.PasswordHashingUtils.Test
{
    [TestClass]
    public class PasswordHashingUtilsTest
    {
        [TestMethod]
        public void TestMethod1() {
            string hash = PasswordHasher.GetHash("iloveC#");
            Assert.IsNotNull(hash);
        }
    }
}

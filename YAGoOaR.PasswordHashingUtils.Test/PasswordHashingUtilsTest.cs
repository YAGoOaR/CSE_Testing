using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.PasswordHashingUtils;
using System.Text;

namespace YAGoOaR.PasswordHashingUtils.Test
{
    [TestClass]
    public class PasswordHashingUtilsTest
    {
        PasswordHasher passwordHasher;
        FieldInfo saltField;
        FieldInfo modAdler32Field;
        string initialSalt;
        uint initialModAdler32;

        const string customSalt = "just a normal salt";
        const string nonAsciiSalt = "お前はもう死んでいる";
        const uint customModAdler32 = 12345;

        const string customPassword = "password";
        const string nonAsciiPassword = "パスワード";

        [TestInitialize]
        public void TestInitialize() {
            passwordHasher = new PasswordHasher();

            BindingFlags privateStatic = BindingFlags.NonPublic | BindingFlags.Static;
            saltField = typeof(PasswordHasher).GetField("_salt", privateStatic);
            modAdler32Field = typeof(PasswordHasher).GetField("_modAdler32", privateStatic);

            initialSalt = (string)saltField.GetValue(passwordHasher);
            initialModAdler32 = (uint)modAdler32Field.GetValue(passwordHasher);
        }

        [TestCleanup]
        public void Reset() {
            saltField.SetValue(passwordHasher, initialSalt);
            modAdler32Field.SetValue(passwordHasher, initialModAdler32);
        }

        [DataTestMethod]
        [DataRow("0-5", "", (uint)0, true, true)]
        [DataRow("0-5", null, (uint)0, true, true)]
        [DataRow("0-4-5", "", customModAdler32, true, false)]
        [DataRow("0-1-3-5", customSalt, (uint)0, false, true)]
        [DataRow("0-1-3-4-5", customSalt, customModAdler32, false, false)]
        [DataRow("0-1-2-3-5", nonAsciiSalt, (uint)0, false, true, true)]
        [DataRow("0-1-2-3-4-5", nonAsciiSalt, customModAdler32, false, false, true)]
        public void InitMethodTests(string pathName, string salt, uint adlerMod32,
            bool saltRemainsInitial, bool modAdlerRemainsInitial, bool expectNonAsciiDecryption = false) {

            string failMessage = $"{pathName} thread test failed.";
            PasswordHasher.Init(salt, adlerMod32);

            string newSalt = (string)saltField.GetValue(passwordHasher);
            Assert.AreEqual(saltRemainsInitial, newSalt == initialSalt, failMessage);

            uint newModAdler32 = (uint)modAdler32Field.GetValue(passwordHasher);
            Assert.AreEqual(modAdlerRemainsInitial, newModAdler32 == initialModAdler32, failMessage);

            if (expectNonAsciiDecryption) {
                string expected = Encoding.ASCII.GetString(Encoding.Unicode.GetBytes(salt));
                Assert.AreEqual(expected, newSalt, failMessage);
            }
        }

        [DataTestMethod]
        [DataRow("0-4-5", null, true)]
        [DataRow("0-1-3-5", customPassword, false)]
        [DataRow("0-1-2-3-5", nonAsciiPassword, false)]
        public void GetHashMethodTests(string pathName, string initialPassword, bool expectNull) {
            string failMessage = $"{pathName} thread test failed.";

            string hash = PasswordHasher.GetHash(initialPassword, customSalt, customModAdler32);

            if (expectNull) {
                Assert.IsNull(hash);
            } else {
                Assert.IsNotNull(hash);
            }
        }
    }
}

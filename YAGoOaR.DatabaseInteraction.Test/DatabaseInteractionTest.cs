using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.CoSFE.DatabaseUtils;
using IIG.PasswordHashingUtils;

namespace YAGoOaR.DatabaseInteraction.Test
{
    [TestClass]
    public class DatabaseInteractionTest
    {
        const string Server = @"YAGOOAR-DESKTOP\MSSQLSERVER1";
        const bool IsTrusted = false;
        const string Login = @"sa";
        const string Password = @"aVerySecurePassword";
        const int ConnectionTimeout = 75;

        [TestClass]
        public class TestAuthDB
        {
            const string Database = @"IIG.CoSWE.AuthDB";
            const string salt = "just a salt";
            const uint adlerMod32 = 54321;
            AuthDatabaseUtils DB;

            public void ClearDB() {
                DB.ExecSql("DELETE FROM Credentials");
            }

            [TestInitialize]
            public void TestInit() {
                DB = new AuthDatabaseUtils(Server, Database, IsTrusted, Login, Password, ConnectionTimeout);
                ClearDB();
            }

            [TestMethod]
            public void Test_Check_Credentials() {
                string login = "user1";
                string password = "abcd";
                string message = $"login: {login}, password: {password}";
                string passHash = PasswordHasher.GetHash(password, salt, adlerMod32);

                Assert.IsTrue(DB.AddCredentials(login, passHash),
                    $"AddCredentials failed." + message);

                Assert.IsTrue(DB.CheckCredentials(login, passHash),
                    $"CheckCredentials failed." + message);
            }

            [TestMethod]
            public void Test_Delete_Credentials() {
                string login = "user2";
                string password = "qwerty";
                string message = $"login: {login}, password: {password}";
                string passHash = PasswordHasher.GetHash(password, salt, adlerMod32);

                Assert.IsTrue(DB.AddCredentials(login, passHash),
                    $"AddCredentials failed." + message);

                Assert.IsTrue(DB.DeleteCredentials(login, passHash),
                    $"DeleteCredentials failed." + message);

                Assert.IsFalse(DB.CheckCredentials(login, passHash),
                    $"CheckCredentials failed." + message);
            }

            [TestMethod]
            public void Test_Add_Existing_Credentials() {
                string login = "user2";
                string password = "qwerty";
                string message = $"login: {login}, password: {password}";
                string passHash = PasswordHasher.GetHash(password, salt, adlerMod32);

                Assert.IsTrue(DB.AddCredentials(login, passHash),
                    $"AddCredentials failed." + message);
                Assert.IsFalse(DB.AddCredentials(login, passHash),
                    $"AddCredentials(existing) result is True. Expected: False. " + message);
            }

            [DataTestMethod]
            [DataRow("Valid data", "user1", "abcd", true)]
            [DataRow("Unicode data", "█", "□", true)]
            [DataRow("Null data", null, null, false)]
            [DataRow("Valid login, null password", "login", null, false)]
            [DataRow("Null login, valid password", null, "password", false)]
            [DataRow("Empty data", "", "", false)]
            [DataRow("Valid login, empty password", "login", "", false)]
            [DataRow("Empty login, valid password", "", "password", false)]
            public void Test_Valid_Invalid_Credentials(string testName, string login, string password, bool isValid) {
                string message = $"Test name: {testName}. login: {login}, password: {password}";
                string passHash = PasswordHasher.GetHash(password, salt, adlerMod32);

                Assert.AreEqual(isValid, DB.AddCredentials(login, passHash),
                    $"AddCredentials result is {!isValid}. Expected: {isValid} " + message);

                Assert.AreEqual(isValid, DB.CheckCredentials(login, passHash),
                    $"CheckCredentials result is {!isValid}. Expected: {isValid} " + message);

                Assert.AreEqual(isValid, DB.DeleteCredentials(login, passHash),
                    $"DeleteCredentials result is {!isValid}. Expected: {isValid} " + message);
            }

            [TestCleanup]
            public void Cleanup() {
                ClearDB();
            }
        }
    }
}

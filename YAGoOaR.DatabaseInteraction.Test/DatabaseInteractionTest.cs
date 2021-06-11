using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.CoSFE.DatabaseUtils;
using IIG.PasswordHashingUtils;

namespace YAGoOaR.DatabaseInteraction.Test
{
    [TestClass]
    public class DatabaseInteractionTest
    {
        const string Server = @"YAGOOAR-DESKTOP\MSSQLSERVER1";
        const string Database = @"IIG.CoSWE.AuthDB";
        const bool IsTrusted = false;
        const string Login = @"sa";
        const string Password = @"aVerySecurePassword";
        const int ConnectionTimeout = 75;

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
        public void Test_Add_Check_Credentials() {
            string login = "user1";
            string password = "abcd";
            string message = $"login: {login}, password: {password}";
            string passHash = PasswordHasher.GetHash(password, salt, adlerMod32);
            Assert.IsTrue(DB.AddCredentials(login, passHash), "AddCredentials failed. " + message);
            Assert.IsTrue(DB.CheckCredentials(login, passHash), "CheckCredentials failed. " + message);
        }

        [TestCleanup]
        public void Cleanup() {
            ClearDB();
        }
    }
}

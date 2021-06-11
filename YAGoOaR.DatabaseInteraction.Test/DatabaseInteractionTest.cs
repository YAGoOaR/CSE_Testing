using Microsoft.VisualStudio.TestTools.UnitTesting;
using IIG.CoSFE.DatabaseUtils;
using IIG.PasswordHashingUtils;
using IIG.BinaryFlag;
using IIG.FileWorker;
using System.IO;

namespace YAGoOaR.DatabaseInteraction.Test
{
    [TestClass]
    public class TestAuthDB
    {
        const string Server = @"YAGOOAR-DESKTOP\MSSQLSERVER1";
        const bool IsTrusted = false;
        const string Login = @"sa";
        const string Password = @"aVerySecurePassword";
        const int ConnectionTimeout = 75;

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

        [TestCleanup]
        public void Cleanup() {
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

        [TestMethod]
        public void Test_Update_Credentials() {
            string login = "user1";
            string password = "abcd";
            string login2 = "user2";
            string password2 = "efgh";
            string message = $"login: {login}, password: {password}" +
                $"login2: {login2}, password2: {password2}";

            string passHash = PasswordHasher.GetHash(password, salt, adlerMod32);
            string passHash2 = PasswordHasher.GetHash(password2, salt, adlerMod32);

            Assert.IsTrue(DB.AddCredentials(login, passHash),
                $"AddCredentials failed. " + message);

            Assert.IsTrue(DB.UpdateCredentials(login, passHash, login2, passHash2),
                $"UpdateCredentials failed. " + message);

            Assert.IsFalse(DB.CheckCredentials(login, passHash),
                $"CheckCredentials is unexpected. " + message);

            Assert.IsTrue(DB.CheckCredentials(login2, passHash2),
                $"CheckCredentials failed. " + message);
        }

        [DataTestMethod]
        [DataRow("Valid data", "user1", "abcd", true)]
        [DataRow("Unicode data", "█", "□", true)]
        [DataRow("Null data", null, null, false)]
        [DataRow("Valid login, null password", "login", null, false)]
        [DataRow("Null login, valid password", null, "password", false)]
        [DataRow("Empty data", "", "", false)]
        [DataRow("Valid login, empty password", "login", "", true)]
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
    }

    [TestClass]
    public class Test_BinaryFlag_FileWorker
    {
        const string outputFile = "output.txt";

        [TestCleanup]
        public void Cleanup() {
            File.Delete(outputFile);
        }

        [TestMethod]
        public void Test_Write() {
            MultipleBinaryFlag flags = new MultipleBinaryFlag(10);
            Assert.IsTrue(BaseFileWorker.Write(flags.GetFlag().ToString(), outputFile));
        }

        [TestMethod]
        public void Test_Read_True_Flag() {
            MultipleBinaryFlag flags = new MultipleBinaryFlag(10);
            string res = flags.GetFlag().ToString();
            Assert.IsTrue(BaseFileWorker.Write(res, outputFile), "Write failed");
            Assert.AreEqual(res, BaseFileWorker.ReadAll(outputFile), "Results did not match");
        }

        [TestMethod]
        public void Test_False_Flag() {
            MultipleBinaryFlag flags = new MultipleBinaryFlag(10, false);

            string res = flags.GetFlag().ToString();
            Assert.IsTrue(BaseFileWorker.Write(res, outputFile), "Write failed");
            Assert.AreEqual(res, BaseFileWorker.ReadAll(outputFile), "Results did not match");
        }

        [TestMethod]
        public void Test_Null_Flag() {
            MultipleBinaryFlag flags = new MultipleBinaryFlag(10, false);
            flags.Dispose();

            string res = flags.GetFlag().ToString();
            Assert.IsTrue(BaseFileWorker.Write(res, outputFile), "Write failed");
            Assert.AreEqual(res, BaseFileWorker.ReadAll(outputFile), "Results did not match");
        }

        [TestMethod]
        public void Test_Set_Flags() {
            MultipleBinaryFlag flags = new MultipleBinaryFlag(3, false);
            flags.SetFlag(0);
            flags.SetFlag(1);
            flags.SetFlag(2);
            bool? res = flags.GetFlag();
            Assert.IsTrue(res ?? false, "Unexpected GetFlag result");
            Assert.IsTrue(BaseFileWorker.Write(res.ToString(), outputFile), "Write failed");
            Assert.AreEqual(res.ToString(), BaseFileWorker.ReadAll(outputFile), "Results did not match");
        }

        [DataTestMethod]
        [DataRow("output12345.txt", true)]
        [DataRow("output12345", true)]
        [DataRow("output12345.jpg", true)]
        [DataRow("►◄↕‼¶§.jpg", true)]
        [DataRow("looooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo" +
            "ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo" +
            "ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooong.txt", false)] // Length: 257
        [DataRow("!@#$%^&*()|=-<>?/':;.", false)]
        public void Test_FileNames(string fileName, bool expectSuccess) {
            MultipleBinaryFlag flags = new MultipleBinaryFlag(10);
            string res = flags.GetFlag().ToString();
            bool actual = BaseFileWorker.Write(res, fileName);
            Assert.AreEqual(expectSuccess, actual, "Write failed");
            if (!actual) return;
            Assert.AreEqual(res, BaseFileWorker.ReadAll(fileName), "Results did not match");
            File.Delete(fileName);
        }
    }
}

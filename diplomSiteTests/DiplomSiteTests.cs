using diplomOriginal.Controllers;
using diplomOriginal.Models;
using diplomOriginal.Modules;

namespace diplomSiteTests
{
    [TestClass]
    public class DiplomSiteTests
    {
        [TestMethod]
        public void does_user_at_gmailcom_exists_in_database()
        {
            ConnectToDB();

            Assert.IsTrue(Database.DoesAccountExist("admin@gmail.com").Result);
        }

        private void ConnectToDB()
        {
            Database.InitConnection("Server=127.0.0.1;User ID=root;Password=admin;Database=mydb");
        }
    }
}
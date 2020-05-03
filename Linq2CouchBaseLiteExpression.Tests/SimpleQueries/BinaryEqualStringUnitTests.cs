using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests
{
    [TestClass]
    public class BinaryEqualStringUnitTests : BaseUnitTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public override void CloseConnection()
        {
            base.CloseConnection();
        }

        [TestMethod]
        public void Binary_Equal_ValueConstants_Void()
        {
            CheckCount<EntityObject>((e) => e.Name == "test", 0);
        }

        [TestMethod]
        public void Binary_Equal_ValueConstants_Exists()
        {
            CheckCount<EntityObject>((e) => e.Name == "name4", 1);
        }

        [TestMethod]
        public void Binary_Equal_ValueByVariable()
        {
            var nameValue = "name3";
            CheckCount<EntityObject>((e) => e.Name == nameValue, 1);

        }
    }
}

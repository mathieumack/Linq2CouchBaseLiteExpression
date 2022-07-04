using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToExpression.SimpleQueries
{
    [TestClass]
    public class BinaryEqualStringUnitTests : BaseUnitTestToExpression
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

        [TestMethod]
        public void Binary_Equal_ValueConstants_Void_Invert()
        {
            CheckCount<EntityObject>((e) => "test" == e.Name, 0);
        }

        [TestMethod]
        public void Binary_Equal_ValueConstants_Exists_Invert()
        {
            CheckCount<EntityObject>((e) => "name4" == e.Name, 1);
        }

        [TestMethod]
        public void Binary_Equal_ValueByVariable_Invert()
        {
            var nameValue = "name3";
            CheckCount<EntityObject>((e) => nameValue == e.Name, 1);
        }
    }
}

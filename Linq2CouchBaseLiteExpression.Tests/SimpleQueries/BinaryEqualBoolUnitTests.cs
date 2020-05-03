using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests
{
    [TestClass]
    public class BinaryEqualBoolUnitTests : BaseUnitTest
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
        public void Binary_Boolean_ValueConstants()
        {
            CheckCount<EntityObject>((e) => e.IsHuman == true, 3);
        }

        [TestMethod]
        public void Binary_Boolean_ValueByVariable()
        {
            var nameValue = true;
            CheckCount<EntityObject>((e) => e.IsHuman == nameValue, 3);
        }
    }
}

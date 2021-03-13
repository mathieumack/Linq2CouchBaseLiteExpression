using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.SimpleQueries
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

        [TestMethod]
        public void Binary_Boolean_ValueConstants_Invert()
        {
            CheckCount<EntityObject>((e) => true == e.IsHuman, 3);
        }

        [TestMethod]
        public void Binary_Boolean_ValueByVariable_Invert()
        {
            var nameValue = true;
            CheckCount<EntityObject>((e) => nameValue == e.IsHuman, 3);
        }
    }
}

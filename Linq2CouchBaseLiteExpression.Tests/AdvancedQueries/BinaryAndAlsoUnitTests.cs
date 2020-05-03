using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.AdvancedQueries
{
    [TestClass]
    public class BinaryAndAlsoUnitTests : BaseUnitTest
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
        public void Binary_Not_Expression_Exists()
        {
            var nameValue = "name2";
            CheckCount<EntityObject>((e) => e.Name == nameValue && e.IsHuman == true, 1);
        }

        [TestMethod]
        public void Binary_Not_Expression_Void()
        {
            var nameValue = "name2";
            CheckCount<EntityObject>((e) => e.Name == nameValue && e.IsHuman == false, 0);
        }
    }
}

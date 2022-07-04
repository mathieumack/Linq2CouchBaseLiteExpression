using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToSqlQuery.AdvancedQueries
{
    [TestClass]
    public class BinaryAndAlsoUnitTests : BaseUnitTestToSqlQuery
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

        [TestMethod]
        public void Binary_Not_Expression_Exists_Invert()
        {
            var nameValue = "name2";
            CheckCount<EntityObject>((e) => nameValue == e.Name && e.IsHuman, 1);
        }

        [TestMethod]
        public void Binary_Not_Expression_Void_Invert()
        {
            var nameValue = "name2";
            CheckCount<EntityObject>((e) => e.Name == nameValue && false == e.IsHuman, 0);
        }
    }
}

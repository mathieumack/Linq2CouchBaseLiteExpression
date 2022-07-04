using System.Linq;
using Couchbase.Lite.Query;
using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToSqlQuery.AdvancedQueries
{
    [TestClass]
    public class BinaryNotUnitTests : BaseUnitTestToSqlQuery
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
        public void Binary_Is_Expression()
        {
            CheckCount<EntityObject>((e) => (e.IsHuman), 3);
        }

        [TestMethod]
        public void Binary_Not_Expression()
        {
            CheckCount<EntityObject>((e) => !(e.IsHuman), 2);
        }
    }
}

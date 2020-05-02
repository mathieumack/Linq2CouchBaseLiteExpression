using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests
{
    [TestClass]
    public class BinaryAndAlsoUnitTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Couchbase.Lite.Support.NetDesktop.Activate();
        }

        [TestMethod]
        public void Binary_AndAlso_Expression()
        {
            var queryOptions = new QueryObject<EntityObject>();

            // Add filters :
            var nameValue = "Mack";
            queryOptions.FilterQuery = (e) => e.Name == nameValue && e.IsAMan == false;

            var resultFilter = Linq2CouchbaseLiteExpression.GenerateFromExpression(queryOptions.FilterQuery);

            // Check filters :

        }
    }
}

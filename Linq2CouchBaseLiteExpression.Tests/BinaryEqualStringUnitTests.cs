using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests
{
    [TestClass]
    public class BinaryEqualStringUnitTests
    {
        [TestMethod]
        public void Binary_Equal_ValueConstants()
        {
            var queryOptions = new QueryObject<EntityObject>();

            // Add filters :
            queryOptions.FilterQuery = (e) => e.Name == "test";

            var resultFilter = Linq2CouchbaseLiteExpression.GenerateFromExpression(queryOptions.FilterQuery);

            // Check filters :

        }

        [TestMethod]
        public void Binary_Equal_ValueByVariable()
        {
            var queryOptions = new QueryObject<EntityObject>();

            // Add filters :
            var nameValue = "test";
            queryOptions.FilterQuery = (e) => e.Name == nameValue;

            var resultFilter = Linq2CouchbaseLiteExpression.GenerateFromExpression(queryOptions.FilterQuery);

            // Check filters :

        }
    }
}

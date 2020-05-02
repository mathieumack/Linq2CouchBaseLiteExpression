using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests
{
    [TestClass]
    public class BinaryEqualBoolUnitTests
    {
        [TestMethod]
        public void Binary_Boolean_ValueConstants()
        {
            var queryOptions = new QueryObject<EntityObject>();

            // Add filters :
            queryOptions.FilterQuery = (e) => e.IsAMan == true;

            var resultFilter = Linq2CouchbaseLiteExpression.GenerateFromExpression(queryOptions.FilterQuery);

            // Check filters :

        }

        [TestMethod]
        public void Binary_Boolean_ValueByVariable()
        {
            var queryOptions = new QueryObject<EntityObject>();

            // Add filters :
            var nameValue = true;
            queryOptions.FilterQuery = (e) => e.IsAMan == nameValue;

            var resultFilter = Linq2CouchbaseLiteExpression.GenerateFromExpression(queryOptions.FilterQuery);

            // Check filters :

        }
    }
}

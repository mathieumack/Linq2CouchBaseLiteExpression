using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Linq2CouchBaseLiteExpression.Tests.Ordering
{
    [TestClass]
    public class OrderingByNameTest : BaseUnitTest
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
        public void OrderByName_Ascending()
        {
            var results = GetAllAndSort<EntityObject, string>(e => e.Name);
            for(int i = 1; i <=5; i++)
            {
                Assert.AreEqual($"name{i}", results[i - 1]);
            }
        }

        [TestMethod]
        public void OrderByName_Descending()
        {
            var results = GetAllAndSortDescending<EntityObject, string>(e => e.Name);
            for (int i = 1; i <= 5; i++)
            {
                Assert.AreEqual($"name{6-i}", results[i - 1]);
            }
        }

        [TestMethod]
        public void OrderByAgeOnSubField_Ascending()
        {
            var results = GetAllAndSortInt<EntityObject, int>(e => e.Age);

            var values = new List<int>() { 7, 8, 8, 9, 12 };
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(values[i], results[i]);
            }
        }

        [TestMethod]
        public void OrderByAgeOnSubField_Descending()
        {
            var results = GetAllAndSortDescendingInt<EntityObject, int>(e => e.Age);
            
            var values = new List<int>() { 7, 8, 8, 9, 12 };
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(values[4-i], results[i]);
            }
        }
    }
}

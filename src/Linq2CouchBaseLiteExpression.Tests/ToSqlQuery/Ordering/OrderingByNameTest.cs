using Linq2CouchBaseLiteExpression.Tests.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq2CouchBaseLiteExpression.Tests.ToSqlQuery.Ordering
{
    [TestClass]
    public class OrderingByNameTest : BaseUnitTestToSqlQuery
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
            EntityObject entity;
            var results = GetAllAndSort((EntityObject e) => e.Name);
            for(int i = 1; i <=5; i++)
            {
                Assert.AreEqual($"name{i}", results[i - 1]);
            }
        }

        [TestMethod]
        public void OrderByName_Descending()
        {
            var results = GetAllAndSortDescending((EntityObject e) => e.Name);
            for (int i = 1; i <= 5; i++)
            {
                Assert.AreEqual($"name{6-i}", results[i - 1]);
            }
        }

        [TestMethod]
        public void OrderByAge_Ascending()
        {
            var results = GetAllAndSortInt((EntityObject e) => e.Age);

            var values = new List<int>() { 7, 8, 8, 9, 12 };
            
            Enumerable.OrderBy(values, e => e);

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(values[i], results[i]);
            }
        }

        [TestMethod]
        public void OrderByAge_Descending()
        {
            var results = GetAllAndSortDescendingInt((EntityObject e) => e.Age);
            
            var values = new List<int>() { 7, 8, 8, 9, 12 };
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(values[4-i], results[i]);
            }
        }
    }
}

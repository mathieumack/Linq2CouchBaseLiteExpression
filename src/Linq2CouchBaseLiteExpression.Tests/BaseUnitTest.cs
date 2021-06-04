using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Couchbase.Lite;
using Couchbase.Lite.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests
{
    public abstract class BaseUnitTest
    {
        protected Database db;

        /// <summary>
        /// Create a local database and insert some elements.
        /// The database is the same for all unit tests as we just tru queries :
        /// </summary>
        public virtual void TestInitialize()
        {
            Couchbase.Lite.Support.NetDesktop.Activate();

            db = new Database(Guid.NewGuid().ToString());

            // Empty database, so we will create 6 sample documents :
            CreateDocument("name1", "firstName1", 8, true, null, DateTimeOffset.UtcNow);
            CreateDocument("name2", null, 8, true, null, DateTimeOffset.UtcNow.AddDays(-1));
            CreateDocument("name3", "firstName3", 12, true, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
            CreateDocument("name4", "", 9, false, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(-1));
            CreateDocument("name5", "firstName5", 7, false, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
        }

        public virtual void CloseConnection()
        {
            db.Delete();
            db.Dispose();
        }

        /// <summary>
        /// Create a new document in the local storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="surName"></param>
        /// <param name="age"></param>
        /// <param name="isHuman"></param>
        /// <param name="birthday"></param>
        /// <param name="createdAt"></param>
        private void CreateDocument(string name, string surName, int age, bool isHuman, DateTimeOffset? birthday, DateTimeOffset createdAt)
        {
            using (var newDocument = new MutableDocument())
            {
                newDocument.SetString("Name", name)
                            .SetString("SurName", surName)
                            .SetInt("Age", age)
                            .SetInt("SubObject.Value", age)
                            .SetInt("SubObject.SubSubObject.Value", age)
                            .SetBoolean("IsHuman", isHuman)
                            .SetDate("CreatedAt", createdAt);

                if (birthday.HasValue)
                    newDocument.SetDate("BirthDay", birthday.Value);
                else
                    newDocument.SetValue("BirthDay", null);

                db.Save(newDocument);
            }
        }

        /// <summary>
        /// Execute a query, count results and call an assertion to check that results count is equal to expected results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterExpression"></param>
        /// <param name="expectedCount"></param>
        protected void CheckCount<T>(Expression<Func<T, bool>> filterExpression, int expectedCount) where T : class
        {
            var resultFilter = Linq2CouchbaseLiteQueryExpression.GenerateFromExpression(filterExpression);

            // Check filters :
            using (var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID))
                                            .From(DataSource.Database(db))
                                            .Where(resultFilter))
            {
                var count = query.Execute().Count();
                Assert.AreEqual(expectedCount, count);
            }
        }

        /// <summary>
        /// Execute a query, count results and call an assertion to check that results count is equal to expected results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderByExpression"></param>
        protected List<string> GetAllAndSort<TSource, TKey>(Expression<Func<TSource, TKey>> orderByExpression)
        {
            var resultFilter = Linq2CouchbaseLiteOrderingExpression.GenerateFromExpression(orderByExpression, true);

            // Check filters :
            using (var query = QueryBuilder.Select(SelectResult.Property("Name"))
                                            .From(DataSource.Database(db))
                                            .OrderBy(resultFilter))
            {
                return query.Execute()
                        .Select(row => row.GetString("Name"))
                        .ToList();
            }
        }

        /// <summary>
        /// Execute a query, count results and call an assertion to check that results count is equal to expected results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderByExpression"></param>
        protected List<string> GetAllAndSortDescending<TSource, TKey>(Expression<Func<TSource, TKey>> orderByExpression)
        {
            var resultFilter = Linq2CouchbaseLiteOrderingExpression.GenerateFromExpression(orderByExpression, false);

            // Check filters :
            using (var query = QueryBuilder.Select(SelectResult.Property("Name"))
                                            .From(DataSource.Database(db))
                                            .OrderBy(resultFilter))
            {
                return query.Execute()
                        .Select(row => row.GetString("Name"))
                        .ToList();
            }
        }

        /// <summary>
        /// Execute a query, count results and call an assertion to check that results count is equal to expected results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderByExpression"></param>
        protected List<int> GetAllAndSortInt<TSource, TKey>(Expression<Func<TSource, TKey>> orderByExpression)
        {
            var resultFilter = Linq2CouchbaseLiteOrderingExpression.GenerateFromExpression(orderByExpression, true);

            // Check filters :
            using (var query = QueryBuilder.Select(SelectResult.Property("Age"))
                                            .From(DataSource.Database(db))
                                            .OrderBy(resultFilter))
            {
                return query.Execute()
                        .Select(row => row.GetInt("Age"))
                        .ToList();
            }
        }

        /// <summary>
        /// Execute a query, count results and call an assertion to check that results count is equal to expected results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orderByExpression"></param>
        protected List<int> GetAllAndSortDescendingInt<TSource, TKey>(Expression<Func<TSource, TKey>> orderByExpression)
        {
            var resultFilter = Linq2CouchbaseLiteOrderingExpression.GenerateFromExpression(orderByExpression, false);

            // Check filters :
            using (var query = QueryBuilder.Select(SelectResult.Property("Age"))
                                            .From(DataSource.Database(db))
                                            .OrderBy(resultFilter))
            {
                return query.Execute()
                        .Select(row => row.GetInt("Age"))
                        .ToList();
            }
        }
    }
}

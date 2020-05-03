using System;
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
            CreateDocument("name1", true, DateTimeOffset.UtcNow);
            CreateDocument("name2", true, DateTimeOffset.UtcNow.AddDays(-1));
            CreateDocument("name3", true, DateTimeOffset.UtcNow);
            CreateDocument("name4", false, DateTimeOffset.UtcNow.AddDays(-1));
            CreateDocument("name5", false, DateTimeOffset.UtcNow);
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
        /// <param name="isHuman"></param>
        /// <param name="createdAt"></param>
        private void CreateDocument(string name, bool isHuman, DateTimeOffset createdAt)
        {
            using (var newDocument = new MutableDocument())
            {
                newDocument.SetString("Name", name)
                            .SetBoolean("IsHuman", isHuman)
                            .SetDate("CreatedAt", createdAt);

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
            var resultFilter = Linq2CouchbaseLiteExpression.GenerateFromExpression(filterExpression);

            // Check filters :
            using (var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID))
                                            .From(DataSource.Database(db))
                                            .Where(resultFilter))
            {
                var count = query.Execute().Count();
                Assert.AreEqual(expectedCount, count);
            }
        }
    }
}

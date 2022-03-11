using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Couchbase.Lite.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToExpression
{
    public abstract class BaseUnitTestToExpression : BaseUnitTest
    {
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
            var resultFilter = Linq2CouchbaseLiteOrderingExpression.GenerateOrderByFromExpression(orderByExpression, true);

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
            var resultFilter = Linq2CouchbaseLiteOrderingExpression.GenerateOrderByFromExpression(orderByExpression, false);

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
            var resultFilter = Linq2CouchbaseLiteOrderingExpression.GenerateOrderByFromExpression(orderByExpression, true);

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
            var resultFilter = Linq2CouchbaseLiteOrderingExpression.GenerateOrderByFromExpression(orderByExpression, false);

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

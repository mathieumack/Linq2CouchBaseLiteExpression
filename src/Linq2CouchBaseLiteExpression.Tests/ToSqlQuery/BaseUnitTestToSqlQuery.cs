using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Couchbase.Lite.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Linq2CouchBaseLiteExpression.Tests.ToSqlQuery
{
    public abstract class BaseUnitTestToSqlQuery : BaseUnitTest
    {
        /// <summary>
        /// Execute a query, count results and call an assertion to check that results count is equal to expected results
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterExpression"></param>
        /// <param name="expectedCount"></param>
        protected void CheckCount<T>(Expression<Func<T, bool>> filterExpression, int expectedCount) where T : class
        {
            var sqlQuery = Linq2CouchbaseLiteSqlQueryExpression.GenerateFromExpression(filterExpression);

            // Check filters :
            using (var query = db.CreateQuery($"SELECT ID FROM _ {sqlQuery}"))
            {
                var count = query.Execute().Count();
                Assert.AreEqual(expectedCount, count);
            }
        }

        ///// <summary>
        ///// Execute a query, count results and call an assertion to check that results count is equal to expected results
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="orderByExpression"></param>
        //protected List<string> GetAllAndSort<TSource, TKey>(Expression<Func<TSource, TKey>> orderByExpression)
        //{
        //    var sqlQuery = Linq2CouchbaseLiteSqlQueryExpression.GenerateOrderByFromExpression(orderByExpression, true);

        //    // Check filters :
        //    using (var query = db.CreateQuery(sqlQuery))
        //    {
        //        return query.Execute()
        //                .Select(row => row.GetString("Name"))
        //                .ToList();
        //    }
        //}

        ///// <summary>
        ///// Execute a query, count results and call an assertion to check that results count is equal to expected results
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="orderByExpression"></param>
        //protected List<string> GetAllAndSortDescending<TSource, TKey>(Expression<Func<TSource, TKey>> orderByExpression)
        //{
        //    var sqlQuery = Linq2CouchbaseLiteSqlQueryExpression.GenerateOrderByFromExpression(orderByExpression, false);

        //    // Check filters :
        //    using (var query = db.CreateQuery(sqlQuery))
        //    {
        //        return query.Execute()
        //                .Select(row => row.GetString("Name"))
        //                .ToList();
        //    }
        //}

        ///// <summary>
        ///// Execute a query, count results and call an assertion to check that results count is equal to expected results
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="orderByExpression"></param>
        //protected List<int> GetAllAndSortInt<TSource, TKey>(Expression<Func<TSource, TKey>> orderByExpression)
        //{
        //    var sqlQuery = Linq2CouchbaseLiteSqlQueryExpression.GenerateOrderByFromExpression(orderByExpression, true);

        //    // Check filters :
        //    using (var query = db.CreateQuery(sqlQuery))
        //    {
        //        return query.Execute()
        //                .Select(row => row.GetInt("Age"))
        //                .ToList();
        //    }
        //}

        ///// <summary>
        ///// Execute a query, count results and call an assertion to check that results count is equal to expected results
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="orderByExpression"></param>
        //protected List<int> GetAllAndSortDescendingInt<TSource, TKey>(Expression<Func<TSource, TKey>> orderByExpression)
        //{
        //    var sqlQuery = Linq2CouchbaseLiteSqlQueryExpression.GenerateOrderByFromExpression(orderByExpression, false);

        //    // Check filters :
        //    using (var query = db.CreateQuery(sqlQuery))
        //    {
        //        return query.Execute()
        //                .Select(row => row.GetInt("Age"))
        //                .ToList();
        //    }
        //}
    }
}

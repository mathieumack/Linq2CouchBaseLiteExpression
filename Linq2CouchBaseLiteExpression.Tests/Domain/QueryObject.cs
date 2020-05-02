using System;
using System.Linq.Expressions;

namespace Linq2CouchBaseLiteExpression.Tests.Domain
{
    public class QueryObject<T> where T : class
    {
        /// <summary>
        /// Gets or sets an lambda expression filters results before getting data from database.
        /// the row is skipped.
        /// </summary>
        public Expression<Func<T, bool>> FilterQuery { get; set; }
    }
}

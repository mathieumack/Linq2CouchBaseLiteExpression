using Couchbase.Lite.Query;
using System;
using System.Linq.Expressions;

namespace Linq2CouchBaseLiteExpression
{
    public static class Linq2CouchbaseLiteOrderingExpression
    {
        /// <summary>
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="ascending">True = ascending, False = Descending</param>
        /// <returns></returns>
        public static IOrdering GenerateFromExpression<T, TResult>(Expression<Func<T, TResult>> expression,
                                                                    bool ascending)
        {
            return GenerateFromExpression(expression.Body, ascending);
        }

        #region Global type expression :

        /// <summary>
        /// Transform an <see cref="Expression" /> to an <see cref="Couchbase.Lite.Query.IExpression"/> object
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static IOrdering GenerateFromExpression(System.Linq.Expressions.Expression expression,
                                                            bool ascending)
        {
            var memberValue = GetValueFromExpression(expression, null);
            var sort = Ordering.Property((string)memberValue);
            if(ascending)
                return sort.Ascending();
            else
                return sort.Descending();
        }

        #endregion


        #region Generic methods

        private static object GetValueFromExpression(System.Linq.Expressions.Expression expression, string memberName)
        {
            if (expression is MemberExpression)
            {
                var exp = expression as MemberExpression;
                if (exp.Expression is ParameterExpression)
                    return exp.Member.Name;
                else
                {
                    // Current limitation : You can only filter on level one for fields
                    var currentName = GetValueFromExpression(exp.Expression, exp.Member.Name);
                    return currentName;
                }
            }

            throw new NotSupportedException("expression of type type (" + expression.NodeType.ToString() + ") are not supported.");
        }

        #endregion
    }
}

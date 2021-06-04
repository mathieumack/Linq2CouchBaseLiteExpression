using Couchbase.Lite.Query;
using System;
using System.Collections.Generic;
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
        public static IOrdering GenerateOrderByFromLambda(System.Linq.Expressions.Expression expression,
                                                                        bool ascending)
        {
            return GenerateFromExpression(expression, ascending);
        }

        /// <summary>
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="ascending">True = ascending, False = Descending</param>
        /// <returns></returns>
        public static IOrdering GenerateOrderByFromExpression<TSource, TKey>(System.Linq.Expressions.Expression<Func<TSource, TKey>> expression,
                                                                        bool ascending)
        {
            return GenerateFromExpression(expression, ascending);
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
            var memberValue = GetValueFromExpression(expression);
            var sort = Ordering.Property((string)memberValue);
            if(ascending)
                return sort.Ascending();
            else
                return sort.Descending();
        }

        #endregion


        #region Generic methods

        private static object GetValueFromExpression(System.Linq.Expressions.Expression expression)
        {
            if (expression is MemberExpression)
            {
                var exp = expression as MemberExpression;
                if (exp.Expression is ParameterExpression)
                    return exp.Member.Name;
                else
                {
                    // Current limitation : You can only filter on level one for fields
                    var currentName = GetValueFromExpression(exp.Expression);
                    return currentName;
                }
            }
            else if (expression is LambdaExpression)
            {
                var exp = expression as LambdaExpression;
                return GetValueFromExpression(exp.Body);
            }

            throw new NotSupportedException("expression of type type (" + expression.NodeType.ToString() + ") are not supported.");
        }

        #endregion
    }
}

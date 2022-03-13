using Couchbase.Lite.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Linq2CouchBaseLiteExpression
{
    public static class Linq2CouchbaseLiteOrderingSqlQueryExpression
    {
        /// <summary>
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="ascending">True = ascending, False = Descending</param>
        /// <returns></returns>
        public static string GenerateOrderByFromExpression<TSource, TKey>(Expression<Func<TSource, TKey>> expression,
                                                                        bool ascending)
        {
            var sqlQuery = GenerateFromExpression(expression.Body, ascending);
            return $"ORDER BY {sqlQuery}";
        }

        #region Global type expression :

        /// <summary>
        /// Transform an <see cref="Expression" /> to an <see cref="Couchbase.Lite.Query.IExpression"/> object
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string GenerateFromExpression(System.Linq.Expressions.Expression expression,
                                                            bool ascending)
        {
            var memberValue = GetValueFromExpression(expression);
            if(ascending)
                return $"{memberValue} ASC";
            else
                return $"{memberValue} DESC";
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

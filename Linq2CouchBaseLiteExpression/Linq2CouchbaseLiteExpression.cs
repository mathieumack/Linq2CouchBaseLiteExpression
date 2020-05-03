using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Linq2CouchBaseLiteExpression
{
    public static class Linq2CouchbaseLiteExpression
    {
        /// <summary>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Couchbase.Lite.Query.IExpression GenerateFromExpression<T>(Expression<Func<T, bool>> expression)
        {
            return GenerateFromExpression(expression.Body);
        }

        #region Global type expression :

        /// <summary>
        /// Transform an <see cref="Expression" /> to an <see cref="Couchbase.Lite.Query.IExpression"/> object
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static Couchbase.Lite.Query.IExpression GenerateFromExpression(Expression expression)
        {
            if (expression is BinaryExpression)
                return GenerateFromExpression(expression as BinaryExpression);
            if (expression is MethodCallExpression)
                return GenerateFromExpression(expression as MethodCallExpression);
            if (expression is UnaryExpression)
                return GenerateFromExpression(expression as UnaryExpression);
            return null;
        }

        /// <summary>
        /// Transform an <see cref="BinaryExpression"/>.
        /// Here we manage expression type : <see cref="ExpressionType.Equal"/>, <see cref="ExpressionType.Not"/> or <see cref="ExpressionType.AndAlso"/>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static Couchbase.Lite.Query.IExpression GenerateFromExpression(UnaryExpression expression)
        {
            switch (expression.NodeType)
            {
                //case ExpressionType.Equal:
                //    // Left must be the member
                //    // Right must be the value
                //    return Couchbase.Lite.Query.Expression.Property(GetValueFromExpression(expression., null).ToString())
                //                .EqualTo(Couchbase.Lite.Query.Expression.Value(GetValueFromExpression(expression.Right, null)));
                case ExpressionType.Not:
                    return Couchbase.Lite.Query.Expression.Property(GetValueFromExpression(expression.Operand, null).ToString())
                                .EqualTo(Couchbase.Lite.Query.Expression.Boolean(true));
                //case ExpressionType.AndAlso:
                //    var leftExpression = GenerateFromExpression(expression.Left);
                //    var rightExpression = GenerateFromExpression(expression.Right);
                //    return leftExpression.And(rightExpression);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Transform an <see cref="BinaryExpression"/>.
        /// Here we manage expression type : <see cref="ExpressionType.Equal"/>, <see cref="ExpressionType.Not"/> or <see cref="ExpressionType.AndAlso"/>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static Couchbase.Lite.Query.IExpression GenerateFromExpression(BinaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    // Left must be the member
                    // Right must be the value
                    return Couchbase.Lite.Query.Expression.Property(GetValueFromExpression(expression.Left, null).ToString())
                                .EqualTo(Couchbase.Lite.Query.Expression.Value(GetValueFromExpression(expression.Right, null)));
                case ExpressionType.Not:
                    return Couchbase.Lite.Query.Expression.Property(GetValueFromExpression(expression.Left, null).ToString())
                                .NotEqualTo(Couchbase.Lite.Query.Expression.Value(GetValueFromExpression(expression.Right, null)));
                case ExpressionType.AndAlso:
                    var leftExpression = GenerateFromExpression(expression.Left);
                    var rightExpression = GenerateFromExpression(expression.Right);
                    return leftExpression.And(rightExpression);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Transform an <see cref="BinaryExpression"/>. No <see cref="ExpressionType"/> supported yet.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static Couchbase.Lite.Query.IExpression GenerateFromExpression(MethodCallExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    return null;
                default:
                    return null;
            }
        }

        #endregion

        #region Generic methods

        private static object GetValueFromExpression(Expression expression, string memberName)
        {
            if (expression is MemberExpression)
            {
                var exp = expression as MemberExpression;
                if (exp.Expression is ParameterExpression)
                    return exp.Member.Name;
                else
                    return GetValueFromExpression(exp.Expression, exp.Member.Name);
            }
            else if (expression is ConstantExpression)
            {
                var constExpression = (expression as ConstantExpression);
                Type t = constExpression.Value.GetType();
                if (t.Equals(typeof(bool)) || t.Equals(typeof(int)) || t.Equals(typeof(string)))
                    return constExpression.Value;
                return t.InvokeMember(memberName, BindingFlags.GetField, null, constExpression.Value, null);
            }
            return null;
        }

        #endregion
    }
}

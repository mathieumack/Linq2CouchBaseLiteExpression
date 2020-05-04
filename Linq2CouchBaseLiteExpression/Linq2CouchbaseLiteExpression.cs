using System;
using System.Collections.Generic;
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
            else if (expression is MethodCallExpression)
                return GenerateFromExpression(expression as MethodCallExpression);
            else if (expression is UnaryExpression)
                return GenerateFromExpression(expression as UnaryExpression);
            else if (expression is MemberExpression)
            {
                var memberValue = GetValueFromExpression(expression, null);
                return Couchbase.Lite.Query.Expression.Property(memberValue.ToString())
                            .EqualTo(Couchbase.Lite.Query.Expression.Boolean(true));
            }
            
            throw new NotSupportedException("expression of type (" + expression.GetType().ToString() + ") are not supported.");
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
                case ExpressionType.Not:
                {
                    var subExpression = GenerateFromExpression(expression.Operand);
                    return Couchbase.Lite.Query.Expression.Not(subExpression);
                }
                default:
                    throw new NotSupportedException("expression node type (" + expression.NodeType.ToString() + ") are not supported.");
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
                    var leftExpressionEqual = GetValueFromExpression(expression.Left, null);
                    var rightExpressionEqual = GetValueFromExpression(expression.Right, null);
                    return Couchbase.Lite.Query.Expression.Property(leftExpressionEqual.ToString())
                                .EqualTo(Couchbase.Lite.Query.Expression.Value(rightExpressionEqual));
                case ExpressionType.Not:
                    return Couchbase.Lite.Query.Expression.Property(GetValueFromExpression(expression.Left, null).ToString())
                                .NotEqualTo(Couchbase.Lite.Query.Expression.Value(GetValueFromExpression(expression.Right, null)));
                case ExpressionType.AndAlso:
                    var leftExpressionAnd = GenerateFromExpression(expression.Left);
                    var rightExpressionAnd = GenerateFromExpression(expression.Right);
                    return leftExpressionAnd.And(rightExpressionAnd);
                case ExpressionType.OrElse:
                    var leftExpressionOr = GenerateFromExpression(expression.Left);
                    var rightExpressionOr = GenerateFromExpression(expression.Right);
                    return leftExpressionOr.Or(rightExpressionOr);
                case ExpressionType.GreaterThan:
                    var leftExpressionGreaterThan = GetValueFromExpression(expression.Left, null);
                    var rightExpressionGreaterThan = GetValueFromExpression(expression.Right, null);
                    return Couchbase.Lite.Query.Expression.Property(leftExpressionGreaterThan.ToString())
                                .GreaterThan(Couchbase.Lite.Query.Expression.Value(rightExpressionGreaterThan));
                case ExpressionType.GreaterThanOrEqual:
                    var leftExpressionGreaterThanOrEqual = GetValueFromExpression(expression.Left, null);
                    var rightExpressionGreaterThanOrEqual = GetValueFromExpression(expression.Right, null);
                    return Couchbase.Lite.Query.Expression.Property(leftExpressionGreaterThanOrEqual.ToString())
                                .GreaterThanOrEqualTo(Couchbase.Lite.Query.Expression.Value(rightExpressionGreaterThanOrEqual));
                case ExpressionType.LessThan:
                    var leftExpressionLessThan = GetValueFromExpression(expression.Left, null);
                    var rightExpressionLessThan = GetValueFromExpression(expression.Right, null);
                    return Couchbase.Lite.Query.Expression.Property(leftExpressionLessThan.ToString())
                                .LessThan(Couchbase.Lite.Query.Expression.Value(rightExpressionLessThan));
                case ExpressionType.LessThanOrEqual:
                    var leftExpressionLessThanOrEqual = GetValueFromExpression(expression.Left, null);
                    var rightExpressionLessThanOrEqual = GetValueFromExpression(expression.Right, null);
                    return Couchbase.Lite.Query.Expression.Property(leftExpressionLessThanOrEqual.ToString())
                                .LessThanOrEqualTo(Couchbase.Lite.Query.Expression.Value(rightExpressionLessThanOrEqual));
                default:
                    throw new NotSupportedException("expression node type (" + expression.NodeType.ToString() + ") are not supported.");
            }
        }

        /// <summary>
        /// Transform an <see cref="BinaryExpression"/>. No <see cref="ExpressionType"/> supported yet.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static Couchbase.Lite.Query.IExpression GenerateFromExpression(MethodCallExpression expression)
        {
            if(expression.Method.Name.Equals("Equals"))
            {
                return Couchbase.Lite.Query.Expression.Property(GetValueFromExpression(expression.Object, null).ToString())
                                   .EqualTo(Couchbase.Lite.Query.Expression.Value(GetValueFromExpression(expression.Arguments[0], null)));
            }
            else if (expression.Method.Name.Equals("IsNullOrEmpty"))
            {
                var fieldName = expression.Arguments[0];
                return Couchbase.Lite.Query.Expression.Property(GetValueFromExpression(fieldName, null).ToString())
                                   .EqualTo(Couchbase.Lite.Query.Expression.Value(null))
                                   .Or(
                        Couchbase.Lite.Query.Expression.Property(GetValueFromExpression(fieldName, null).ToString())
                                   .EqualTo(Couchbase.Lite.Query.Expression.Value(string.Empty)));
            }
            else if (expression.Arguments.Count == 0)
            {
                // The method return a value : can only be called on public and static methods
                var method = expression.Method;
                object result = method.Invoke(null, null);
                return Couchbase.Lite.Query.Expression.Value(result);
            }

            throw new NotSupportedException("expression of type (" + expression.NodeType.ToString() + ") are not supported : " + expression.Method.Name);
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
            else if (expression is MethodCallExpression)
            {
                return GenerateFromExpression(expression as MethodCallExpression);
            }

            throw new NotSupportedException("expression of type type (" + expression.NodeType.ToString() + ") are not supported.");
        }

        #endregion
    }
}

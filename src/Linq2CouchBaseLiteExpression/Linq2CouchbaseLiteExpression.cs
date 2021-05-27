using System;
using System.Collections;
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
                if((expression as MemberExpression).Member.Name.Equals("HasValue"))
                    return Couchbase.Lite.Query.Expression.Property(memberValue.ToString())
                                .NotEqualTo(Couchbase.Lite.Query.Expression.Value(null));
                else 
                    return Couchbase.Lite.Query.Expression.Property(memberValue.ToString())
                            .EqualTo(Couchbase.Lite.Query.Expression.Boolean(true));
            }
            else if(expression is ConditionalExpression)
            {
                var testExpression = expression as ConditionalExpression;

                var result = (bool)(Expression.Lambda(testExpression.Test).Compile().DynamicInvoke());
                if (result)
                    return GenerateFromExpression(testExpression.IfTrue);
                else
                    return GenerateFromExpression(testExpression.IfFalse);
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
            if (IsCompareFieldNodeType(expression.NodeType))
            {
                var leftExpression = GetValueFromExpression(expression.Left, null);
                var rightExpression = GetValueFromExpression(expression.Right, null);
                bool isFieldAtLeft = IsFieldExpression(expression.Left);

                if (!isFieldAtLeft)
                {
                    // We invert expressions :
                    var temp = leftExpression;
                    leftExpression = rightExpression;
                    rightExpression = temp;
                }

                switch (expression.NodeType)
                {
                    case ExpressionType.Equal:
                        return Couchbase.Lite.Query.Expression.Property(leftExpression.ToString())
                                .EqualTo(Couchbase.Lite.Query.Expression.Value(rightExpression));
                    case ExpressionType.NotEqual:
                    case ExpressionType.Not:
                        return Couchbase.Lite.Query.Expression.Property(leftExpression.ToString())
                                .NotEqualTo(Couchbase.Lite.Query.Expression.Value(rightExpression));
                    case ExpressionType.GreaterThan:
                        return Couchbase.Lite.Query.Expression.Property(leftExpression.ToString())
                                .GreaterThan(Couchbase.Lite.Query.Expression.Value(rightExpression));
                    case ExpressionType.GreaterThanOrEqual:
                        return Couchbase.Lite.Query.Expression.Property(leftExpression.ToString())
                                .GreaterThanOrEqualTo(Couchbase.Lite.Query.Expression.Value(rightExpression));
                    case ExpressionType.LessThan:
                        return Couchbase.Lite.Query.Expression.Property(leftExpression.ToString())
                                .LessThan(Couchbase.Lite.Query.Expression.Value(rightExpression));
                    case ExpressionType.LessThanOrEqual:
                        return Couchbase.Lite.Query.Expression.Property(leftExpression.ToString())
                                .LessThanOrEqualTo(Couchbase.Lite.Query.Expression.Value(rightExpression));
                    default:
                        throw new NotSupportedException("expression node type (" + expression.NodeType.ToString() + ") are not supported.");
                }
            }
            else
            {
                var leftExpression = GenerateFromExpression(expression.Left);
                var rightExpression = GenerateFromExpression(expression.Right);
                bool isFieldAtLeft = IsFieldExpression(expression.Left);

                if (!isFieldAtLeft)
                {
                    // We invert expressions :
                    var temp = leftExpression;
                    leftExpression = rightExpression;
                    rightExpression = temp;
                }

                switch (expression.NodeType)
                {
                    case ExpressionType.AndAlso:
                        return leftExpression.And(rightExpression);
                    case ExpressionType.OrElse:
                        return leftExpression.Or(rightExpression);
                    default:
                        throw new NotSupportedException("expression node type (" + expression.NodeType.ToString() + ") are not supported.");
                }
            }
        }

        private static bool IsCompareFieldNodeType(ExpressionType expressionType)
        {
            return expressionType == ExpressionType.Equal ||
                expressionType == ExpressionType.NotEqual ||
                expressionType == ExpressionType.Not ||
                expressionType == ExpressionType.GreaterThan ||
                expressionType == ExpressionType.GreaterThanOrEqual ||
                expressionType == ExpressionType.LessThan ||
                expressionType == ExpressionType.LessThanOrEqual;
        }

        /// <summary>
        /// Check if an expression corresponds to an member field
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static bool IsFieldExpression(Expression expression)
        {
            if (expression is MemberExpression)
            {
                var exp = expression as MemberExpression;
                if (exp.Expression is ParameterExpression)
                    return true;
                else
                    return IsFieldExpression(exp.Expression);
            }

            return false;
        }

        /// <summary>
        /// Transform an <see cref="BinaryExpression"/>. No <see cref="ExpressionType"/> supported yet.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static Couchbase.Lite.Query.IExpression GenerateFromExpression(MethodCallExpression expression)
        {
            if (expression.Method.Name.Equals("Equals"))
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
            else if (expression.Method.Name.Equals("Contains"))
            {
                var fieldName = expression.Arguments[0];
                var fieldPropertyName = GetValueFromExpression(fieldName, null)?.ToString();

                Couchbase.Lite.Query.IExpression sampleOr = null;

                var value = GetValueFromExpression(expression.Object, null);
                if (value is string)
                {
                    if(!string.IsNullOrWhiteSpace(fieldPropertyName))
                        return Couchbase.Lite.Query.Expression.Property(value.ToString())
                                   .Like(Couchbase.Lite.Query.Expression.String($"%{fieldPropertyName}%"));
                    else
                        // no elements in the source, so the test can not work
                        return Couchbase.Lite.Query.Expression.Boolean(false);
                }
                else
                {
                    var myList = value as IEnumerable;
                    if (!(myList is null))
                    {
                        foreach (var subValue in myList)
                        {
                            var currentLoopExpression = Couchbase.Lite.Query.Expression.Property(fieldPropertyName)
                                                            .EqualTo(Couchbase.Lite.Query.Expression.Value(subValue));
                            if (sampleOr is null)
                                sampleOr = currentLoopExpression;
                            else
                                sampleOr = sampleOr.Or(currentLoopExpression);
                        }

                        if (sampleOr != null) // At least one element available in the list
                            return sampleOr;
                        else
                            // no elements in the source, so the test can not work
                            return Couchbase.Lite.Query.Expression.Boolean(false);
                    }
                }
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
                var constExpression = expression as ConstantExpression;
                var t = constExpression?.Value?.GetType();

                if (t is null)
                {
                    return null;
                }
                else if (t.Equals(typeof(bool)) || t.Equals(typeof(int)) || t.Equals(typeof(string)))
                {
                    return constExpression.Value;
                }
                else
                {
                    return t.InvokeMember(memberName, BindingFlags.GetField, null, constExpression.Value, null);
                }
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

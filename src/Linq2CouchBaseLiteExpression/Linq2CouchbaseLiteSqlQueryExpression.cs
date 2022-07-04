using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Linq2CouchBaseLiteExpression
{
    public static class Linq2CouchbaseLiteSqlQueryExpression
    {
        /// <summary>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>SQL expression</returns>
        public static string GenerateSqlWhereExpression<T>(Expression<Func<T, bool>> expression)
        {
            var sqlQuery = GenerateFromExpression(expression.Body);
            return $"WHERE {sqlQuery}";
        }

        #region Global type expression :

        /// <summary>
        /// Transform an <see cref="Expression" /> to an <see cref="Couchbase.Lite.Query.IExpression"/> object
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string GenerateFromExpression(Expression expression)
        {
            if (expression is BinaryExpression)
                return GenerateFromExpression(expression as BinaryExpression);
            else if (expression is MethodCallExpression)
                return GenerateFromExpression(expression as MethodCallExpression).ToString();
            else if (expression is UnaryExpression)
                return GenerateFromExpression(expression as UnaryExpression);
            else if (expression is MemberExpression)
            {
                var memberValue = GetValueFromExpression(expression, null);
                if ((expression as MemberExpression).Member.Name.Equals("HasValue"))
                    return $"{memberValue} <> null";
                else
                    return $"{memberValue} == true";
            }
            else if (expression is ConditionalExpression)
            {
                var testExpression = expression as ConditionalExpression;

                var result = (bool)(Expression.Lambda(testExpression.Test).Compile().DynamicInvoke());
                if (result)
                    return GenerateFromExpression(testExpression.IfTrue);
                else
                    return GenerateFromExpression(testExpression.IfFalse);
            }
            else if (expression is ConstantExpression)
            {
                var memberValue = GetValueFromExpression(expression, null);
                return ConvertToSqlString(memberValue);
            }

            throw new NotSupportedException("expression of type (" + expression.GetType().ToString() + ") are not supported.");
        }
        
        private static string ConvertToSqlString(object memberValue)
        {
            if (memberValue is null)
                return "NULL";
            else if (memberValue is string)
                return $"\"{memberValue.ToString().Replace("\"", "\"\"")}\"";
            else
                return $"{memberValue}";
        }

        private static object ConvertToSqlObject(object memberValue)
        {
            if (memberValue is null)
                return "NULL";
            if (memberValue is string)
                return $"\"{memberValue.ToString().Replace("\"", "\"\"")}\"";
            else
                return memberValue;
        }

        /// <summary>
        /// Transform an <see cref="BinaryExpression"/>.
        /// Here we manage expression type : <see cref="ExpressionType.Equal"/>, <see cref="ExpressionType.Not"/> or <see cref="ExpressionType.AndAlso"/>
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static string GenerateFromExpression(UnaryExpression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Not:
                    {
                        var subExpression = GenerateFromExpression(expression.Operand);
                        return $"NOT ({subExpression})";
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
        private static string GenerateFromExpression(BinaryExpression expression)
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

                rightExpression = ConvertToSqlObject(rightExpression);

                switch (expression.NodeType)
                {
                    case ExpressionType.Equal:
                        return $"({leftExpression} == {rightExpression})";
                    case ExpressionType.NotEqual:
                    case ExpressionType.Not:
                        return $"({leftExpression} != {rightExpression})";
                    case ExpressionType.GreaterThan:
                        return $"({leftExpression} > {rightExpression})";
                    case ExpressionType.GreaterThanOrEqual:
                        return $"({leftExpression} >= {rightExpression})";
                    case ExpressionType.LessThan:
                        return $"({leftExpression} < {rightExpression})";
                    case ExpressionType.LessThanOrEqual:
                        return $"({leftExpression} <= {rightExpression})";
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
                        return $"(({leftExpression}) and ({rightExpression}))";
                    case ExpressionType.OrElse:
                        return $"(({leftExpression}) or ({rightExpression}))";
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
        private static object GenerateFromExpression(MethodCallExpression expression)
        {
            if (expression.Method.Name.Equals("Equals"))
            {
                var objectExpression = GetValueFromExpression(expression.Object, null);
                var argumentExpression = GetValueFromExpression(expression.Arguments[0], null);

                if (!IsFieldExpression(expression.Object))
                    objectExpression = ConvertToSqlObject(objectExpression);
                if (!IsFieldExpression(expression.Arguments[0]))
                    argumentExpression = ConvertToSqlObject(argumentExpression);

                return $"{objectExpression} == {argumentExpression}";
            }
            else if (expression.Method.Name.Equals("IsNullOrEmpty"))
            {
                var fieldName = expression.Arguments[0];
                var value = GetValueFromExpression(fieldName, null);
                return $"({value} == null) or ({value} == \"\")";
            }
            else if (expression.Method.Name.Equals("Contains"))
            {
                string currentFieldName;
                object value;

                if (IsFieldExpression(expression.Object))
                {
                    currentFieldName = GetValueFromExpression(expression.Object, null).ToString();
                    value = GetValueFromExpression(expression.Arguments[0], null);
                }
                else
                {
                    currentFieldName = GetValueFromExpression(expression.Arguments[0], null).ToString();
                    value = GetValueFromExpression(expression.Object, null);
                }

                if (value is IEnumerable && !(value is string))
                {
                    var myList = value as IEnumerable;
                    
                    var sqlconditions = new StringBuilder("");
                    int i = 0;
                    foreach (var subValue in myList)
                    {
                        var stringValue = ConvertToSqlString(subValue);
                        sqlconditions.Append($"{(i == 0 ? "" : " OR ")}({currentFieldName} == {stringValue})");
                        i++;
                    }

                    if (i > 0) // At least one element available in the list
                        return sqlconditions;
                    else
                        // no elements in the source, so the test can not work
                        return "false";
                }
                else
                {
                    var stringValue = ConvertToSqlString(value);
                    if (!string.IsNullOrWhiteSpace(stringValue))
                        return $"CONTAINS({currentFieldName}, {stringValue})";
                    else
                        // no elements in the source, so the test can not work
                        return "false";
                }
            }
            else if (expression.Arguments.Count == 0)
            {
                // The method return a value : can only be called on public and static methods
                var method = expression.Method;
                object result = method.Invoke(null, null);

                if (result is string)
                    return $"\"{result}\"";
                else
                    return result;
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

                object resultObject;
                if (t is null)
                {
                    resultObject = null;
                }
                else if (t.Equals(typeof(bool)) || t.Equals(typeof(int)) || t.Equals(typeof(string)))
                {
                    resultObject = constExpression.Value;
                }
                else
                {
                    resultObject = t.InvokeMember(memberName, BindingFlags.GetField, null, constExpression.Value, null);
                }

                return resultObject;
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

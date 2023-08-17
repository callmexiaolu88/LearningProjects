using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;


namespace expressionLearning
{
    public class CustomQueryContext
    {
        public static object Execute(Expression expression, Type elementType, bool IsEnumerable)
        {
            if (expression is MethodCallExpression)
            {
                MethodCallExpression whereExpression = new InnermostWhereFinder().GetInnermostWhere(expression);
                LambdaExpression lambdaExpression = (LambdaExpression)((UnaryExpression)whereExpression.Arguments[1]).Operand;
                lambdaExpression = (LambdaExpression)Evaluator.PartialEval(lambdaExpression);

                var queryString = new QueryParameterFinder().GetParamters(lambdaExpression.Body);
                //TODO query data from data source
                var dataSource = "[\"this\",\"is\",\"yulong\",\"this is yulong test, pls test it with this data.\"]";
                var type = elementType.MakeArrayType();
                var result = JsonSerializer.Deserialize(dataSource, type) as Array;
                var queryable = result.AsQueryable();

                var expressionMidfier = new ExpressionModifer(queryable);
                var newExpression = expressionMidfier.Visit(expression);
                if (IsEnumerable)
                    return queryable.Provider.CreateQuery(newExpression);
                else
                    return queryable.Provider.Execute(newExpression);
            }
            else
            {
                throw new InvalidProgramException("No query over the data source was specified.");
            }
        }
    }

    class InnermostWhereFinder : ExpressionVisitor
    {

        private MethodCallExpression _innermostWhereExpression;

        internal MethodCallExpression GetInnermostWhere(Expression expression)
        {
            Visit(expression);
            return _innermostWhereExpression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (expression.Method.DeclaringType == typeof(Queryable) && expression.Method.Name == "Where")
                _innermostWhereExpression = expression;
            Visit(expression.Arguments[0]);
            return expression;
        }

    }

    class QueryParameterFinder : ExpressionVisitor
    {
        StringBuilder sb = new StringBuilder();
        internal string GetParamters(Expression expression)
        {
            sb = new StringBuilder();
            Visit(expression);
            return sb.ToString();
        }

        private static Expression stripQuoteExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.Quote)
                return ((UnaryExpression)expression).Operand;
            return expression;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Not:
                    sb.Append(" NOT ");
                    Visit(node.Operand);
                    break;
                default:
                    throw new NotSupportedException($"The unary operator {node.NodeType} is not supported.");
            }
            return node;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            sb.Append("(");
            Visit(node.Left);
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    sb.Append(" <> ");
                    break;
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    sb.Append(" OR ");
                    break;
                case ExpressionType.LessThan:
                    sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sb.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException($"The bianry operator {node.NodeType} is not supported.");
            }
            Visit(node.Right);
            sb.Append(")");

            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value != null)
            {
                switch (Type.GetTypeCode(node.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        sb.Append((bool)node.Value ? 1 : 0);
                        break;
                    case TypeCode.String:
                        sb.Append($"\"{node.Value}\"");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException($"The constant for '{node.Value}' is not supported.");
                    default:
                        sb.Append(node.Value);
                        break;
                }
            }
            else
            {
                sb.Append(" NULL ");
            }
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.NodeType == ExpressionType.MemberAccess &&
                node.Expression != null &&
                node.Expression.NodeType == ExpressionType.Parameter)
            {
                sb.Append(node.Member.Name);
            }
            else
            {
                throw new NotSupportedException($"The member {node.Member.Name} is not supported.");
            }
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(string))
            {
                switch (node.Method.Name)
                {
                    case "Contains":
                        sb.Append(" LIKE \"%");
                        Visit(node.Arguments);
                        sb.Append("%\"");
                        break;
                    case "StartsWith":
                        sb.Append(" LIKE ");
                        Visit(node.Arguments);
                        sb.Append("%");
                        break;
                    case "EndsWith":
                        sb.Append(" LIKE %");
                        Visit(node.Arguments);
                        break;
                    default:
                        throw new NotSupportedException($"The method string.{node.Method.Name} is not supported.");
                }
            }
            else
            {
                throw new NotSupportedException($"The method {node.Method.DeclaringType.FullName}.{node.Method.Name} is not supported.");
            }
            return node;
        }

        private void getValuePairFromExpression(BinaryExpression be)
        {
            if (be.Left.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression member = be.Left as MemberExpression;
                var value = getValue(be.Right);
                var _queryString = $"{member.Member.Name} = {value}";
            }
            else if (be.Right.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression member = be.Right as MemberExpression;
                var value = getValue(be.Left);
                var _queryString = $"{member.Member.Name} = {value}";
            }
        }

        private string getValue(Expression be)
        {
            if (be.NodeType == ExpressionType.Constant)
            {
                return (string)((ConstantExpression)be).Value;
            }
            throw new NotSupportedException($"Does not support [{be.NodeType}] {be}");
        }

        private bool isMemberEqualsValueExpression(BinaryExpression be)
        {
            if (be.NodeType != ExpressionType.Equal)
                return false;
            return isMemberExpression(be.Left) || isMemberExpression(be.Right);
        }

        private bool isMemberExpression(Expression be)
                => be is MemberExpression;


    }

    class ExpressionModifer : ExpressionVisitor
    {
        private readonly IQueryable queryable;
        private readonly Type type;

        public ExpressionModifer(IQueryable queryable)
        {
            this.queryable = queryable;
            this.type = typeof(CustomQueryable<>).MakeGenericType(queryable.ElementType);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == type)
            {
                return Expression.Constant(queryable);
            }
            return node;
        }
    }

    static class Evaluator
    {
        public static Expression PartialEval(Expression expression)
        {
            var filterExpressions = new Nominator(e => e.NodeType != ExpressionType.Parameter).Nominate(expression);
            return new SubtreeEvaluator(filterExpressions).Eval(expression);
        }

        class Nominator : ExpressionVisitor
        {
            private readonly Func<Expression, bool> _canBeEvaluatedFn;
            private bool _cannotBeEvaluated;
            private HashSet<Expression> _expressions;
            public Nominator(Func<Expression, bool> canBeEvaluated)
            {
                this._canBeEvaluatedFn = canBeEvaluated;
            }

            public HashSet<Expression> Nominate(Expression expression)
            {
                _expressions = new HashSet<Expression>();
                Visit(expression);
                return _expressions;
            }

            public override Expression Visit(Expression node)
            {
                if (node != null)
                {
                    var saveCannotBeEvaluated = _cannotBeEvaluated;
                    _cannotBeEvaluated = false;
                    base.Visit(node);
                    if (!_cannotBeEvaluated)
                    {
                        if (_canBeEvaluatedFn(node))
                        {
                            _expressions.Add(node);
                        }
                        else
                        {
                            _cannotBeEvaluated = true;
                        }
                    }
                    _cannotBeEvaluated |= saveCannotBeEvaluated;
                }
                return node;
            }

        }

        class SubtreeEvaluator : ExpressionVisitor
        {
            private readonly HashSet<Expression> _expressions;


            public SubtreeEvaluator(HashSet<Expression> expressions)
            {
                this._expressions = expressions;
            }

            public Expression Eval(Expression expression)
            {
                return Visit(expression);
            }

            public override Expression Visit(Expression node)
            {
                if (node == null)
                    return node;
                if (_expressions.Contains(node))
                {
                    if (node.NodeType == ExpressionType.Constant)
                        return node;
                    else
                    {
                        var lambdaExpression = Expression.Lambda(node);
                        var action = lambdaExpression.Compile();
                        return Expression.Constant(action.DynamicInvoke(null), node.Type);
                    }
                }
                return base.Visit(node);
            }

        }
    }

}
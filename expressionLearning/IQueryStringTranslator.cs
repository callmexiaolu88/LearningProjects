using System.Linq.Expressions;

namespace expressionLearning
{
    public interface IQueryStringTranslator
    {
        string TranslateExpression(Expression expression);
    }
}
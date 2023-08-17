using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace expressionLearning
{
    public class CustomQueryable<T> : IOrderedQueryable<T>
    {
        public Type ElementType => typeof(T);

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }

        internal CustomQueryable(Expression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
                throw new ArgumentOutOfRangeException("expression");
            Expression = expression;
            Provider = new CustomQueryProvider<T>();
        }

        public CustomQueryable()
        {
            Expression = Expression.Constant(this);
            Provider = new CustomQueryProvider<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var enumerable = Provider.Execute<IEnumerable<T>>(Expression);
            if (enumerable != null && enumerable != this)
            {
                return enumerable.GetEnumerator();
            }
            else
            {
                return Enumerable.Empty<T>().GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
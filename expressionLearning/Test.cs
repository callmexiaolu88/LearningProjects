using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace expressionLearning
{
    class Test<T>
    {

        public Action<T> Setter { get; set; }

        public void Set<TProp>(Expression<Func<T, TProp>> expression, TProp value)
        {
            if (expression?.Body is MemberExpression  memberExpression)
            {
                var v = Expression.Constant(value, typeof(TProp));
                var parameter = Expression.Parameter(typeof(T), "a");
                var prop = Expression.PropertyOrField(parameter, memberExpression.Member.Name);
                var body = Expression.Assign(prop, v);
                Setter = Expression.Lambda<Action<T>>(body, parameter).Compile();
            }
        }
    }
}

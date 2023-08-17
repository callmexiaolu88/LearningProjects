using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace expressionLearning
{
    public class CustomQueryProvider<TSource> : IQueryProvider
    {
        #region CreateQuery

        public IQueryable CreateQuery(Expression expression)
        {
            try
            {
                var type = expression.Type;
                return (IQueryable)Activator.CreateInstance(typeof(CustomQueryable<>).MakeGenericType(type), expression);
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException == null)
                {
                    throw;
                }

                throw e.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new CustomQueryable<TElement>(expression);
        }

        #endregion

        #region Interpret

        public object Execute(Expression expression)
        {
            return CustomQueryContext.Execute(expression, typeof(TSource), false);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            System.Console.WriteLine($"Execute expression: {expression}");
            bool IsEnumerable = typeof(TResult).Name == "IEnumerable`1";
            return (TResult)CustomQueryContext.Execute(expression, typeof(TSource), IsEnumerable);
        }

        #endregion

    }
}
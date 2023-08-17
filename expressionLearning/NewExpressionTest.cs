using System;
using System.Linq.Expressions;

namespace expressionLearning
{
    internal class NewExpressionTest<TPlayback>
    {
        private readonly Func<NewExpressionTestParameter,object, TPlayback> _playbackGenerator;

        public NewExpressionTest(Func<TPlayback> playbackGenerator = null)
        {
            if (playbackGenerator == null)
            {
                var constructorInfo = typeof(TPlayback).GetConstructor(new Type[] { typeof(NewExpressionTestParameter), typeof(object) });
                if (constructorInfo == null)
                {
                    constructorInfo = typeof(TPlayback).GetConstructor(new Type[] { typeof(NewExpressionTestParameter) });
                    if (constructorInfo == null)
                    {
                        constructorInfo = typeof(TPlayback).GetConstructor(new Type[0]);
                        if (constructorInfo == null)
                        {
                            throw new MissingMethodException($"{ typeof(TPlayback).Name} miss { typeof(TPlayback).Name}({typeof(NewExpressionTestParameter).Name}) or { typeof(TPlayback).Name}() constructor.");
                        }
                        else
                        {
                            var p1 = Expression.Parameter(typeof(NewExpressionTestParameter));
                            var p2 = Expression.Parameter(typeof(object));
                            var defaultNewExpression = Expression.New(constructorInfo);
                            _playbackGenerator = Expression.Lambda<Func<NewExpressionTestParameter, object, TPlayback>>(defaultNewExpression, p1, p2).Compile();

                        }
                    }
                    else
                    {
                        var p1 = Expression.Parameter(typeof(NewExpressionTestParameter));
                        var p2 = Expression.Parameter(typeof(object));
                        var defaultNewExpression = Expression.New(constructorInfo, p1);
                        _playbackGenerator = Expression.Lambda<Func<NewExpressionTestParameter, object, TPlayback>>(defaultNewExpression, p1, p2).Compile();
                    }
                }
                else
                {
                    var p1 = Expression.Parameter(typeof(NewExpressionTestParameter));
                    var p2 = Expression.Parameter(typeof(object));
                    var defaultNewExpression = Expression.New(constructorInfo, p1, p2);
                    _playbackGenerator = Expression.Lambda<Func<NewExpressionTestParameter, object, TPlayback>>(defaultNewExpression, p1, p2).Compile();
                }
            }
            else
            {
                _playbackGenerator = (a, s) => playbackGenerator();
            }
        }

        public TPlayback GeneratePlayback(NewExpressionTestParameter adapterPlaybackSkeleton, object obj)
            => _playbackGenerator.Invoke(adapterPlaybackSkeleton, obj);
    }

    internal class NewExpressionPlayback
    {
        public NewExpressionPlayback()
        {
            Parameter = new NewExpressionTestParameter(Guid.Empty);
        }

        public NewExpressionPlayback(NewExpressionTestParameter str)
        {
            Parameter = str;
        }

        public NewExpressionTestParameter Parameter { get; }
    }

    internal class NewExpressionTestParameter
    {
        public Guid Id { get; } = Guid.NewGuid();

        public NewExpressionTestParameter(Guid guid)
        {
            Id = guid;
        }
    }
}
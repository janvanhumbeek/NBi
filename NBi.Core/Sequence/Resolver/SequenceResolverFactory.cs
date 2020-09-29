﻿using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Scalar.Duration;
using NBi.Core.Sequence.Resolver.Loop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver
{
    public class SequenceResolverFactory
    {
        private ServiceLocator ServiceLocator { get; }
        public SequenceResolverFactory(ServiceLocator serviceLocator)
            => ServiceLocator = serviceLocator;
        
        internal ISequenceResolver<T> Instantiate<T>(ISequenceResolverArgs args)
        {
            switch (args)
            {
                case QuerySequenceResolverArgs queryArgs: return new QuerySequenceResolver<T>(queryArgs, ServiceLocator);
                case ListSequenceResolverArgs listArgs: return new ListSequenceResolver<T>(listArgs);
                case CustomSequenceResolverArgs customArgs: return new CustomSequenceResolver<T>(customArgs);
                case FileLoopSequenceResolverArgs fileArgs: return (ISequenceResolver<T>)new FileLoopSequenceResolver(fileArgs);
                case ILoopSequenceResolverArgs loopArgs:
                    {
                        var strategy = MapStrategy<T>(loopArgs);
                        return new LoopSequenceResolver<T>(strategy);
                    }
                case FilterSequenceResolverArgs filterArgs: return new FilterSequenceResolver<T>(filterArgs);
                default:
                    throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a Sequence");
            }
        }

        public ISequenceResolver Instantiate(ColumnType type, ISequenceResolverArgs args)
        {
            switch (type)
            {
                case ColumnType.Text: return Instantiate<string>(args);
                case ColumnType.Numeric: return Instantiate<decimal>(args);
                case ColumnType.DateTime: return Instantiate<DateTime>(args);
                case ColumnType.Boolean: return Instantiate<bool>(args);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private ILoopStrategy MapStrategy<T>(ILoopSequenceResolverArgs args)
        {
            switch (args)
            {
                case CountLoopSequenceResolverArgs<decimal, decimal> x:
                    return new CountNumericLoopStrategy(x.Count, x.Seed, x.Step) as ILoopStrategy;
                case CountLoopSequenceResolverArgs<DateTime, IDuration> x:
                    return new CountDateTimeLoopStrategy(x.Count, x.Seed, x.Step) as ILoopStrategy;
                case SentinelLoopSequenceResolverArgs<decimal, decimal> x:
                    switch (x.IntervalMode)
                    {
                        case IntervalMode.Close:
                            return new SentinelCloseNumericLoopStrategy(x.Seed, x.Terminal, x.Step) as ILoopStrategy;
                        case IntervalMode.HalfOpen:
                            return new SentinelHalfOpenNumericLoopStrategy(x.Seed, x.Terminal, x.Step) as ILoopStrategy;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case SentinelLoopSequenceResolverArgs<DateTime, IDuration> x:
                    switch (x.IntervalMode)
                    {
                        case IntervalMode.Close:
                            return new SentinelCloseDateTimeLoopStrategy(x.Seed, x.Terminal, x.Step) as ILoopStrategy;
                        case IntervalMode.HalfOpen:
                            return new SentinelHalfOpenDateTimeLoopStrategy(x.Seed, x.Terminal, x.Step) as ILoopStrategy;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

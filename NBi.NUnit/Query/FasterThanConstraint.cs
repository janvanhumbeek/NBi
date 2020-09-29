﻿using System;
using System.Data;
using NBi.Core.Query;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Core.Query.Performance;
using NBi.Extensibility.Query;

namespace NBi.NUnit.Query
{
    public class FasterThanConstraint : NBiConstraint
    {
        private PerformanceResult performanceResult;
        private int maxTimeMilliSeconds;
        private int timeOutMilliSeconds;
        private bool cleanCache;
        private IPerformanceEngine engine;
        protected internal IPerformanceEngine Engine
        { set => engine = value ?? throw new ArgumentNullException(); }

        public FasterThanConstraint()
        { }

        public FasterThanConstraint MaxTimeMilliSeconds(int value)
        {
            this.maxTimeMilliSeconds = value;
            return this;
        }

        public FasterThanConstraint TimeOutMilliSeconds(int value)
        {
            this.timeOutMilliSeconds = value;
            return this;
        }

        public FasterThanConstraint CleanCache()
        {
            cleanCache = true;
            return this;
        }

        protected IPerformanceEngine GetEngine(IQuery actual)
         => engine ?? (engine = new PerformanceEngineFactory().Instantiate(actual));

        /// <summary>
        /// Handle a sql string or a sqlCommand and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string or SQL Command</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        public override bool Matches(object actual)
        {
            if (actual is IQuery)
                return doMatch((IQuery)actual);
            else
                return false;
        }

        public bool doMatch(IQuery actual)
        {
            var engine = GetEngine(actual);
            if (cleanCache)
                engine.CleanCache();
            performanceResult = engine.Execute(new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds));
            return
                (
                    performanceResult.TimeElapsed.TotalMilliseconds < maxTimeMilliSeconds
                    && !performanceResult.IsTimeOut
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Execution of the query is slower than expected");
            sb.AppendFormat("Maximum expected was {0}ms", maxTimeMilliSeconds);
            writer.WritePredicate(sb.ToString());
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (performanceResult.IsTimeOut)
                writer.WriteActualValue(string.Format("query interrupted after {0}ms (timeout)", performanceResult.TimeOut));
            else
                writer.WriteActualValue(string.Format("{0}ms", performanceResult.TimeElapsed.TotalMilliseconds));
        }
    }
}
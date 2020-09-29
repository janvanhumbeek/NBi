﻿using NBi.Core.ResultSet.Lookup;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Lookup
{
    class LookupReplaceEngine : ILookupEngine
    {
        private LookupReplaceArgs Args { get; }

        public LookupReplaceEngine(LookupReplaceArgs args)
            => (Args) = (args);

        public ResultSet Execute(ResultSet candidate)
        {
            var reference = Args.Reference.Execute();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var referenceKeyRetriever = BuildColumnsRetriever(Args.Mapping, x => x.ReferenceColumn);
            var referenceValueRetriever = BuildColumnsRetriever(new ColumnMapping(Args.Replacement, ColumnType.Untyped), x => x.ReferenceColumn);
            var index = BuildReferenceIndex(reference.Table, referenceKeyRetriever, referenceValueRetriever);
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Built the index for reference table containing {index.Count()} rows [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");

            stopWatch.Restart();
            var candidateKeyBuilder = BuildColumnsRetriever(Args.Mapping, x => x.CandidateColumn);

            var originalColumn = candidate.Table.GetColumn(Args.Mapping.CandidateColumn);
            var newColumn = candidate.Table.Columns.Add($"tmp_{originalColumn.ColumnName}", typeof(object));
            foreach (DataRow row in candidate.Table.Rows)
            {
                var candidateKeys = candidateKeyBuilder.GetColumns(row);
                if (index.Keys.Contains(candidateKeys))
                    row[newColumn.Ordinal] = index[candidateKeys].Single().Members[0];
                else
                    Args.MissingStrategy.Execute(row, originalColumn, newColumn);
            }
            
            //Replace the original column by the new column
            newColumn.SetOrdinal(originalColumn.Ordinal);
            var columnName = originalColumn.ColumnName;
            candidate.Table.Columns.Remove(originalColumn);
            newColumn.ColumnName = columnName;

            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"Performed lookup replacement (based on keys) for the {candidate.Rows.Count} rows from candidate table [{stopWatch.Elapsed:d'.'hh':'mm':'ss'.'fff'ms'}]");
            candidate.Table.AcceptChanges();
            return candidate;
        }

        protected CellRetriever BuildColumnsRetriever(ColumnMapping column, Func<ColumnMapping, IColumnIdentifier> target)
        {
            var defColumns = new Collection<IColumnDefinition>();
            var defColumn = column.ToColumnDefinition(() => target(column));
            defColumns.Add(defColumn);

            switch(target(column))
            {
                case ColumnOrdinalIdentifier _: return new CellRetrieverByOrdinal(defColumns);
                case ColumnNameIdentifier _: return new CellRetrieverByName(defColumns);
                default: throw new ArgumentException();
            }
        }

        protected IDictionary<KeyCollection, ICollection<KeyCollection>> BuildReferenceIndex(DataTable table, CellRetriever keyRetriever, CellRetriever valuesRetriever)
        {
            var references = new Dictionary<KeyCollection, ICollection<KeyCollection>>();

            foreach (DataRow row in table.Rows)
            {
                var keys = keyRetriever.GetColumns(row);
                var values = valuesRetriever.GetColumns(row);
                if (!references.ContainsKey(keys))
                    references.Add(keys, new HashSet<KeyCollection>() { values });
                else
                    references[keys].Add(values);
            }

            return references;
        }
    }
}

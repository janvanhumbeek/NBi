﻿using NBi.Core.Sequence.Resolver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Combination
{
    class CartesianProductSequenceCombination
    {
        private ISequenceResolver Resolver { get; }
        public CartesianProductSequenceCombination(ISequenceResolver resolver)
            => Resolver = resolver;

        public ResultSet Execute(ResultSet rs)
        {
            var newColumn = new DataColumn($"Column{rs.Columns.Count}", typeof(object));
            rs.Columns.Add(newColumn);

            var sequence = Resolver.Execute();
            if (sequence.Count == 0 || rs.Columns.Count == 1)
            {
                rs.Table.Clear();
            }
            else
            {
                var firstItem = sequence[0];
                foreach (DataRow row in rs.Rows)
                    row[newColumn] = firstItem;

                var newRows = new HashSet<DataRow>();
                foreach (var item in sequence.Cast<object>().Skip(1))
                {
                    foreach (DataRow row in rs.Rows)
                    {
                        var newRow = rs.Table.NewRow();
                        newRow.ItemArray = row.ItemArray;
                        newRow[newColumn] = item;
                        newRows.Add(newRow);
                    }
                }
                foreach (var newRow in newRows)
                    rs.Table.Rows.Add(newRow);
            }
            rs.Table.AcceptChanges();
            return rs;
        }
    }
}

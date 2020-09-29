﻿using NBi.Core.ResultSet;
using NBi.Core.Scalar.Presentation;
using NBi.Framework.Markdown.MarkdownLogExtension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Markdown.Helper
{
    class CompareTableHelperMarkdown : TableHelperMarkdown
    {
        public CompareTableHelperMarkdown(EngineStyle style)
        : base(style) { }

        protected override List<TableRowExtended> BuildRows(IEnumerable<DataRow> dataRows, List<ColumnType> columnTypes)
        {
            var rows = new List<TableRowExtended>();
            foreach (DataRow dataRow in dataRows)
            {
                var cells = new List<TableCellExtended>();
                for (int i = 0; i < dataRow.Table.Columns.Count; i++)
                {
                    var text = GetText(columnTypes, dataRow, i);
                    var compared = GetCompareText(columnTypes, dataRow, i);
                    var fullText = string.Format("{0}{1}{2}", text, string.IsNullOrEmpty(compared) ? "" : " <> ", compared);
                    cells.Add(new TableCellExtended() { Text = fullText });
                }
                rows.Add(new TableRowExtended() { Cells = cells });
            }
            return rows;
        }

        protected string GetCompareText(List<ColumnType> columnTypes, DataRow dataRow, int i)
        {
            if (string.IsNullOrEmpty(dataRow.GetColumnError(i)))
                return string.Empty;
            
            var factory = new PresenterFactory();
            var formatter = factory.Instantiate(columnTypes[i]);

            return formatter.Execute(dataRow.GetColumnError(i));
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    public class ColumnNameIdentifier : IColumnIdentifier, IEquatable<ColumnNameIdentifier>
    {
        public string Name { get; private set; }
        public string Label => $"[{Name}]";

        public ColumnNameIdentifier(string name)
        {
            Name = name;
        }

        public DataColumn GetColumn(DataTable dataTable) => dataTable.Columns[Name];

        public object GetValue(DataRow dataRow) => dataRow[Name];

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object value)
        {
            switch (value)
            {
                case ColumnNameIdentifier x: return Equals(x);
                default: return false;
            }
        }

        public bool Equals(ColumnNameIdentifier other)
            => !(other is null) && Name == other.Name;
    }
}

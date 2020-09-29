﻿using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet
{
    public abstract class DataRowKeysComparer : IEqualityComparer<DataRow>
    {

        public bool Equals(DataRow x, DataRow y)
        {
            if (!CheckKeysExist(x))
                throw new ArgumentException("First datarow has not the required key fields");
            if (!CheckKeysExist(y))
                throw new ArgumentException("Second datarow has not the required key fields");

            return GetHashCode(x) == GetHashCode(y);
        }

        protected abstract bool CheckKeysExist(DataRow dr);
        public abstract KeyCollection GetKeys(DataRow row);

        public int GetHashCode(DataRow dr)
        {
            int hash = 0;
            foreach (var value in GetKeys(dr).Members)
            {
                string v = null;
                if (value is IConvertible)
                    v = ((IConvertible)value).ToString(CultureInfo.InvariantCulture);
                else
                    v = value.ToString();

                hash = (hash * 397) ^ v.GetHashCode();

            }
            return hash;
        }

        protected internal object FormatValue(ColumnType columnType, object value)
        {
            if (value==null || value==DBNull.Value || value as string == "(null)")
                return "(null)";

            switch (columnType)
            {
                case ColumnType.Numeric:
                    return new NumericCaster().Execute(value);
                case ColumnType.DateTime:
                    return new DateTimeCaster().Execute(value);
                case ColumnType.Boolean:
                    return new ThreeStateBooleanCaster().Execute(value);
                default:
                    if (value == DBNull.Value)
                        return "(null)";
                    else if (value is IConvertible)
                        return ((IConvertible)value).ToString(CultureInfo.InvariantCulture);
                    else
                        return value.ToString();
            }
        }
    }
}

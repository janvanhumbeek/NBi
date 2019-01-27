﻿using NBi.Core.Scalar.Casting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace NBi.Core.ResultSet.Lookup
{
    class KeysRetrieverByName : KeysRetriever
    {
        public KeysRetrieverByName(IEnumerable<IColumnDefinition> settings)
            : base(settings)
        { }

        public override KeyCollection GetKeys(DataRow row)
        {
            var keys = new List<object>();
            foreach (var setting in Settings)
            {
                var name = (setting.Identifier as ColumnNameIdentifier).Name;
                try
                {
                    var value = FormatValue(setting.Type, row[name]);
                    keys.Add(value);
                }
                catch (FormatException)
                {
                    throw new NBiException($"In the column with name '{name}', NBi can't convert the value '{row[name]}' to the type '{setting.Type}'. Key columns must match with their respective types and don't support null, generic or interval values.");
                }
                catch (InvalidCastException ex)
                {
                    if (ex.Message.Contains("Object cannot be cast from DBNull to other types"))
                    {
                        throw new NBiException($"In the column with name '{name}', NBi can't convert the value 'DBNull' to the type '{setting.Type}'. Key columns must match with their respective types and don't support null, generic or interval values.");
                    }
                    else
                        throw ex;
                }
            }
            return new KeyCollection(keys.ToArray());
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Data;
using NBi.Core.Scalar.Comparer;
using NBi.Core.ResultSet.Analyzer;

namespace NBi.Core.ResultSet.Equivalence
{
    public class OrdinalEquivaler : BaseEquivaler
    {
        private new SettingsOrdinalResultSet Settings
        {
            get => base.Settings as SettingsOrdinalResultSet;
        }

        public OrdinalEquivaler(IEnumerable<IRowsAnalyzer> analyzers, SettingsOrdinalResultSet settings)
            : base(analyzers)
        {
            base.Settings = settings;
        }

        protected override void PreliminaryChecks(DataTable x, DataTable y)
        {
            var columnsCount = Math.Max(y.Columns.Count, x.Columns.Count);
            if (Settings == null)
                BuildDefaultSettings(columnsCount);
            else
                Settings.ApplyTo(columnsCount);

            WriteSettingsToDataTableProperties(y, Settings);
            WriteSettingsToDataTableProperties(x, Settings);

            CheckSettingsAndDataTable(y, Settings);
            CheckSettingsAndDataTable(x, Settings);

            CheckSettingsAndFirstRow(y, Settings);
            CheckSettingsAndFirstRow(x, Settings);
        }

        public override EngineStyle Style
        {
            get => EngineStyle.ByIndex;
        }

        protected override DataRowKeysComparer BuildDataRowsKeyComparer(DataTable x)
            => new DataRowKeysComparerByOrdinal(Settings, x.Columns.Count);

        protected override bool CanSkipValueComparison()
            => Settings.KeysDef == SettingsOrdinalResultSet.KeysChoice.All;

        protected override DataRow CompareRows(DataRow rx, DataRow ry)
        {
            var isRowOnError = false;
            for (int i = 0; i < rx.Table.Columns.Count; i++)
            {
                if (Settings.GetColumnRole(i) == ColumnRole.Value)
                {
                    var x = rx.IsNull(i) ? DBNull.Value : rx[i];
                    var y = ry.IsNull(i) ? DBNull.Value : ry[i];
                    var rounding = Settings.IsRounding(i) ? Settings.GetRounding(i) : null;
                    var result = CellComparer.Compare(x, y, Settings.GetColumnType(i), Settings.GetTolerance(i), rounding);

                    if (!result.AreEqual)
                    {
                        ry.SetColumnError(i, result.Message);
                        if (!isRowOnError)
                            isRowOnError = true;
                    }
                }
            }
            if (isRowOnError)
                return ry;
            else
                return null;
        }

        protected void WriteSettingsToDataTableProperties(DataTable dt, SettingsOrdinalResultSet settings)
        {
            foreach (DataColumn column in dt.Columns)
            {
                WriteSettingsToDataTableProperties(
                    column
                    , settings.GetColumnRole(column.Ordinal)
                    , settings.GetColumnType(column.Ordinal)
                    , settings.GetTolerance(column.Ordinal)
                    , settings.GetRounding(column.Ordinal)
                );
            }
        }

        protected void CheckSettingsAndDataTable(DataTable dt, SettingsOrdinalResultSet settings)
        {
            var max = settings.GetMaxColumnOrdinalDefined();
            if (dt.Columns.Count <= max)
            {
                var exception = string.Format("You've defined a column with an index of {0}, meaning that your result set would have at least {1} columns but your result set has only {2} columns."
                    , max
                    , max + 1
                    , dt.Columns.Count);

                if (dt.Columns.Count == max && settings.GetMinColumnOrdinalDefined() == 1)
                    exception += " You've no definition for a column with an index of 0. Are you sure you'vent started to index at 1 in place of 0?";

                throw new EquivalerException(exception);
            }
        }

        protected void CheckSettingsAndFirstRow(DataTable dt, SettingsOrdinalResultSet settings)
        {
            if (dt.Rows.Count == 0)
                return;

            var dr = dt.Rows[0];
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                CheckSettingsFirstRowCell(
                        settings.GetColumnRole(i)
                        , settings.GetColumnType(i)
                        , dr.Table.Columns[i]
                        , dr.IsNull(i) ? DBNull.Value : dr[i]
                        , new string[]
                            {
                                "The column with index '{0}' is expecting a numeric value but the first row of your result set contains a value '{1}' not recognized as a valid numeric value or a valid interval."
                                , " Aren't you trying to use a comma (',' ) as a decimal separator? NBi requires that the decimal separator must be a '.'."
                                , "The column with index '{0}' is expecting a 'date & time' value but the first row of your result set contains a value '{1}' not recognized as a valid date & time value."
                            }
                );
            }
        }

        protected virtual void BuildDefaultSettings(int columnsCount)
        {
            base.Settings = new SettingsOrdinalResultSet(
                columnsCount,
                SettingsOrdinalResultSet.KeysChoice.AllExpectLast,
                SettingsOrdinalResultSet.ValuesChoice.Last);
        }

    }
}

﻿using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Calculation;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet.Filtering;

namespace NBi.NUnit.Query
{
    public class SomeRowsConstraint : NoRowsConstraint
    {
        public SomeRowsConstraint(IResultSetFilter filter)
            : base(filter)
        { }

        protected override bool doMatch(int actual)
            => filterResultSet.Rows.Count >= 1;

        public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WritePredicate($"some rows validate the predicate '{filter.Describe()}'.");
        }

        public override void WriteFilterMessageTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WriteLine("Rows validating the predicate:");
        }

        public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                return;
            writer.WriteLine($"No rows validate the predicate '{filter.Describe()}'.");
        }
    }
}
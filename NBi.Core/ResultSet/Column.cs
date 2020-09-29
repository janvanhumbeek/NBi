﻿using System;
using System.Linq;
using NBi.Core.Scalar.Comparer;
using NBi.Core.Transformation;

namespace NBi.Core.ResultSet
{
    public class Column : IColumnDefinition
    {
        public IColumnIdentifier Identifier { get; set; }
        public ColumnRole Role {get; set;} 
        public ColumnType Type {get; set;}

        public bool IsToleranceSpecified
        {
            get { return !string.IsNullOrEmpty(Tolerance); }
        }

        public string Tolerance { get; set; }

        public Rounding.RoundingStyle RoundingStyle { get; set; }
        public string RoundingStep { get; set; }

        public ITransformationInfo Transformation { get; set; }
    }
}

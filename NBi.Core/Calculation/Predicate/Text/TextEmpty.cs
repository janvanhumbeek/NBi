﻿using NBi.Core.Scalar.Comparer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Text
{
    class TextEmpty : AbstractPredicate
    {
        public TextEmpty(bool not)
            : base(not)
        { }

        protected override bool Apply(object x)
        {
            return (x as string)!=null && ((x as string).Length == 0 || (x as string)=="(empty)");
        }


        public override string ToString()
        {
            return $"is empty";
        }
    }
}

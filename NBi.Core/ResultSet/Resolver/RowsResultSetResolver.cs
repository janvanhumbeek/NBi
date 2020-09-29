﻿using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    public class RowsResultSetResolver : IResultSetResolver
    {
        private readonly RowsResultSetResolverArgs args;

        public RowsResultSetResolver(RowsResultSetResolverArgs args)
        {
            this.args = args;
        }

        public virtual ResultSet Execute()
        {
            var rs = new ResultSet();
            rs.Load(args.Rows);
            return rs;
            }
    }
}

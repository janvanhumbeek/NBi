﻿using NBi.Core.ResultSet.Alteration.Renaming.Strategies.Missing;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Renaming
{
    class NewNameRenamingEngine : IRenamingEngine
    {
        private IColumnIdentifier OriginalIdentification { get; }
        private IScalarResolver<string> NewIdentification { get; }
        private IMissingColumnStrategy MissingColumnStrategy { get; }

        protected internal NewNameRenamingEngine(IColumnIdentifier originalIdentification, IScalarResolver<string> newIdentification)
            : this(originalIdentification, newIdentification, new FailureMissingColumnStrategy()) { }

        public NewNameRenamingEngine(IColumnIdentifier originalIdentification, IScalarResolver<string> newIdentification, IMissingColumnStrategy missingColumnStrategy)
            => (OriginalIdentification, NewIdentification, MissingColumnStrategy) = (originalIdentification, newIdentification, missingColumnStrategy);

        public ResultSet Execute(ResultSet rs)
        {
            var originalColumn = OriginalIdentification.GetColumn(rs.Table);

            if (originalColumn == null)
                MissingColumnStrategy.Execute(OriginalIdentification.Label, rs.Table);
            else
                originalColumn.ColumnName = NewIdentification.Execute();
            return rs;
        }
    }
}

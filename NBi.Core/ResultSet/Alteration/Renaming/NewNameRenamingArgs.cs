﻿using NBi.Core.ResultSet.Alteration.Renaming.Strategies.Missing;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Renaming
{
    public class NewNameRenamingArgs : IRenamingArgs
    {
        public IColumnIdentifier OriginalIdentification { get; set; }
        public IScalarResolver<string> NewIdentification { get; set; }
        public IMissingColumnStrategy MissingColumnStrategy { get; set; }

        public NewNameRenamingArgs(IColumnIdentifier originalIdentification, IScalarResolver<string> newIdentification, IMissingColumnStrategy missingColumnStrategy)
            => (OriginalIdentification, NewIdentification, MissingColumnStrategy) = (originalIdentification, newIdentification, missingColumnStrategy);
    }
}

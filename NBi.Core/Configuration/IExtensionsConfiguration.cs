﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Configuration
{
    public interface IExtensionsConfiguration
    {
        IReadOnlyDictionary<Type, IDictionary<string, string>> Extensions { get; }
    }
}

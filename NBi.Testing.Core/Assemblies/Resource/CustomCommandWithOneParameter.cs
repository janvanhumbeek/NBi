﻿using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Assemblies.Resource
{
    class CustomCommandWithOneParameter : ICustomCommand
    {
        public CustomCommandWithOneParameter(string name)
        { }

        public void Execute() { }
    }
}

﻿using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Scalar.Resolver.Resources
{
    public class MyCustomClass : IScalarResolver
    {
        public object Execute() => "myValue";
    }
}

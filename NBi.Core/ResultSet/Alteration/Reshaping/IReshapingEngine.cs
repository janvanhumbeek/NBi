﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Reshaping
{
    public interface IReshapingEngine
    {
        ResultSet Execute(ResultSet rs);
    }
}

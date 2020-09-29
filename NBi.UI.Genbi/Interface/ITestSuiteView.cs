﻿using System;
using System.ComponentModel;
using System.Linq;
using NBi.GenbiL.Stateful;
using NBi.IO.Genbi.Dto;

namespace NBi.UI.Genbi.Interface
{
    interface ITestSuiteView : IView
    {
        BindingList<Test> Tests { get; set; }
        Test TestSelection { get; set; }
        int TestSelectedIndex { get; set; }
    }
}

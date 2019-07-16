﻿using NBi.GenbiL;
using NBi.GenbiL.Action.Suite;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.GenbiL.Action.Suite
{
    
    public class GenerateSuiteActionTest
    {
        protected GenerationState BuildInitialState(object obj)
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("one");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("two", typeof(object));
            var firstRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "a";
            firstRow[1] = obj;
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            state.Templates.Add("<test name='$one$ + $two$'/>");
            return state;
        }

        [Test]
        public void Execute_SimpleDataTable_Rendered()
        {
            var state = BuildInitialState("b");
            var action = new GenerateSuiteAction(false);
            action.Execute(state);

            Assert.That(state.List.Tests, Has.Count.EqualTo(1));
            var test = state.List.Tests[0];
            Assert.That(test.Name, Is.EqualTo("a + b"));
        }

        [Test]
        public void Execute_SimpleDataTable2_Rendered()
        {
            var state = BuildInitialState("foo");
            var action = new GenerateSuiteAction(false);
            action.Execute(state);

            Assert.That(state.List.Tests, Has.Count.EqualTo(1));
            var test = state.List.Tests[0];
            Assert.That(test.Name, Is.EqualTo("a + foo"));
        }

        [Test]
        public void Execute_ComplexDataTable_Rendered()
        {
            var state = BuildInitialState(new [] {"b", "c"});
            var action = new GenerateSuiteAction(false);
            action.Execute(state);

            Assert.That(state.List.Tests, Has.Count.EqualTo(1));
            var test = state.List.Tests[0];
            Assert.That(test.Name, Is.EqualTo("a + bc"));
        }

        [Test]
        public void Execute_EmptyDataTable_Rendered()
        {
            var state = BuildInitialState("b");
            state.TestCaseCollection.CurrentScope.Content.Rows.Clear();
            var action = new GenerateSuiteAction(false);
            action.Execute(state);

            Assert.That(state.List.Tests, Has.Count.EqualTo(0));
        }
    }
}

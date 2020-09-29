﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NBi.Core.Calculation;
using NBi.Core.Evaluate;
using NBi.NUnit.Builder.Helper;
using NBi.Core.Calculation.Predicate;
using NBi.Core.ResultSet;
using NBi.Core.Variable;
using NBi.Core.ResultSet.Filtering;

namespace NBi.NUnit.Builder
{
    class ResultSetNoRowsBuilder : AbstractResultSetBuilder
    {
        protected NoRowsXml ConstraintXml {get; set;}
        
        protected override void SpecificSetup(AbstractSystemUnderTestXml sutXml, AbstractConstraintXml ctrXml)
        {
            if (!(ctrXml is NoRowsXml))
                throw new ArgumentException("Constraint must be a 'NoRowXml'");

            ConstraintXml = (NoRowsXml)ctrXml;
        }

        protected override void SpecificBuild()
        {
            Constraint = InstantiateConstraint();
        }

        protected virtual NBiConstraint InstantiateConstraint()
        {
            var filter = InstantiateFilter();
            var ctr = new NoRowsConstraint(filter);
            return ctr;
        }

        protected IResultSetFilter InstantiateFilter()
        {
            var context = new Context(Variables, ConstraintXml.Aliases, ConstraintXml.Expressions);
            var factory = new ResultSetFilterFactory(ServiceLocator);
            if (ConstraintXml.Predication != null)
            {
                var helper = new PredicateArgsBuilder(ServiceLocator, context);
                var args = helper.Execute(ConstraintXml.Predication.ColumnType, ConstraintXml.Predication.Predicate);

                return factory.Instantiate
                            (
                                new PredicationArgs(ConstraintXml.Predication.Operand, args)
                                , context
                            );
            }
            else if (ConstraintXml.Combination != null)
            {
                var helper = new PredicateArgsBuilder(ServiceLocator, context);

                var predicationArgs = new List<PredicationArgs>();
                foreach (var predicationXml in ConstraintXml.Combination.Predications)
                {
                    var args = helper.Execute(predicationXml.ColumnType, predicationXml.Predicate);
                    predicationArgs.Add(new PredicationArgs(predicationXml.Operand, args));
                }

                return factory.Instantiate
                            (
                                ConstraintXml.Combination.Operator
                                , predicationArgs
                                , context
                            );
            }
            else
                throw new ArgumentException("You must specify a predicate or a combination of predicates. None of them is specified");
        }
    }
}

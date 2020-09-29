﻿using System;
using System.Data.SqlClient;
using Moq;
using NBi.NUnit;
using NBi.NUnit.Builder;
using NBi.NUnit.Member;
using NBi.NUnit.Query;
using NBi.NUnit.ResultSetComparison;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Framework;
using NBi.NUnit.DataType;
using NBi.Core.DataType;
using NBi.Core.Structure.Relational;
using System.Collections.Generic;
using NBi.Core.Variable;
using NBi.Core.Injection;
using NBi.NUnit.Scoring;

namespace NBi.Testing.Unit.NUnit
{
    [TestFixture]
    public class TestCaseFactoryTest
    {
        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        { }

        [TearDown]
        public void TearDown()
        { }

        #endregion

        [Test]
        public void IsHandling_QuerySyntacticallyCorrect_True()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new SyntacticallyCorrectXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }
        
        [Test]
        public void Instantiate_QuerySyntacticallyCorrect_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new SyntacticallyCorrectXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new SqlCommand());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new SyntacticallyCorrectConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ExecutionXml), typeof(SyntacticallyCorrectXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_MembersSyntacticallyCorrect_False()
        {
            var sutXml = new MembersXml();
            var ctrXml = new SyntacticallyCorrectXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_MembersSyntacticallyCorrect_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new SyntacticallyCorrectXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_StructureSyntacticallyCorrect_False()
        {
            var sutXml = new StructureXml();
            var ctrXml = new SyntacticallyCorrectXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_StructureSyntacticallyCorrect_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new SyntacticallyCorrectXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_QueryFasterThan_True()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new FasterThanXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_QueryFasterThan_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new FasterThanXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new SqlCommand());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new FasterThanConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ExecutionXml), typeof(FasterThanXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_MembersFasterThan_False()
        {
            var sutXml = new MembersXml();
            var ctrXml = new FasterThanXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_MembersFasterThan_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new FasterThanXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_StructureFasterThan_False()
        {
            var sutXml = new StructureXml();
            var ctrXml = new FasterThanXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_StructureFasterThan_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new FasterThanXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_QueryEqualTo_True()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new EqualToXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void IsHandling_QuerySupersetOf_True()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new SupersetOfXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_QueryEqualTo_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new EqualToXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new SqlCommand());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new EqualToConstraint(null));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ExecutionXml), typeof(EqualToXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_MembersEqualTo_False()
        {
            var sutXml = new MembersXml();
            var ctrXml = new EqualToXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_MembersEqualTo_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new EqualToXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_StructureEqualTo_False()
        {
            var sutXml = new StructureXml();
            var ctrXml = new EqualToXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_StructureEqualTo_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new EqualToXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_QueryContains_False()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new ContainXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_QueryContains_ArgumentException()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new ContainXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
       }

        [Test]
        public void IsHandling_MembersContains_True()
        {
            var sutXml = new MembersXml();
            var ctrXml = new ContainXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_MembersContains_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new ContainXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new NBi.NUnit.Member.ContainConstraint("expected"));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(MembersXml), typeof(ContainXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_StructureContains_True()
        {
            var sutXml = new StructureXml();
            var ctrXml = new ContainXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_StructureContains_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new ContainXml();
            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new NBi.NUnit.Structure.ContainConstraint("expected"));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(StructureXml), typeof(ContainXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_QueryCount_False()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new CountXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_QueryCount_ArgumentException()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new CountXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_MembersCount_True()
        {
            var sutXml = new MembersXml();
            var ctrXml = new CountXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_MembersCount_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new CountXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new CountConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(MembersXml), typeof(CountXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_StructureCount_True()
        {
            var sutXml = new StructureXml();
            var ctrXml = new CountXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_StructureCount_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new CountXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_QueryOrdered_False()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new OrderedXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_QueryOrdered_ArgumentException()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new OrderedXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_MembersOrdered_True()
        {
            var sutXml = new MembersXml();
            var ctrXml = new OrderedXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_MembersOrdered_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new OrderedXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new OrderedConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(MembersXml), typeof(OrderedXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_StructureOrdered_True()
        {
            var sutXml = new StructureXml();
            var ctrXml = new OrderedXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_StructureOrdered_ArgumentException()
        {
            var sutXml = new StructureXml();
            var ctrXml = new OrderedXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_QueryExists_False()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new ExistsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_QueryExists_ArgumentException()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new ExistsXml();
            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_MembersExists_False()
        {
            var sutXml = new MembersXml();
            var ctrXml = new ExistsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.False);
        }

        [Test]
        public void Instantiate_MembersExists_ArgumentException()
        {
            var sutXml = new MembersXml();
            var ctrXml = new ExistsXml();

            var testCaseFactory = new TestCaseFactory();

            Assert.Throws<ArgumentException>(delegate { testCaseFactory.Instantiate(sutXml, ctrXml); });
        }

        [Test]
        public void IsHandling_StructureExists_True()
        {
            var sutXml = new StructureXml();
            var ctrXml = new ExistsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_StructureExists_TestCase()
        {
            var sutXml = new StructureXml();
            var ctrXml = new ExistsXml();
            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new ExistsConstraint("foo"));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(StructureXml), typeof(ExistsXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_MembersMatchPattern_True()
        {
            var sutXml = new MembersXml();
            var ctrXml = new MatchPatternXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_MembersMatchPattern_TestCase()
        {
            var sutXml = new MembersXml();
            var ctrXml = new MatchPatternXml();
            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new object());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new NBi.NUnit.Member.MatchPatternConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(MembersXml), typeof(MatchPatternXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_ExecutionMatchPattern_True()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new MatchPatternXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_ExecutionMatchPattern_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new MatchPatternXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new SqlCommand());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new NBi.NUnit.Query.MatchPatternConstraint());
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ExecutionXml), typeof(MatchPatternXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_QueryRowCount_True()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new RowCountXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_QueryRowCount_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new RowCountXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new SqlCommand());
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new RowCountConstraint(null));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ExecutionXml), typeof(RowCountXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_DataTypeIs_True()
        {
            var sutXml = new DataTypeXml();
            var ctrXml = new IsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_DataTypeIs_TestCase()
        {
            var sutXml = new DataTypeXml();
            var ctrXml = new IsXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new RelationalCommand(new SqlCommand(), null, null));
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new IsConstraint("x"));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(DataTypeXml), typeof(IsXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_ResultSetReferenceExists_True()
        {
            var sutXml = new ResultSetSystemXml();
            var ctrXml = new LookupExistsXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_ResultSetReferenceExists_TestCase()
        {
            var sutXml = new ResultSetSystemXml();
            var ctrXml = new LookupExistsXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new RelationalCommand(new SqlCommand(), null, null));
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new IsConstraint("x"));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ResultSetSystemXml), typeof(LookupExistsXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void IsHandling_ExecutionEqualToOld_True()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new EqualToOldXml();
            var testCaseFactory = new TestCaseFactory();

            var actual = testCaseFactory.IsHandling(sutXml.GetType(), ctrXml.GetType());

            Assert.That(actual, Is.True);
        }

        [Test]
        public void Instantiate_ExecutionEqualToOld_TestCase()
        {
            var sutXml = new ExecutionXml();
            var ctrXml = new EqualToOldXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(new RelationalCommand(new SqlCommand(), null, null));
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new IsConstraint("x"));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ExecutionXml), typeof(EqualToOldXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();
        }

        [Test]
        public void Instantiate_ScalarScoreExists_TestCase()
        {
            var sutXml = new ScalarXml();
            var ctrXml = new ScoreXml();

            var builderMockFactory = new Mock<ITestCaseBuilder>();
            builderMockFactory.Setup(b => b.Setup(sutXml, ctrXml, NBi.Core.Configuration.Configuration.Default, It.IsAny<Dictionary<string, ITestVariable>>(), It.IsAny<ServiceLocator>()));
            builderMockFactory.Setup(b => b.Build());
            builderMockFactory.Setup(b => b.GetSystemUnderTest()).Returns(1);
            builderMockFactory.Setup(b => b.GetConstraint()).Returns(new ScoreConstraint(1m));
            var builder = builderMockFactory.Object;

            var testCaseFactory = new TestCaseFactory();
            testCaseFactory.Register(typeof(ScalarXml), typeof(ScoreXml), builder);

            var tc = testCaseFactory.Instantiate(sutXml, ctrXml);

            Assert.That(tc, Is.Not.Null);
            builderMockFactory.VerifyAll();

        }

    }
}

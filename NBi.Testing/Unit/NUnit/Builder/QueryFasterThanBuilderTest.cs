﻿#region Using directives
using System.Data;
using Moq;
using NBi.NUnit.Builder;
using NBi.NUnit.Query;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using NBi.Core.Query;
using NBi.Core.Injection;
using NBi.Extensibility.Query;
using NBi.Xml.Settings;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class QueryFasterThanBuilderTest
    {
        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void GetConstraint_Build_CorrectConstraint()
        {
            var sutXmlStubFactory = new Mock<ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryXml>();
            itemXmlStubFactory.Setup(i => i.InlineQuery).Returns("query");
            itemXmlStubFactory.Setup(i => i.Settings).Returns(SettingsXml.Empty);
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new FasterThanXml();

            var builder = new ExecutionFasterThanBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<FasterThanConstraint>());
        }

        [Test]
        public void GetSystemUnderTest_Build_CorrectIResultSetService()
        {
            var sutXmlStubFactory = new Mock<ExecutionXml>();
            var itemXmlStubFactory = new Mock<QueryXml>();
            itemXmlStubFactory.Setup(i => i.InlineQuery).Returns("query");
            itemXmlStubFactory.Setup(i => i.Settings).Returns(SettingsXml.Empty);
            sutXmlStubFactory.Setup(s => s.Item).Returns(itemXmlStubFactory.Object);
            var sutXml = sutXmlStubFactory.Object;
            sutXml.Item = itemXmlStubFactory.Object;

            var ctrXml = new FasterThanXml();

            var builder = new ExecutionFasterThanBuilder();
            builder.Setup(sutXml, ctrXml, null, null, new ServiceLocator());
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<IQuery>());
        }

    }
}

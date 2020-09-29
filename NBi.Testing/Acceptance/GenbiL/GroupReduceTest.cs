﻿using System;
using System.IO;
using System.Linq;
using NBi.GenbiL;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.GenbiL
{
    [TestFixture]
    public class GroupReduceTest
    {
        private const string TEST_SUITE_NAME="GroupReduce";
        private string DefinitionFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".genbil"; } }
        private string TargetFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".nbits"; } }

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
            if (File.Exists(TargetFilename))
                File.Delete(TargetFilename);

            //if(File.Exists(CsvFilename))
            //    File.Delete(CsvFilename);
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
            if (File.Exists(TargetFilename))
                File.Delete(TargetFilename);
        }
        #endregion

        [Test]
        public void Execute_GroupReduce_FileGenerated()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            Assert.That(File.Exists(TargetFilename));
        }

        [Test]
        public void Execute_GroupReduce_CorrectRendering()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            if (!File.Exists(TargetFilename))
                Assert.Inconclusive("Test Suite not generated!");

            var content = File.ReadAllText(TargetFilename);
            Assert.That(content.Count(c => c=='\r'), Is.LessThan(26));
            content = content.Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace(" ", "");
            Assert.That(content, Does.Contain("<contain><item>first-dimension</item><item>second-dimension</item><item>exclude-dimension</item></contain>"));
            Assert.That(content, Does.Contain("...\"><category>x</category><category>y</category><system-under-test>"));
            Assert.That(content, Does.Contain("<structure><dimensionsperspective=\"first-perspective\"/></structure>"));
        }

    }
}

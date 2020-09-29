﻿using NBi.Core.Calculation.Predicate.DateTime;
using NBi.Core.IO.Filtering;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Transformation.Transformer.Native.IO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.Core.Sequence.Resolver
{
    public class FileSequenceResolverTest
    {
        private string DirectoryName { get => $@"Temp\{GetType().Name}\"; }

        [SetUp]
        public void Setup()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, true);
            Directory.CreateDirectory(DirectoryName);

        }

        [TearDown]
        public void Cleanup()
        {
            if (Directory.Exists(DirectoryName))
                Directory.Delete(DirectoryName, true);
        }

        [Test]
        public void Execute_Pattern_CorrectCount()
        {
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");

            var resolver = new FileLoopSequenceResolver
            (
                new FileLoopSequenceResolverArgs()
                {
                    Path = DirectoryName,
                    Filters = new List<IFileFilter> { new PatternRootFilter("foo-*.txt") },
                }
            );

            var result = resolver.Execute();
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result, Has.Member($"foo-01.txt"));
            Assert.That(result, Has.Member($"foo-1.txt"));
            Assert.That(result, Has.Member($"foo-0.txt"));
        }


        [Test]
        public void Execute_PatternWithCreationFilter_CorrectCount()
        {
            var files = new[] { "bar-0.txt", "foo-0.txt", "foo-1.txt", "foo-01.txt", "foo-0.csv" };
            foreach (var file in files)
                File.AppendAllText(Path.Combine(DirectoryName, file), ".");

            File.SetCreationTime(Path.Combine(DirectoryName, "foo-0.txt"), DateTime.Now.AddDays(-3));
            File.SetLastWriteTime(Path.Combine(DirectoryName, "foo-0.txt"), DateTime.Now.AddDays(-1));
            File.SetCreationTime(Path.Combine(DirectoryName, "foo-01.txt"), DateTime.Now.AddDays(-1));
            File.SetLastWriteTime(Path.Combine(DirectoryName, "foo-01.txt"), DateTime.Now.AddDays(-1));

            var resolver = new FilterSequenceResolver<string>
            (
                new FilterSequenceResolverArgs
                (
                    new FileLoopSequenceResolver
                    (
                        new FileLoopSequenceResolverArgs()
                        {
                            Path = DirectoryName,
                            Filters = new List<IFileFilter> { new PatternRootFilter("foo-*.txt") },
                        }
                    ),
                    new DateTimeMoreThanOrEqual(false, new LiteralScalarResolver<DateTime>(DateTime.Now.AddDays(-2))),
                    new NativeTransformer<string>(null, null, new FileToCreationDateTime(DirectoryName))
                )
            );

            var result = resolver.Execute();
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result, Has.Member($"foo-01.txt"));
            Assert.That(result, Has.Member($"foo-1.txt"));
        }
    }
}

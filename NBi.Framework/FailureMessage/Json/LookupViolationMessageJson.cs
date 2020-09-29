﻿using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Framework.FailureMessage.Common;
using NBi.Framework.FailureMessage.Common.Helper;
using NBi.Framework.FailureMessage.Json.Helper;
using NBi.Framework.Sampling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage.Json
{
    abstract class LookupViolationMessageJson : LookupViolationMessage<JsonWriter>
    {
        private readonly StringBuilder sbReference = new StringBuilder();
        private readonly StringBuilder sbCandidate = new StringBuilder();
        private readonly StringBuilder sbAnalysis = new StringBuilder();

        public LookupViolationMessageJson(IDictionary<string, ISampler<DataRow>> samplers)
            : base(samplers)
        {
            reference = new JsonTextWriter(new StringWriter(sbReference));
            candidate = new JsonTextWriter(new StringWriter(sbCandidate));
            analysis = new JsonTextWriter(new StringWriter(sbAnalysis));
        }


        protected override void RenderStandardTable(IEnumerable<DataRow> rows, IEnumerable<ColumnMetadata> metadata, ISampler<DataRow> sampler, string title, JsonWriter writer)
        {
            sampler.Build(rows);
            var tableHelper = new StandardTableHelperJson(rows, metadata, sampler);
            tableHelper.Render(writer);
        }

        public override string RenderMessage()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("timestamp");
                writer.WriteValue(DateTime.Now);
                if (!string.IsNullOrEmpty(sbReference.ToString()))
                {
                    writer.WritePropertyName(ReferenceName);
                    writer.WriteRawValue(sbReference.ToString());
                }
                if (!string.IsNullOrEmpty(sbCandidate.ToString()))
                {
                    writer.WritePropertyName(CandidateName);
                    writer.WriteRawValue(sbCandidate.ToString());
                }
                if (!string.IsNullOrEmpty(sbAnalysis.ToString()))
                {
                    writer.WritePropertyName("analysis");
                    writer.WriteRawValue(sbAnalysis.ToString());
                }
                writer.WriteEndObject();
                return sb.ToString();
            }
        }

        protected virtual string ReferenceName { get => "expected"; }
        protected virtual string CandidateName { get => "actual"; }

        public override string RenderReference() => sbReference.ToString();
        public override string RenderCandidate() => sbCandidate.ToString();
        public override string RenderAnalysis() => sbAnalysis.ToString();
    }
}

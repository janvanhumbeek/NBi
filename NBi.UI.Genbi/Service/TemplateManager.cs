﻿using NBi.GenbiL.Templating.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NBi.UI.Genbi.Service
{
    public class TemplateManager
    {
        private string GetTemplateFolder() => typeof(ResourcesFolder).Namespace;
        private const string TEMPLATE_DEFAULT = "ExistsDimension";
        public string Code { get; set; }

        public TemplateManager()
        {

        }

        public void Persist(string filename, string content)
        {
            using (TextWriter tw = new StreamWriter(filename))
            {
                tw.Write(content);
            }
        }

        public string[] GetEmbeddedLabels()
        {
            var resources = typeof(ResourcesFolder).Assembly.GetManifestResourceNames();
            IEnumerable<string> labels = resources.Where(t => t.StartsWith(GetTemplateFolder()) && t.EndsWith(".txt")).ToList();
            labels = labels.Select(t => t.Replace($"{GetTemplateFolder()}.", ""));
            labels = labels.Select(t => t.Substring(0, t.Length - 4));
            labels = labels.Select(t => SplitCamelCase(t));
            return labels.ToArray();
        }

        private static string SplitCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        public string GetDefaultContent()
        {
            return GetEmbeddedTemplate(TEMPLATE_DEFAULT);
        }

        public string GetEmbeddedTemplate(string resourceName)
        {
            var value = string.Empty;           //Template
            using (var stream = typeof(ResourcesFolder).Assembly.GetManifestResourceStream($"{GetTemplateFolder()}.{resourceName}.txt"))
            {
                if (stream == null)
                    throw new ArgumentOutOfRangeException($"{resourceName}");
                using (var reader = new StreamReader(stream))
                    value = reader.ReadToEnd();
            }
            Code = value;
            return value;
        }

        public string GetExternalTemplate(string resourceName)
        {
            var tpl = string.Empty;           //Template
            using (var stream = new StreamReader(resourceName))
            {
                tpl = stream.ReadToEnd();
            }
            Code = tpl;
            return tpl;
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;
using System.Diagnostics;
using NBi.Extensibility;

namespace NBi.Core.Decoration.IO.Commands
{
    class CopyCommand : IDecorationCommand
    {
        private readonly ICopyCommandArgs args;

        public CopyCommand(ICopyCommandArgs args) => this.args = args;

        public void Execute()
        {
            var sourceFullPath = PathExtensions.CombineOrRoot(args.BasePath, args.SourcePath.Execute(), args.SourceName.Execute());
            var destinationFullPath = PathExtensions.CombineOrRoot(args.BasePath, args.DestinationPath.Execute(), args.DestinationName.Execute());
            Execute(sourceFullPath, destinationFullPath);
        }

        internal void Execute(string original, string destination)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Copying file from '{original}' to '{destination}' ...");
            if (!File.Exists(original))
                throw new ExternalDependencyNotFoundException(original);

            var destinationFolder = Path.GetDirectoryName(destination);
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            File.Copy(original, destination, true);
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"File copied from '{original}' to '{destination}'");
        }
    }
}

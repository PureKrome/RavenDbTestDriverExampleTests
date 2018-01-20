using System;
using System.IO;
using Raven.TestDriver;

namespace RavenDbTestDriverExampleTests
{
    public class MyRavenDbLocator : RavenServerLocator
    {
        private string _serverPath;
        private string _command = "dotnet";
        private string _arguments;

        public override string ServerPath => "C:\\RavenDb\\RavenDB-4.0.0-rc-40025-windows-x64\\Server\\Raven.Server.dll";

        public override string Command => _command;
        public override string CommandArguments => ServerPath;
    }
}
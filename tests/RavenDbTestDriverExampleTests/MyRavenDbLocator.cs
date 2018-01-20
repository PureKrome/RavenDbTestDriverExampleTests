using System;
using System.IO;
using Raven.TestDriver;

namespace RavenDbTestDriverExampleTests
{
    public class CustomRavenServerLocator : RavenServerLocator
    {
        private string _serverPath;

        public override string ServerPath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_serverPath))
                {
                    return _serverPath;
                }

                // Lets look for the exe or dll via (in order):
                // 1 - environmental variable.
                // 2 - hardcoded location.


                var environmentalVariable = Environment.GetEnvironmentVariable("RavenServerTestPath");
                var serverPath = !string.IsNullOrWhiteSpace(environmentalVariable)
                                     ? environmentalVariable
                                     : "C:\\RavenDb\\RavenDB-4.0.0-rc-40025-windows-x64\\Server\\Raven.Server.dll";

                // Given a serverPath - does something actually exist?
                if (File.Exists(serverPath))
                {
                    _serverPath = serverPath;
                    return _serverPath;
                }

                throw new Exception($"Failed to find the location of a raven.server.exe/dll. Tried checking for: {serverPath}. Unable to run any tests :/");
            }
        }

        public override string Command => "dotnet";
        //public override string CommandArguments => ServerPath + " --ServerUrl=http://127.0.0.1:0 --RunInMemory=true";
        public override string CommandArguments => ServerPath;
    }
}
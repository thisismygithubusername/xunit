using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web.UI;

namespace Xunit
{
    /// <summary>
    /// Default implementation of <see cref="IXunit1Executor"/>. Creates a remote app domain for the test
    /// assembly to be loaded into. Disposing of the executor releases the app domain.
    /// </summary>
    public class Xunit1Executor : IXunit1Executor
    {
        private readonly object executor;

        readonly RemoteAppDomainManager appDomain;
        readonly AssemblyName xunitAssemblyName;
        readonly string xunitAssemblyPath;

        public Xunit1Executor(string testAssemblyFileName, string configFileName, bool shadowCopy)
        {
            appDomain = new RemoteAppDomainManager(testAssemblyFileName, configFileName, shadowCopy);
            xunitAssemblyPath = GetXunitAssemblyPath(testAssemblyFileName);
            xunitAssemblyName = AssemblyName.GetAssemblyName(xunitAssemblyPath);
            executor = CreateObject("Xunit.Sdk.Executor", testAssemblyFileName);
            TestFrameworkDisplayName = String.Format(CultureInfo.InvariantCulture, "xUnit.net {0}", AssemblyName.GetAssemblyName(xunitAssemblyPath).Version);
        }

        public string TestFrameworkDisplayName { get; private set; }

        object CreateObject(string typeName, params object[] args)
        {
            return appDomain.CreateObject<object>(xunitAssemblyName.FullName, typeName, args);
        }

        public void Dispose()
        {
            appDomain.SafeDispose();
        }

        public void EnumerateTests(ICallbackEventHandler handler)
        {
            CreateObject("Xunit.Sdk.Executor+EnumerateTests", executor, handler);
        }

        static string GetXunitAssemblyPath(string testAssemblyFileName)
        {
            Guard.FileExists("testAssemblyFileName", testAssemblyFileName);

            var xunitPath = Path.Combine(Path.GetDirectoryName(testAssemblyFileName), "xunit.dll");
            Guard.FileExists("testAssemblyFileName", xunitPath);

            return xunitPath;
        }

        public void RunTests(string type, List<string> methods, ICallbackEventHandler handler)
        {
            CreateObject("Xunit.Sdk.Executor+RunTests", executor, type, methods, handler);
        }
    }
}
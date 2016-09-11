#tool "nuget:?package=Fixie"
#addin "nuget:?package=Cake.Watch"

#r "./Cake.Refactor.Tests/bin/Debug/Cake.Refactor.Tests.dll"
// #r "./Cake.Refactor.Tests/bin/Debug/Microsoft.CodeAnalysis.CSharp.dll"
// #r "./Cake.Refactor.Tests/bin/Debug/Microsoft.CodeAnalysis.CSharp.Workspaces.dll"

using Cake.Refactor.Tests;

Task("Load").Does(() => {
    var cls = new LoadSpec();
    cls.ShouldReadProjectFile();
});

var target = Argument("target", "default");
RunTarget(target);
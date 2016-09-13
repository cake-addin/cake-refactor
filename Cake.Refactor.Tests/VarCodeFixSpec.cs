
using Microsoft.CodeAnalysis.MSBuild;
using System.Linq;
using Xunit;

namespace Cake.Refactor.Tests
{
    public class VarCodeFixSpec
    {
        [Fact]
        public void ShouldRemoveAllVars()
        {
            var project = @"Z:\Source\github\cake-addin\Cake.Refactor\Cake.Refactor.TestProject\Cake.Refactor.TestProject.csproj";
            var codeFix = new VarCodeFix(new VarSettings { ProjectPath = project });
            codeFix.Fix();
        }
    }
}

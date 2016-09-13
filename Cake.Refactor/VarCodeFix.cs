using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.IO;
using System.Linq;

namespace Cake.Refactor
{
    public class VarSettings
    {
        public string ProjectPath { set; get; }
    }

    public class VarCodeFix
    {
        private readonly VarSettings _settings;
        public VarCodeFix(VarSettings settings)
        {
            _settings = settings;
        }

        public void Fix()
        {
            var msWorkspace = MSBuildWorkspace.Create();
            var project = msWorkspace.OpenProjectAsync(_settings.ProjectPath).Result;
            var files = project.Documents.Select(x => x.FilePath).Where(x => x.EndsWith(".cs")).ToList();
            var rewriterPath = @"Z:\Source\github\cake-addin\Cake.Refactor\Cake.Refactor\TypeInferenceRewriter.cs";

            files.ForEach(file => {
                var compilation = Compile(file, rewriterPath);

                foreach (var sourceTree in compilation.SyntaxTrees)
                {
                    var model = compilation.GetSemanticModel(sourceTree);
                    var rewriter = new TypeInferenceRewriter(model);
                    var newSource = rewriter.Visit(sourceTree.GetRoot());
                    if (newSource != sourceTree.GetRoot())
                    {
                        if (!sourceTree.FilePath.Contains("TypeInferenceRewriter.cs"))
                        {
                            File.WriteAllText(sourceTree.FilePath, newSource.ToFullString());
                        }
                    }
                }
            });
        }

        public Compilation Compile(string programPath, string rewriterPath)
        {
            var programText = File.ReadAllText(programPath);
            var programTree = CSharpSyntaxTree.ParseText(programText)
                .WithFilePath(programPath);
            var rewriterText = File.ReadAllText(rewriterPath);
            var rewriterTree = CSharpSyntaxTree.ParseText(rewriterText)
                .WithFilePath(rewriterPath);
            SyntaxTree[] sourceTrees = { programTree, rewriterTree };

            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var codeAnalysis = MetadataReference.CreateFromFile(typeof(SyntaxTree).Assembly.Location);
            var csharpCodeAnalysis = MetadataReference.CreateFromFile(typeof(CSharpSyntaxTree).Assembly.Location);

            MetadataReference[] references = { mscorlib, codeAnalysis, csharpCodeAnalysis };

            return CSharpCompilation.Create("TransformationCS",
                sourceTrees,
                references,
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }
    }
}
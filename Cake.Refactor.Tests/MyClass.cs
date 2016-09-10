﻿using System;
using Microsoft.CodeAnalysis.MSBuild;
               
namespace Cake.Refactor.Tests {
	public class MyClass {

		public void ShouldReadProjectFile() {
			var projectPath = "../../LearnRoslynNow.Tests.csproj";
			var msWorkspace = MSBuildWorkspace.Create();
			var project = msWorkspace.OpenProjectAsync(projectPath).Result;
			var names = project.Documents.Select(x => x.Name).ToList();
		}

	}
}
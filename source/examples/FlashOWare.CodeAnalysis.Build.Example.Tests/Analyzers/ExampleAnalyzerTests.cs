using FlashOWare.CodeAnalysis.Build.Example.Analyzers;
using Verify = Microsoft.CodeAnalysis.CSharp.Testing.CSharpAnalyzerVerifier<FlashOWare.CodeAnalysis.Build.Example.Analyzers.ExampleAnalyzer, Microsoft.CodeAnalysis.Testing.DefaultVerifier>;

namespace FlashOWare.CodeAnalysis.Build.Example.Tests.Analyzers;

[TestClass]
public sealed class ExampleAnalyzerTests
{
	[TestMethod]
	public async Task Initialize_NoNamedTypes_ReportsNoDiagnostics()
	{
		await new CSharpAnalyzerTest<ExampleAnalyzer, DefaultVerifier>
		{
			TestState =
			{
				Sources =
				{
					"",
				},
			},
		}.RunAsync(CancellationToken.None);
	}

	[TestMethod]
	public async Task Initialize_WithNamedTypes_ReportsDiagnostics()
	{
		await new CSharpAnalyzerTest<ExampleAnalyzer, DefaultVerifier>
		{
			TestState =
			{
				Sources =
				{
					"""
					namespace FlashOWare.CodeAnalysis.Build.Example.Package.MyNamespace;

					public sealed class {|#0:MyClass|#0};
					""",
					"""
					namespace FlashOWare.CodeAnalysis.Build.Example.Package.MyNamespace;

					public readonly struct {|#1:MyStruct|#1};
					""",
				},
				ExpectedDiagnostics =
				{
					Verify.Diagnostic("ExampleId").WithSeverity(DiagnosticSeverity.Warning).WithMessage("Example MessageFormat: 'MyClass'").WithLocation(0),
					Verify.Diagnostic("ExampleId").WithSeverity(DiagnosticSeverity.Warning).WithMessage("Example MessageFormat: 'MyStruct'").WithLocation(1),
				},
			},
		}.RunAsync(CancellationToken.None);
	}
}

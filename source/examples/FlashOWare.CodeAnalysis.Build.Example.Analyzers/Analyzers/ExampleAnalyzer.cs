namespace FlashOWare.CodeAnalysis.Build.Example.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ExampleAnalyzer : DiagnosticAnalyzer
{
	private static readonly DiagnosticDescriptor Rule = new("ExampleId", "Example Title", "Example MessageFormat: '{0}'", "ExampleCategory", DiagnosticSeverity.Warning, true, "Example Description.", @"https://github.com/FlashOWare/FlashOWare.CodeAnalysis.Build", [WellKnownDiagnosticTags.NotConfigurable]);

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
		context.EnableConcurrentExecution();

		context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
	}

	private static void AnalyzeSymbol(SymbolAnalysisContext context)
	{
		var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

		var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
		context.ReportDiagnostic(diagnostic);
	}
}

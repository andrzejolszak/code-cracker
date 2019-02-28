using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using P = Microsoft.CodeAnalysis.CSharp.PatternMatching.Pattern;

namespace PerformanceAllocationAnalyzers.CSharp.Performance
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class RemoveWhereWhenItIsPossibleAnalyzer : DiagnosticAnalyzer
    {
        internal const string Title = "You should remove the 'Where' invocation when it is possible.";
        internal const string MessageFormat = "You can remove 'Where' moving the predicate to '{0}'.";
        internal const string Category = SupportedCategories.Performance;

        internal static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId.RemoveWhereWhenItIsPossible.ToDiagnosticId(),
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description,
            helpLinkUri: HelpLink.ForDiagnostic(DiagnosticId.RemoveWhereWhenItIsPossible));

        private const string Description = "When a linq operator support a predicate parameter it should be used instead of "
                    + "using 'Where' followed by the operator";

        private static readonly string[] supportedMethods = new[] {
            "First",
            "FirstOrDefault",
            "Last",
            "LastOrDefault",
            "Any",
            "Single",
            "SingleOrDefault",
            "Count"
        };

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) =>
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.InvocationExpression);

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            if (context.IsGenerated()) return;
            var node = (InvocationExpressionSyntax)context.Node;
            IdentifierNameSyntax name = null;
            var p0 = P.InvocationExpression(expression: P.MemberAccessExpression(name: P.IdentifierName("Where", action: x => name = x)));
            if (!p0.IsMatch(context.Node))
            {
                return;
            }

            bool match1 = P.InvocationExpression(
                argumentList: P.ArgumentList(
                    P.Argument(
                        expression: P.SimpleLambdaExpression())))
                .IsMatch(node);

            bool match2 = P.InvocationExpression(argumentList: P.ArgumentList(
                P.Argument(
                    expression: P.ParenthesizedLambdaExpression(
                        parameterList: P.ParameterList(
                            P.Parameter())))))
                .IsMatch(node);

            if (!(match1 || match2))
            {
                return;
            }

            var nextMethodInvoke = node.Parent.
                FirstAncestorOrSelf<InvocationExpressionSyntax>();
            if (nextMethodInvoke == null) return;

            var candidate = GetNameOfTheInvokedMethod(nextMethodInvoke)?.ToString();
            if (!supportedMethods.Contains(candidate)) return;

            if (nextMethodInvoke.ArgumentList.Arguments.Any()) return;
            var properties = new Dictionary<string, string> { { "methodName", candidate } }.ToImmutableDictionary();
            var diagnostic = Diagnostic.Create(Rule, name.GetLocation(), properties, candidate);
            context.ReportDiagnostic(diagnostic);
        }

        private static SimpleNameSyntax GetNameOfTheInvokedMethod(InvocationExpressionSyntax invoke) =>
        invoke.ChildNodes().OfType<MemberAccessExpressionSyntax>().FirstOrDefault()?.Name;
    }
}
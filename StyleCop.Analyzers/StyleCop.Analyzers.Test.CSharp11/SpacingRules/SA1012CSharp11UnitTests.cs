﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp10.SpacingRules;
    using StyleCop.Analyzers.Test.Verifiers;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1012OpeningBracesMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public class SA1012CSharp11UnitTests : SA1012CSharp10UnitTests
    {
        [Fact]
        [WorkItem(3509, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3509")]
        public async Task TestPropertyPatternInsideListPatternAsync()
        {
            var testCode = @"
class C
{
    void M(string[] a)
    {
        _ = a is [ {|#0:{|} Length: 1 }];
        _ = a is [{ Length: 0 },{|#1:{|} Length: 1 }];
    }
}
";

            var fixedCode = @"
class C
{
    void M(string[] a)
    {
        _ = a is [{ Length: 1 }];
        _ = a is [{ Length: 0 }, { Length: 1 }];
    }
}
";

            await new CSharpTest()
            {
                ReferenceAssemblies = GenericAnalyzerTest.ReferenceAssembliesNet50,
                TestCode = testCode,
                ExpectedDiagnostics =
                {
                    // Opening brace should not be preceded by a space
                    Diagnostic().WithLocation(0).WithArguments(" not", "preceded"),

                    // Opening brace should be preceded by a space
                    Diagnostic().WithLocation(1).WithArguments(string.Empty, "preceded"),
                },
                FixedCode = fixedCode,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}

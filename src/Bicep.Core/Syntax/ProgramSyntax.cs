// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bicep.Core.Parser;
using Bicep.Core.Diagnostics;
using Bicep.Core.Navigation;

namespace Bicep.Core.Syntax
{
    public class ProgramSyntax : SyntaxBase
    {
        public ProgramSyntax(IEnumerable<SyntaxBase> children, Token endOfFile, IEnumerable<Diagnostic> lexerDiagnostics)
        {
            this.Children = children.ToImmutableArray();
            this.EndOfFile = endOfFile;
            this.LexerDiagnostics = lexerDiagnostics.ToImmutableArray();
        }

        public ImmutableArray<SyntaxBase> Children { get; }
        
        public Token EndOfFile { get; }

        public ImmutableArray<Diagnostic> LexerDiagnostics { get; }

        public override void Accept(ISyntaxVisitor visitor)
            => visitor.VisitProgramSyntax(this);

        public override TextSpan Span => this.Children.Any()
            ? TextSpan.Between(this.Children.First(), this.EndOfFile)
            : this.EndOfFile.Span;

        public TextSpan FullSpan => TextSpan.Between(new TextSpan(0, 0), this.EndOfFile);

        // TODO: Should we have a DeclarationSyntax abstract class?
        public IEnumerable<SyntaxBase> Declarations => this.Children.Where(c => c is INamedDeclarationSyntax);
    }
}

// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using System.Collections.Generic;
using Bicep.Core.SemanticModel;
using Bicep.Core.Syntax;

namespace Bicep.Decompiler.Visitors
{
    public class ResourceDependencyFinderVisitor : SyntaxVisitor
    {
        private readonly Core.SemanticModel.SemanticModel semanticModel;
        private readonly HashSet<DeclaredSymbol> resourceDependencies;

        private ResourceDependencyFinderVisitor(Core.SemanticModel.SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
            this.resourceDependencies = new HashSet<DeclaredSymbol>();
        }

        public static HashSet<DeclaredSymbol> GetResourceDependencies(Core.SemanticModel.SemanticModel semanticModel, SyntaxBase syntax)
        {
            var visitor = new ResourceDependencyFinderVisitor(semanticModel);
            visitor.Visit(syntax);

            return visitor.resourceDependencies;
        }

        public override void VisitVariableAccessSyntax(VariableAccessSyntax syntax)
        {
            var symbol = semanticModel.GetSymbolInfo(syntax);
            switch (symbol)
            {
                case ResourceSymbol resourceSymbol:
                    resourceDependencies.Add(resourceSymbol);
                    return;
                case ModuleSymbol moduleSymbol:
                    resourceDependencies.Add(moduleSymbol);
                    return;
                case VariableSymbol variableSymbol:
                    Visit(variableSymbol.DeclaringSyntax);
                    return;
                default:
                    return;
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.CSharp.PatternMatching
{
    public class SingleStatementPattern : StatementPattern
    {
        private readonly StatementPattern _statement;
        private readonly Action<StatementSyntax> _action;

        internal SingleStatementPattern(StatementPattern statement, Action<StatementSyntax> action)
        {
            _statement = statement;
            _action = action;
        }

        internal override bool Test(SyntaxNode node, SemanticModel semanticModel)
        {
            if (node is BlockSyntax block)
                return block.Statements.Count == 1 && Test(block.Statements[0], semanticModel);

            if (node is StatementSyntax statement)
                return _statement == null || _statement.Test(statement, semanticModel);

            return false;
        }

        internal override void RunCallback(SyntaxNode node, SemanticModel semanticModel)
        {
            if (node is BlockSyntax block)
            {
                RunCallback(block.Statements[0], semanticModel);
            }
            else
            {
                var statement = (StatementSyntax)node;

                _statement?.RunCallback(statement, semanticModel);
                _action?.Invoke(statement);
            }
        }
    }
}

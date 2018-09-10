using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

namespace SuperB
{
    public class FindTypesVisitor<Result> : SuperBBaseVisitor<Result>, ISuperBVisitor<Result>
    {
        IList<int> LineNumbers = new List<int>();
        bool StartOfLine = true;
        SymbolTable symbols = null;

        public FindTypesVisitor(SymbolTable symbolTable) => symbols = symbolTable;

        public override Result VisitIdent([NotNull] SuperBParser.IdentContext context)
        {
            return base.VisitIdent(context);
        }

        public override Result VisitIdentifier([NotNull] SuperBParser.IdentifierContext context)
        {
            return base.VisitIdentifier(context);
        }

        public override Result VisitExpr([NotNull] SuperBParser.ExprContext context)
        {
            return base.VisitExpr(context);
        }

        public override Result VisitAssignment([NotNull] SuperBParser.AssignmentContext context)
        {
            dynamic firstOperandType = Visit(context.children[0]);
            dynamic secondOperandTypeOpType = Visit(context.children[0]);
            if (firstOperandType != secondOperandTypeOpType) throw new ParseError("Incompatible types");
            else return firstOperandType;
        }

        public override Result VisitTerminal([NotNull] ITerminalNode node)
        {
            CommonToken token = (CommonToken)node.Payload;
            if (token.Type == SuperBLexer.Integer && StartOfLine)
            {
                LineNumbers.Add(Int32.Parse(token.Text));
                token.Type = SuperBLexer.LineNumber;
            }
            StartOfLine = (token.Text == "\n") ? true : false;

            if (token.Type == SuperBLexer.ID)
            {
               Symbol sym = symbols.ReadSymbol(token.Text, "~GLOBAL");
                return (Result)Convert.ChangeType(token.Type, typeof(int));
            }
            return base.VisitTerminal(node);
        }

        public override Result VisitLine([NotNull] SuperBParser.LineContext context)
        {
            return base.VisitLine(context);
        }

        public override Result VisitStmtlist([NotNull] SuperBParser.StmtlistContext context)
        {
            return base.VisitStmtlist(context);
        }

        public override Result VisitStmt([NotNull] SuperBParser.StmtContext context)
        {
            return base.VisitStmt(context);
        }

        public override Result VisitSeparator([NotNull] SuperBParser.SeparatorContext context)
        {
            return base.VisitSeparator(context);
        }
    }
}

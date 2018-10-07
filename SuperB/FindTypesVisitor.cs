using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

namespace SuperB
{
    public class FindTypesVisitor<Result> : SuperBBaseVisitor<Result>, ISuperBVisitor<Result>
    {
        private IList<int> LineNumbers = new List<int>();
        private bool StartOfLine = true;
        private SymbolTable symbols = null;

        public FindTypesVisitor(SymbolTable symbolTable)
        {
            symbols = symbolTable;
        }

        public override Result VisitExpr([NotNull] SuperBParser.ExprContext context)
        {
            return base.VisitExpr(context);
        }

        public override Result VisitAssignment([NotNull] SuperBParser.AssignmentContext context)
        {
            dynamic firstOperandType = Visit(context.children[0]);
            dynamic secondOperandTypeOpType = Visit(context.children[2]);
            if (firstOperandType != secondOperandTypeOpType)
            {
                throw new ParseError("Incompatible types");
            }
            ((SuperBToken)context.start).EvaluatedType = (int)firstOperandType;
                return firstOperandType;
        }

        public override Result VisitStmtlist([NotNull] SuperBParser.StmtlistContext context)
        {
            Result res = default;
            try
            {
                res = base.VisitStmtlist(context);
            }
            catch (ParseError pe)
            { }

            return res;
        }

        public override Result VisitTerminal([NotNull] ITerminalNode node)
        {
            CommonToken token = (CommonToken)node.Payload;
            if (token.Type == SuperBLexer.Integer && StartOfLine)
            {
                LineNumbers.Add(int.Parse(token.Text));
                token.Type = SuperBLexer.LineNumber;
            }
            StartOfLine = (token.Text == "\n" || token.Text == "\r\n") ? true : false;

            if (token.Type == SuperBLexer.ID)
            {
                Symbol sym = symbols.ReadSymbol(token.Text, "~GLOBAL");
                return (Result)Convert.ChangeType(sym.Type, typeof(int));
            }
            return (Result)Convert.ChangeType(token.Type, typeof(int));

            return base.VisitTerminal(node);
        }

    }
}

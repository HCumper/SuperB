using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

namespace SuperB
{
    public class FindTypesVisitor<TResult> : SuperBBaseVisitor<TResult>, ISuperBVisitor<TResult>
    {
        private readonly IList<int> LineNumbers = new List<int>();
        private bool StartOfLine = true;
        private readonly SymbolTable symbols = null;
        private string LocalScope = "~GLOBAL";
        public FindTypesVisitor(SymbolTable symbolTable)
        {
            symbols = symbolTable;
        }

        public override TResult VisitExpr(SuperBParser.ExprContext context)
        {
            return base.VisitExpr(context);
        }

        public override TResult VisitAssignment(SuperBParser.AssignmentContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            dynamic firstOperandType = Visit(context.children[0]);
            dynamic secondOperandTypeOpType = Visit(context.children[2]);
            if (firstOperandType != secondOperandTypeOpType)
            {
                throw new ParseErrorException("Incompatible types");
            }
            ((SuperBToken)context.start).EvaluatedType = (int)firstOperandType;
            return firstOperandType;
        }

        public override TResult VisitStmtlist([NotNull] SuperBParser.StmtlistContext context)
        {
            TResult res = default;
            try
            {
                res = base.VisitStmtlist(context);
            }
            catch (ParseErrorException pe)
            { }

            return res;
        }

        public override TResult VisitTerminal([NotNull] ITerminalNode node)
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
                Symbol sym = symbols.ReadSymbol(token.Text, LocalScope);
                if (sym == null)
                {
                    sym = symbols.ReadSymbol(token.Text, "~GLOBAL");
                }

                return (TResult)Convert.ChangeType(sym.Type, typeof(int));
            }
            return (TResult)Convert.ChangeType(token.Type, typeof(int));
        }

        public override TResult VisitProcheader([NotNull] SuperBParser.ProcheaderContext context)
        {
            var node = (CommonToken)context.children[1].GetChild(0).Payload;
            LocalScope = node.Text;
            return default;
        }

        public override TResult VisitFuncheader([NotNull] SuperBParser.FuncheaderContext context)
        {
            var node = (CommonToken)context.children[1].GetChild(0).Payload;
            LocalScope = node.Text;
            return default;
        }

        public override TResult VisitProc([NotNull] SuperBParser.ProcContext context)
        {
            var baseResult = base.VisitProc(context);
            LocalScope = "~GLOBAL";
            return baseResult;
        }

        public override TResult VisitFunc([NotNull] SuperBParser.FuncContext context)
        {
            var baseResult = base.VisitFunc(context);
            LocalScope = "~GLOBAL";
            return baseResult;
        }

    }
}
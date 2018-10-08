using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;

namespace SuperB
{
    public class FindTypesVisitor<Result> : SuperBBaseVisitor<Result>
    {
        // ReSharper disable once CollectionNeverQueried.Local
        private readonly IList<int> _lineNumbers = new List<int>();
        private bool _startOfLine = true;
        private readonly SymbolTable.SymbolTable _symbols;
        private string _scope = "~GLOBAL";

        public FindTypesVisitor(SymbolTable.SymbolTable symbolTable) => _symbols = symbolTable;

        public override Result VisitBinary(SuperBParser.BinaryContext context)
        {
            dynamic firstOperandType = Visit(context.children[0]);
            dynamic secondOperandTypeOpType = Visit(context.children[2]);
            if (firstOperandType != secondOperandTypeOpType) throw new ParseError("Incompatible types");
            ((SuperBToken) context.start).EvaluatedType = (int) firstOperandType;
            return firstOperandType;

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
            catch (ParseError)
            { }

            return res;
        }

        public override Result VisitTerminal([NotNull] ITerminalNode node)
        {
            CommonToken token = (CommonToken)node.Payload;
            if (token.Type == SuperBLexer.Integer && _startOfLine)
            {
                _lineNumbers.Add(int.Parse(token.Text));
                token.Type = SuperBLexer.LineNumber;
            }
            _startOfLine = (token.Text == "\n" || token.Text == "\r\n");

            if (token.Type == SuperBLexer.ID)
            {
                var sym = _symbols.ReadAnySymbol(token.Text, token.Text != _scope ? _scope : "~GLOBAL");
                return (Result)Convert.ChangeType(sym.Type, typeof(int));
            }
            return (Result)Convert.ChangeType(token.Type, typeof(int));
        }

        public override Result VisitProc(SuperBParser.ProcContext context)
        {
            var tok = (SuperBToken)context.children[0].GetChild(1).GetChild(0).Payload;
            _scope = tok.Text;
            var result = base.VisitProc(context);
            _scope = "~GLOBAL";
            return result;
        }

        public override Result VisitFunc(SuperBParser.FuncContext context)
        {
            var tok = (SuperBToken)context.children[0].GetChild(1).GetChild(0).Payload;
            _scope = tok.Text;
            var result = base.VisitFunc(context);
            _scope = "~GLOBAL";
            return result;
        }

    }
}

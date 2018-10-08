using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using SuperB.SymbolTable;
using static SuperB.SuperBParser;

namespace SuperB
{
    public class BuildSymbolTableVisitor<TResult> : SuperBBaseVisitor<TResult>
    {
        private readonly ISet<string> _implicitInts;
        private readonly ISet<string> _implicitStrings;
        private ISet<string> _references;

        public BuildSymbolTableVisitor(SymbolTable.SymbolTable symbolTable)
        {
            SymbolTable = symbolTable;
            _implicitInts = new HashSet<string>();
            _implicitStrings = new HashSet<string>();
            _references = new HashSet<string>();
        }

        private SymbolTable.SymbolTable SymbolTable { get; }
        private string FunctionScopeName { get; set; }
        private bool FuncScopeActive { get; set; }
        public bool FirstPass { get; set; }

        public override TResult VisitTerminal(ITerminalNode node)
        {
            var funcProc = false;
            var payload = (CommonToken) node.Payload;

            if (FuncScopeActive && FirstPass || !FuncScopeActive && !FirstPass)
            {
                string localScope;
                if (FuncScopeActive && payload.Text != FunctionScopeName)
                {
                    localScope = FunctionScopeName;
                }
                else
                {
                    localScope = "~GLOBAL";
                    if (FirstPass) funcProc = true;
                }

                if (payload.Type == SuperBLexer.ID && SymbolTable.ReadSymbol(payload.Text, FunctionScopeName) == null)
                {
                    var name = payload.Text;
                    var type = ExtractType(name);
                    Symbol symbol;
                    if (localScope == FunctionScopeName)
                    {
                        var refer = _references.Contains(name);
                        symbol = new ParamSymbol(name, type, localScope, refer);
                    }
                    else
                    {
                        symbol = funcProc ? new FuncSymbol(name, DefFunc, localScope, Unknowntype) : new Symbol(name, type, localScope);
                    }

                    SymbolTable.AddSymbol(symbol);
                }
            }

            return default;
        }

        public override TResult VisitDim([NotNull] DimContext context)
        {
            var name = (CommonToken) context.children[1].Payload;
            var paramList = (ParenthesizedlistContext) context.children[2].Payload;
            var dimensions = new List<int>();
            foreach (var node in paramList.children)
                if (node is LiteralContext)
                {
                    var payload = (CommonToken) node.GetChild(0).Payload;
                    dimensions.Add(int.Parse(payload.Text));
                }

            Symbol symbol = new ArraySymbol(name.Text, ExtractType(name.Text), "~GLOBAL", dimensions);
            SymbolTable.AddSymbol(symbol);
            return base.VisitDim(context);
        }

        public override TResult VisitFuncheader([NotNull] FuncheaderContext context)
        {
            var node = (CommonToken) context.children[1].GetChild(0).Payload;
            FunctionScopeName = node.Text;
            FuncScopeActive = true;
            base.VisitFuncheader(context);
            FuncScopeActive = false;
            return default;
        }

        public override TResult VisitProcheader([NotNull] ProcheaderContext context)
        {
            var node = (CommonToken) context.children[1].GetChild(0).Payload;
            FunctionScopeName = node.Text;
            FuncScopeActive = true;
            base.VisitProcheader(context);
            FuncScopeActive = false;
            _references = new HashSet<string>();
            return default;
        }

        public override TResult VisitLoc([NotNull] LocContext context)
        {
            FuncScopeActive = true;
            base.VisitLoc(context);
            FuncScopeActive = false;
            _references = new HashSet<string>();
            return default;
        }

        public override TResult VisitImplicit([NotNull] ImplicitContext context)
        {
            if (FirstPass)
            {
                var implicitDecl = ((CommonToken) context.children[0].Payload).Text;
                var implicitType = ExtractType(implicitDecl);
                var subContext = (UnparenthesizedContext) context.children[1];
                foreach (var child in subContext.children)
                    if (child is IdentContext identContext)
                    {
                        var identCtx = identContext.children[0].Payload;
                        var terminalNode = ((IdentifierContext) identCtx).Payload.GetChild(0);
                        var name = ((TerminalNodeImpl) terminalNode).Payload.Text;
                        if (implicitType == SuperBLexer.Integer)
                            _implicitInts.Add(name);
                        else
                            _implicitStrings.Add(name);
                    }

                ((StmtlistContext) context.Parent).children = null;
            }

            return base.VisitImplicit(context);
        }

        public override TResult VisitReference([NotNull] ReferenceContext context)
        {
            if (FirstPass)
            {
                var refDecl = ((CommonToken) context.children[0].Payload).Text;
                ExtractType(refDecl);
                var subContext = (UnparenthesizedContext) context.children[1];
                foreach (var child in subContext.children)
                    if (child is IdentContext identContext)
                    {
                        var identCtx = identContext.children[0].Payload;
                        var terminalNode = ((IdentifierContext) identCtx).Payload.GetChild(0);
                        var name = ((TerminalNodeImpl) terminalNode).Payload.Text;
                        _references.Add(name);
                    }

                ((StmtlistContext) context.Parent).children = null;
            }

            return base.VisitReference(context);
        }

        public override TResult VisitAdditive([NotNull] AdditiveContext context)
        {
            return base.VisitAdditive(context);
        }

        public override TResult VisitExpr([NotNull] ExprContext context)
        {
            return base.VisitExpr(context);
        }

        public override TResult VisitLiteral([NotNull] LiteralContext context)
        {
            return base.VisitLiteral(context);
        }

        private int ExtractType(string name)
        {
            if (_implicitInts.Contains(name)) return SuperBLexer.Integer;
            if (_implicitStrings.Contains(name)) return SuperBLexer.String;
            var type = SuperBLexer.Real;
            switch (name.Substring(name.Length - 1))
            {
                case "%":
                    type = SuperBLexer.Integer;
                    break;
                case "$":
                    type = SuperBLexer.String;
                    break;
            }

            return type;
        }
    }
}
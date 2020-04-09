using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using static SuperB.SuperBParser;

namespace SuperB
{
    public class BuildSymbolTableVisitor<TResult> : SuperBBaseVisitor<TResult>, ISuperBVisitor<TResult>
    {
        private SymbolTable SymbolTable { get; set; }
        private string FunctionScopeName { get; set; }
        private bool FuncScopeActive { get; set; }

        private readonly ISet<string> ImplicitInts;
        private readonly ISet<string> ImplicitStrings;
        private ISet<string> References;
        public bool FirstPass { get; set; }

        public BuildSymbolTableVisitor(SymbolTable symbolTable)
        {
            SymbolTable = symbolTable;
            ImplicitInts = new HashSet<string>();
            ImplicitStrings = new HashSet<string>();
            References = new HashSet<string>();
        }

        public override TResult VisitTerminal([NotNull] ITerminalNode node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            bool funcProc = false;
            string localScope = "";
            var payload = (CommonToken)node.Payload;

            if (FuncScopeActive && FirstPass || !FuncScopeActive && !FirstPass)
            {
                if (FuncScopeActive && payload.Text != FunctionScopeName)
                {
                    localScope = FunctionScopeName;
                }
                else
                {
                    localScope = "~GLOBAL";
                    if (FirstPass)
                    {
                        funcProc = true;
                    }
                }
                if (payload.Type == SuperBLexer.ID && SymbolTable.ReadSymbol(payload.Text, FunctionScopeName) == null)
                {
                    var name = payload.Text;
                    int type = ExtractType(name);
                    Symbol symbol = null;
                    if (localScope == FunctionScopeName)
                    {
                        bool refer = References.Contains(name);
                        symbol = new ParamSymbol(name, type, localScope, refer);
                    }
                    else
                    {
                        if (funcProc)
                        {
                            symbol = new FuncSymbol(name, DefFunc, localScope, Unknowntype);
                        }
                        else
                        {
                            symbol = new Symbol(name, type, localScope);
                        }
                    }
                    SymbolTable.AddSymbol(symbol);
                }
            }
            return default(TResult);
        }

        public override TResult VisitDim([NotNull] SuperBParser.DimContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var paramList = (ParenthesizedlistContext)context.children[2].Payload;
            List<int> dimensions = new List<int>();
            foreach (var node in paramList.children)
            {
                if (node is LiteralContext)
                {
                    var payload = (CommonToken)node.GetChild(0).Payload;
                    dimensions.Add(Int32.Parse(payload.Text));
                }
            }
            var name = (CommonToken)context.children[1].Payload;
            Symbol symbol = new ArraySymbol(name.Text, ExtractType(name.Text), "~GLOBAL", dimensions);
            SymbolTable.AddSymbol(symbol);
            return base.VisitDim(context);
        }

        public override TResult VisitFuncheader([NotNull] FuncheaderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var node = (CommonToken)context.children[1].GetChild(0).Payload;
            FunctionScopeName = node.Text;
            FuncScopeActive = true;
            base.VisitFuncheader(context);
            FuncScopeActive = false;
            return default(TResult);
        }

        public override TResult VisitProcheader([NotNull] ProcheaderContext context)
        {
            var node = (CommonToken)context.children[1].GetChild(0).Payload;
            FunctionScopeName = node.Text;
            FuncScopeActive = true;
            base.VisitProcheader(context);
            FuncScopeActive = false;
            References = new HashSet<string>();
            return default(TResult);
        }

        public override TResult VisitLoc([NotNull] LocContext context)
        {
            FuncScopeActive = true;
            base.VisitLoc(context);
            FuncScopeActive = false;
            References = new HashSet<string>();
            return default(TResult);
        }

        public override TResult VisitImplicit([NotNull] ImplicitContext context)
        {
            if (FirstPass)
            {
                var implicitDecl = ((CommonToken)context.children[0].Payload).Text;
                var implicitType = ExtractType(implicitDecl);
                var subContext = (UnparenthesizedContext)context.children[1];
                foreach (var child in subContext.children)
                {
                    if (child is IdentContext)
                    {
                        var identCtx = ((IdentContext)child).children[0].Payload;
                        var terminalNode = ((IdentifierContext)identCtx).Payload.GetChild(0);
                        var name = ((TerminalNodeImpl)terminalNode).Payload.Text;
                        if (implicitType == SuperBLexer.Integer)
                        {
                            ImplicitInts.Add(name);
                        }
                        else
                        {
                            ImplicitStrings.Add(name);
                        }

                    }
                }
               ((StmtlistContext)(context.Parent)).children = null;
            }
            return base.VisitImplicit(context);
        }

        public override TResult VisitReference([NotNull] ReferenceContext context)
        {
            if (FirstPass)
            {
                var refDecl = ((CommonToken)context.children[0].Payload).Text;
                var refType = ExtractType(refDecl);
                var subContext = (UnparenthesizedContext)context.children[1];
                foreach (var child in subContext.children)
                {
                    if (child is IdentContext)
                    {
                        var identCtx = ((IdentContext)child).children[0].Payload;
                        var terminalNode = ((IdentifierContext)identCtx).Payload.GetChild(0);
                        var name = ((TerminalNodeImpl)terminalNode).Payload.Text;
                        References.Add(name);
                    }
                }
               ((StmtlistContext)(context.Parent)).children = null;
            }
            return base.VisitReference(context);
        }

        private int ExtractType(string name)
        {
            if (ImplicitInts.Contains(name))
            {
                return SuperBLexer.Integer;
            }

            if (ImplicitStrings.Contains(name))
            {
                return SuperBLexer.String;
            }

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
    }
}

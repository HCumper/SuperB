namespace SuperBLib

open System
open System.Collections.Generic

type Symbol = {
   Name : string
   Type : int
   Scope : string }


type SymbolTable(tree) = 
    member this.Tree = tree
    member this.table = new System.Collections.Generic.Dictionary<System.Tuple<string, string>, Symbol>()

    member public this.AddSymbol symbol =
        if not (this.table.ContainsKey(Tuple.Create(symbol.Name, symbol.Scope))) then
            let tup = System.Tuple.Create(symbol.Name, symbol.Scope)
            let kvp = new System.Collections.Generic.KeyValuePair<System.Tuple<string, string>, Symbol>(tup, symbol)
            this.table.Add(tup, symbol);

        //public Symbol ReadSymbol(string name, string scope) => _table.ContainsKey(Tuple.Create(name, scope)) ? _table[Tuple.Create(name, scope)] : null;

        //public Symbol ReadAnySymbol(string name, string scope)
        //{
        //    return _table.ContainsKey(Tuple.Create(name, scope)) ? _table[Tuple.Create(name, scope)]
        //        : _table.ContainsKey(Tuple.Create(name, "~GLOBAL")) ? _table[Tuple.Create(name, "~GLOBAL")] : null;
        //}
//public partial class SuperBBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, ISuperBVisitor<Result> {
type bstv(x:int) =
    let x = 3

type derived(x:int) = 
    inherit SuperB.SuperBBaseVisitor<int>()
    let VisitFuncHeader context =
        printfn context


    //public override Result VisitFuncheader([NotNull] FuncheaderContext context)
    //{
    //    var node = (CommonToken) context.children[1].GetChild(0).Payload;
    //    FunctionScopeName = node.Text;
    //    FuncScopeActive = true;
    //    base.VisitFuncheader(context);
    //    FuncScopeActive = false;
    //    return default;
    //}

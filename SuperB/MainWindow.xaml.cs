using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

[assembly: CLSCompliant(false)]

namespace SuperB
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    // ReSharper disable once UnusedMember.Global
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

              var filename = @"C:\Users\hcump\Source\Repos\SuperB\SuperB\q3.sb";
            //var filename = @"z:\SuperB\SuperB\q3.sb";

            var reader = File.OpenText(filename);

            ICharStream cs = new AntlrInputStream(reader);
            var factory = new SuperBTokenFactory();
            var lexer = new SuperBLexer(cs)
            {
                TokenFactory = factory
            };
            var tokens = new CommonTokenStream(lexer);

            var parser = new SuperBParser(tokens);
            //parser.TokenFactory = factory;
            IParseTree tree = parser.program();

            Debug.WriteLine(tree.ToStringTree(parser));

            var symbolTable = new SymbolTable.SymbolTable();
            var symbolTableVisitor = new BuildSymbolTableVisitor<int>(symbolTable)
            {
                FirstPass = true // Array functions and procedures
            };
            symbolTableVisitor.Visit(tree);
            symbolTableVisitor.FirstPass = false; // Everything else
            symbolTableVisitor.Visit(tree);

            var findTypesVisitor = new FindTypesVisitor<int>(symbolTable);
            findTypesVisitor.Visit(tree);


            // | Select On expr Equal(ID | literal | toexpr) : stmtlist
            //| select On ID Newline On

            /*(EndRepeat ID? | { _input.Lt(1).Type == EndDef }?)*/
            //var name scope type
            //array adds parameterlist
            //function extends array adds astnode or delegate
        }
    }
}
using Antlr4.Runtime;
using System;
using System.IO;
using System.Text;
using System.Windows;

[assembly: CLSCompliant(false)]
namespace SuperB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var t = new SymbolTable();

            //    string filename = @"C:\Users\hcump\Source\Repos\SuperB\SuperB\q3.sb";
            string filename = @"d:\temp\SuperB\SuperB\q3.sb";

            StringBuilder builder = new StringBuilder();
            StreamReader reader = File.OpenText(filename);

            ICharStream cs = new AntlrInputStream(reader);
            SuperBTokenFactory factory = new SuperBTokenFactory();
            SuperBLexer lexer = new SuperBLexer(cs);
            lexer.TokenFactory = factory;
            CommonTokenStream tokens = new CommonTokenStream(lexer);

            SuperBParser parser = new SuperBParser(tokens);
            //parser.TokenFactory = factory;
            Antlr4.Runtime.Tree.IParseTree tree = parser.program();

            System.Diagnostics.Debug.WriteLine(tree.ToStringTree(parser));

            SymbolTable symbolTable = new SymbolTable();
            BuildSymbolTableVisitor<int> symbolTableVisitor = new BuildSymbolTableVisitor<int>(symbolTable)
            {
                FirstPass = true   // Array functions and procedures
            };
            symbolTableVisitor.Visit(tree);
            symbolTableVisitor.FirstPass = false; // Everything else
            symbolTableVisitor.Visit(tree);

            FindTypesVisitor<int> findTypesVisitor = new FindTypesVisitor<int>(symbolTable);
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

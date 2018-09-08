using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            string filename = @"C:\Users\Hugh Cumper\Source\Repos\SuperB\SuperB\q3.sb";
            StringBuilder builder = new StringBuilder();
            StreamReader reader = File.OpenText(filename);

            ICharStream cs = new AntlrInputStream(reader);
            SuperBLexer lexer = new SuperBLexer(cs);
            CommonTokenStream tokens = new CommonTokenStream(lexer);

            SuperBParser parser = new SuperBParser(tokens);
            Antlr4.Runtime.Tree.IParseTree tree = parser.program();

            System.Console.WriteLine(tree.ToStringTree(parser));

            SymbolTable symbolTable = new SymbolTable();
            SuperBVisitor<int> visitor = new SuperBVisitor<int>(symbolTable);
            visitor.FirstPass = true;   // Array functions and procedures
            visitor.Visit(tree);
            visitor.FirstPass = false; // Everything else
            visitor.Visit(tree);
            System.Console.ReadKey();


               // | Select On expr Equal(ID | literal | toexpr) : stmtlist
               //| select On ID Newline On

            /*(EndRepeat ID? | { _input.Lt(1).Type == EndDef }?)*/
             //var name scope type
             //array adds parameterlist
             //function extends array adds astnode or delegate

        }
    }
}

using Antlr4.Runtime;
using Core.Common.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Diagnostics;

namespace Core.Common.Parsing
{

  class TemplateAst
  {

  }
  class TemplateVisitor : TemplateBaseVisitor<TemplateAst>
  {
    
  }
  class TemplateListener : TemplateBaseListener
  {
    public override void VisitTerminal([NotNull] ITerminalNode node)
    {
      var current = node.GetText();
      base.VisitTerminal(node);
    }
    

    public override void VisitErrorNode([NotNull] IErrorNode node)
    {
      base.VisitErrorNode(node);
    }
    public override void EnterEveryRule([NotNull] ParserRuleContext context)
    {
      var text = context.GetText();
      base.EnterEveryRule(context);
    }
  }
  [TestClass]
  public class Class1
  {
    [TestMethod]
    [TestCategory("Parsing")]
    public void Test1()
    {
      String input = "hello wordl ${eval \"{\"  /* { */ // { \n this}  1 + 2";

      AntlrInputStream stream = new AntlrInputStream(input);
      ITokenSource lexer = new TemplateLexer(stream);
      IToken token;
      List<string> tokens = new List<string>();
      while ((token = lexer.NextToken()).Type != Parser.Eof)
      {
        var text = token.Text;
        tokens.Add(text);
      }
      var tokenStream = new CommonTokenStream(lexer);
      lexer = new TemplateLexer(stream);      
      var parser = new TemplateParser(tokenStream);

      parser.BuildParseTree = true;
      var listener = new TemplateListener();
      parser.AddParseListener(listener);
      
      var context = parser.parse();
      ParseTreeWalker.Default.Walk(listener,context);
      
      ////var listener = new TemplateVisitor();
      ////ParseTreeWalker.Default.Walk(listener, context);
      var visitor = new TemplateVisitor();
      var result = visitor.Visit(context);

    }
  }
}

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TWitcher3.Core;
using TWitcher3.Core.ScriptParser;

namespace TWitcher3.Tests
{
    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void Tokenize_DecimalNumber_no_whitespace()
        {
            var lexer = new Lexer(Logger.Console);
            var tokens = lexer.Tokenize("setplayerscale 0.25 3 0.25");            
            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual("0.25", tokens[1].Value);
            Assert.AreEqual("3", tokens[2].Value);
            Assert.AreEqual("0.25", tokens[3].Value);
        }

        [TestMethod]
        public void Tokenize_DecimalNumber_no_whitespace_2()
        {
            
            var lexer = new Lexer(Logger.Console);
            var tokens = lexer.Tokenize("setplayerscale(0.25,3,0.25)");

            Assert.AreEqual(8, tokens.Count);
            Assert.AreEqual("0.25", tokens[2].Value);
            Assert.AreEqual("3", tokens[4].Value);
            Assert.AreEqual("0.25", tokens[6].Value);
        }

        [TestMethod]
        public void Tokenize_DecimalNumber_whitespace()
        {
            var lexer = new Lexer(Logger.Console);
            var tokens = lexer.Tokenize("setplayerscale 0.25 3 0.25", true);
            Assert.AreEqual(7, tokens.Count);
            Assert.AreEqual("0.25", tokens[2].Value);
            Assert.AreEqual("3", tokens[4].Value);
            Assert.AreEqual("0.25", tokens[6].Value);
        }

        [TestMethod]
        public void Tokenize_DecimalNumber_whitespace_2()
        {
            var lexer = new Lexer(Logger.Console);
            var tokens = lexer.Tokenize("setplayerscale 0.1,3,0.3", true);
            Assert.AreEqual(7, tokens.Count);
            Assert.AreEqual("0.1", tokens[2].Value);
            Assert.AreEqual("3", tokens[4].Value);
            Assert.AreEqual("0.3", tokens[6].Value);
        }

        [TestMethod]
        public void Tokenize_DecimalNumber_whitespace_3()
        {
            var lexer = new Lexer(Logger.Console);
            var tokens = lexer.Tokenize("setplayerscale .1,.3,.3", true);
            Assert.AreEqual(7, tokens.Count);
            Assert.AreEqual("0.1", tokens[2].Value);
            Assert.AreEqual("0.3", tokens[4].Value);
            Assert.AreEqual("0.3", tokens[6].Value);
        }
    }
}

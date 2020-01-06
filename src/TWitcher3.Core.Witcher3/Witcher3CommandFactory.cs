using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TWitcher3.Core.ScriptParser;

namespace TWitcher3.Core.Games.Witcher3
{
    public class Witcher3CommandFactory : IGameCommandFactory
    {
        private readonly Lexer lexer;

        public Witcher3CommandFactory(ILogger logger)
        {
            this.lexer = new Lexer(logger);
        }

        public IGameCommandReference Create(string sender, string rawCommandText)
        {
            var args = new List<string>();

            var tokens = lexer.Tokenize(rawCommandText.Trim(), true);

            var command = tokens.Next().Value;

            // additem 'item id' 123

            if (tokens.NextIs(TokenType.Whitespace))
            {
                tokens.Consume(TokenType.Whitespace);
                while (tokens.TokensAvailable)
                {
                    AddArgument(tokens, args);
                }
            }
            else if (tokens.NextIs(TokenType.LParen))
            {
                tokens.Consume(TokenType.LParen);
                while (!tokens.NextIs(TokenType.RParen) && tokens.TokensAvailable)
                {
                    AddArgument(tokens, args);
                }
                if (tokens.TokensAvailable)
                    tokens.Consume();
            }

            if (Witcher3Commands.TryGet(command, out var cmd))
            {
                return new GameCommand(
                    sender,
                    cmd.Name,
                    args.ToArray(),
                    cmd.InvokeCost,
                    cmd.TimeToLevelReduction,
                    cmd.GlobalCooldown,
                    cmd.LocalCooldown);
            }

            return null;
        }

        private static void AddArgument(TokenStream tokens, List<string> args)
        {
            while (tokens.NextIs(TokenType.Whitespace))
                tokens.ConsumeOptional(TokenType.Whitespace);

            if (tokens.NextIs(TokenType.NumberDecimal)
                || tokens.NextIs(TokenType.SingleQuoteString)
                || tokens.NextIs(TokenType.DoubleQuoteString)
                || tokens.NextIs(TokenType.Identifier))
            {
                var token = tokens.Consume();
                var value = token.Value;
                if (token.Type == TokenType.DoubleQuoteString)
                    value = $"\"{value}\"";
                if (token.Type == TokenType.SingleQuoteString)
                    value = $"'{value}'";

                args.Add(value);
                tokens.ConsumeOptional(TokenType.Whitespace);
                tokens.ConsumeOptional(TokenType.Comma);
            }
        }
    }
}
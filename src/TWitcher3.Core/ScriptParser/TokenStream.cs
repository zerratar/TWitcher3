using System.Collections;
using System.Collections.Generic;

namespace TWitcher3.Core.ScriptParser
{
    public class TokenStream : IEnumerable<Token>
    {
        private readonly IReadOnlyList<Token> tokens;
        private int index = 0;

        public TokenStream(IReadOnlyList<Token> tokens)
        {
            this.tokens = tokens;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return this.tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Token this[int index] => this.tokens[index];

        public int Count => this.tokens.Count;

        public bool EndOfStream => index >= this.tokens.Count - 1;

        public bool TokensAvailable => index < this.tokens.Count;

        public Token Current => this.EndOfStream ? null : this.tokens[index];

        public void ConsumeOptional(TokenType type)
        {
            if (!this.TokensAvailable) return;
            var next = this.tokens[index];
            if (next.Type == type)
            {
                Next();
            }
        }

        public Token Consume(string value)
        {
            if (index >= this.tokens.Count)
            {
                throw new ParserException($"Expected token value '{value}' but no more tokens available.");
            }

            var next = this.tokens[index];
            if (next.Value == value)
            {
                return Next();
            }

            var pos = next.GetSourcePosition();
            throw new ParserException($"Expected token '{value}' but found '{next.Value}' found. At line {pos.Row}, column {pos.Column}");
        }

        public Token Consume()
        {
            if (index >= this.tokens.Count)
            {
                throw new ParserException($"No more tokens available.");
            }

            return Next();
        }

        public Token Consume(TokenType type)
        {
            if (index >= this.tokens.Count)
            {
                throw new ParserException($"Expected token '{type.ToString()}' but no more tokens available.");
            }

            var next = this.tokens[index];
            if (next.Type == type)
            {
                return Next();
            }

            var pos = next.GetSourcePosition();
            throw new ParserException($"Expected token '{type.ToString()}' but found '{next.Type}' found. At line {pos.Row}, column {pos.Column}");
        }

        public bool NextIs(string value)
        {
            var next = Peek();
            if (next == null) return false;
            return next.Value == value;
        }

        public bool NextIs(TokenType type)
        {
            var next = Peek();
            if (next == null) return false;
            return next.Type == type;
        }

        public bool CurrentIs(TokenType type)
        {
            return this.Current.Type == type;
        }

        public Token Peek(int offset = 0)
        {
            if (index + offset >= this.tokens.Count)
            {
                return null;
            }

            return this.tokens[index + offset];
        }

        public Token Next()
        {
            if (index >= this.tokens.Count)
            {
                throw new ParserException($"End of stream, no more tokens to read.");
            }

            return this.tokens[index++];
        }


        public Token Skip(int count)
        {
            if (index + 1 + count >= this.tokens.Count)
            {
                throw new ParserException($"End of stream, no more tokens to read.");
            }

            index += count;
            return this.tokens[index];
        }
    }
}
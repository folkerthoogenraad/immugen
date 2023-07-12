using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Language
{
    public class MGNParser
    {
        private MGNToken[] Input;
        private int Index = 0;

        public MGNParser(IEnumerable<MGNToken> tokens)
        {
            Input = tokens.Where(token => token.Type != MGNTokenType.Whitespace).ToArray();
        }

        public bool HasNext()
        {
            return HasCurrentToken;
        }

        public MGNCommand Next()
        {
            if (CurrentToken.Type == MGNTokenType.KeywordNamespace)
            {
                NextToken();

                return new MGNNamespaceCommand(ExpectAndConsume(MGNTokenType.Identifier));
            }
            else if (CurrentToken.Type == MGNTokenType.KeywordClass)
            {
                NextToken();

                return new MGNClassCommand(ExpectAndConsume(MGNTokenType.Identifier));
            }
            else if (CurrentToken.Type == MGNTokenType.KeywordExtends)
            {
                NextToken();

                return new MGNExtendsCommand(ExpectAndConsume(MGNTokenType.Identifier));
            }

            // Property
            else if (CurrentToken.Type == MGNTokenType.Identifier)
            {
                string name = CurrentToken.Data;

                NextToken();

                ExpectAndConsume(MGNTokenType.Colon);

                var type = ParseType();

                return new MGNPropertyCommand(name, type);
            }
            else
            {
                throw new ApplicationException("Cannot parse, unknown format.");
            }
        }

        private MGNType ParseType()
        {
            string typeName = ExpectAndConsume(MGNTokenType.Identifier);

            int arrayRank = 0;

            while (HasCurrentToken && CurrentToken.Type == MGNTokenType.ArrayOpen)
            {
                NextToken();

                arrayRank++;

                ExpectAndConsume(MGNTokenType.ArrayClose);
            }

            // TODO nested types, generics, etc
            return new MGNType(typeName, arrayRank);
        }

        private string ExpectAndConsume(MGNTokenType type)
        {
            if(CurrentToken.Type != type)
            {
                UnexpectedToken();
            }

            string data = CurrentToken.Data;

            NextToken();

            return data;
        }

        private void UnexpectedToken()
        {
            throw new ApplicationException("Unexpected token " + CurrentToken);
        }

        private MGNToken CurrentToken => Input[Index];

        private bool HasCurrentToken => Index < Input.Length;

        private void NextToken()
        {
            Index++;
        }

        public static IEnumerable<MGNCommand> Parse(string input)
        {
            var tokens = MGNLexer.Lex(input);

            return Parse(tokens);
        }

        public static IEnumerable<MGNCommand> Parse(IEnumerable<MGNToken> input)
        {
            var parser = new MGNParser(input);

            while (parser.HasNext())
            {
                yield return parser.Next();
            }
        }

    }
}

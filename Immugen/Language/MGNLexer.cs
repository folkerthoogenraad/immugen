using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Language
{
    public class MGNLexer
    {
        private string Input { get; set; }
        private int Index = 0;

        public MGNLexer(string input)
        {
            Input = input;
        }

        public MGNToken Next()
        {
            if (IsWhitespace(CurrentCharacter))
            {
                StringBuilder builder = new StringBuilder();

                while (HasCurrentCharacter && IsWhitespace(CurrentCharacter))
                {
                    builder.Append(CurrentCharacter);

                    NextCharacter();
                }

                return new MGNToken(MGNTokenType.Whitespace, builder.ToString());
            }
            else if (IsLetter(CurrentCharacter))
            {
                StringBuilder builder = new StringBuilder();

                while (HasCurrentCharacter && IsLetter(CurrentCharacter))
                {
                    builder.Append(CurrentCharacter);

                    NextCharacter();
                }

                string data = builder.ToString();

                if (data == "class")
                {
                    return new MGNToken(MGNTokenType.KeywordClass, builder.ToString());
                }
                else if (data == "namespace")
                {
                    return new MGNToken(MGNTokenType.KeywordNamespace, builder.ToString());
                }
                else if (data == "extends")
                {
                    return new MGNToken(MGNTokenType.KeywordExtends, builder.ToString());
                }
                else if (data == "element")
                {
                    return new MGNToken(MGNTokenType.KeywordElement, builder.ToString());
                }

                return new MGNToken(MGNTokenType.Identifier, builder.ToString());
            }
            else if (CurrentCharacter == '[')
            {
                NextCharacter();

                return new MGNToken(MGNTokenType.ArrayOpen, "[");
            }
            else if (CurrentCharacter == ']')
            {
                NextCharacter();

                return new MGNToken(MGNTokenType.ArrayClose, "]");
            }
            else if (CurrentCharacter == ':')
            {
                NextCharacter();

                return new MGNToken(MGNTokenType.Colon, ":");
            }
            else
            {
                throw new ApplicationException("Cannot parse file");
            }
        }

        public bool HasNext()
        {
            return HasCurrentCharacter;
        }

        public static IEnumerable<MGNToken> Lex(string input)
        {
            var lexer = new MGNLexer(input);

            while (lexer.HasNext())
            {
                yield return lexer.Next();
            }
        }

        private bool IsWhitespace(char ch)
        {
            return ch == ' ' || ch == '\n' || ch == '\t' || ch == '\r';
        }
        private bool IsLetter(char ch)
        {
            return (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || ch == '_' || (ch >= '0' && ch <= '9');
        }

        private char CurrentCharacter => Input[Index];

        private bool HasCurrentCharacter => Index < Input.Length;

        private void NextCharacter()
        {
            Index++;
        }
    }
}

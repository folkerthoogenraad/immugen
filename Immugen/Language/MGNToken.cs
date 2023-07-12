using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Language
{
    public enum MGNTokenType
    {
        KeywordNamespace,
        KeywordClass,
        KeywordExtends,

        Whitespace,

        Identifier,
        Colon,
        ArrayOpen,
        ArrayClose,
    }

    public record MGNToken(MGNTokenType Type, string Data)
    {
    }
}

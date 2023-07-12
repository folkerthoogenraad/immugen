using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Language
{
    public abstract record MGNCommand();

    public record MGNNamespaceCommand(string Namespace) : MGNCommand;
    public record MGNClassCommand(string Name) : MGNCommand;
    public record MGNExtendsCommand(string BaseClass) : MGNCommand;
    public record MGNPropertyCommand(string Name, string? ElementName, MGNType Type) : MGNCommand;
}

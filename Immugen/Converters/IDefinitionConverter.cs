using Immugen.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Converters
{
    public interface IDefinitionConverter
    {
        public string Convert(Definition definition);
        public string FileExtension { get; }
    }
}

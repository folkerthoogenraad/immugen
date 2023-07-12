using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immugen.Projects
{
    public record PropertyType(string Type, int ArrayRank)
    {
        public PropertyType ElementType => ArrayRank > 0 ? new PropertyType(Type, ArrayRank - 1) : this;

        public bool IsArray => ArrayRank > 0;
    }
}

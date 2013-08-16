using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Molten.Core.Tests
{
    public class NonSerializableMockData
    {
        public System.IO.Stream stream = new System.IO.MemoryStream();
        public System.Net.WebClient c = new System.Net.WebClient();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Molten.Core.Tests
{
    [Serializable]
    class MockData
    {
        [StringLength(5, ErrorMessage = "Name is too long!")]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Za-z0-9]+@[A-Za-z0-9]+\.[A-Za-z]{2,6}$", ErrorMessage = "Email address is invalid.")]
        public string Email { get; set; }

        [Range(13, 120, ErrorMessage = "Age is outside valid range!")]
        public int Age { get; set; }

        [Url]
        public string Website { get; set; }
    }
}

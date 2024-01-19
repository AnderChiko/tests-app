using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Models.Data
{
    public class ErrorModel
    {
        public int? Code { get; set; }

        public string? Status { get; set; }

        public string? Message { get; set; }
    }
}
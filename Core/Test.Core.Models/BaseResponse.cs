using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Models.Data;

namespace Test.Core.Models
{
    public abstract class BaseResponse<T>
          where T : class
    {
        public Status Status { get; set; }

        public T? Data { get; set; }

        public ErrorModel? Error { get; set; }
    }
}

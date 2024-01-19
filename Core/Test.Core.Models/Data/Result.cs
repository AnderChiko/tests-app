using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Models.Data
{
    public class Result : IResult
    {
        public Result(Status status)
        {
            Status = status;
        }

        public Result(Status status, string message)
        {
            Status = status;
            Error = new ErrorModel() { Message = message };
        }

        public Status Status { get; set; }

        public ErrorModel? Error { get; set; }
    }
}
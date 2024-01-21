using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Models.Data
{
    public class DataResult<T> : IDataResult<T>
    {
        public DataResult()
        {
        }
        public DataResult( T entry)
        {
            Data = entry;
        }
        public T Data { get; set; }
        public Status Status { get; set; } = Status.Success;
        public ErrorModel? Error { get; set; }
    }
}
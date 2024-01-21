using Microsoft.AspNetCore.Mvc;
using Test.Core.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Core.Interfaces.Web
{
    public interface ICrudController<T, TKey>
        where TKey : IEquatable<TKey>
        where T : class, new()
    {
        Task<ActionResult<DataResult<List<T>>>> Get();

        Task<ActionResult<DataResult<T>>> Get(TKey id);

        Task<ActionResult<DataResult<T>>> Post([FromBody] T entry);

        Task<ActionResult<DataResult<T>>> Put([FromBody] T entry);

        Task<ActionResult<DataResult<Result>>> Delete(TKey id);
    }
}

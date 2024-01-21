using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Context;

namespace Test.BusinessLogic.Unit.Tests
{
    public static class TestDataHelper
    {
        public static List<Tasks> GetFakeTasksList()
        {
            return new List<Tasks>()
             {
                new Tasks {Id = 1, Title="Test1", Description="Test1 Description"},
                new Tasks {Id = 2, Title="Test2", Description="Test2 Description"},
                new Tasks {Id = 3, Title="Test3", Description="Test3 Description"},
            };
        }
    }
}

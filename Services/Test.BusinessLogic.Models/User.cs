
using System;
using System.ComponentModel.DataAnnotations;

namespace Test.BusinessLogic.Models
{
   
    public class User
    {
        public int Id { get; set; }
        
        [StringLength(60)]
        public string? Username { get; set; }

        [StringLength(60)]
        public string? Email { get; set; }

        [StringLength(60)]
        public string? Password { get; set; }

    }
}
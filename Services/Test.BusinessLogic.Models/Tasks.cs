
using System;
using System.ComponentModel.DataAnnotations;

namespace Test.BusinessLogic.Models
{
   
    public class Tasks
    {
        public int Id { get; set; }

        [StringLength(60)]
        public string? Title { get; set; }

        [StringLength(60)]
        public string? Description { get; set; }

        public int? Assignee { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
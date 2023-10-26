using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("salaries", Schema = "dbo")]
    public partial class Salary
    {
        [Key]
        [Required]
        public int yearID { get; set; }

        [Key]
        [Required]
        public string teamID { get; set; }

        [Key]
        [Required]
        public string lgID { get; set; }

        [Key]
        [Required]
        public string playerID { get; set; }

        [Column("salary")]
        public int? salary1 { get; set; }

    }
}
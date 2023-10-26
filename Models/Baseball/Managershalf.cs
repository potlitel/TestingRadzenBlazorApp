using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("managershalf", Schema = "dbo")]
    public partial class Managershalf
    {
        [Key]
        [Required]
        public string managerID { get; set; }

        [Key]
        [Required]
        public int yearID { get; set; }

        [Key]
        [Required]
        public string teamID { get; set; }

        public string lgID { get; set; }

        public int? inseason { get; set; }

        [Key]
        [Required]
        public int half { get; set; }

        public int? G { get; set; }

        public int? W { get; set; }

        public int? L { get; set; }

        public int? rank { get; set; }

    }
}
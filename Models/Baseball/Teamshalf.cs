using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("teamshalf", Schema = "dbo")]
    public partial class Teamshalf
    {
        [Key]
        [Required]
        public int yearID { get; set; }

        [Key]
        [Required]
        public string lgID { get; set; }

        [Key]
        [Required]
        public string teamID { get; set; }

        [Key]
        [Required]
        public string Half { get; set; }

        public string divID { get; set; }

        public string DivWin { get; set; }

        public int? Rank { get; set; }

        public int? G { get; set; }

        public int? W { get; set; }

        public int? L { get; set; }

    }
}
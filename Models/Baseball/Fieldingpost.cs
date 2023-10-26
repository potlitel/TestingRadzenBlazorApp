using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("fieldingpost", Schema = "dbo")]
    public partial class Fieldingpost
    {
        [Key]
        [Required]
        public string playerID { get; set; }

        [Key]
        [Required]
        public int yearID { get; set; }

        public string teamID { get; set; }

        public string lgID { get; set; }

        [Key]
        [Required]
        public string round { get; set; }

        [Key]
        [Required]
        public string POS { get; set; }

        public int? G { get; set; }

        public int? GS { get; set; }

        public int? InnOuts { get; set; }

        public int? PO { get; set; }

        public int? A { get; set; }

        public int? E { get; set; }

        public int? DP { get; set; }

        public int? TP { get; set; }

        public int? PB { get; set; }

        public int? SB { get; set; }

        public int? CS { get; set; }

    }
}
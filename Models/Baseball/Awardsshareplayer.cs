using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("awardsshareplayers", Schema = "dbo")]
    public partial class Awardsshareplayer
    {
        [Key]
        [Required]
        public string awardID { get; set; }

        [Key]
        [Required]
        public int yearID { get; set; }

        [Key]
        [Required]
        public string lgID { get; set; }

        [Key]
        [Required]
        public string playerID { get; set; }

        public int? pointsWon { get; set; }

        public int? pointsMax { get; set; }

        public int? votesFirst { get; set; }

    }
}
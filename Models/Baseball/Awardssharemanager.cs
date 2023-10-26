using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("awardssharemanagers", Schema = "dbo")]
    public partial class Awardssharemanager
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
        public string managerID { get; set; }

        public int? pointsWon { get; set; }

        public int? pointsMax { get; set; }

        public int? votesFirst { get; set; }

    }
}
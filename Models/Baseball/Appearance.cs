using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("appearances", Schema = "dbo")]
    public partial class Appearance
    {
        [Key]
        [Required]
        public int yearID { get; set; }

        [Key]
        [Required]
        public string teamID { get; set; }

        public string lgID { get; set; }

        [Key]
        [Required]
        public string playerID { get; set; }

        public int? G_all { get; set; }

        public int? G_batting { get; set; }

        public int? G_defense { get; set; }

        public int? G_p { get; set; }

        public int? G_c { get; set; }

        public int? G_1b { get; set; }

        public int? G_2b { get; set; }

        public int? G_3b { get; set; }

        public int? G_ss { get; set; }

        public int? G_lf { get; set; }

        public int? G_cf { get; set; }

        public int? G_rf { get; set; }

        public int? G_of { get; set; }

        public int? G_dh { get; set; }

        public int? G_ph { get; set; }

        public int? G_pr { get; set; }

    }
}
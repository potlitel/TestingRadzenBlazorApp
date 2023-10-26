using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("pitching", Schema = "dbo")]
    public partial class Pitching
    {
        [Key]
        [Required]
        public string playerID { get; set; }

        [Key]
        [Required]
        public int yearID { get; set; }

        [Key]
        [Required]
        public int stint { get; set; }

        public string teamID { get; set; }

        public string lgID { get; set; }

        public int? W { get; set; }

        public int? L { get; set; }

        public int? G { get; set; }

        public int? GS { get; set; }

        public int? CG { get; set; }

        public int? SHO { get; set; }

        public int? SV { get; set; }

        public int? IPouts { get; set; }

        public int? H { get; set; }

        public int? ER { get; set; }

        public int? HR { get; set; }

        public int? BB { get; set; }

        public int? SO { get; set; }

        public int? BAOpp { get; set; }

        public int? ERA { get; set; }

        public int? IBB { get; set; }

        public int? WP { get; set; }

        public int? HBP { get; set; }

        public int? BK { get; set; }

        public int? BFP { get; set; }

        public int? GF { get; set; }

        public int? R { get; set; }

        public int? SH { get; set; }

        public int? SF { get; set; }

        public int? GIDP { get; set; }

    }
}
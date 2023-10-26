using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("teams", Schema = "dbo")]
    public partial class Team
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

        public string franchID { get; set; }

        public string divID { get; set; }

        public int? Rank { get; set; }

        public int? G { get; set; }

        public int? Ghome { get; set; }

        public int? W { get; set; }

        public int? L { get; set; }

        public string DivWin { get; set; }

        public string WCWin { get; set; }

        public string LgWin { get; set; }

        public string WSWin { get; set; }

        public int? R { get; set; }

        public int? AB { get; set; }

        public int? H { get; set; }

        [Column("2B")]
        public int? C2B { get; set; }

        [Column("3B")]
        public int? C3B { get; set; }

        public int? HR { get; set; }

        public int? BB { get; set; }

        public int? SO { get; set; }

        public int? SB { get; set; }

        public int? CS { get; set; }

        public int? HBP { get; set; }

        public int? SF { get; set; }

        public int? RA { get; set; }

        public int? ER { get; set; }

        public int? ERA { get; set; }

        public int? CG { get; set; }

        public int? SHO { get; set; }

        public int? SV { get; set; }

        public int? IPouts { get; set; }

        public int? HA { get; set; }

        public int? HRA { get; set; }

        public int? BBA { get; set; }

        public int? SOA { get; set; }

        public int? E { get; set; }

        public int? DP { get; set; }

        public int? FP { get; set; }

        public string name { get; set; }

        public string park { get; set; }

        public int? attendance { get; set; }

        public int? BPF { get; set; }

        public int? PPF { get; set; }

        public string teamIDBR { get; set; }

        public string teamIDlahman45 { get; set; }

        public string teamIDretro { get; set; }

    }
}
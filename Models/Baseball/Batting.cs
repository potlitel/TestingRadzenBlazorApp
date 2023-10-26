using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("batting", Schema = "dbo")]
    public partial class Batting
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

        public int? G { get; set; }

        public int? G_batting { get; set; }

        public int? AB { get; set; }

        public int? R { get; set; }

        public int? H { get; set; }

        [Column("2B")]
        public int? C2B { get; set; }

        [Column("3B")]
        public int? C3B { get; set; }

        public int? HR { get; set; }

        public int? RBI { get; set; }

        public int? SB { get; set; }

        public int? CS { get; set; }

        public int? BB { get; set; }

        public int? SO { get; set; }

        public int? IBB { get; set; }

        public int? HBP { get; set; }

        public int? SH { get; set; }

        public int? SF { get; set; }

        public int? GIDP { get; set; }

        public int? G_old { get; set; }

    }
}
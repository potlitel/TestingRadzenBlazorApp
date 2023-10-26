using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("seriespost", Schema = "dbo")]
    public partial class Seriespost
    {
        [Key]
        [Required]
        public int yearID { get; set; }

        [Key]
        [Required]
        public string round { get; set; }

        public string teamIDwinner { get; set; }

        public string lgIDwinner { get; set; }

        public string teamIDloser { get; set; }

        public string lgIDloser { get; set; }

        public int? wins { get; set; }

        public int? losses { get; set; }

        public int? ties { get; set; }

    }
}
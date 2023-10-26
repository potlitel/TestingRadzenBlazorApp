using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("awardsplayers", Schema = "dbo")]
    public partial class Awardsplayer
    {
        [Key]
        [Required]
        public string playerID { get; set; }

        [Key]
        [Required]
        public string awardID { get; set; }

        [Key]
        [Required]
        public int yearID { get; set; }

        [Key]
        [Required]
        public string lgID { get; set; }

        public string tie { get; set; }

        public string notes { get; set; }

    }
}
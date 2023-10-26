using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("allstarfull", Schema = "dbo")]
    public partial class Allstarfull
    {
        [Key]
        [Required]
        public string playerID { get; set; }

        [Key]
        [Required]
        public int yearID { get; set; }

        [Key]
        [Required]
        public int gameNum { get; set; }

        public string gameID { get; set; }

        public string teamID { get; set; }

        public string lgID { get; set; }

        public int? GP { get; set; }

        public int? startingPos { get; set; }

    }
}
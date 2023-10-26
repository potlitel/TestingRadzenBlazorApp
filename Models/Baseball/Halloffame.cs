using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("halloffame", Schema = "dbo")]
    public partial class Halloffame
    {
        [Key]
        [Required]
        public string hofID { get; set; }

        [Key]
        [Required]
        public int yearID { get; set; }

        public string votedBy { get; set; }

        public int? ballots { get; set; }

        public int? needed { get; set; }

        public int? votes { get; set; }

        public string inducted { get; set; }

        public string category { get; set; }

    }
}
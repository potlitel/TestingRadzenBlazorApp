using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("fieldingof", Schema = "dbo")]
    public partial class Fieldingof
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

        public int? Glf { get; set; }

        public int? Gcf { get; set; }

        public int? Grf { get; set; }

    }
}
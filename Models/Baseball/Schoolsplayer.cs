using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("schoolsplayers", Schema = "dbo")]
    public partial class Schoolsplayer
    {
        [Key]
        [Required]
        public string playerID { get; set; }

        [Key]
        [Required]
        public string schoolID { get; set; }

        public int? yearMin { get; set; }

        public int? yearMax { get; set; }

    }
}
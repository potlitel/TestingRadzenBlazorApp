using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("teamsfranchises", Schema = "dbo")]
    public partial class Teamsfranchise
    {
        [Key]
        [Required]
        public string franchID { get; set; }

        public string franchName { get; set; }

        public string active { get; set; }

        public string NAassoc { get; set; }

    }
}
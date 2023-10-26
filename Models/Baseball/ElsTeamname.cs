using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("els_teamnames", Schema = "dbo")]
    public partial class ElsTeamname
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string lgid { get; set; }

        [Required]
        public string teamid { get; set; }

        public string franchid { get; set; }

        public string name { get; set; }

        public string park { get; set; }

    }
}
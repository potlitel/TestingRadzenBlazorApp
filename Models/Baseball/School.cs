using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("schools", Schema = "dbo")]
    public partial class School
    {
        [Key]
        [Required]
        public string schoolID { get; set; }

        public string schoolName { get; set; }

        public string schoolCity { get; set; }

        public string schoolState { get; set; }

        public string schoolNick { get; set; }

    }
}
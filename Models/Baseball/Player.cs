using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingRadzenBlazorApp.Models.Baseball
{
    [Table("players", Schema = "dbo")]
    public partial class Player
    {
        [Key]
        [Required]
        public int lahmanID { get; set; }

        public string playerID { get; set; }

        public string managerID { get; set; }

        public string hofID { get; set; }

        public int? birthYear { get; set; }

        public int? birthMonth { get; set; }

        public int? birthDay { get; set; }

        public string birthCountry { get; set; }

        public string birthState { get; set; }

        public string birthCity { get; set; }

        public int? deathYear { get; set; }

        public int? deathMonth { get; set; }

        public int? deathDay { get; set; }

        public string deathCountry { get; set; }

        public string deathState { get; set; }

        public string deathCity { get; set; }

        public string nameFirst { get; set; }

        public string nameLast { get; set; }

        public string nameNote { get; set; }

        public string nameGiven { get; set; }

        public string nameNick { get; set; }

        public int? weight { get; set; }

        public int? height { get; set; }

        public string bats { get; set; }

        public string throws { get; set; }

        public string debut { get; set; }

        public string finalGame { get; set; }

        public string college { get; set; }

        public string lahman40ID { get; set; }

        public string lahman45ID { get; set; }

        public string retroID { get; set; }

        public string holtzID { get; set; }

        public string bbrefID { get; set; }

    }
}
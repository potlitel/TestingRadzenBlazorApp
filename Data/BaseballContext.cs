using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestingRadzenBlazorApp.Models.Baseball;

namespace TestingRadzenBlazorApp.Data
{
    public partial class BaseballContext : DbContext
    {
        public BaseballContext()
        {
        }

        public BaseballContext(DbContextOptions<BaseballContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.ElsTeamname>().HasNoKey();

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Allstarfull>().HasKey(table => new {
                table.playerID, table.yearID, table.gameNum
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Appearance>().HasKey(table => new {
                table.yearID, table.teamID, table.playerID
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager>().HasKey(table => new {
                table.managerID, table.awardID, table.yearID, table.lgID
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Awardsplayer>().HasKey(table => new {
                table.playerID, table.awardID, table.yearID, table.lgID
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager>().HasKey(table => new {
                table.awardID, table.yearID, table.lgID, table.managerID
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer>().HasKey(table => new {
                table.awardID, table.yearID, table.lgID, table.playerID
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Batting>().HasKey(table => new {
                table.playerID, table.yearID, table.stint
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Battingpost>().HasKey(table => new {
                table.yearID, table.round, table.playerID
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Fielding>().HasKey(table => new {
                table.playerID, table.yearID, table.stint, table.POS
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Fieldingof>().HasKey(table => new {
                table.playerID, table.yearID, table.stint
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Fieldingpost>().HasKey(table => new {
                table.playerID, table.yearID, table.round, table.POS
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Halloffame>().HasKey(table => new {
                table.hofID, table.yearID
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Manager>().HasKey(table => new {
                table.yearID, table.teamID, table.inseason
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Managershalf>().HasKey(table => new {
                table.managerID, table.yearID, table.teamID, table.half
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Pitching>().HasKey(table => new {
                table.playerID, table.yearID, table.stint
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Pitchingpost>().HasKey(table => new {
                table.playerID, table.yearID, table.round
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Salary>().HasKey(table => new {
                table.yearID, table.teamID, table.lgID, table.playerID
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer>().HasKey(table => new {
                table.playerID, table.schoolID
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Seriespost>().HasKey(table => new {
                table.yearID, table.round
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Team>().HasKey(table => new {
                table.yearID, table.lgID, table.teamID
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.Teamshalf>().HasKey(table => new {
                table.yearID, table.lgID, table.teamID, table.Half
            });

            builder.Entity<TestingRadzenBlazorApp.Models.Baseball.ElsTeamname>()
              .Property(p => p.id)
              .ValueGeneratedOnAddOrUpdate()
              .Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);
            this.OnModelBuilding(builder);
        }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Allstarfull> Allstarfulls { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Appearance> Appearances { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> Awardsmanagers { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Awardsplayer> Awardsplayers { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager> Awardssharemanagers { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Awardsshareplayer> Awardsshareplayers { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Batting> Battings { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Battingpost> Battingposts { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.ElsTeamname> ElsTeamnames { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Fielding> Fieldings { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Fieldingof> Fieldingofs { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Fieldingpost> Fieldingposts { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Halloffame> Halloffames { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Manager> Managers { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Managershalf> Managershalves { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Pitching> Pitchings { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Pitchingpost> Pitchingposts { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Player> Players { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Salary> Salaries { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.School> Schools { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer> Schoolsplayers { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Seriespost> Seriesposts { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Team> Teams { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Teamsfranchise> Teamsfranchises { get; set; }

        public DbSet<TestingRadzenBlazorApp.Models.Baseball.Teamshalf> Teamshalves { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    
    }
}
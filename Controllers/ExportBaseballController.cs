using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using TestingRadzenBlazorApp.Data;

namespace TestingRadzenBlazorApp.Controllers
{
    public partial class ExportBaseballController : ExportController
    {
        private readonly BaseballContext context;
        private readonly BaseballService service;

        public ExportBaseballController(BaseballContext context, BaseballService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/Baseball/allstarfulls/csv")]
        [HttpGet("/export/Baseball/allstarfulls/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAllstarfullsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAllstarfulls(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/allstarfulls/excel")]
        [HttpGet("/export/Baseball/allstarfulls/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAllstarfullsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAllstarfulls(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/appearances/csv")]
        [HttpGet("/export/Baseball/appearances/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAppearancesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAppearances(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/appearances/excel")]
        [HttpGet("/export/Baseball/appearances/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAppearancesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAppearances(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/awardsmanagers/csv")]
        [HttpGet("/export/Baseball/awardsmanagers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAwardsmanagersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAwardsmanagers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/awardsmanagers/excel")]
        [HttpGet("/export/Baseball/awardsmanagers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAwardsmanagersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAwardsmanagers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/awardsplayers/csv")]
        [HttpGet("/export/Baseball/awardsplayers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAwardsplayersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAwardsplayers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/awardsplayers/excel")]
        [HttpGet("/export/Baseball/awardsplayers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAwardsplayersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAwardsplayers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/awardssharemanagers/csv")]
        [HttpGet("/export/Baseball/awardssharemanagers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAwardssharemanagersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAwardssharemanagers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/awardssharemanagers/excel")]
        [HttpGet("/export/Baseball/awardssharemanagers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAwardssharemanagersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAwardssharemanagers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/awardsshareplayers/csv")]
        [HttpGet("/export/Baseball/awardsshareplayers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAwardsshareplayersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAwardsshareplayers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/awardsshareplayers/excel")]
        [HttpGet("/export/Baseball/awardsshareplayers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAwardsshareplayersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAwardsshareplayers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/battings/csv")]
        [HttpGet("/export/Baseball/battings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBattingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBattings(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/battings/excel")]
        [HttpGet("/export/Baseball/battings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBattingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBattings(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/battingposts/csv")]
        [HttpGet("/export/Baseball/battingposts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBattingpostsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetBattingposts(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/battingposts/excel")]
        [HttpGet("/export/Baseball/battingposts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportBattingpostsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetBattingposts(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/elsteamnames/csv")]
        [HttpGet("/export/Baseball/elsteamnames/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportElsTeamnamesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetElsTeamnames(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/elsteamnames/excel")]
        [HttpGet("/export/Baseball/elsteamnames/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportElsTeamnamesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetElsTeamnames(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/fieldings/csv")]
        [HttpGet("/export/Baseball/fieldings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFieldingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetFieldings(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/fieldings/excel")]
        [HttpGet("/export/Baseball/fieldings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFieldingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetFieldings(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/fieldingofs/csv")]
        [HttpGet("/export/Baseball/fieldingofs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFieldingofsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetFieldingofs(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/fieldingofs/excel")]
        [HttpGet("/export/Baseball/fieldingofs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFieldingofsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetFieldingofs(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/fieldingposts/csv")]
        [HttpGet("/export/Baseball/fieldingposts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFieldingpostsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetFieldingposts(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/fieldingposts/excel")]
        [HttpGet("/export/Baseball/fieldingposts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFieldingpostsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetFieldingposts(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/halloffames/csv")]
        [HttpGet("/export/Baseball/halloffames/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportHalloffamesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetHalloffames(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/halloffames/excel")]
        [HttpGet("/export/Baseball/halloffames/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportHalloffamesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetHalloffames(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/managers/csv")]
        [HttpGet("/export/Baseball/managers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportManagersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetManagers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/managers/excel")]
        [HttpGet("/export/Baseball/managers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportManagersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetManagers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/managershalves/csv")]
        [HttpGet("/export/Baseball/managershalves/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportManagershalvesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetManagershalves(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/managershalves/excel")]
        [HttpGet("/export/Baseball/managershalves/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportManagershalvesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetManagershalves(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/pitchings/csv")]
        [HttpGet("/export/Baseball/pitchings/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPitchingsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPitchings(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/pitchings/excel")]
        [HttpGet("/export/Baseball/pitchings/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPitchingsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPitchings(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/pitchingposts/csv")]
        [HttpGet("/export/Baseball/pitchingposts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPitchingpostsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPitchingposts(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/pitchingposts/excel")]
        [HttpGet("/export/Baseball/pitchingposts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPitchingpostsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPitchingposts(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/players/csv")]
        [HttpGet("/export/Baseball/players/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPlayersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPlayers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/players/excel")]
        [HttpGet("/export/Baseball/players/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPlayersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPlayers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/salaries/csv")]
        [HttpGet("/export/Baseball/salaries/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSalariesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSalaries(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/salaries/excel")]
        [HttpGet("/export/Baseball/salaries/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSalariesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSalaries(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/schools/csv")]
        [HttpGet("/export/Baseball/schools/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchoolsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSchools(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/schools/excel")]
        [HttpGet("/export/Baseball/schools/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchoolsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSchools(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/schoolsplayers/csv")]
        [HttpGet("/export/Baseball/schoolsplayers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchoolsplayersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSchoolsplayers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/schoolsplayers/excel")]
        [HttpGet("/export/Baseball/schoolsplayers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSchoolsplayersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSchoolsplayers(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/seriesposts/csv")]
        [HttpGet("/export/Baseball/seriesposts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSeriespostsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSeriesposts(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/seriesposts/excel")]
        [HttpGet("/export/Baseball/seriesposts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSeriespostsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSeriesposts(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/teams/csv")]
        [HttpGet("/export/Baseball/teams/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTeamsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTeams(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/teams/excel")]
        [HttpGet("/export/Baseball/teams/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTeamsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTeams(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/teamsfranchises/csv")]
        [HttpGet("/export/Baseball/teamsfranchises/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTeamsfranchisesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTeamsfranchises(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/teamsfranchises/excel")]
        [HttpGet("/export/Baseball/teamsfranchises/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTeamsfranchisesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTeamsfranchises(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/teamshalves/csv")]
        [HttpGet("/export/Baseball/teamshalves/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTeamshalvesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTeamshalves(), Request.Query), fileName);
        }

        [HttpGet("/export/Baseball/teamshalves/excel")]
        [HttpGet("/export/Baseball/teamshalves/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTeamshalvesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTeamshalves(), Request.Query), fileName);
        }
    }
}

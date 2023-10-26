using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace TestingRadzenBlazorApp.Pages
{
    public partial class Awardsmanagers
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public BaseballService BaseballService { get; set; }

        protected IEnumerable<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> awardsmanagers;

        protected RadzenDataGrid<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> grid0;
        protected override async Task OnInitializedAsync()
        {
            awardsmanagers = await BaseballService.GetAwardsmanagers();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddAwardsmanager>("Add Awardsmanager", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<TestingRadzenBlazorApp.Models.Baseball.Awardsmanager> args)
        {
            await DialogService.OpenAsync<EditAwardsmanager>("Edit Awardsmanager", new Dictionary<string, object> { {"managerID", args.Data.managerID}, {"awardID", args.Data.awardID}, {"yearID", args.Data.yearID}, {"lgID", args.Data.lgID} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, TestingRadzenBlazorApp.Models.Baseball.Awardsmanager awardsmanager)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await BaseballService.DeleteAwardsmanager(awardsmanager.managerID, awardsmanager.awardID, awardsmanager.yearID, awardsmanager.lgID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Awardsmanager"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await BaseballService.ExportAwardsmanagersToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "Awardsmanagers");
            }

            if (args == null || args.Value == "xlsx")
            {
                await BaseballService.ExportAwardsmanagersToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "Awardsmanagers");
            }
        }
    }
}
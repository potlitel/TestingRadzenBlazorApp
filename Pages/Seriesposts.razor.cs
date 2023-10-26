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
    public partial class Seriesposts
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

        protected IEnumerable<TestingRadzenBlazorApp.Models.Baseball.Seriespost> seriesposts;

        protected RadzenDataGrid<TestingRadzenBlazorApp.Models.Baseball.Seriespost> grid0;
        protected override async Task OnInitializedAsync()
        {
            seriesposts = await BaseballService.GetSeriesposts();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddSeriespost>("Add Seriespost", null);
            await grid0.Reload();
        }

        protected async Task EditRow(DataGridRowMouseEventArgs<TestingRadzenBlazorApp.Models.Baseball.Seriespost> args)
        {
            await DialogService.OpenAsync<EditSeriespost>("Edit Seriespost", new Dictionary<string, object> { {"yearID", args.Data.yearID}, {"round", args.Data.round} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, TestingRadzenBlazorApp.Models.Baseball.Seriespost seriespost)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await BaseballService.DeleteSeriespost(seriespost.yearID, seriespost.round);

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
                    Detail = $"Unable to delete Seriespost"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await BaseballService.ExportSeriespostsToCSV(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "Seriesposts");
            }

            if (args == null || args.Value == "xlsx")
            {
                await BaseballService.ExportSeriespostsToExcel(new Query
{
    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
    OrderBy = $"{grid0.Query.OrderBy}",
    Expand = "",
    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
}, "Seriesposts");
            }
        }
    }
}
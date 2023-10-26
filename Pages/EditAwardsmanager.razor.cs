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
    public partial class EditAwardsmanager
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

        [Parameter]
        public string managerID { get; set; }

        [Parameter]
        public string awardID { get; set; }

        [Parameter]
        public int yearID { get; set; }

        [Parameter]
        public string lgID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            awardsmanager = await BaseballService.GetAwardsmanagerByManagerIdAndAwardIdAndYearIdAndLgId(managerID, awardID, yearID, lgID);
        }
        protected bool errorVisible;
        protected TestingRadzenBlazorApp.Models.Baseball.Awardsmanager awardsmanager;

        protected async Task FormSubmit()
        {
            try
            {
                await BaseballService.UpdateAwardsmanager(managerID, awardID, yearID, lgID, awardsmanager);
                DialogService.Close(awardsmanager);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
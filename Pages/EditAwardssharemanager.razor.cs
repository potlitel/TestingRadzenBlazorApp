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
    public partial class EditAwardssharemanager
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
        public string awardID { get; set; }

        [Parameter]
        public int yearID { get; set; }

        [Parameter]
        public string lgID { get; set; }

        [Parameter]
        public string managerID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            awardssharemanager = await BaseballService.GetAwardssharemanagerByAwardIdAndYearIdAndLgIdAndManagerId(awardID, yearID, lgID, managerID);
        }
        protected bool errorVisible;
        protected TestingRadzenBlazorApp.Models.Baseball.Awardssharemanager awardssharemanager;

        protected async Task FormSubmit()
        {
            try
            {
                await BaseballService.UpdateAwardssharemanager(awardID, yearID, lgID, managerID, awardssharemanager);
                DialogService.Close(awardssharemanager);
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
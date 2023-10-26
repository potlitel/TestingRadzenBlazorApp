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
    public partial class EditSchoolsplayer
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
        public string playerID { get; set; }

        [Parameter]
        public string schoolID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            schoolsplayer = await BaseballService.GetSchoolsplayerByPlayerIdAndSchoolId(playerID, schoolID);
        }
        protected bool errorVisible;
        protected TestingRadzenBlazorApp.Models.Baseball.Schoolsplayer schoolsplayer;

        protected async Task FormSubmit()
        {
            try
            {
                await BaseballService.UpdateSchoolsplayer(playerID, schoolID, schoolsplayer);
                DialogService.Close(schoolsplayer);
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
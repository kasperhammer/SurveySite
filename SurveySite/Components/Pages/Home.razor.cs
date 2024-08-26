
using BuisnessLogic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models;
using Models.UIModels;
using System;

namespace SurveySite.Components.Pages
{
    public partial class Home
    {
        [Parameter]
        public int SurveyId { get; set; }
        public int pageCount = 0;
        public int totalPages = 0;
        public bool showSurvey = false;
        public bool Edit = true;
        public bool isOwner = false;
        public Survey Survey { get; set; } = new();
        public SurveyUI Survey1 { get; set; } = new();

        [Inject]
        IRepository Repo { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (SurveyId != 0)
                {
                    Survey = await Repo.GetOneSurvey(SurveyId);

                    if (Survey == null)
                    {
                        Survey = new();
                    }
                    else
                    {
                        showSurvey = true;
                        Edit = false;
                    }

                    StateHasChanged();
                }
            }
        }



        

      

      
        public async Task Start()
        {
            showSurvey = true;
        }

        public async Task Owner(Survey survey)
        {
            Survey = survey;
            Edit = true;
            showSurvey = true;
            isOwner = true;
            StateHasChanged() ;
        }

        public async Task PageCount((int page, int total) args)
        {
            pageCount = args.page;
            totalPages = args.total;
            StateHasChanged();
        }

    }
}

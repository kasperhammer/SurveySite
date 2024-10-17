
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

        public int PageCount
        {
            get
            {
                return pageCount;
            }
            set
            {

                if (value >= anwsers.Count + 1)
                {}
                else
                {
                    if (value <= 0)
                    {
                        pageCount = 0;
                    }
                    else
                    {
                        pageCount = value;
                    }

                }


            }
        }

        public int totalPages = 0;
        public bool showSurvey = false;
        public bool Edit = true;
        public bool isOwner = false;
        public List<AnwserModuleUI> anwsers = new();

        public SurveyUI Survey { get; set; } = new();

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

        public async Task Owner(SurveyUI survey)
        {
            anwsers = await Repo.GetSurvetAnwsers(Survey.Id);
            totalPages = anwsers.Count;
            Survey = survey;
            Edit = true;
            showSurvey = true;
            isOwner = true;
            StateHasChanged();
        }



    }
}

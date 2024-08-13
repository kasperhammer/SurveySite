
using BuisnessLogic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models;
using System;

namespace SurveySite.Components.Pages
{
    public partial class Home
    {
        [Parameter]
        public int SurveyId { get; set; }

        public bool ShowSurvey
        {
            get { return _showSurvey; }
            set
            { _showSurvey = value;  StateHasChanged(); }
        }
        private bool _showSurvey = false;

        public bool Edit = true;

        public Survey Survey { get; set; } = new();

        public List<Survey> Anwsers { get; set; } = new();

        public int PageCount
        { 
            get { return _pageCount; }
            set
            {
                if (value <= 0)
                {
                    _pageCount = 0;
                }
                else
                {
                    if (value >= Anwsers.Count)
                    {
                        _pageCount = Anwsers.Count;
                    }
                    else
                    {
                        _pageCount = value;
                    }
                }

                


            }
        }
        private int _pageCount = 0;

        public string Code { get; set; }



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
                        ShowSurvey = true;
                        Edit = false;
                    }

                    StateHasChanged();
                }
            }
        }

        public async Task AddComp()
        {
            if (Survey.SComps == null)
            {
                Survey.SComps = new();
            }

            // Calculate the new Id by adding 1 to the count of existing SComps
            int newId = Survey.SComps.Count + 1;

            // Add the new SComp with the calculated Id
            Survey.SComps.Add(new SComp { Id = newId });

            StateHasChanged();
        }

        public async Task TypeChange(int compId)
        {
            SComp comp = Survey.SComps.FirstOrDefault(x => x.Id == compId);
            if (comp != null)
            {
                if (comp.Type == 0)
                {
                    comp.MultiAnwsers = null;
                    comp.SingleAnwser = null;
                    comp.TextAnwser = "";
                }
                if (comp.Type == 1)
                {
                    comp.MultiAnwsers = new();
                    comp.MultiAnwsers.Add(new AnwserModule { Id = 1, Text = $"Valgmulighed {comp.MultiAnwsers.Count + 1}" });
                    comp.SingleAnwser = null;
                    comp.TextAnwser = "";
                }
                if (comp.Type == 2)
                {
                    comp.MultiAnwsers = null;
                    comp.SingleAnwser = new();
                    comp.SingleAnwser.Add(new AnwserModule { Id = 1, Text = $"Valgmulighed {comp.SingleAnwser.Count + 1}" });
                    comp.TextAnwser = "";
                }

                StateHasChanged();
            }

        }

        public void AddQuestions(int compId)
        {
            SComp comp = Survey.SComps.FirstOrDefault(x => x.Id == compId);
            if (comp != null)
            {
                if (comp.Type == 1)
                {
                    comp.MultiAnwsers.Add(new AnwserModule { Id = comp.MultiAnwsers.Count + 1, Text = $"Valgmulighed {comp.MultiAnwsers.Count + 1}" });
                }

                if (comp.Type == 2)
                {
                    comp.SingleAnwser.Add(new AnwserModule { Id = comp.SingleAnwser.Count + 1, Text = $"Valgmulighed {comp.SingleAnwser.Count + 1}" });
                }



            }

            StateHasChanged();
        }


        public async Task Submit()
        {
            await Repo.AddSurvey(Survey);
        }

        public async Task SingleAwnser(SComp module, AnwserModule anwser)
        {
            foreach (var item in module.SingleAnwser)
            {
                if (item.Selected == true && item.Id != anwser.Id)
                {
                    item.Selected = false;
                }
            }
            StateHasChanged();
        }

        public async Task AnwserSurvey()
        {
            Survey.SurveyAnwser = true;
            Survey.OwnerCode = "";
            Survey.OriginId = Survey.Id;
            Survey.Id = 0;
            await Repo.AddSurvey(Survey);
        }

        public async Task Owner()
        {
            if (Survey.OwnerCode == Code)
            {
                Survey = await Repo.GetOneSurvey(SurveyId);
                Edit = true;
                Anwsers = await Repo.GetSurvetAnwsers(SurveyId);
                StateHasChanged();
            }
        }



    }
}

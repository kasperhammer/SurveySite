using BuisnessLogic;
using Microsoft.AspNetCore.Components;
using Models;
using Models.UIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class AnwserSurvey
    {

        [Parameter]
        public Survey Survey { get; set; }
        [Parameter]
        public EventCallback<Survey> IsOwner { get; set; }

        [Inject]
        IRepository Repo { get; set; }

        public bool complete = false;
        public string Code { get; set; }

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

        public async Task AnwserSurve()
        {
            Survey.SurveyAnwser = true;
            Survey.OwnerCode = "";
            Survey.OriginId = Survey.Id;
            Survey.Id = 0;
           // await Repo.AddSurvey(Survey);

            complete = true;
            StateHasChanged();
        }

        public async Task Owner()
        {
            if (Survey.OwnerCode == Code)
            {
                Survey = await Repo.GetOneSurvey(Survey.Id);
                await IsOwner.InvokeAsync(Survey);  
              
                StateHasChanged();
            }
        }

    }
}

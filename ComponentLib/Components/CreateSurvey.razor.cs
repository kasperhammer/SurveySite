using BuisnessLogic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Models;
using Models.UIModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class CreateSurvey
    {
        [Parameter]
        public SurveyUI Survey { get; set; } = new();


        public bool complete = false;
        public bool invalid = false;

        [Inject]
        IRepository Repo { get; set; }

        [Inject]
        NavigationManager NavMan { get; set; }


        public async Task AddComp()
        {
            if (Survey.Comps == null)
            {
                Survey.Comps = new();
            }

            // Calculate the new Id by adding 1 to the count of existing SComps
            int newId = Survey.Comps.Count + 1;

            // Add the new SComp with the calculated Id
            Survey.Comps.Add(new CompUI { Id = newId });
            invalid = false;

            StateHasChanged();
        }

        public async Task TypeChange(int compId)
        {
            CompUI comp = Survey.Comps.FirstOrDefault(x => x.Id == compId);
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
                    comp.MultiAnwsers.Add(new AnwserModuleUI { Id = 1, Text = $"Valgmulighed {comp.MultiAnwsers.Count + 1}" });
                    comp.SingleAnwser = null;
                    comp.TextAnwser = "";
                }
                if (comp.Type == 2)
                {
                    comp.MultiAnwsers = null;
                    comp.SingleAnwser = new();
                    comp.SingleAnwser.Add(new AnwserModuleUI { Id = 1, Text = $"Valgmulighed {comp.SingleAnwser.Count + 1}" });
                    comp.TextAnwser = "";
                }
                if (comp.Type == 3)
                {
                    Survey.Comps.Remove(comp);
                }

                StateHasChanged();
            }

        }

        public void AddQuestions(int compId)
        {
            CompUI comp = Survey.Comps.FirstOrDefault(x => x.Id == compId);
            if (comp != null)
            {
                if (comp.Type == 1)
                {
                    comp.MultiAnwsers.Add(new AnwserModuleUI { Id = comp.MultiAnwsers.Count + 1, Text = $"Valgmulighed {comp.MultiAnwsers.Count + 1}" });
                }

                if (comp.Type == 2)
                {
                    comp.SingleAnwser.Add(new AnwserModuleUI { Id = comp.SingleAnwser.Count + 1, Text = $"Valgmulighed {comp.SingleAnwser.Count + 1}" });
                }



            }

            StateHasChanged();
        }

      

        public async Task Submit()
        {
            // await Repo.AddSurvey(Survey);
            invalid = false;
            complete = true;
            StateHasChanged();
        }

        public async Task Invalid()
        {
            invalid = true;
            StateHasChanged();
        }

        public async Task closemodal()
        {
            NavMan.NavigateTo("/", true);
        }


    }
}

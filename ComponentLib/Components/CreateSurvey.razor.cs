using BuisnessLogic;
using Microsoft.AspNetCore.Components;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class CreateSurvey
    {
        [Parameter]
        public Survey Survey { get; set; } = new();

        public bool complete = false;

        [Inject]
        IRepository Repo { get; set; }

        [Inject]
        NavigationManager NavMan { get; set; }

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
                if (comp.Type == 3)
                {
                    Survey.SComps.Remove(comp);
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
            complete = true;
            StateHasChanged();
        }

        public async Task closemodal()
        {
            NavMan.NavigateTo("/", true);
        }


    }
}

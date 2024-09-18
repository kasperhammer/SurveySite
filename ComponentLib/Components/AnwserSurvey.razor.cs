using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        public SurveyUI Survey { get; set; }

        public AnwserModuleUI Module { get; set; } = new();

        public bool ready { get; set; } = false;

        [Inject]
        IJSRuntime jsRuntime { get; set; }

        IJSObjectReference module;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/ComponentLib/Components/AnwserSurvey.razor.js");
                if (Survey != null)
                {
                    if (Survey.Comps != null)
                    {
                        if (Survey.Comps.Count != 0)
                        {
                            Module = new();
                            Module.SurveyId = Survey.Id;
                            Module.anwsers = new();
                            foreach (var item in Survey.Comps)
                            {                           
                                Module.anwsers.Add(new AnwserUI { CompId = item.SurveyId,AnwserText = ""});
                            }
                            ready = true;
                            StateHasChanged();
                        }
                    }
                }
            }
        }

        public async void selectitem(int i, int k, bool multi)
        {
            if (multi == false)
            {
                Module.anwsers[i].AnwserText = k.ToString();
            }
            else
            {
            
                // Get the current answer string
                string currentAnswer = Module.anwsers[i].AnwserText;

                // Split the current answers into a list of integers (if not empty)
                List<int> selectedAnswers = string.IsNullOrEmpty(currentAnswer) ? new List<int>() : currentAnswer.Split(',').Select(int.Parse).ToList();

                // Check if the index is already in the list
                if (selectedAnswers.Contains(k))
                {
                    // If it is, remove it (deselect)
                    selectedAnswers.Remove(k);
                    await module.InvokeVoidAsync("CheckBtn", k);
                }
                else
                {
                    // If not, add it (select)
                    selectedAnswers.Add(k);
                }

                // Join the updated list back into a comma-separated string and assign it back
                Module.anwsers[i].AnwserText = string.Join(",", selectedAnswers);
            }

           
        }

    }
}

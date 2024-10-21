using BuisnessLogic;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models.UIModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class ViewSurvey
    {
        [Parameter]
        public SurveyUI Survey { get; set; }
        [Parameter]
        public EventCallback<SurveyUI> EditSurvey { get; set; }

        EditContext formContext;

        [Parameter]
        public AnwserModuleUI Module { get; set; }

        public bool ready { get; set; } = false;

        public bool complete = false;



        [Inject]
        IJSRuntime jsRuntime { get; set; }

        IJSObjectReference module;

        [Inject]
        IRepository repo { get; set; }


        [Inject]
        NavigationManager NavMan { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {



            if (firstRender)
            {
                formContext = new EditContext(Module);
                module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/ComponentLib/Components/ViewSurvey.razor.js");
                ready = true;
                StateHasChanged();

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
                    await module.InvokeVoidAsync("CheckBtn", i, k);
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


        
        public async Task closemodal()
        {
            NavMan.NavigateTo("/", true);
        }




    }
}

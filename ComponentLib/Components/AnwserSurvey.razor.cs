using BuisnessLogic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
    public partial class AnwserSurvey
    {
        [Parameter]
        public SurveyUI Survey { get; set; }
        [Parameter]
        public EventCallback<SurveyUI> EditSurvey { get; set; }

        EditContext formContext;
    
        public AnwserModuleUI Module { get; set; } = new();

        public bool ready { get; set; } = false;

        public bool complete = false;

        public bool edit;

        private string password = "";
        EditContext secondContext;

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
                secondContext = new EditContext(password);
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
                                Module.anwsers.Add(new AnwserUI { CompId = item.Id,AnwserText = ""});
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
                    await module.InvokeVoidAsync("CheckBtn",i,k);
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

       
        public async Task SubmitAsync()
        {
            formContext = new EditContext(Module);
            ValidationMessageStore validationMessageStore = new ValidationMessageStore(formContext);

            for (int i = 0; i < Module.anwsers.Count; i++)
            {
                if (Survey.Comps[i].Required)
                {

                    //laver en Liste til at holde mine error beskeder
                    List<ValidationResult> validationResults = new List<ValidationResult>();
                    //validere min model
                    ValidationContext validationContext = new ValidationContext(Module.anwsers[i]);
                    Validator.TryValidateObject(Module.anwsers[i], validationContext, validationResults, true);
                    Module.anwsers[i].Error = false;
                    //jeg gennemgår mine error beskeder og tilføjer dem til min validation message store
                    foreach (var validationResult in validationResults)
                    {
                        var memberName = validationResult.MemberNames.FirstOrDefault();
                        var fieldIdentifier = new FieldIdentifier(Module.anwsers[i], memberName);
                        // Manually add the validation error to the ValidationMessageStore
                        validationMessageStore.Add(fieldIdentifier, "This field is required");
                        Module.anwsers[i].Error = true;

                    } 
                }
            }
            formContext.NotifyValidationStateChanged();
            if (formContext.GetValidationMessages().Count() == 0)
            {

                //NO errors Found
                if (await repo.SubmitAnwserAsync(Module))
                {
                    complete = true;
                    StateHasChanged();

                }
             


            }
        


        }
        public async Task closemodal()
        {
            NavMan.NavigateTo("/", true);
        }

        public async Task CloseEditModal()
        {
            if (edit)
            {
                edit = false;
            }
            else
            {
                edit = true;
            }
            StateHasChanged();
        }

        public async Task Edit()
        {
            await EditSurvey.InvokeAsync(Survey);
        }
    }
}

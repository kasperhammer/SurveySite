using BuisnessLogic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
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

        EditContext formContext;
        public bool complete = false;
        public bool invalid = false;

        [Inject]
        IRepository Repo { get; set; }

        [Inject]
        NavigationManager NavMan { get; set; }

        [Inject]
        IJSRuntime jsRuntime { get; set; }

        IJSObjectReference module;

        protected override async Task OnInitializedAsync()
        {
            formContext = new EditContext(Survey);
            module = await jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/ComponentLib/Components/CreateSurvey.razor.js");
        
        }




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
                    comp.MultiAnwsers.Add(new AnwserModuleUI { Id = 1, Text = "" });
                    comp.SingleAnwser = null;
                    comp.TextAnwser = "";
                }
                if (comp.Type == 2)
                {
                    comp.MultiAnwsers = null;
                    comp.SingleAnwser = new();
                    comp.SingleAnwser.Add(new AnwserModuleUI { Id = 1, Text = "" });
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
                    comp.MultiAnwsers.Add(new AnwserModuleUI { Id = comp.MultiAnwsers.Count + 1, Text = "" });
                }

                if (comp.Type == 2)
                {
                    comp.SingleAnwser.Add(new AnwserModuleUI { Id = comp.SingleAnwser.Count + 1, Text = "" });
                }



            }

            StateHasChanged();
        }

        public async void RemoveQuestion(AnwserModuleUI item, int compId)
        {
            CompUI comp = Survey.Comps.FirstOrDefault(x => x.Id == compId);
            if (comp != null)
            {
                if (comp.Type == 1)
                {
                    comp.MultiAnwsers.Remove(item);
                }
               
                if (comp.Type == 2)
                {
                    comp.SingleAnwser.Remove(item);
                }

                StateHasChanged();
                await module.InvokeVoidAsync("resetAllRadioButtons");
            }

        }


        public async Task Submit()
        {
       
            if (await Repo.AddSurvey(Survey))
            {
                
                invalid = false;
                complete = true;
                StateHasChanged(); 
            }
        }

        public async Task Invalid()
        {

            // Validate each Comp individually
            foreach (var comp in Survey.Comps)
            {
                ValidateComp(comp);
                if (comp.Type == 1)
                {
                    comp.MultiAnwsers.ForEach(x => ValidateAnwser(x));
                }
                if (comp.Type == 2)
                {
                    comp.SingleAnwser.ForEach(x => ValidateAnwser(x));
                }
            }

            invalid = true;
            StateHasChanged();
        }

        public async Task closemodal()
        {
            NavMan.NavigateTo("/", true);
        }

        private void ValidateComp(CompUI comp)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(comp);
            bool isCompValid = Validator.TryValidateObject(comp, validationContext, validationResults, true);

            // Manually update validation state in EditContext for Comp fields
            foreach (var validationResult in validationResults)
            {
                var fieldIdentifier = new FieldIdentifier(comp, validationResult.MemberNames.FirstOrDefault());
                formContext.NotifyFieldChanged(fieldIdentifier);
            }
        }

        private void ValidateAnwser(AnwserModuleUI anwser)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(anwser);
            bool isCompValid = Validator.TryValidateObject(anwser, validationContext, validationResults, true);

            // Manually update validation state in EditContext for Comp fields
            foreach (var validationResult in validationResults)
            {
                var fieldIdentifier = new FieldIdentifier(anwser, validationResult.MemberNames.FirstOrDefault());
                formContext.NotifyFieldChanged(fieldIdentifier);
            }
        }

    }
}

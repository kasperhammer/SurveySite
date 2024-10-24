
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

        [Parameter]
        public bool Edit { get; set; }

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
                }
                if (comp.Type == 1)
                {
                    comp.MultiAnwsers = new();
                    comp.MultiAnwsers.Add(new CompModuleUI { Id = 1, Text = "" });
                    comp.SingleAnwser = null;
                }
                if (comp.Type == 2)
                {
                    comp.MultiAnwsers = null;
                    comp.SingleAnwser = new();
                    comp.SingleAnwser.Add(new CompModuleUI { Id = 1, Text = "" });
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
                    comp.MultiAnwsers.Add(new CompModuleUI { Id = comp.MultiAnwsers.Count + 1, Text = "" });
                }

                if (comp.Type == 2)
                {
                    comp.SingleAnwser.Add(new CompModuleUI { Id = comp.SingleAnwser.Count + 1, Text = "" });
                }



            }

            StateHasChanged();
        }

        public async void RemoveQuestion(CompModuleUI item, int compId)
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
            //clear all validation messages
            formContext = new EditContext(Survey);
            //opretter en validation message store til at indeholde alle min error beskeder
            ValidationMessageStore validationMessageStore = new ValidationMessageStore(formContext);

            //laver en Liste til at holde mine error beskeder
            List<ValidationResult> validationResults = new List<ValidationResult>();
            //Opretter min validation context (Validation contexten er den som går ud fra de tags man en gang har og validere for en.)
            ValidationContext validationContext = new ValidationContext(Survey);
            //validere min model
            Validator.TryValidateObject(Survey, validationContext, validationResults, true);
            //jeg gennemgår mine error beskeder og tilføjer dem til min validation message store
            foreach (var validationResult in validationResults)
            {
                var memberName = validationResult.MemberNames.FirstOrDefault();
                var fieldIdentifier = new FieldIdentifier(Survey, memberName);
                // Manually add the validation error to the ValidationMessageStore
                validationMessageStore.Add(fieldIdentifier, validationResult.ErrorMessage);

            }

            if (Survey.Comps != null)
            {
                //itterer igennem mine comonents
                foreach (var item in Survey.Comps)
                {
                    //laver en Liste til at holde mine error beskeder
                    validationResults = new List<ValidationResult>();
                    //Opretter min validation context (Validation contexten er den som går ud fra de tags man en gang har og validere for en.)
                    validationContext = new ValidationContext(item);

                    //Her prøver jeg at validere min model
                    bool isCompValid = Validator.TryValidateObject(item, validationContext, validationResults, true);


                    //jeg gennemgår mine error beskeder og tilføjer dem til min validation message store
                    foreach (var validationResult in validationResults)
                    {
                        var memberName = validationResult.MemberNames.FirstOrDefault();
                        var fieldIdentifier = new FieldIdentifier(item, memberName);


                        // Manually add the validation error to the ValidationMessageStore
                        validationMessageStore.Add(fieldIdentifier, validationResult.ErrorMessage);



                    }


                    ValidteAnwserUI(item, validationContext);
                } 
            }

            // Notify the form that validation messages have been updated
            formContext.NotifyValidationStateChanged();

            if (formContext.GetValidationMessages().Count() == 0)
            {
                if (Edit)
                {
                    if (await Repo.UpdateSurveyAsync(Survey))
                    {

                        invalid = false;
                        complete = true;

                    }
                }
                else
                {
                    if (await Repo.AddSurvey(Survey))
                    {

                        invalid = false;
                        complete = true;

                    }
                }
               
            }
            else
            {
                invalid = true;
            }




            StateHasChanged();
        }




        public void ValidteAnwserUI(CompUI item, ValidationContext validationContext)
        {
            ValidationMessageStore validationMessageStore = new ValidationMessageStore(formContext);

            //eftersom mine awsersUI ikke har nogle Custom validering men kun tags kan jeg gå rundt om min validation store
            //og derimod bare notifyfield change på stedet.
            if (item.MultiAnwsers != null)
            {

                foreach (var anwser in item.MultiAnwsers)
                {
                    var compValidationResults = new List<ValidationResult>();
                    var compContext = new ValidationContext(anwser, validationContext, validationContext.Items);
                    Validator.TryValidateObject(anwser, compContext, compValidationResults, true);
                    foreach (var validationResult in compValidationResults)
                    {
                        // Returnerer hvert valideringsresultat, der er fundet
                        validationMessageStore.Add(new FieldIdentifier(anwser, validationResult.MemberNames.FirstOrDefault()), validationResult.ErrorMessage);

                    }
                }
            }
            if (item.SingleAnwser != null)
            {
                foreach (var anwser in item.SingleAnwser)
                {
                    var compValidationResults = new List<ValidationResult>();
                    var compContext = new ValidationContext(anwser, validationContext, validationContext.Items);
                    Validator.TryValidateObject(anwser, compContext, compValidationResults, true);
                    foreach (var validationResult in compValidationResults)
                    {
                        // Returnerer hvert valideringsresultat, der er fundet
                        validationMessageStore.Add(new FieldIdentifier(anwser, validationResult.MemberNames.FirstOrDefault()), validationResult.ErrorMessage);
                    }
                }

            }

                formContext.NotifyValidationStateChanged();

        }



        public async Task closemodal()
        {
            NavMan.NavigateTo("/", true);
        }



    }
}

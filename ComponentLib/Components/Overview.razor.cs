using BlazorBootstrap;
using BuisnessLogic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.JSInterop;
using Models;
using Models.UIModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ComponentLib.Components
{

    public class Chart
    {
        public PieChart Pie;
        public PieChartOptions PieOptions;
        public ChartData Data;
    }

    public class Words
    {
        public double average;
        public double max;
        public double min;
        public int compId;
    }

    public partial class Overview
    {
        [Parameter]
        public SurveyUI Survey { get; set; }

        [Parameter]
        public List<AnwserModuleUI> AnwserModules { get; set; }



        public bool ready;

        private string[]? backgroundColors;
        private int dataLabelsCount = 0;

        public List<Words> words { get; set; } = new();
        public List<Chart> pies { get; set; } = new();


        protected override void OnInitialized()
        {
            Survey.Comps.ToList().ForEach(async x =>
            {
                if (x.Type != 0)
                {
                    Chart chart = new Chart();
                    chart.Pie = default!; chart.PieOptions = default!; chart.Data = default!;
                    backgroundColors = ColorUtility.CategoricalTwelveColors;
                    chart.Data = new ChartData { Labels = new List<string>(), Datasets = new List<IChartDataset> { new PieChartDataset() } };
                    chart.PieOptions = new();
                    chart.PieOptions.Responsive = true;
                    chart.PieOptions.Plugins.Title!.Text = x.Question;
                    chart.PieOptions.Plugins.Title.Display = true;

                    if (x.Type == 1)
                    {
                        dataLabelsCount = 0;
                        var dataSet = (PieChartDataset)chart.Data.Datasets.First();
                        x.MultiAnwsers.ForEach(y =>
                        {

                            chart.Data.Labels.Add(y.Text);

                            // Set background color for the chart segment
                            dataSet.BackgroundColor ??= new List<string>();
                            dataSet.BackgroundColor.Add(backgroundColors![dataLabelsCount % backgroundColors.Length]);

                            dataLabelsCount++;

                        });

                        dataSet.Data ??= new List<double?>();
                        dataSet.Data.AddRange(Count(x.Id));


                    }
                    if (x.Type == 2)
                    {
                        dataLabelsCount = 0;
                        var dataSet = (PieChartDataset)chart.Data.Datasets.First();
                        x.SingleAnwser.ForEach(y =>
                        {
                            chart.Data.Labels.Add(y.Text);
                            dataSet.BackgroundColor ??= new List<string>();
                            dataSet.BackgroundColor.Add(backgroundColors![dataLabelsCount % backgroundColors.Length]);

                            dataLabelsCount++;
                        });

                        dataSet.Data ??= new List<double?>();
                        dataSet.Data.AddRange(Count(x.Id));
                        // Set background color for the chart segment



                    }

                    //await chart.Pie.InitializeAsync(chart.Data, chart.PieOptions);
                    pies.Add(chart);
                }
                else
                {
                    pies.Add(new Chart { });
                }

            });
            ready = true;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Survey.Comps.Where(x => x.Type == 0).ToList().ForEach(comp =>
                {
                    var answers = AnwserModules.SelectMany(module => module.anwsers).Where(answer => answer.CompId == comp.Id).ToList();
                    var answerLengths = answers.Select(answer => answer.AnwserText.Length).ToList();

                    if (answerLengths.Count == 0)
                    { 
                        answerLengths.Add(0);    
                    }

                    answerLengths.Sort();
                    int averageLength = answerLengths.Sum() / answerLengths.Count;

                    words.Add(new Words
                    {
                        min = answerLengths.First(),
                        max = answerLengths.Last(),
                        average = averageLength,
                        compId = comp.Id
                    });
                });
                words.Sort();
                StateHasChanged();
                pies.ForEach(async x =>
                {
                    if (x.Data != null)
                    {
                        await x.Pie.InitializeAsync(x.Data, x.PieOptions);
                    }
                });
                // await pieChart.InitializeAsync(chartData, pieChartOptions);
            }
            await base.OnAfterRenderAsync(firstRender);
        }


        private List<double?> Count(int compId)
        {
            // Step 1: Collect all valid answers for the given compId
            var validAnswers = new List<double?>();

            foreach (var answerModule in AnwserModules) // Loop through each answer module
            {
                foreach (var answer in answerModule.anwsers) // Loop through each answer in the module
                {
                    // Check if the answer's CompId matches and is not empty
                    if (answer.CompId == compId && !string.IsNullOrEmpty(answer.AnwserText))
                    {
                        // Split the answer text by commas and try to parse each part as a double
                        var answerParts = answer.AnwserText.Split(',', StringSplitOptions.RemoveEmptyEntries);

                        foreach (var part in answerParts) // Loop through each part
                        {
                            // Try to parse the part as a double
                            if (double.TryParse(part, out var parsedValue))
                            {
                                validAnswers.Add(parsedValue); // Add the parsed value to the list
                            }
                        }
                    }
                }
            }

            // Step 2: Count occurrences of each unique answer
            var answerOccurrences = new Dictionary<double, int>();

            foreach (var value in validAnswers) // Loop through each valid answer
            {
                if (value.HasValue) // Check if the value is not null
                {
                    if (answerOccurrences.ContainsKey(value.Value))
                    {
                        answerOccurrences[value.Value]++; // Increment count if it exists
                    }
                    else
                    {
                        answerOccurrences[value.Value] = 1; // Initialize count to 1
                    }
                }
            }


            // Step 3: Create a list of occurrence values
            var occurrenceValues = new List<double?>();
            CompUI comp = Survey.Comps.FirstOrDefault(x => x.Id == compId);

            // Determine the target answers list based on comp.Type
            var targetAnswers = comp.Type == 1 ? comp.MultiAnwsers : comp.SingleAnwser;

            // Populate occurrenceValues based on answerOccurrences
            for (int i = 0; i < targetAnswers.Count; i++)
            {
                bool found = false;
                foreach (var item in answerOccurrences)
                {
                    if (item.Key == i)
                    {
                        occurrenceValues.Add((double?)item.Value); // Add the count to the occurrence list
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    occurrenceValues.Add(0); // Add zero if no match is found in answerOccurrences
                }
            }


            // Step 4: Return the list of occurrence values
            return occurrenceValues;
        }







    }
}

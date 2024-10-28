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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ComponentLib.Components
{

    public class Chart
    {
        public PieChart Pie { get; set; }
        public PieChartOptions PieOptions { get; set; }
        public ChartData Data { get; set; }
    }

    public partial class Overview
    {
        [Parameter]
        public SurveyUI Survey { get; set; }

        [Parameter]
        public List<AnwserModuleUI> AnwserModules { get; set; }

        public bool ready;

        private PieChart pieChart = default!;
        private PieChartOptions pieChartOptions = default!;
        private ChartData chartData = default!;
        private string[]? backgroundColors;
        private int dataLabelsCount = 0;

        public List<Chart> pies { get; set; } = new();


        protected override void OnInitialized()
        {
            backgroundColors = ColorUtility.CategoricalTwelveColors;
            chartData = new ChartData { Labels = new List<string>(), Datasets = new List<IChartDataset> { new PieChartDataset() } };

            pieChartOptions = new();
            pieChartOptions.Responsive = true;
            pieChartOptions.Plugins.Title!.Text = "2022 - Sales";
            pieChartOptions.Plugins.Title.Display = true;
            ready = true;

            LoadSurveyData(); // Load actual data from the Survey and AnwserModules
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Survey.Comps.ToList().ForEach(x =>
                {
                    if (x.Type != 0)
                    {
                        Chart chart = new Chart();
                        chart.Pie = default!; chart.PieOptions = default!; chart.Data = default!;
                        backgroundColors = ColorUtility.CategoricalTwelveColors;
                        chart.Data = new ChartData { Labels = new List<string>(), Datasets = new List<IChartDataset> { new PieChartDataset() } };
                        chart.PieOptions = new();
                        pieChartOptions.Responsive = true;
                        pieChartOptions.Plugins.Title!.Text = x.Question;
                        pieChartOptions.Plugins.Title.Display = true;

                        if (x.Type == 1)
                        {
                            x.MultiAnwsers.ForEach(y =>
                            {
                                chart.Data.Labels.Add(y.Text);
                            });
                            var dataSet = (PieChartDataset)chart.Data.Datasets.First();

                        }
                        if (x.Type == 2)
                        {
                            x.SingleAnwser.ForEach(y =>
                            {
                                chart.Data.Labels.Add(y.Text);
                                var dataSet = (PieChartDataset)chart.Data.Datasets.First();
                                dataSet.Data ??= new List<double?>();
                                //Get Number of Anwsers
                           

                                //dataSet.Data.Add()
                            });
                

                        }
                    }
                });


                await pieChart.InitializeAsync(chartData, pieChartOptions);
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private void LoadSurveyData()
        {
            if (Survey == null || Survey.Comps == null || !Survey.Comps.Any() || AnwserModules == null)
                return;

            foreach (var comp in Survey.Comps)
            {
                var answerCount = CountAnswersForComponent(comp.Id);

                // Update Labels and Data
                chartData.Labels.Add(comp.Question); // Add question as a label
                var dataSet = (PieChartDataset)chartData.Datasets.First();

                // Add corresponding answer counts to the dataset
                dataSet.Data ??= new List<double?>();
                dataSet.Data.Add(answerCount);

                // Set background color for the chart segment
                dataSet.BackgroundColor ??= new List<string>();
                dataSet.BackgroundColor.Add(backgroundColors![dataLabelsCount % backgroundColors.Length]);

                dataLabelsCount++;
            }
        }

        private double CountAnswersForComponent(int componentId)
        {
            // Count the number of answers for a specific component ID
            // Assuming that AnwserModules contains lists of Anwser with a CompId to match SComp
            List<(int, int)> answerCount = new List<(int, int)>();
            List<AnwserUI> myModules = AnwserModules.SelectMany(am => am.anwsers).Where(answer => answer.CompId == componentId).ToList();
            answerCount.AddRange(myModules.Where(x => !string.IsNullOrEmpty(x.AnwserText)) .SelectMany(x => x.AnwserText.Split(',')
            .Select(int.Parse).Select(parsedValue => (x.CompId, parsedValue)))
);



            return AnwserModules
            .SelectMany(am => am.anwsers)
                .Count(answer => answer.CompId == componentId);
        }



    }
}

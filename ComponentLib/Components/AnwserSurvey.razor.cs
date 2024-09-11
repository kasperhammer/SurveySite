﻿using Microsoft.AspNetCore.Components;
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

        bool ready;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
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
                                Module.anwsers.Add(new AnwserUI { CompId = item.SurveyId});
                            }
                            StateHasChanged();
                        }
                    }
                }
            }
        }
    }
}

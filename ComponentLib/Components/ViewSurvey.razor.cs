using BuisnessLogic;
using Microsoft.AspNetCore.Components;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class ViewSurvey
    {
        [Parameter]
        public int SurveyId { get; set; }
        [Parameter]
        public EventCallback<(int, int)> PageUpdate { get; set; }

        public List<Survey> Anwsers { get; set; } = new();
        public bool ready;

        [Parameter]
        public int PageCount
        {
            get { return _pageCount; }
            set
            {
                if (value != PageCount)
                {
                    if (value <= 0)
                    {
                        _pageCount = 0;
                        PageUpdate.InvokeAsync((PageCount, Anwsers.Count));
                    }
                    else
                    {

                        if (value >= Anwsers.Count)
                        {
                            _pageCount = Anwsers.Count;
                        }
                        else
                        {
                            _pageCount = value;
                        }
                        PageUpdate.InvokeAsync((PageCount, Anwsers.Count));

                    }
                }







            }
        }
        private int _pageCount = 0;

        [Inject]
        IRepository Repo { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Anwsers = await Repo.GetSurvetAnwsers(SurveyId);
                await PageUpdate.InvokeAsync((0, Anwsers.Count));
                ready = true;
                StateHasChanged();
            }
        }

        public async Task SingleAwnser(SComp module, AnwserModule anwser)
        {
            foreach (var item in module.SingleAnwser)
            {
                if (item.Selected == true && item.Id != anwser.Id)
                {
                    item.Selected = false;
                }
            }
            StateHasChanged();
        }
    }
}

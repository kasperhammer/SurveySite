﻿using AutoMapper;
using Database;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.UIModels;

namespace BuisnessLogic
{
    public class Repository : IRepository
    {
        public Db Database;
        private readonly IMapper _mappingProfile;
        public Repository(IMapper map)
        {
            this._mappingProfile = map;
            Database = new();
        }

        public async Task<bool> AddSurvey(SurveyUI surveyUI)
        {
            Survey survey = _mappingProfile.Map<Survey>(surveyUI);
            if (survey == null || string.IsNullOrEmpty(survey.Name) || survey.SComps == null || !survey.SComps.Any())
            {
                return false;
            }

            if (survey.SComps.Any(comp => string.IsNullOrEmpty(comp.Question)))
            {
                return false;
            }


            List<SComp> comps = survey.SComps;
            List<SComp> compsOld = new List<SComp>();
            foreach (var x in comps)
            {
                compsOld.Add(new SComp
                {
                    Id = x.Id,
                    MultiAnwsers = x.MultiAnwsers,
                    Question = x.Question,
                    SingleAnwser = x.SingleAnwser,
                    Survey = x.Survey,
                    SurveyId = x.SurveyId,
                    Type = x.Type
                });
            }


            if (await CreateSurvey(survey))
            {
                int Id = survey.Id;



                if (await CreateComps(comps, Id))
                {
                    if (await CreateModules(comps, compsOld))
                    {
                        surveyUI.Id = survey.Id;
                        return true;
                    }
                }

            }

            return false;

        }

        private async Task<bool> CreateSurvey(Survey survey)
        {
            survey.Id = 0;
            survey.SComps = null;
            await Database.Surveys.AddAsync(survey);
            return await Database.SaveChangesAsync() > 0;

        }

        private async Task<bool> CreateComps(List<SComp> comps, int surveyId)
        {
            foreach (var item in comps)
            {
                item.Id = 0;
                item.SurveyId = surveyId;
                item.MultiAnwsers = null;
                item.SingleAnwser = null;

            }

            await Database.Components.AddRangeAsync(comps);
            return await Database.SaveChangesAsync() > 0;
        }

        private async Task<bool> CreateModules(List<SComp> comps, List<SComp> compsOld)
        {
            List<CompModule> modules = new();
            for (int i = 0; i < comps.Count; i++)
            {
                if (comps[i].Type == 1)
                {
                    compsOld[i].MultiAnwsers.ForEach(x => x.CompMultiId = comps[i].Id);
                    modules.AddRange(compsOld[i].MultiAnwsers);
                }

                if (comps[i].Type == 2)
                {
                    compsOld[i].SingleAnwser.ForEach(x => x.CompSingleId = comps[i].Id);
                    modules.AddRange(compsOld[i].SingleAnwser);
                }
            }

            modules.ForEach(x => x.Id = 0);



            await Database.CompModules.AddRangeAsync(modules);
            await Database.SaveChangesAsync();
            return true;
        }

        public async Task<List<Survey>> GetAllSurveys()
        {
            List<Survey> surveys = await Database.Surveys
                .Include(x => x.SComps)
                    .ThenInclude(r => r.MultiAnwsers)
                .Include(x => x.SComps)
                    .ThenInclude(r => r.SingleAnwser)
                .ToListAsync();

            return surveys;
        }

        public async Task<SurveyUI> GetOneSurvey(int id)
        {

            if (id != 0)
            {
                Survey survey = await Database.Surveys.Include(x => x.SComps)
                  .ThenInclude(r => r.MultiAnwsers)
              .Include(x => x.SComps)
                  .ThenInclude(r => r.SingleAnwser).FirstOrDefaultAsync(x => x.Id == id);

                if (survey != null)
                {
                    return _mappingProfile.Map<SurveyUI>(survey);
                }
            }

            return null;
        }

        public async Task<List<AnwserModuleUI>> GetSurvetAnwsers(int id)
        {
            List<AnwserModule> anwersList = await Database.AnwserModules.Include(x => x.anwsers).Where(x => x.SurveyId == id).ToListAsync();
            if (anwersList != null)
            {
                if (anwersList.Count != 0)
                {
              
                    return _mappingProfile.Map<List<AnwserModuleUI>>(anwersList); 
                }
            }
            return null;
        }

        public async Task<bool> SubmitAnwserAsync(AnwserModuleUI anwser)
        {
            if (anwser != null)
            {
                try
                {
                    AnwserModule anwserDTO = _mappingProfile.Map<AnwserModule>(anwser);
                    await Database.AnwserModules.AddAsync(anwserDTO);
                    return await Database.SaveChangesAsync() > 0;
                }
                catch (Exception ex)
                {

                }

            }
            return false;
        }



    }
}

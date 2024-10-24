using AutoMapper;
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

        public async Task<bool> UpdateSurveyAsync(SurveyUI surveyUI)
        {
            if (surveyUI == null)
                return false;

            // Map incoming survey to the Survey entity
            Survey updatedSurvey = _mappingProfile.Map<Survey>(surveyUI);

            // Fetch the existing survey from the database with components and their modules (MultiAnwsers and SingleAnwser)
            Survey existingSurvey = await Database.Surveys
                .Include(x => x.SComps)
                    .ThenInclude(r => r.MultiAnwsers)
                .Include(x => x.SComps)
                    .ThenInclude(r => r.SingleAnwser)
                .FirstOrDefaultAsync(x => x.Id == updatedSurvey.Id);

            if (existingSurvey == null)
                return false;

            // Update the survey details (not including the components)
            Database.Entry(existingSurvey).CurrentValues.SetValues(updatedSurvey);

            // Handle components - Add new, update existing, delete removed
            var existingComponents = existingSurvey.SComps.ToList();
            var updatedComponents = updatedSurvey.SComps.ToList();

            // Identify components to add, update, or delete
            var newComponents = updatedComponents.Where(uc => !existingComponents.Any(ec => ec.Id == uc.Id)).ToList();
            var updatedComponentsToUpdate = updatedComponents.Where(uc => existingComponents.Any(ec => ec.Id == uc.Id)).ToList();
            var removedComponents = existingComponents.Where(ec => !updatedComponents.Any(uc => uc.Id == ec.Id)).ToList();

            // Add new components
            foreach (var newComp in newComponents)
            {
                newComp.SurveyId = existingSurvey.Id;
                newComp.Id = 0; // Ensure new components have no ID set
                Database.Components.Add(newComp);
            }

            // Update existing components and their modules
            foreach (var updatedComp in updatedComponentsToUpdate)
            {
                var existingComp = existingComponents.First(ec => ec.Id == updatedComp.Id);
                Database.Entry(existingComp).CurrentValues.SetValues(updatedComp);

                // Update MultiAnwsers and SingleAnwser for each component
                UpdateCompModules(existingComp, updatedComp);
            }

            // Delete removed components
            foreach (var removedComp in removedComponents)
            {
                Database.Components.Remove(removedComp);
            }

            // Save all changes
            try
            {
                await Database.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Handle exceptions (e.g., logging)
                return false;
            }
        }

        private void UpdateCompModules(SComp existingComp, SComp updatedComp)
        {
            // MultiAnwsers update logic
            if (updatedComp.MultiAnwsers != null && updatedComp.Type == 1)
            {
                // Add new MultiAnwsers
                var newMultiAnswers = updatedComp.MultiAnwsers
                    .Where(ua => !existingComp.MultiAnwsers.Any(ea => ea.Id == ua.Id))
                    .ToList();

                foreach (var newMulti in newMultiAnswers)
                {
                    newMulti.CompMultiId = existingComp.Id;
                    newMulti.Id = 0;
                    existingComp.MultiAnwsers.Add(newMulti);
                }

                // Update existing MultiAnwsers
                foreach (var multiToUpdate in updatedComp.MultiAnwsers)
                {
                    var existingMulti = existingComp.MultiAnwsers.FirstOrDefault(ea => ea.Id == multiToUpdate.Id);
                    if (existingMulti != null)
                    {
                        Database.Entry(existingMulti).CurrentValues.SetValues(multiToUpdate);
                    }
                }

                // Remove deleted MultiAnwsers
                var removedMulti = existingComp.MultiAnwsers
                    .Where(ea => !updatedComp.MultiAnwsers.Any(ua => ua.Id == ea.Id))
                    .ToList();

                foreach (var multi in removedMulti)
                {
                    Database.CompModules.Remove(multi);
                }
            }

            // SingleAnwser update logic (similar pattern to MultiAnwsers)
            if (updatedComp.SingleAnwser != null && updatedComp.Type == 2)
            {
                var newSingleAnswers = updatedComp.SingleAnwser
                    .Where(ua => !existingComp.SingleAnwser.Any(ea => ea.Id == ua.Id))
                    .ToList();

                foreach (var newSingle in newSingleAnswers)
                {
                    newSingle.CompSingleId = existingComp.Id;
                    newSingle.Id = 0;
                    existingComp.SingleAnwser.Add(newSingle);
                }

                foreach (var singleToUpdate in updatedComp.SingleAnwser)
                {
                    var existingSingle = existingComp.SingleAnwser.FirstOrDefault(ea => ea.Id == singleToUpdate.Id);
                    if (existingSingle != null)
                    {
                        Database.Entry(existingSingle).CurrentValues.SetValues(singleToUpdate);
                    }
                }

                var removedSingle = existingComp.SingleAnwser
                    .Where(ea => !updatedComp.SingleAnwser.Any(ua => ua.Id == ea.Id))
                    .ToList();

                foreach (var single in removedSingle)
                {
                    Database.CompModules.Remove(single);
                }
            }
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

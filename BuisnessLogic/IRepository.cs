﻿using Models;
using Models.UIModels;

namespace BuisnessLogic
{
    public interface IRepository
    {
        Task<bool> AddSurvey(SurveyUI surveyUI);
        Task<List<Survey>> GetAllSurveys();
        Task<SurveyUI> GetOneSurvey(int id);
        Task<List<AnwserModuleUI>> GetSurvetAnwsers(int id);
        Task<bool> SubmitAnwserAsync(AnwserModuleUI anwser);
        Task<bool> UpdateSurveyAsync(SurveyUI surveyUI);
    }
}
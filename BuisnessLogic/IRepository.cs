using Models;
using Models.UIModels;

namespace BuisnessLogic
{
    public interface IRepository
    {
        Task<bool> AddSurvey(SurveyUI surveyUI);
        Task<List<Survey>> GetAllSurveys();
        Task<Survey> GetOneSurvey(int id);
        Task<List<Survey>> GetSurvetAnwsers(int id);
    }
}
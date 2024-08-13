using Models;

namespace BuisnessLogic
{
    public interface IRepository
    {
        Task<bool> AddSurvey(Survey survey);
        Task<List<Survey>> GetAllSurveys();
        Task<Survey> GetOneSurvey(int id);
        Task<List<Survey>> GetSurvetAnwsers(int id);
    }
}
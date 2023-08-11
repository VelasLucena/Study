using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface ISubjectRepository
    {

        Task<List<SubjectModel>> GetAllSubjects();

        Task<SubjectModel> GetSubjectByNameFirst(string name);

        Task<List<SubjectModel>> GetSubjectByDisciplineId(int disciplineId);

        Task<SubjectModel> GetSubjectBySubjectId(int subjectId);
    }
}

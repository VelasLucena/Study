using SudyApi.Models.Subject;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface ISubjectRepository
    {
        #region GetSubjects

        Task<List<SubjectModel>> GetAllSubjects(int limit, Ordering ordering, string attributeName);

        Task<List<SubjectModel>> GetAllSubjectsNoTracking(int limit, Ordering ordering, string attributeName);

        #endregion

        #region GetSubjectByNameFirst

        Task<SubjectModel> GetSubjectByNameFirst(string name);

        Task<SubjectModel> GetSubjectByNameFirstNoTracking(string name);

        #endregion

        #region GetSubjectByUserId

        Task<List<SubjectModel>> GetSubjectByUserId(int userId);

        Task<List<SubjectModel>> GetSubjectByUserIdNoTracking(int userId);

        #endregion

        #region GetSubjectBySubjectId

        Task<SubjectModel> GetSubjectBySubjectId(int subjectId);

        Task<SubjectModel> GetSubjectBySubjectIdNoTracking(int subjectId);

        #endregion


    }
}

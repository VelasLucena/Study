using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface ICollegeSubjectRepository
    {
        #region GetAllCollegeSubjects

        Task<List<CollegeSubjectModel>> GetAllCollegeSubjects(int semesterId);

        Task<List<CollegeSubjectModel>> GetAllCollegeSubjectsNoTracking(int semesterId);

        #endregion
    }
}

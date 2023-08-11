using SudyApi.Models;
using SudyApi.Properties.Enuns;

namespace SudyApi.Data.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<CourseModel>> GetAllCourses();

        Task<List<CourseModel>> GetCourseByName(string courseName);

        Task<CourseModel> GetCourseById(int courseId);
    }
}

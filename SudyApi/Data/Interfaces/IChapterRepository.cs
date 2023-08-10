using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IChapterRepository
    {
        Task<ChapterModel> GetChapterById(int chapterId);

        Task<List<ChapterModel>> GetAllChaptersBySubjectId(int subjectId);

        Task<List<ChapterModel>> GetChapterByName(string name);
    }
}

using SudyApi.Models;

namespace SudyApi.Data.Interfaces
{
    public interface IChapterRepository
    {
        #region GetChapterByChapterId

        Task<ChapterModel> GetChapterById(int chapterId);

        Task<ChapterModel> GetChapterByIdNoTracking(int chapterId);

        #endregion

        #region GetAllChaptersBySubjectId

        Task<List<ChapterModel>> GetAllChaptersBySubjectId(int subjectId);

        Task<List<ChapterModel>> GetAllChaptersBySubjectIdNoTracking(int subjectId);

        #endregion

        #region GetChapterByName

        Task<List<ChapterModel>> GetChapterByName(string name);

        Task<List<ChapterModel>> GetChapterByNameNoTracking(string name);

        #endregion
    }
}
